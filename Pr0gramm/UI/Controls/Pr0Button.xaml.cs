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

        public static readonly DependencyProperty ToggledProperty = DependencyProperty.Register("Toggled", typeof(bool), typeof(bool), new PropertyMetadata(false));
        public bool Toggled
        {
            get { return (bool)GetValue(ToggledProperty); }
            set
            {
                SetValue(ToggledProperty, value);
                btn.IsChecked = value;
                var eh = CheckStateChanged;
                if (eh != null)
                    eh(this, btn.IsChecked.Value);
            }
        }

        public static readonly DependencyProperty AllowUserToggleProperty = DependencyProperty.Register("AllowUserToggle", typeof(bool), typeof(bool), new PropertyMetadata(false));
        public bool AllowUserToggle
        {
            get { return (bool)GetValue(AllowUserToggleProperty); }
            set { SetValue(AllowUserToggleProperty, value); }
        }

        public Pr0Button()
        {
            this.InitializeComponent();
        }

        public event EventHandler<RoutedEventArgs> Click;
        public event EventHandler<bool> CheckStateChanged;
        private void btn_Click(object sender, RoutedEventArgs e)
        {
            var eh = Click;
            if (eh != null)
                eh(this, e);
        }

        private void btn_CheckedChanged(object sender, RoutedEventArgs e)
        {
            if (this.AllowUserToggle)
            {
                if (btn.IsChecked != this.Toggled)
                {
                    var eh = CheckStateChanged;
                    if (eh != null)
                        eh(this, btn.IsChecked.Value);
                    this.Toggled = btn.IsChecked.Value;
                }
            }
            else
            {
                if (btn.IsChecked != this.Toggled)
                {
                    btn.IsChecked = this.Toggled;
                }
            }
        }
    }
}
