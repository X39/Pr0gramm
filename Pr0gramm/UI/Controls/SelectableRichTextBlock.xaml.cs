﻿using System;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Documents;

// The User Control item template is documented at http://go.microsoft.com/fwlink/?LinkId=234236

namespace Pr0gramm.UI.Controls
{
    public sealed partial class SelectableRichTextBlock : UserControl
    {
        public event EventHandler<Uri> LinkClicked;

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
            var linkPrefixes = new string[] { "http://", "https://" };
            int index = 0;
            int lastIndex = 0;
            do
            {
                index = content.IndexOf(linkPrefixes[0], lastIndex);
                int index2 = content.IndexOf(linkPrefixes[1], lastIndex);
                string currentPrefix;
                if ((index2 < index || index == -1) && index2 != -1)
                {
                    index = index2;
                    currentPrefix = linkPrefixes[1];
                }
                else
                {
                    currentPrefix = linkPrefixes[0];
                }
                if (index < 0)
                {
                    Span span = new Span();
                    span.Inlines.Add(new Run { Text = content.Substring(lastIndex) });
                    Paragraph p = new Paragraph();
                    p.Inlines.Add(span);
                    this.ContentElement.Blocks.Add(p);
                    break;
                }
                else if (index > 0)
                {
                    Span span = new Span();
                    span.Inlines.Add(new Run { Text = content.Substring(lastIndex, index) });

                    Paragraph p = new Paragraph();
                    p.Inlines.Add(span);
                    this.ContentElement.Blocks.Add(p);
                }
                int indexEnd = content.IndexOfAny(new char[] { ' ', '\r', '\n', '\0', '\t' }, index);
                if (indexEnd < 0)
                    indexEnd = content.Length;

                string link = content.Substring(index, indexEnd - index);
                string linkText = link.StartsWith(currentPrefix + "pr0gramm.com") ? link.Substring(currentPrefix.Length + "pr0gramm.com".Length) : link;

                Hyperlink lnk = new Hyperlink();
                lnk.Inlines.Add(new Run { Text = linkText });
                lnk.NavigateUri = new Uri(link);
                lnk.Foreground = Application.Current.Resources["pr0_orange"] as SolidColorBrush;
                lnk.Click += (sender, e) =>
                {
                    if (this.LinkClicked != null)
                    {
                        this.LinkClicked(lnk, lnk.NavigateUri);
                    }
                };
                {
                    Paragraph p = new Paragraph();
                    p.Inlines.Add(lnk);
                    this.ContentElement.Blocks.Add(p);
                    lastIndex = indexEnd;
                }
            } while (true);
        }
    }
}
