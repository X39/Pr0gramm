using System;
using Pr0gramm.API;
using Windows.Foundation;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

// The User Control item template is documented at http://go.microsoft.com/fwlink/?LinkId=234236

namespace Pr0gramm.UI.Controls
{
    public sealed partial class Tag : UserControl
    {
        public static readonly DependencyProperty TextProperty = DependencyProperty.Register("Text", typeof(string), typeof(string), new PropertyMetadata(default(string)));
        public string Text
        {
            get { return (string)GetValue(TextProperty); }
            set { SetValue(TextProperty, value); }
        }

        public ItemInfo.Tag FullTag { get; private set; }

        public double ElementWidth
        {
            get
            {
                if (this.TextBlockElement.Text != this.Text)
                    this.TextBlockElement.Text = this.Text;
                this.TextBlockElement.Measure(new Size(Double.PositiveInfinity, Double.PositiveInfinity));
                return this.TextBlockElement.DesiredSize.Width + 2 * 32;
            }
        }

        public Tag(ItemInfo.Tag tag)
        {
            this.InitializeComponent();
            this.Text = tag.Text;
            this.FullTag = tag;
        }
    }
}
