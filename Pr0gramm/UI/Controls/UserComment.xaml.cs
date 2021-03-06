﻿using Pr0gramm.API;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

// The User Control item template is documented at http://go.microsoft.com/fwlink/?LinkId=234236

namespace Pr0gramm.UI.Controls
{
    public sealed partial class UserComment : UserControl
    {
        private ItemInfo.Comment CommentInfo;
        
        public UserComment(ItemInfo.Comment comment)
        {
            this.InitializeComponent();
            this.CommentInfo = comment;
            this.CommentElement.Text = comment.Content;
            this.AuthorButton.Text = comment.Author;
            foreach (var it in comment.Children)
            {
                this.ChildrensStack.Children.Add(new UserComment(it));
            }

            this.isOpElement.Visibility = comment.Author == comment.Owner.Owner.User ? Visibility.Visible : Visibility.Collapsed;
            this.AuthorRank.Fill = new API.ProfileUtil.Mark((int)comment.Mark).Color;
            this.Created.Text = comment.Created.ToString();
        }

        private void CommentElement_TextChanged(object sender, TextChangedEventArgs e)
        {

        }

        private async void AuthorButton_Click(object sender, RoutedEventArgs e)
        {
            if(sender is Pr0Button)
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
