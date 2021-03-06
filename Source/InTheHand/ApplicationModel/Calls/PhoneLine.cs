﻿// --------------------------------------------------------------------------------------------------------------------
// <copyright file="PhoneLine.cs" company="In The Hand Ltd">
//   Copyright (c) 2016-17 In The Hand Ltd, All rights reserved.
// </copyright>
// <summary>
//   Provides methods for launching the built-in phone call UI.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

using System;
using System.Reflection;
using System.Threading.Tasks;

#if __ANDROID__
using Android.App;
using Android.Content;
#elif WINDOWS_APP
using Windows.UI.Popups;
#elif WINDOWS_PHONE_APP
using Windows.ApplicationModel.Calls;
#elif WINDOWS_PHONE
using Microsoft.Phone.Tasks;
#endif

namespace InTheHand.ApplicationModel.Calls
{
    /// <summary>
    /// Provides methods for launching the built-in phone call UI.
    /// </summary>
    public sealed class PhoneLine
    {
#if WINDOWS_UWP
        private Windows.ApplicationModel.Calls.PhoneLine _line;

        private PhoneLine(Windows.ApplicationModel.Calls.PhoneLine line)
        {
            _line = line;
        }

        public static implicit operator Windows.ApplicationModel.Calls.PhoneLine(PhoneLine l)
        {
            return l._line;
        }

        public static implicit operator PhoneLine(Windows.ApplicationModel.Calls.PhoneLine l)
        {
            return new PhoneLine(l);
        }
#elif WINDOWS_APP || WINDOWS_PHONE_APP
        
        internal static Type _type10;

        static PhoneLine()
        {
            _type10 = Type.GetType("Windows.ApplicationModel.Calls.PhoneLine, Windows, ContentType=WindowsRuntime");
        }

        private object _line;
        private Type _type;

        internal PhoneLine(object line)
        {
            _line = line;
            _type = _line.GetType();
        }
#else
        internal PhoneLine()
        {
        }
#endif

        /// <summary>
        /// This static method asynchronously retrieves a PhoneLine object that represents a specific phone line on the device based on the line ID.
        /// </summary>
        /// <param name="lineId">The line ID of the phone line to retrieve.</param>
        /// <returns></returns>
        public static Task<PhoneLine> FromIdAsync(Guid lineId)
        {
#if __ANDROID__ || __IOS__
            return Task.FromResult<PhoneLine>(new PhoneLine());
#elif WINDOWS_UWP
            return Task.Run<PhoneLine>(async () =>
            {
                var l = await Windows.ApplicationModel.Calls.PhoneLine.FromIdAsync(lineId);
                return l == null ? null : l;
            });
#elif WINDOWS_APP || WINDOWS_PHONE_APP
            if(_type10 != null)
            {
                return Task.Run<PhoneLine>(async () =>
                {
                    return new PhoneLine(await ((Windows.Foundation.IAsyncOperation<object>)_type10.GetRuntimeMethod("FromIdAsync", new Type[] { typeof(Guid) }).Invoke(null, new object[] { lineId })));
                });
            }

            return null;
#else
            return null;
#endif
        }

        /// <summary>
        /// Place a phone call on the phone line.
        /// The caller must be in the foreground.
        /// </summary>
        /// <param name="number">The number to dial.</param>
        /// <param name="displayName">The display name of the party receiving the phone call.
        /// This parameter is optional.</param>
        public void Dial(string number, string displayName)
        {
#if __ANDROID__
            string action = Intent.ActionCall;
            Intent callIntent = new Intent(action, Android.Net.Uri.FromParts("tel", PhoneCallManager.CleanPhoneNumber(number), null));
            callIntent.AddFlags(ActivityFlags.ClearWhenTaskReset);
            Plugin.CurrentActivity.CrossCurrentActivity.Current.Activity.StartActivity(callIntent);
#elif __IOS__
            global::Foundation.NSUrl url = new global::Foundation.NSUrl("tel:" + PhoneCallManager.CleanPhoneNumber(number));
            UIKit.UIApplication.SharedApplication.OpenUrl(url);
#elif WINDOWS_UWP
            _line.Dial(number, displayName);
#endif
        }
    }
}