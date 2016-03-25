using Windows.UI.Xaml.Controls;

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
        }

        private void clearToggleStates()
        {
            btnNew.Toggled = false;
            btnTop.Toggled = false;
        }

        private void btnNew_Click(object sender, Windows.UI.Xaml.RoutedEventArgs e)
        {
            clearToggleStates();
            btnNew.Toggled = true;
            this.ContentFrame.Navigate(typeof(Fragments.ContentPresenter), new Fragments.ContentPresenter.ViewSource(Fragments.ContentPresenter.ViewSource.ViewType.New));
        }

        private void btnTop_Click(object sender, Windows.UI.Xaml.RoutedEventArgs e)
        {
            clearToggleStates();
            btnTop.Toggled = true;
            this.ContentFrame.Navigate(typeof(Fragments.ContentPresenter), new Fragments.ContentPresenter.ViewSource(Fragments.ContentPresenter.ViewSource.ViewType.Top));
        }
    }
}
