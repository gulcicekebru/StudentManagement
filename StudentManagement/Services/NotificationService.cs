using Microsoft.AspNetCore.Components;
using StudentManagement.Models;

namespace StudentManagement.Services
{
    public class NotificationService
    {
        //notification listesi tutulacak
        //listeye yeni notification eklenecek
        //liste değişince abonelere haber vermek

        private readonly List<Notification> _notificationList = new();

        //IReadOnlyList -> okunabilir, değiştirilemez bu listede add remove vs hiçbir işlem yapılmayacak
        //dışarıdan bu gibi işlemlere izin vermemek için içerideki private liste üzerinden işlem  yapıyoruz.

        public IReadOnlyList<Notification> NotificationList => _notificationList.AsReadOnly();


        public event Action? NotificationAction;

        public void AddNotification(Notification notification)
        {
            if(notification != null)
            {
                _notificationList.Add(notification);
                NotificationAction?.Invoke();
            }
            
        }


    }
}
