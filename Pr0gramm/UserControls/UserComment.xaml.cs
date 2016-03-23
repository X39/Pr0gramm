using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Pr0gramm.pr0;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// The User Control item template is documented at http://go.microsoft.com/fwlink/?LinkId=234236

namespace Pr0gramm.UserControls
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
            this.AuthorRank.Fill = new pr0.User.Rank((int)comment.Mark).Color;
            this.Created.Text = comment.Created.ToString();
        }
    }
}
