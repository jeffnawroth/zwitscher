using System;
using System.Collections.Generic;
using System.Text;

/*namespace App3.Services
{
    public static class NotificationService
    {
        private const int NotificationId = 1001;

        public static void CreateNotification(Context context, string title, string message)
        {
            // Create an intent to launch when the notification is tapped
            var intent = new Intent(context, typeof(MainActivity));
            intent.AddFlags(ActivityFlags.ClearTop);
            var pendingIntent = PendingIntent.GetActivity(context, 0, intent, PendingIntentFlags.OneShot);

            // Create the notification
            var notificationBuilder = new NotificationCompat.Builder(context, "channel_id")
                .SetSmallIcon(Resource.Drawable.notification_icon)
                .SetLargeIcon(BitmapFactory.DecodeResource(context.Resources, Resource.Drawable.large_icon))
                .SetContentTitle(title)
                .SetContentText(message)
                .SetAutoCancel(true)
                .SetContentIntent(pendingIntent);

            // Get the notification manager
            var notificationManager = NotificationManagerCompat.From(context);

            // Display the notification
            notificationManager.Notify(NotificationId, notificationBuilder.Build());
        }
    }
} */
