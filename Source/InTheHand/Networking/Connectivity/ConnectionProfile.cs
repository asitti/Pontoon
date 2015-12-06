﻿// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ConnectionProfile.cs" company="In The Hand Ltd">
//   Copyright (c) 2015 In The Hand Ltd, All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

using System;
#if __ANDROID__
using Android.Net;
#elif __IOS__
using SystemConfiguration;
#endif

namespace InTheHand.Networking.Connectivity
{
    public sealed class ConnectionProfile
    {
#if __ANDROID__
        private NetworkInfo _info;
        internal ConnectionProfile(NetworkInfo info)
        {
            _info = info;
        }
#elif __IOS__
        private NetworkReachabilityFlags _flags;

        internal ConnectionProfile(NetworkReachabilityFlags flags)
        {
            _flags = flags;
        }
#elif WINDOWS_UWP || WINDOWS_APP || WINDOWS_PHONE_APP || WINDOWS_PHONE
        private Windows.Networking.Connectivity.ConnectionProfile _profile;
        internal ConnectionProfile(Windows.Networking.Connectivity.ConnectionProfile profile)
        {
            _profile = profile;
        }
#endif

        public NetworkConnectivityLevel GetNetworkConnectivityLevel()
        {
#if __ANDROID__
            if(_info.IsConnected)
            {
                return NetworkConnectivityLevel.InternetAccess;
            }

            return NetworkConnectivityLevel.None;
#elif __IOS__
            switch(_flags)
            {
                case NetworkReachabilityFlags.Reachable:
                    return NetworkConnectivityLevel.InternetAccess;

                default:
                    return NetworkConnectivityLevel.None;
            }
#elif WINDOWS_UWP || WINDOWS_APP || WINDOWS_PHONE_APP || WINDOWS_PHONE
            return (NetworkConnectivityLevel)((int)_profile.GetNetworkConnectivityLevel());
#else
            return NetworkConnectivityLevel.None;
#endif
        }
    }
}