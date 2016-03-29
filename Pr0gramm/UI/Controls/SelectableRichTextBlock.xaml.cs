using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using Windows.UI.Text;

// The User Control item template is documented at http://go.microsoft.com/fwlink/?LinkId=234236

namespace Pr0gramm.UI.Controls
{
    public sealed partial class SelectableRichTextBlock : UserControl
    {
        public static readonly DependencyProperty TextProperty = DependencyProperty.Register("Text", typeof(string), typeof(string), new PropertyMetadata(default(string)));
        public string Text
        {
            get { return (string)GetValue(TextProperty); }
            set { SetValue(TextProperty, value); parseIntoComment(value); }
        }
        public SelectableRichTextBlock()
        {
            this.InitializeComponent();
        }
        private void parseIntoComment(string content)
        {
            //DO NOT ASK WHY ... WinRT is love :facepalm:
            //thx to you Jason Jarrett ... http://elegantcode.com/2013/03/19/richeditbox-gives-unauthorizedaccessexception-access-is-denied-error-when-settext-called/
            this.ContentElement.IsReadOnly = false;
            this.ContentElement.Document.SetText(TextSetOptions.None, content);
            int index = 0;
            do
            {
                index = content.IndexOf("http://", index);
                if (index < 0)
                    break;
                int indexEnd = content.IndexOfAny(new char[] { ' ', '\r', '\n', '\0', '\t' }, index);
                if (indexEnd < 0)
                    indexEnd = content.Length;
                var range = this.ContentElement.Document.GetRange(index, indexEnd);
                if (range.Text.StartsWith("http://pr0gramm.com"))
                    range.Text = range.Text.Substring("http://pr0gramm.com".Length);
                range.Link = '"' + content.Substring(index, indexEnd - index) + '"';
                
                index = range.EndPosition;
                this.ContentElement.Document.GetText(TextGetOptions.None, out content);
            } while (true);
            index = 0;
            do
            {
                index = content.IndexOf("https://", index);
                if (index < 0)
                    break;
                int indexEnd = content.IndexOfAny(new char[] { ' ', '\r', '\n', '\0', '\t' }, index);
                if (indexEnd < 0)
                    indexEnd = content.Length;
                var range = this.ContentElement.Document.GetRange(index, indexEnd);
                if (range.Text.StartsWith("https://pr0gramm.com"))
                    range.Text = range.Text.Substring("https://pr0gramm.com".Length);
                range.Link = '"' + content.Substring(index, indexEnd - index) + '"';
                index = range.EndPosition;
                this.ContentElement.Document.GetText(TextGetOptions.None, out content);
            } while (true);
            this.ContentElement.Document.ApplyDisplayUpdates();
            //*sigh* ...
            this.ContentElement.IsReadOnly = true;
        }

        public event EventHandler<string> LinkClicked;
        private void ContentElement_Tapped(object sender, TappedRoutedEventArgs e)
        {
            if (LinkClicked != null)
            {
                var p = e.GetPosition((UIElement)sender);
                var range = this.ContentElement.Document.GetRangeFromPoint(p, PointOptions.ClientCoordinates);
                range.StartOf(TextRangeUnit.Link, true);
                if(!string.IsNullOrWhiteSpace(range.Link))
                {
                    LinkClicked(this, range.Link);
                }
            }
        }

        private void ContentElement_PointerPressed(object sender, PointerRoutedEventArgs e)
        {
            var p = e.GetCurrentPoint((UIElement)sender);
            var range = this.ContentElement.Document.GetRangeFromPoint(p.Position, PointOptions.ClientCoordinates);
            range.StartOf(TextRangeUnit.Link, true);
            if (!string.IsNullOrWhiteSpace(range.Link))
            {
                if(this.LinkClicked != null)
                    this.LinkClicked(this, range.Link);
                e.Handled = true;
            }
        }
    }
}
