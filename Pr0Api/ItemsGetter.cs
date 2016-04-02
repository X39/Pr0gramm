using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Pr0gramm.API.ItemsGetterUtil;
using Windows.Web.Http;

namespace Pr0gramm.API
{
    public class ItemsGetter
    {

        private ItemsGetter(asapJson.JsonNode node, ViewSource vs, ApiProvider apiProvider)
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
            this.Provider = apiProvider;
        }

        public bool AtEnd { get; private set; }
        public bool AtStart { get; private set; }
        public string Cache { get; private set; }
        public List<Image> Items { get; private set; }
        public long Qc { get; private set; }
        public long Rt { get; private set; }
        public DateTime Timestamp { get; private set; }
        public ViewSource View { get; private set; }
        public ApiProvider Provider { get; private set; }
        public HttpCookie Cookie { get; private set; }

        public async Task GetNewer(Action<List<Image>> callback)
        {
            if (this.AtStart)
            {
                callback.Invoke(new List<Image>());
                return;
            }
            string fetchUrl = this.Provider.Api + this.View.RequestPath + "&newer=" + this.Items.First().Id;
            var response = await Provider.Client.GetAsync(new Uri(fetchUrl));

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
            string fetchUrl = this.Provider.Api + this.View.RequestPath + "&older=" + this.Items.Last().Id;
            var response = await Provider.Client.GetAsync(new Uri(fetchUrl));

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

        public static async Task<ItemsGetter> Fetch(ApiProvider apiProvider, ViewSource vs)
        {
            string fetchUrl = apiProvider.Api + vs.RequestPath;
            var response = await apiProvider.Client.GetAsync(new Uri(fetchUrl));

            asapJson.JsonNode responseNode = new asapJson.JsonNode(await response.Content.ReadAsStringAsync(), true);
            response.Dispose();
            return new ItemsGetter(responseNode, vs, apiProvider);
        }
    }
}
