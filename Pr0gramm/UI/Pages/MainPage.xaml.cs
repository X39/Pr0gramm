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
        public string username = "Username";

        public MainPage()
        {
            this.InitializeComponent();
            user.Text = "X39";
            this.applyTitleBarTheme();
            this.ContentFrame.Navigate(typeof(Fragments.ContentPresenter), new Fragments.ContentPresenter.ViewSource(Fragments.ContentPresenter.ViewSource.ViewType.New));


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
    }
}
