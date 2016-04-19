using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Pr0gramm.API.UserUtil
{
    public class Sync
    {
        private Sync(OpenPr0gramm.GetMessagesResponse<OpenPr0gramm.InboxItem> node)
        {
            this.InboxCount = (int)node.Messages.Count;
            this.Log = new List<int>();
            foreach(var it in node.Messages)
            {
                this.Log.Add((int)it.Id);
            }
            this.LastId = (int)node.Messages.Last().Id;
            this.Timestamp = node.TS;
            //this.Cache = node.Cache;
            this.Rt = (long)node.RT;
            this.Qc = (long)node.QC;
        }

        public int InboxCount { get; private set; }
        public int LastId { get; private set; }
        public List<int> Log { get; private set; }
        public long Qc { get; private set; }
        public long Rt { get; private set; }
        public DateTime Timestamp { get; private set; }

        public static async Task<Sync> Fetch(ApiProvider apiProvider, DateTime? lastSync = null)
        {
            var response = await apiProvider.bridge.Client.Inbox.GetAllMessages();
            return new Sync(response);
        }
    }
}
