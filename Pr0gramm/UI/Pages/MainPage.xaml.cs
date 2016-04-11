using Windows.UI;
using Windows.UI.ViewManagement;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409

namespace Pr0gramm.UI.Pages
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        public MainPage()
        {
            this.InitializeComponent();
            btnNew_Click(btnNew, new Windows.UI.Xaml.RoutedEventArgs());
            this.applyTitleBarTheme();
            if(app.Settings.Instance.Pr0User != null)
            {
                this.btnUser.Text = app.Settings.Instance.Pr0User.Username;
            }

            var versionTextBlock = new TextBlock();
            var version = Windows.ApplicationModel.Package.Current.Id.Version;
#if DEBUG
            versionTextBlock.Text = "DEBUG";
#else
            versionTextBlock.Text = string.Format("{0}.{1}.{2}", version.Major, version.Minor, version.Build);
#endif
            versionTextBlock.VerticalAlignment = VerticalAlignment.Center;
            versionTextBlock.HorizontalAlignment = HorizontalAlignment.Right;
            versionTextBlock.Foreground = (Application.Current.Resources["pr0_orange"] as SolidColorBrush);
            versionTextBlock.Margin = new Thickness(12, 0, 12, 0);
            this.Footer.Children.Add(versionTextBlock);
        }

        public void clearToggleStates()
        {
            btnNew.Toggled = false;
            btnTop.Toggled = false;
            btnSettings.Toggled = false;
            btnUser.Toggled = false;
        }

        private void btnNew_Click(object sender, Windows.UI.Xaml.RoutedEventArgs e)
        {
            clearToggleStates();
            btnNew.Toggled = true;
            this.ContentFrame.Navigate(typeof(Fragments.ContentPresenter), new API.ItemsGetterUtil.ViewSource(API.ItemsGetterUtil.ViewSource.ViewType.New));
        }

        private void applyTitleBarTheme()
        {
            ApplicationView AppView = ApplicationView.GetForCurrentView();
            AppView.TitleBar.ButtonInactiveBackgroundColor = (Application.Current.Resources["pr0_orange"] as SolidColorBrush).Color;
            AppView.TitleBar.ButtonInactiveForegroundColor = Colors.White;
            AppView.TitleBar.ButtonBackgroundColor = (Application.Current.Resources["pr0_orange"] as SolidColorBrush).Color;
            AppView.TitleBar.ButtonHoverBackgroundColor = (Application.Current.Resources["pr0_orange"] as SolidColorBrush).Color;
            AppView.TitleBar.ButtonPressedBackgroundColor = (Application.Current.Resources["pr0_orange"] as SolidColorBrush).Color;
            AppView.TitleBar.BackgroundColor = (Application.Current.Resources["pr0_orange"] as SolidColorBrush).Color;
            AppView.TitleBar.InactiveBackgroundColor = (Application.Current.Resources["pr0_orange"] as SolidColorBrush).Color;
        }

        private void btnTop_Click(object sender, Windows.UI.Xaml.RoutedEventArgs e)
        {
            clearToggleStates();
            btnTop.Toggled = true;
            this.ContentFrame.Navigate(typeof(Fragments.ContentPresenter), new API.ItemsGetterUtil.ViewSource(API.ItemsGetterUtil.ViewSource.ViewType.Top));
        }

        private void btnSettings_Click(object sender, RoutedEventArgs e)
        {
            clearToggleStates();
            btnSettings.Toggled = true;
            this.ContentFrame.Navigate(typeof(Fragments.SettingsPage));
        }

        private void btnUser_Click(object sender, RoutedEventArgs e)
        {
            clearToggleStates();
            btnUser.Toggled = true;


            if (app.Settings.Instance.Pr0User == null)
            {
                this.ContentFrame.Navigate(typeof(Fragments.LoginView));
            }
            else
            {
                //ToDo: Show Profile Pane
            }
        }
    }
}
