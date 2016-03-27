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
    public sealed partial class LoginView : Page
    {
        public LoginView()
        {
            this.InitializeComponent();
        }

        private async void btnLogin_Click(object sender, RoutedEventArgs e)
        {
            var obj = await API.User.Login(this.tbUser.Text, this.pbPassowrd.Password, app.Settings.Instance.Url);
            if (obj == null)
            {
                var toast = new app.TextToast("Login Err0r", "Der Login war leider nicht sehr erfolgreich :(");
            }
            else
            {
                app.Settings.Instance.Pr0User = obj.Item1;
                app.Settings.Instance.Cookie = obj.Item2;
                //ToDo: Add logic to change current displayed user etc.
            }
        }
    }
}
