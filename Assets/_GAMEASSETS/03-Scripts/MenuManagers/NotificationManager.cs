using System;
using Unity.Notifications.Android;
using UnityEngine;

namespace LittleSubmarine2
{
    public class NotificationManager : MonoBehaviour
    {
        private void Start()
        {
            if (GameObject.FindGameObjectWithTag(GameTags.NOTIFICATIONMANAGER) == this.gameObject)
            {
                DontDestroyOnLoad(this.gameObject);
                Debug.Log("NotificationManager loaded");
            }
            else
            {
                Destroy(this.gameObject);
                Debug.Log("NotificationManager destroyed");
            }
                
            AndroidNotificationChannel channel = new AndroidNotificationChannel()
            {
                Id = "ls2_channel",
                Name = "Little Submarine 2 Channel",
                Importance = Importance.Default,
                Description = "Notifications from Little Submarine 2",
            };
            AndroidNotificationCenter.RegisterNotificationChannel(channel);
            
            ScheduleNotification("Test","a small test submarine!",2);
        }

        public void ScheduleNotification(string title, string description, int minutesFromNow, String largeIcon = "ls2_large_icon")
        {
            ScheduleNotification(title, description, System.DateTime.Now.AddMinutes(minutesFromNow), largeIcon);
        }

        public void ScheduleNotification(string title, string description, DateTime time, String largeIcon = "ls2_large_icon")
        {
            var notification = new AndroidNotification();
            notification.Title = title;
            notification.Text = description;
            notification.FireTime = time;
            notification.SmallIcon = "ls2_icon";
            notification.LargeIcon = "ls2_large_icon";
            
            AndroidNotificationCenter.SendNotification(notification, "ls2_channel");
        }
    }
}