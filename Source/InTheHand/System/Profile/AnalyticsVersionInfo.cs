﻿//-----------------------------------------------------------------------
// <copyright file="AnalyticsVersionInfo.cs" company="In The Hand Ltd">
//     Copyright © 2016-17 In The Hand Ltd. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

using System.Reflection;

namespace InTheHand.System.Profile
{
    /// <summary>
    /// Provides version information about the device family.
    /// </summary>
    /// <remarks>
    /// <para/><list type="table">
    /// <listheader><term>Platform</term><description>Version supported</description></listheader>
    /// <item><term>Android</term><description>Android 4.4 and later</description></item>
    /// <item><term>iOS</term><description>iOS 9.0 and later</description></item>
    /// <item><term>macOS</term><description>OS X 10.7 and later</description></item>
    /// <item><term>tvOS</term><description>tvOS 9.0 and later</description></item>
    /// <item><term>watchOS</term><description>watchOS 2.0 and later</description></item>
    /// <item><term>Tizen</term><description>Tizen 3.0</description></item>
    /// <item><term>Windows UWP</term><description>Windows 10</description></item>
    /// <item><term>Windows Store</term><description>Windows 8.1 or later</description></item>
    /// <item><term>Windows Phone Store</term><description>Windows Phone 8.1 or later</description></item>
    /// <item><term>Windows Phone Silverlight</term><description>Windows Phone 8.0 or later</description></item>
    /// <item><term>Windows (Desktop Apps)</term><description>Windows 7 or later</description></item></list>
    /// </remarks>

    public sealed class AnalyticsVersionInfo
    {
        private object _native;

        internal AnalyticsVersionInfo(object native)
        {
            _native = native;
        }

        /// <summary>
        /// Gets a string that represents the type of device the application is running on.
        /// </summary>
        public string DeviceFamily
        {
            get
            {
                if(_native != null)
                {
                    return _native.GetType().GetRuntimeProperty("DeviceFamily").GetValue(_native).ToString();
                }
#if __ANDROID__
                // TODO: determine wearable / phone / tablet / tv
                return "Android";

#elif __IOS__
                switch(UIKit.UIDevice.CurrentDevice.UserInterfaceIdiom)
                {
                    case UIKit.UIUserInterfaceIdiom.Phone:
                        return "Apple.Phone";

                    case UIKit.UIUserInterfaceIdiom.Pad:
                        return "Apple.Tablet";

                    case UIKit.UIUserInterfaceIdiom.TV:
                        return "Apple.TV";

                    default:
                        return "Apple.Mobile";
                }

#elif __TVOS__
                return "Apple.TV";

#elif __WATCHOS__
                return "Apple.Watch";

#elif TIZEN
                // TODO: determine wearable / phone / tv
                return "Tizen";

#elif __MAC__
                return "Apple.Desktop";

#elif WINDOWS_UWP
                return Windows.System.Profile.AnalyticsInfo.VersionInfo.DeviceFamily;

#elif WINDOWS_APP || WIN32
                return "Windows.Desktop";

#elif WINDOWS_PHONE_APP || WINDOWS_PHONE
                return "Windows.Mobile";

#else
                return string.Empty;
#endif
            }
        }

        /// <summary>
        /// Gets the version within the device family.
        /// </summary>
        public string DeviceFamilyVersion
        {
            get
            {
                if (_native != null)
                {
                    return _native.GetType().GetRuntimeProperty("DeviceFamilyVersion").GetValue(_native).ToString();
                }

#if __ANDROID__ || __UNIFIED__ || WINDOWS_PHONE || WIN32
                return global::System.Environment.OSVersion.Version.ToString();
#elif WINDOWS_UWP
                return Windows.System.Profile.AnalyticsInfo.VersionInfo.DeviceFamilyVersion;
#else
                return string.Empty;
#endif
            }
        }
    }
}