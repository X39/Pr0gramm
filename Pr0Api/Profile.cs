using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Pr0gramm.API.ProfileUtil;
using Pr0gramm.API.Util;

namespace Pr0gramm.API
{
    public class Profile
    {
        private Profile(OpenPr0gramm.GetProfileInfoResponse node)
        {
            this.UserID = (long)node.User.Id;
            this.Username = node.User.Name;
            this.Registered = node.User.RegisteredSince;
            this.Score = (long)node.User.Score;
            this.MarkObj = new Mark((int)node.User.Mark);
            this.IsAdmin = node.User.IsAdmin;
            this.IsBanned = node.User.IsBanned;
            this.CommentCount = (long)node.CommentCount;
            this.LikesArePublic = node.LikesArePublic;
            this.LikeCount = (long)node.LikeCount;
            this.TagCount = (long)node.TagCount;
            int followCount = node.FollowCount;
            this.Timestamp = node.TS;
            this.Following = node.IsFollowing;
            //this.Cache = node.Cache;
            this.Rt = (long)node.RT;
            this.Qc = (long)node.QC;
            this.BannedUntil = node.User.BannedUntil;

            this.Comments = new List<Message>();
            foreach (var it in node.Comments)
            {
                this.Comments.Add(new Message(it));
            }

            this.Uploads = new List<ImgLink>();
            foreach (var it in node.Uploads)
            {
                this.Uploads.Add(new ImgLink(it));
            }

            this.Badges = new List<Badge>();
            foreach (var it in node.Badges)
            {
                this.Badges.Add(new Badge(it));
            }
            
            this.Likes = new List<ImgLink>();
            foreach (var it in node.Likes)
            {
                this.Likes.Add(new ImgLink(it));
            }
        }

        public List<Badge> Badges { get; private set; }
        public DateTime? BannedUntil { get; private set; }
        public long CommentCount { get; private set; }
        public List<Message> Comments { get; private set; }
        public long FollowCount { get; private set; }
        public bool Following { get; private set; }
        public bool IsAdmin { get; private set; }
        public bool IsBanned { get; private set; }
        public long LikeCount { get; private set; }
        public List<ImgLink> Likes { get; private set; }
        public bool LikesArePublic { get; private set; }
        public Mark MarkObj { get; private set; }
        public long Qc { get; private set; }
        public DateTime Registered { get; private set; }
        public long Rt { get; private set; }
        public long Score { get; private set; }
        public long TagCount { get; private set; }
        public DateTime Timestamp { get; private set; }
        public List<ImgLink> Uploads { get; private set; }
        public long UserID { get; private set; }
        public string Username { get; private set; }

        public static async Task<Profile> Fetch(string profileName, ApiProvider apiProvider)
        {
            var response = await apiProvider.bridge.Client.Profile.GetInfo(profileName, OpenPr0gramm.ItemFlags.All);
            return new Profile(response);
        }
    }
}
