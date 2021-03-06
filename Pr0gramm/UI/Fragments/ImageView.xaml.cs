﻿using System;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Navigation;
using Pr0gramm.UI.Controls;
// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234238

namespace Pr0gramm.UI.Fragments
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

        public API.ItemsGetterUtil.Image Source { get; private set; }
        public API.ItemInfo Info { get; private set; }

        protected async override void OnNavigatedTo(NavigationEventArgs e)
        {
            if (e.Parameter is API.ItemsGetterUtil.Image)
            {
                this.Source = (API.ItemsGetterUtil.Image)e.Parameter;
                this.Info = await API.ItemInfo.Fetch(this.Source, app.Settings.Instance.APIProvider);
                var bi = new Windows.UI.Xaml.Media.Imaging.BitmapImage();
                bi.UriSource = new Uri(app.Settings.Instance.APIProvider.Image + this.Source.ImagePath, UriKind.Absolute);
                this.CurrentImage.Source = bi;

                foreach(var it in this.Info.Tags)
                {
                    var tag = new Controls.Tag(it);
                    tag.Margin = new Thickness(5, 2, 5, 2);
                    this.TagList.Children.Add(tag);

                    double tagWidth = tag.ElementWidth + tag.Margin.Left + tag.Margin.Right;
                    VariableSizedWrapGrid.SetColumnSpan(tag, (int)tagWidth / 10 + 1);
                }

                this.LabelVotes.Text = (this.Source.Up - this.Source.Down).ToString();
                this.LabelVotesUp.Text = "up: " + this.Source.Up;
                this.LabelVotesDown.Text = "down: " + this.Source.Down;
                this.AuthorButton.Text = this.Source.User;
                this.AuthorRank.Fill = this.Source.UserMark.Color;


                foreach (var it in this.Info.Comments)
                {
                    this.CommentsStackPanel.Children.Add(new Controls.UserComment(it));
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

        private async void AuthorButton_Click(object sender, RoutedEventArgs e)
        {
            if (sender is Pr0Button)
            {
                Pr0Button btn = sender as Pr0Button;
                var profile = await API.Profile.Fetch(btn.Text, app.Settings.Instance.APIProvider);
                var mp = (Window.Current.Content as Frame).Content as Pages.MainPage;
                mp.clearToggleStates();
                mp.ContentFrame.Navigate(typeof(Fragments.ProfilePage), profile);
            }
        }
    }
}
