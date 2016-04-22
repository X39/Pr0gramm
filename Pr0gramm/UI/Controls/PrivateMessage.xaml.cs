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

// The User Control item template is documented at http://go.microsoft.com/fwlink/?LinkId=234236

namespace Pr0gramm.UI.Controls
{
    public sealed partial class PrivateMessage : UserControl
    {
        public static readonly DependencyProperty TextProperty = DependencyProperty.Register("Text", typeof(string), typeof(string), new PropertyMetadata(default(string)));
        public string Text
        {
            get { return (string)GetValue(TextProperty); }
            set { SetValue(TextProperty, value); }
        }
        public bool IsSender
        {
            set
            {
                if(value)
                {
                    this.MarginGrid.Margin = new Thickness(32, 4, 0, 4);
                    this.Background = Application.Current.Resources["pr0_cha0sGrey"] as SolidColorBrush;
                    this.Foreground = Application.Current.Resources["pr0_textColor"] as SolidColorBrush;
                }
                else
                {
                    this.MarginGrid.Margin = new Thickness(0, 4, 32, 4);
                    this.Background = Application.Current.Resources["pr0_cha0sGrey"] as SolidColorBrush;
                    this.Foreground = Application.Current.Resources["pr0_textColor"] as SolidColorBrush;
                }
            }
        }


        public PrivateMessage()
        {
            this.InitializeComponent();
            this.IsSender = false;
        }
    }
}
