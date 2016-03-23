using Pr0gramm.API;
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
            this.AuthorElement.Text = comment.Author;
            foreach (var it in comment.Children)
            {
                this.ChildrensStack.Children.Add(new UserComment(it));
            }

            this.isOpElement.Visibility = comment.Author == comment.Owner.Owner.User ? Visibility.Visible : Visibility.Collapsed;
            this.AuthorRank.Fill = new User.Rank((int)comment.Mark).Color;
            this.Created.Text = comment.Created.ToString();
        }
    }
}
