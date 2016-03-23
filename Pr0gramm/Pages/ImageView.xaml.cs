using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using System.Threading.Tasks;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234238

namespace Pr0gramm.Pages
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class ImageView : Page
    {

        public ImageView()
        {
            this.InitializeComponent();
        }

        public pr0.Image Source { get; private set; }
        public pr0.ItemInfo Info { get; private set; }

        protected async override void OnNavigatedTo(NavigationEventArgs e)
        {
            if (e.Parameter is pr0.Image)
            {
                this.Source = (pr0.Image)e.Parameter;
                this.Info = await pr0.ItemInfo.Fetch(this.Source);
                var bi = new Windows.UI.Xaml.Media.Imaging.BitmapImage();
                bi.UriSource = new Uri(Settings.Pr0grammUrl.Image + this.Source.ImagePath, UriKind.Absolute);
                this.CurrentImage.Source = bi;

                foreach(var it in this.Info.Tags)
                {
                    var tag = new UserControls.Tag(it);
                    tag.Margin = new Thickness(5, 2, 5, 2);
                    this.TagList.Children.Add(tag);

                    double tagWidth = tag.ElementWidth + tag.Margin.Left + tag.Margin.Right;
                    VariableSizedWrapGrid.SetColumnSpan(tag, (int)tagWidth / 10 + 1);
                }

                this.LabelVotes.Text = (this.Source.Up + this.Source.Down).ToString();
                this.LabelVotesUp.Text = "up: " + this.Source.Up;
                this.LabelVotesDown.Text = "down: " + this.Source.Down;


                foreach (var it in this.Info.Comments)
                {
                    this.CommentsStackPanel.Children.Add(new UserControls.UserComment(it));
                }
            }
            else
            {
                throw new Exception("Invalid Parameter provided");
            }
        }

        private void BackButton_Click(object sender, RoutedEventArgs e)
        {
            if(this.Parent is Frame)
            {
                Frame f = ((Frame)this.Parent);
                f.Content = null;
                
                f.Visibility = Visibility.Collapsed;
            }
        }

        private void MainGrid_PointerPressed(object sender, PointerRoutedEventArgs e)
        {
            if (this.Parent is Frame)
            {
                Frame f = ((Frame)this.Parent);
                f.Content = null;

                f.Visibility = Visibility.Collapsed;
                e.Handled = true;
            }
        }

        private void PointerPressed_EmptyHandled(object sender, PointerRoutedEventArgs e)
        {
            e.Handled = true;
        }
    }
}
