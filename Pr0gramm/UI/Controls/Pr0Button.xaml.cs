using System;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

// The User Control item template is documented at http://go.microsoft.com/fwlink/?LinkId=234236

namespace Pr0gramm.UI.Controls
{
    public sealed partial class Pr0Button : UserControl
    {
        public static readonly DependencyProperty PathHeightProperty = DependencyProperty.Register("PathHeight", typeof(double), typeof(double), new PropertyMetadata((double)0));
        public double PathHeight
        {
            get { return (double)GetValue(PathHeightProperty); }
            set { SetValue(PathHeightProperty, value); }
        }
        public static readonly DependencyProperty PathMarginProperty = DependencyProperty.Register("PathMargin", typeof(Thickness), typeof(Thickness), new PropertyMetadata(new Thickness()));
        public Thickness PathMargin
        {
            get { return (Thickness)GetValue(PathMarginProperty); }
            set { SetValue(PathMarginProperty, value); }
        }
        public static readonly DependencyProperty PathDataProperty = DependencyProperty.Register("PathData", typeof(string), typeof(string), new PropertyMetadata(default(string)));
        public string PathData
        {
            get { return (string)GetValue(PathDataProperty); }
            set { SetValue(PathDataProperty, value); }
        }

        public static readonly DependencyProperty TextProperty = DependencyProperty.Register("Text", typeof(string), typeof(string), new PropertyMetadata(default(string)));
        public string Text
        {
            get { return (string)GetValue(TextProperty); }
            set { SetValue(TextProperty, value); }
        }

        public Pr0Button()
        {
            this.InitializeComponent();
        }

        public event EventHandler<RoutedEventArgs> Click;
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            var eh = Click;
            if (eh != null)
                eh(this, e);
        }
    }
}
