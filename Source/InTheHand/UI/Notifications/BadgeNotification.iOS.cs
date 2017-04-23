﻿//-----------------------------------------------------------------------
// <copyright file="BadgeNotification.iOS.cs" company="In The Hand Ltd">
//     Copyright © 2014-17 In The Hand Ltd. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

using UserNotifications;

namespace InTheHand.UI.Notifications
{
    public sealed partial class BadgeNotification
    {
        internal UNMutableNotificationContent _content;

        internal BadgeNotification(UNMutableNotificationContent content)
        {
            _content = content;
        }

        public BadgeNotification(uint value)
        {
            _content = new UNMutableNotificationContent();
            _content.Badge = value;
        }
    }
}