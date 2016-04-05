using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Pr0gramm.API;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234238

namespace Pr0gramm.UI.Fragments
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class ProfilePage : Page
    {
        public ProfilePage()
        {
            this.InitializeComponent();
        }

        public Profile RepresentedProfile { get; private set; }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            if (e.Parameter is API.Profile)
            {
                this.RepresentedProfile = (API.Profile)e.Parameter;
                this.Username.Text = this.RepresentedProfile.Username;
                this.BenisLabel.Text = this.RepresentedProfile.Score.ToString();
                this.UploadCountLabel.Text = this.RepresentedProfile.Uploads.Count.ToString();
                this.TagCountLabel.Text = this.RepresentedProfile.TagCount.ToString();
                this.CommentCountLabel.Text = this.RepresentedProfile.CommentCount.ToString();
                this.FavCountLabel.Text = this.RepresentedProfile.FollowCount.ToString();
                var tSpan = DateTime.Now.Subtract(this.RepresentedProfile.Registered);
                string totalString;
                if(tSpan.TotalDays < 30)
                {
                    totalString = string.Format("{0} Tage", ((int)tSpan.TotalDays));
                }
                else if(tSpan.TotalDays < 365)
                {
                    totalString = string.Format("{0} Monate", ((int)tSpan.TotalDays) / 30);
                }
                else
                {
                    totalString = string.Format("{0} Jahre", ((int)tSpan.TotalDays) / 365);
                }
                this.RegisteredLabel.Text = string.Format("Gewachsen seit {0}.{1}.{2} ({3})", this.RepresentedProfile.Registered.Day, this.RepresentedProfile.Registered.Month, this.RepresentedProfile.Registered.Year, totalString);
                this.RankLabel.Text = this.RepresentedProfile.MarkObj.ToString();
                this.RankCircle.Fill = this.RepresentedProfile.MarkObj.Color;
                if (app.Settings.Instance.Pr0User.Username != null && (!app.Settings.Instance.Pr0User.Paid || this.RepresentedProfile.Username == app.Settings.Instance.Pr0User.Username))
                    this.StelzButton.Visibility = Visibility.Collapsed;

                //ToDo: Navigate profile page frames
            }
            else
            {
                throw new Exception("Invalid Parameter provided");
            }
        }

    }
}
