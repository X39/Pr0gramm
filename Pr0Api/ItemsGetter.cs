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

        private ItemsGetter(OpenPr0gramm.GetItemsResponse node, ViewSource vs, ApiProvider apiProvider)
        {
            this.View = vs;
            this.AtEnd = node.AtEnd;
            this.AtStart = node.AtStart;
            this.Items = new List<Image>();
            foreach(var it in node.Items)
            {
                this.Items.Add(new Image(it));
            }
            this.Timestamp = node.TS;
            this.Cache = node.Cache.ToString();
            this.Rt = (long)node.RT;
            this.Qc = (long)node.QC;
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

        public async Task<List<Image>> GetNewer()
        {
            if (this.AtStart)
            {
                return new List<Image>();
            }
            var response = await Provider.bridge.Client.Item.GetItemsNewer(OpenPr0gramm.ItemFlags.SFW, OpenPr0gramm.ItemStatus.New,false, null, null, null,false, (int)this.Items.First().Id);

            this.AtStart = response.AtStart;
            var newItems = new List<Image>();
            foreach (var it in response.Items)
            {
                newItems.Add(new Image(it));
            }
            this.Items.InsertRange(0, newItems);
            return newItems;
        }
        public async Task<List<Image>> GetOlder()
        {
            if (this.AtEnd)
            {
                return new List<Image>();
            }

            //OpenPr0gramm.ItemFlags.SFW, OpenPr0gramm.ItemStatus.New, false, null, false, (int)this.Items.Last().Id
            var response = await Provider.bridge.Client.Item.GetItemsOlder(OpenPr0gramm.ItemFlags.SFW, OpenPr0gramm.ItemStatus.New, false, null, null, null, false, (int)this.Items.Last().Id);

            this.AtEnd = response.AtEnd;
            var newItems = new List<Image>();
            foreach (var it in response.Items)
            {
                newItems.Add(new Image(it));
            }
            this.Items.AddRange(newItems);
            return newItems;
        }

        public static async Task<ItemsGetter> Fetch(ApiProvider apiProvider, ViewSource vs)
        {
            string fetchUrl = ApiProvider.Api + vs.RequestPath;
            var response = await apiProvider.bridge.Client.Item.GetItems(OpenPr0gramm.ItemFlags.SFW, OpenPr0gramm.ItemStatus.New);
            //var response = await apiProvider.Client.GetAsync(new Uri(fetchUrl));

            return new ItemsGetter(response, vs, apiProvider);
        }
    }
}
