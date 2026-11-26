namespace StudentManagement.Models
{
    public class Notification
    {
        public bool IsVisible { get; set; }
        public int NotificationType { get; set; }
        public string NotificationText { get; set; } = string.Empty;

        //public List<Notification> NotificationList { get; set;} = new List<Notification>();

        public Notification(bool isVisible, int notificationType, string notificationText)
        {
            this.IsVisible = isVisible;
            this.NotificationType = notificationType;
            this.NotificationText = notificationText;
        }


        //public void AddNotification(Notification notification)
        //{
        //    if(notification != null)
        //    {
        //        NotificationList.Add(notification);
        //    }
        //    else
        //    {
        //        throw new Exception("Notification is empty");
        //    }
            
        //}

    }
}
