using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;

namespace Pr0gramm.API.UserUtil
{
    public class Sync
    {
        private Sync(asapJson.JsonNode node)
        {
            this.InboxCount = (int)node.getValue_Object()["inboxCount"].getValue_Number();
            this.Log = new List<int>();
            foreach(var it in node.getValue_Object()["log"].getValue_Array())
            {
                this.Log.Add((int)it.getValue_Number());
            }
            this.LastId = (int)node.getValue_Object()["lastId"].getValue_Number();
            this.Timestamp = ApiProvider.UnixTimestamp0.AddSeconds(node.getValue_Object()["ts"].getValue_Number());
            //this.Cache = node.getValue_Object()["cache"];
            this.Rt = (long)node.getValue_Object()["rt"].getValue_Number();
            this.Qc = (long)node.getValue_Object()["qc"].getValue_Number();
        }

        public int InboxCount { get; private set; }
        public int LastId { get; private set; }
        public List<int> Log { get; private set; }
        public long Qc { get; private set; }
        public long Rt { get; private set; }
        public DateTime Timestamp { get; private set; }

        public static async Task<Sync> Fetch(ApiProvider apiProvider, DateTime? lastSync = null)
        {
            string url = apiProvider.Api;
            url += "user/sync?lastId=" + (lastSync.HasValue ? lastSync.Value.Subtract(ApiProvider.UnixTimestamp0).TotalSeconds : 0);
            var response = await apiProvider.Client.GetAsync(new Uri(url));
            asapJson.JsonNode responseNode = new asapJson.JsonNode(await response.Content.ReadAsStringAsync(), true);
            response.Dispose();
            return new Sync(responseNode);
        }
    }
}
