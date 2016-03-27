using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Data.Xml.Dom;
using Windows.UI.Notifications;

namespace Pr0gramm.app
{
    class TextToast
    {
        private readonly string XAML = "<toast>" +
                         "<visual>" +
                           "<binding template =\"ToastGeneric\">" +
                             "<text>{0}</text>" +
                             "<text>{1}</text>" +
                           "</binding>" +
                         "</visual>" +
                       "</toast>";
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
