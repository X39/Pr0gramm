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

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234238

namespace Pr0gramm.UI.Fragments
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class SettingsPage : Page
    {
        public SettingsPage()
        {
            this.InitializeComponent();
        }

        private void Pr0Button_Click(object sender, RoutedEventArgs e)
        {
            if(app.Settings.Instance.Pr0User != null)
            {
                app.Settings.Instance.Pr0User.Logout(app.Settings.Instance.Url, app.Settings.Instance.Cookie);
                app.Settings.Instance.Pr0User = null;
                app.Settings.Instance.Cookie = null;
            }
        }
    }
}
