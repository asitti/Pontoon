// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Geocircle.cs" company="In The Hand Ltd">
//   Copyright (c) 2015-17 In The Hand Ltd, All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

using System;

namespace InTheHand.Devices.Geolocation
{
    /// <summary>
    /// Interface to define a geographic shape.
    /// </summary>
    /// <remarks>
    /// <list type="table">
    /// <listheader><term>Platform</term><description>Version supported</description></listheader>
    /// <item><term>iOS</term><description>iOS 9.0 and later</description></item>
    /// <item><term>macOS</term><description>OS X 10.7 and later</description></item>
    /// <item><term>Tizen</term><description>Tizen 3.0</description></item>
    /// <item><term>Windows UWP</term><description>Windows 10</description></item>
    /// <item><term>Windows Store</term><description>Windows 8.1 or later</description></item>
    /// <item><term>Windows Phone Store</term><description>Windows Phone 8.1 or later</description></item>
    /// <item><term>Windows Phone Silverlight</term><description>Windows Phone 8.0 or later</description></item></list></remarks>
    public interface IGeoshape
    {
        /// <summary>
        /// The type of geographic shape.
        /// </summary>
        GeoshapeType GeoshapeType { get; }
    }

    /// <summary>
    /// Indicates the shape of a geographic region.
    /// </summary>
    public enum GeoshapeType
    {
        /// <summary>
        /// The geographic region is a circle with a center point and a radius.
        /// </summary>
        Geocircle = 1,
    }

    /// <summary>
    /// Describes a geographic circle with a center point and a radius.
    /// </summary>
    public sealed class Geocircle : IGeoshape
    {
#if WINDOWS_UWP || WINDOWS_APP || WINDOWS_PHONE_APP || WINDOWS_PHONE
        private Windows.Devices.Geolocation.Geocircle _circle;

        private Geocircle(Windows.Devices.Geolocation.Geocircle circle)
        {
            _circle = circle;
        }

        public static implicit operator Windows.Devices.Geolocation.Geocircle(Geocircle gc)
        {
            return gc._circle;
        }

        public static implicit operator Geocircle(Windows.Devices.Geolocation.Geocircle gc)
        {
            return new Geolocation.Geocircle(gc);
        }
#elif __UNIFIED__
        internal CoreLocation.CLRegion _region;

        private Geocircle(CoreLocation.CLRegion region)
        {
            _region = region;
        }

        public static implicit operator CoreLocation.CLRegion(Geocircle c)
        {
            return c._region;
        }

        public static implicit operator Geocircle(CoreLocation.CLRegion region)
        {
            return new Geocircle(region);
        }

        internal string Id
        {
            get
            {
                return _region.Identifier;
            }
        }
#elif TIZEN
        private Tizen.Location.CircleBoundary _boundary;

        private Geocircle(Tizen.Location.CircleBoundary boundary)
        {
            _boundary = boundary;
        }

        public static implicit operator Tizen.Location.CircleBoundary(Geocircle c)
        {
            return c._boundary;
        }

        public static implicit operator Geocircle(Tizen.Location.CircleBoundary b)
        {
            return new Geocircle(b);
        }

#else
        private BasicGeoposition _position;
        private double _radius;
#endif
        /// <summary>
        /// Create a geographic circle object for the given position and radius.
        /// </summary>
        /// <param name="position"></param>
        /// <param name="radius"></param>
        public Geocircle(BasicGeoposition position, double radius)
        {
#if WINDOWS_UWP || WINDOWS_APP || WINDOWS_PHONE_APP || WINDOWS_PHONE
            _circle = new Windows.Devices.Geolocation.Geocircle(position, radius);

#elif __UNIFIED__
            _region = new CoreLocation.CLRegion(position, radius, Guid.NewGuid().ToString());

#elif TIZEN
            _boundary = new Tizen.Location.CircleBoundary(position, radius);
#else
            _position = position;
            _radius = radius;
#endif
        }

        /// <summary>
        /// The center point of a geographic circle.
        /// </summary>
        public BasicGeoposition Center
        {
            get
            {
#if WINDOWS_UWP || WINDOWS_APP || WINDOWS_PHONE_APP || WINDOWS_PHONE
                return _circle.Center;

#elif __UNIFIED__
                return _region.Center;

#elif TIZEN
                return _boundary.Center;

#else
                return _position;
#endif
            }
        }

        /// <summary>
        /// The type of geographic shape.
        /// </summary>
        public GeoshapeType GeoshapeType
        {
            get
            {
#if WINDOWS_UWP || WINDOWS_APP || WINDOWS_PHONE_APP || WINDOWS_PHONE
                return (GeoshapeType)((int)_circle.GeoshapeType);
#else
                return GeoshapeType.Geocircle;
#endif
            }
        }

        /// <summary>
        /// The radius of a geographic circle in meters.
        /// </summary>
        public double Radius
        {
            get
            {
#if WINDOWS_UWP || WINDOWS_APP || WINDOWS_PHONE_APP || WINDOWS_PHONE
                return _circle.Radius;

#elif __UNIFIED__
                return _region.Radius;

#elif TIZEN
                return _boundary.Radius;

#else
                return _radius;
#endif
            }
        }

    }
}