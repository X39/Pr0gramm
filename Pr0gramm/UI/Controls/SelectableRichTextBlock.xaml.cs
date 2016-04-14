using System;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Documents;
using System.Collections.Generic;

// The User Control item template is documented at http://go.microsoft.com/fwlink/?LinkId=234236

namespace Pr0gramm.UI.Controls
{
    public sealed partial class SelectableRichTextBlock : UserControl
    {
        public static readonly char[] HandleTerminateCharacters = { ' ', '\r', '\n', '\0', '\t' };
        public class CustomHandlersReturn
        {
            internal int StartIndex { get; private set; }
            internal int EndIndex { get; private set; }
            internal string Link { get; private set; }
            internal string Text { get; private set; }
            public CustomHandlersReturn(int startIndex, int endIndex = -1, string link = default(string), string linkText = default(string))
            {
                this.StartIndex = startIndex;
                this.EndIndex = endIndex;
                this.Link = link;
                this.Text = string.IsNullOrWhiteSpace(linkText) ? link : linkText;
            }
        }
        public class LinkClickedEventArgs : EventArgs
        {
            public bool Handled { get; set; }
            public Uri URI { get; private set; }
            public LinkClickedEventArgs(Uri uri)
            {
                this.URI = uri;
                this.Handled = false;
            }
        }
        public event EventHandler<LinkClickedEventArgs> LinkClicked;
        private Dictionary<Hyperlink, Uri> HyperlinkRegister;

        public static readonly DependencyProperty TextProperty = DependencyProperty.Register("Text", typeof(string), typeof(string), new PropertyMetadata(default(string)));
        public string Text
        {
            get { return (string)GetValue(TextProperty); }
            set { SetValue(TextProperty, value); parseIntoComment(value); }
        }

        public List<Func<string, int, CustomHandlersReturn>> CustomHandlers { get; private set; }
        public SelectableRichTextBlock()
        {
            HyperlinkRegister = new Dictionary<Hyperlink, Uri>();
            CustomHandlers = new List<Func<string, int, CustomHandlersReturn>>();
            this.InitializeComponent();
        }
        private void parseIntoComment(string content)
        {
            var linkPrefixes = new string[] { "http://", "https://" };
            int index = 0;
            int lastIndex = 0;
            do
            {
                foreach(var fnc in this.CustomHandlers)
                {
                    var result = fnc.Invoke(content, lastIndex);
                    if (result.StartIndex > 0)
                    {
                        Hyperlink lnk = new Hyperlink();
                        lnk.Inlines.Add(new Run { Text = result.Text });
                        HyperlinkRegister[lnk] = new Uri(result.Link);
                        lnk.Foreground = Application.Current.Resources["pr0_orange"] as SolidColorBrush;
                        lnk.Click += (sender, e) =>
                        {
                            sender.NavigateUri = null;
                            var uri = HyperlinkRegister[sender];
                            var args = new LinkClickedEventArgs(uri);
                            if (this.LinkClicked != null)
                            {
                                this.LinkClicked(sender, args);
                            }
                            if (!args.Handled)
                            {
                                sender.NavigateUri = uri;
                            }
                        };
                        Paragraph p = new Paragraph();
                        p.Inlines.Add(lnk);
                        this.ContentElement.Blocks.Add(p);
                        lastIndex = index = result.EndIndex;
                    }
                }
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
                    if (content.Length != lastIndex)
                    {
                        Span span = new Span();
                        span.Inlines.Add(new Run { Text = content.Substring(lastIndex) });
                        Paragraph p = new Paragraph();
                        p.Inlines.Add(span);
                        this.ContentElement.Blocks.Add(p);
                    }
                    break;
                }
                else if (index > 0)
                {
                    Span span = new Span();
                    span.Inlines.Add(new Run { Text = content.Substring(lastIndex, index - lastIndex) });

                    Paragraph p = new Paragraph();
                    p.Inlines.Add(span);
                    this.ContentElement.Blocks.Add(p);
                }
                int indexEnd = content.IndexOfAny(HandleTerminateCharacters, index);
                if (indexEnd < 0)
                    indexEnd = content.Length;

                string link = content.Substring(index, indexEnd - index);
                string linkText = link.StartsWith(currentPrefix + "pr0gramm.com") ? link.Substring(currentPrefix.Length + "pr0gramm.com".Length) : link;

                {
                    Hyperlink lnk = new Hyperlink();
                    lnk.Inlines.Add(new Run { Text = linkText });
                    HyperlinkRegister[lnk] = new Uri(link);
                    lnk.Foreground = Application.Current.Resources["pr0_orange"] as SolidColorBrush;
                    lnk.Click += (sender, e) =>
                    {
                        sender.NavigateUri = null;
                        var uri = HyperlinkRegister[sender];
                        var args = new LinkClickedEventArgs(uri);
                        if (this.LinkClicked != null)
                        {
                            this.LinkClicked(sender, args);
                        }
                        if (!args.Handled)
                        {
                            sender.NavigateUri = uri;
                        }
                    };
                    Paragraph p = new Paragraph();
                    p.Inlines.Add(lnk);
                    this.ContentElement.Blocks.Add(p);
                }
                lastIndex = indexEnd;
            } while (true);
        }
    }
}
