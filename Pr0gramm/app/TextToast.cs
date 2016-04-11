using Windows.Data.Xml.Dom;
using Windows.UI.Notifications;

namespace Pr0gramm.app
{
    class TextToast
    {
        private readonly string XAML = (string)App.Current.Resources["toast_template"];
        public TextToast(string title, string msg)
        {
            var xmlDocument = new XmlDocument();
            xmlDocument.LoadXml(string.Format(XAML, title, msg));

            var toastNotification = new ToastNotification(xmlDocument);

            this.Notification = ToastNotificationManager.CreateToastNotifier();
            Notification.Show(toastNotification);
        }

        public ToastNotifier Notification { get; private set; }
    }
}
