using System;
using System.Collections.Generic;
using System.Linq;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;
using System.Net;
using asapJson;
using System.Threading.Tasks;


// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234238

namespace Pr0gramm.UI.Pages
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class ContentPresenter : Page
    {
        public class ViewSource
        {
            public readonly ViewType viewType;
            public readonly string username;
            public readonly int filterMode;
            public readonly bool isSelf;

            public enum ViewType
            {
                New,
                Top,
                UserImages,
                UserFavorites
            }
            public enum FilterMode
            {
                sfw = 1,
                nsfw,
                sfw_nsfw,
                nsfl,
                sfw_nsfl,
                nsfw_nsfl,
                sfw_nsfw_nsfl
            }

            public ViewSource(ViewType vt)
            {
                this.viewType = vt;
                this.username = "";
                this.filterMode = 0;
                this.isSelf = false;
            }
            public ViewSource(ViewType vt, string username, FilterMode mode = FilterMode.sfw, bool isSelf = false)
            {
                if ((vt == ViewType.UserFavorites || vt == ViewType.UserImages))
                {
                    if (string.IsNullOrWhiteSpace(username))
                        throw new ArgumentException("UserX ViewTypes require a username");
                }
                this.viewType = vt;
                this.username = username;
                this.filterMode = (int)mode;
                this.isSelf = isSelf;
            }
            public ViewSource(ViewType vt, string username, int mode = 0, bool isSelf = false)
            {
                if ((vt == ViewType.UserFavorites || vt == ViewType.UserImages))
                {
                    if (string.IsNullOrWhiteSpace(username))
                        throw new ArgumentException("UserX ViewTypes require a username");
                    if (mode < 0 || mode > 7)
                        throw new ArgumentException("mode has to be in range 0 - 7");
                }
                this.viewType = vt;
                this.username = username;
                this.filterMode = mode;
                this.isSelf = isSelf;
            }

            public string RequestPath
            {
                get
                {
                    switch (this.viewType)
                    {
                        case ViewType.New:
                            return "items/get?promoted=0" + "&flags=" + this.filterMode;
                        case ViewType.Top:
                            return "items/get?promoted=1" + "&flags=" + this.filterMode;
                        case ViewType.UserFavorites:
                            return "items/get?likes=" + this.username + "&flags=" + this.filterMode;
                        case ViewType.UserImages:
                            return "items/get?user=" + this.username + "&flags=" + this.filterMode;
                        default:
                            return "";
                    }
                }
            }
        }

        public enum FetchDirection
        {
            Newer,
            Older
        }

        public ViewSource Source { get; set; }
        private List<API.Image> images;
        private int newestItem;
        private int oldestItem;

        public ContentPresenter()
        {
            this.InitializeComponent();
            newestItem = -1;
            oldestItem = -1;
            images = new List<API.Image>();
        }
        protected async override void OnNavigatedTo(NavigationEventArgs e)
        {
            if(e.Parameter is ViewSource)
            {
                this.Source = (ViewSource)e.Parameter;
                await FetchItems(FetchDirection.Newer);
                UpdatePresentedContent();
            }
            else
            {
                throw new Exception("Invalid Parameter provided");
            }
        }
        public async Task FetchItems(FetchDirection fd)
        {
            string url = Settings.Pr0grammUrl.Api;
            url += this.Source.RequestPath;
            switch(fd)
            {
                case FetchDirection.Newer:
                    if (this.newestItem != -1)
                        url += @"&newer=" + this.newestItem;
                    break;
                case FetchDirection.Older:
                    url += @"&older=" + this.newestItem;
                    break;
            }
            WebRequest request = WebRequest.Create(url);
            request.Method = "GET";
            request.Credentials = CredentialCache.DefaultCredentials;
            request.Headers["User-Agent"] = Settings.UserAgent;
            ((HttpWebRequest)request).CookieContainer = Settings.Instance.Cookie;
            
            var response = await request.GetResponseAsync();

            JsonNode responseNode = new JsonNode(new System.IO.StreamReader(response.GetResponseStream()).ReadToEnd(), true);
            if(responseNode.getValue_Object()["error"].Type != JsonNode.EJType.Object)
            {
                throw new Exception(responseNode.getValue_Object()["error"].getValue_String());
            }
            var isAtEnd = responseNode.getValue_Object()["atEnd"];
            var isAtStart = responseNode.getValue_Object()["atStart"];
            var items = responseNode.getValue_Object()["items"].getValue_Array();

            //get first & last item IDs (or promoted IDs ... depending on if we are in top or not)
            var item = items.First().getValue_Object();
            if (this.Source.viewType == ViewSource.ViewType.Top)
                this.newestItem = (int)item["promoted"].getValue_Number();
            else
                this.newestItem = (int)item["id"].getValue_Number();
            item = items.Last().getValue_Object();
            if (this.Source.viewType == ViewSource.ViewType.Top)
                this.oldestItem = (int)item["promoted"].getValue_Number();
            else
                this.oldestItem = (int)item["id"].getValue_Number();
            List<API.Image> imgList = new List<API.Image>();
            foreach(var it in items)
            {
                imgList.Add(new API.Image(it));
            }
            if (fd == FetchDirection.Newer)
                this.images.InsertRange(0, imgList);
            else
                this.images.AddRange(imgList);
        }
        public void UpdatePresentedContent()
        {
            this.Presenter.Children.Clear();
            foreach(var it in this.images)
            {
                var img = new Image();
                img.Width = 128;
                img.Height = 128;
                
                var bi = new Windows.UI.Xaml.Media.Imaging.BitmapImage();
                bi.UriSource = new Uri(Settings.Pr0grammUrl.Thumb + it.Thumb, UriKind.Absolute);
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
                    this.Overlay.Navigate(typeof(Pages.ImageView), it);
                    this.Overlay.Visibility = Visibility.Visible;
                };
                this.Presenter.Children.Add(btn);
            }
        }
    }
}
