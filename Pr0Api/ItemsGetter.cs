using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using Pr0gramm.API.ItemsGetterUtil;
using Windows.Web.Http;

namespace Pr0gramm.API
{
    public class ItemsGetter
    {

        private ItemsGetter(asapJson.JsonNode node, ViewSource vs, UrlProvider url, Windows.Web.Http.HttpCookie cookie)
        {
            this.View = vs;
            this.AtEnd = node.getValue_Object()["atEnd"].getValue_Boolean();
            this.AtStart = node.getValue_Object()["atStart"].getValue_Boolean();
            this.Items = new List<Image>();
            foreach(var it in node.getValue_Object()["items"].getValue_Array())
            {
                this.Items.Add(new Image(it));
            }
            this.Timestamp = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc).AddSeconds(node.getValue_Object()["ts"].getValue_Number());
            this.Cache = node.getValue_Object()["cache"].getValue_String();
            this.Rt = (long)node.getValue_Object()["rt"].getValue_Number();
            this.Qc = (long)node.getValue_Object()["qc"].getValue_Number();
            this.Url = url;
            this.Cookie = cookie;
        }

        public bool AtEnd { get; private set; }
        public bool AtStart { get; private set; }
        public string Cache { get; private set; }
        public List<Image> Items { get; private set; }
        public long Qc { get; private set; }
        public long Rt { get; private set; }
        public DateTime Timestamp { get; private set; }
        public ViewSource View { get; private set; }
        public UrlProvider Url { get; private set; }
        public HttpCookie Cookie { get; private set; }

        public async Task GetNewer(Action<List<Image>> callback)
        {
            if (this.AtStart)
            {
                callback.Invoke(new List<Image>());
                return;
            }
            string fetchUrl = this.Url.Api + this.View.RequestPath + "&newer=" + this.Items.First().Id;
            var filter = new Windows.Web.Http.Filters.HttpBaseProtocolFilter();
            if (this.Cookie != null)
                filter.CookieManager.SetCookie(this.Cookie);
            Windows.Web.Http.HttpClient client = new Windows.Web.Http.HttpClient(filter);

            client.DefaultRequestHeaders.UserAgent.TryParseAdd(this.Url.UserAgent);
            var response = await client.GetAsync(new Uri(fetchUrl));

            asapJson.JsonNode responseNode = new asapJson.JsonNode(await response.Content.ReadAsStringAsync(), true);
            response.Dispose();
            this.AtStart = responseNode.getValue_Object()["atStart"].getValue_Boolean();
            var newItems = new List<Image>();
            foreach (var it in responseNode.getValue_Object()["items"].getValue_Array())
            {
                newItems.Add(new Image(it));
            }
            this.Items.InsertRange(0, newItems);
            callback.Invoke(newItems);
        }
        public async Task GetOlder(Action<List<Image>> callback)
        {
            if (this.AtStart)
            {
                callback.Invoke(new List<Image>());
                return;
            }
            string fetchUrl = this.Url.Api + this.View.RequestPath + "&older=" + this.Items.Last().Id;
            var filter = new Windows.Web.Http.Filters.HttpBaseProtocolFilter();
            if (this.Cookie != null)
                filter.CookieManager.SetCookie(this.Cookie);
            Windows.Web.Http.HttpClient client = new Windows.Web.Http.HttpClient(filter);

            client.DefaultRequestHeaders.UserAgent.TryParseAdd(this.Url.UserAgent);
            var response = await client.GetAsync(new Uri(fetchUrl));

            asapJson.JsonNode responseNode = new asapJson.JsonNode(await response.Content.ReadAsStringAsync(), true);
            response.Dispose();
            this.AtEnd = responseNode.getValue_Object()["atEnd"].getValue_Boolean();
            var newItems = new List<Image>();
            foreach (var it in responseNode.getValue_Object()["items"].getValue_Array())
            {
                newItems.Add(new Image(it));
            }
            this.Items.AddRange(newItems);
            callback.Invoke(newItems);
        }

        public static async Task<ItemsGetter> Fetch(UrlProvider url, ViewSource vs, Windows.Web.Http.HttpCookie cookie = null)
        {
            string fetchUrl = url.Api + vs.RequestPath;
            var filter = new Windows.Web.Http.Filters.HttpBaseProtocolFilter();
            if (cookie != null)
                filter.CookieManager.SetCookie(cookie);
            Windows.Web.Http.HttpClient client = new Windows.Web.Http.HttpClient(filter);

            client.DefaultRequestHeaders.UserAgent.TryParseAdd(url.UserAgent);
            var response = await client.GetAsync(new Uri(fetchUrl));

            asapJson.JsonNode responseNode = new asapJson.JsonNode(await response.Content.ReadAsStringAsync(), true);
            response.Dispose();
            return new ItemsGetter(responseNode, vs, url, cookie);
        }
    }
}
