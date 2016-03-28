using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;

namespace Pr0gramm.API.ItemsGetterUtil
{
    public class ViewSource
    {
        public readonly ViewType viewType;
        public readonly string data;
        public readonly int filterMode;

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
            this.data = "";
            this.filterMode = 0;
        }
        /// <summary>
        /// Creates new ViewSource with given parameters
        /// </summary>
        /// <param name="vt">ViewType repesented by this ViewSource</param>
        /// <param name="data">If ViewType.New or ViewType.Top then either empty or the search param else the user represented by this ViewSource</param>
        /// <param name="mode">FilterMode represented by this ViewSource</param>
        public ViewSource(ViewType vt, string data, FilterMode mode = FilterMode.sfw)
        {
            if ((vt == ViewType.UserFavorites || vt == ViewType.UserImages))
            {
                if (string.IsNullOrWhiteSpace(data))
                    throw new ArgumentException("UserX ViewTypes require a username in data param");
            }
            this.viewType = vt;
            this.data = data;
            this.filterMode = (int)mode;
        }
        /// <summary>
        /// Creates new ViewSource with given parameters
        /// </summary>
        /// <param name="vt">ViewType repesented by this ViewSource</param>
        /// <param name="data">If ViewType.New or ViewType.Top then either empty or the search param else the user represented by this ViewSource</param>
        /// <param name="mode">FilterMode represented by this ViewSource, range from 0 to 7</param>
        public ViewSource(ViewType vt, string data, int mode = 0)
        {
            if ((vt == ViewType.UserFavorites || vt == ViewType.UserImages))
            {
                if (string.IsNullOrWhiteSpace(data))
                    throw new ArgumentException("UserX ViewTypes require a username in data param");
                if (mode < 0 || mode > 7)
                    throw new ArgumentException("mode has to be in range 0 - 7");
            }
            this.viewType = vt;
            this.data = data;
            this.filterMode = mode;
        }

        public string RequestPath
        {
            get
            {
                switch (this.viewType)
                {
                    case ViewType.New:
                        return "items/get?promoted=0" + "&flags=" + this.filterMode + (string.IsNullOrWhiteSpace(data) ? "" : "&tags=" + System.Net.WebUtility.UrlEncode(data.Replace(' ', '+')));
                    case ViewType.Top:
                        return "items/get?promoted=1" + "&flags=" + this.filterMode + (string.IsNullOrWhiteSpace(data) ? "" : "&tags=" + System.Net.WebUtility.UrlEncode(data.Replace(' ', '+')));
                    case ViewType.UserFavorites:
                        return "items/get?likes=" + WebUtility.UrlEncode(data) + "&flags=" + this.filterMode;
                    case ViewType.UserImages:
                        return "items/get?user=" + WebUtility.UrlEncode(data) + "&flags=" + this.filterMode;
                    default:
                        return "";
                }
            }
        }
    }
}