using System;
using System.Collections.Generic;
using System.Linq;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;
using System.Net;
using asapJson;
using System.Threading.Tasks;
using Pr0gramm.API;


// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234238

namespace Pr0gramm.UI.Fragments
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class ContentPresenter : Page
    {
        public API.ItemsGetterUtil.ViewSource Source { get; set; }
        public ItemsGetter ApiContent { get; private set; }

        private List<API.ItemsGetterUtil.Image> images;
        public ContentPresenter()
        {
            this.InitializeComponent();
            images = new List<API.ItemsGetterUtil.Image>();
        }
        protected async override void OnNavigatedTo(NavigationEventArgs e)
        {
            if(e.Parameter is API.ItemsGetterUtil.ViewSource)
            {
                this.Source = (API.ItemsGetterUtil.ViewSource)e.Parameter;
                this.ApiContent = await API.ItemsGetter.Fetch(app.Settings.Instance.APIProvider, this.Source);
                
                UpdatePresentedContent();
            }
            else
            {
                throw new Exception("Invalid Parameter provided");
            }
        }
        public void UpdatePresentedContent()
        {
            bool fetchUp = this.images.Count > 0 && this.images.First().Created < ApiContent.Items.First().Created;
            var newItems = ApiContent.Items.Except(this.images);
            if (fetchUp)
                newItems = newItems.Reverse();
            foreach (var it in newItems)
            {
                this.images.Add(it);
                var img = new Image();
                img.Width = 128;
                img.Height = 128;
                
                var bi = new Windows.UI.Xaml.Media.Imaging.BitmapImage();
                bi.UriSource = new Uri(app.Settings.Instance.APIProvider.Thumb + it.Thumb, UriKind.Absolute);
                img.Source = bi;
                Button btn = new Button();

                btn.Style = (Style)Application.Current.Resources["imgButton"];
                btn.Content = img;
                btn.PointerEntered += (sender, e) =>
                {
                    Window.Current.CoreWindow.PointerCursor = new Windows.UI.Core.CoreCursor(Windows.UI.Core.CoreCursorType.Hand, 1);
                };
                btn.PointerExited += (sender, e) =>
                {
                    Window.Current.CoreWindow.PointerCursor = new Windows.UI.Core.CoreCursor(Windows.UI.Core.CoreCursorType.Arrow, 1);
                };
                btn.Click += (sender, e) =>
                {
                    this.Overlay.Navigate(typeof(ImageView), it);
                    this.Overlay.Visibility = Visibility.Visible;
                };
                if(fetchUp)
                    this.Presenter.Children.Insert(0, btn);
                else
                    this.Presenter.Children.Add(btn);
            }
        }

        private async void ScrollViewer_ViewChanged(object sender, ScrollViewerViewChangedEventArgs e)
        {
            ScrollViewer sv = sender as ScrollViewer;
            if(sv.VerticalOffset == 0)
            {
                //Fetch newer
                if ((await ApiContent.GetNewer(app.Settings.Instance.APIProvider)).Count > 0)
                    UpdatePresentedContent();
            }
            else if(sv.ScrollableHeight > 256 && sv.VerticalOffset >= sv.ScrollableHeight - 256)
            {
                //Fetch older
                if((await ApiContent.GetOlder(app.Settings.Instance.APIProvider)).Count > 0)
                    UpdatePresentedContent();
            }
        }
    }
}
