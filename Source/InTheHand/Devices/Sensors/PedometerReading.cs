﻿// --------------------------------------------------------------------------------------------------------------------
// <copyright file="PedometerReading.cs" company="In The Hand Ltd">
//   Copyright (c) 2016 In The Hand Ltd, All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

using System;

namespace InTheHand.Devices.Sensors
{
    /// <summary>
    /// Represents a pedometer reading.
    /// </summary>
    /// <remarks>
    /// <para/><list type="table">
    /// <listheader><term>Platform</term><description>Version supported</description></listheader>
    /// <item><term>iOS</term><description>iOS 9.0 and later</description></item>
    /// <item><term>Tizen</term><description>Tizen 3.0</description></item>
    /// <item><term>Windows UWP</term><description>Windows 10</description></item></list>
    /// </remarks>
    public sealed class PedometerReading
    {
#if WINDOWS_UWP || WINDOWS_APP || WINDOWS_PHONE_APP
        private Windows.Devices.Sensors.PedometerReading _pedometerReading;

        private PedometerReading(Windows.Devices.Sensors.PedometerReading pedometerReading)
        {
            _pedometerReading = pedometerReading;
        }

        public static implicit operator Windows.Devices.Sensors.PedometerReading(PedometerReading pr)
        {
            return pr._pedometerReading;
        }

        public static implicit operator PedometerReading(Windows.Devices.Sensors.PedometerReading pr)
        {
            return new PedometerReading(pr);
        }
#elif __IOS__
        private CoreMotion.CMAccelerometerData _accelerometerData;

        private AccelerometerReading(CoreMotion.CMAccelerometerData accelerometerData)
        {
            _accelerometerData = accelerometerData;
        }

        public static implicit operator CoreMotion.CMAccelerometerData(AccelerometerReading ar)
        {
            return ar._accelerometerData;
        }

        public static implicit operator AccelerometerReading(CoreMotion.CMAccelerometerData ad)
        {
            return new AccelerometerReading(ad);
        }
#elif TIZEN
        private int _steps;
        private TimeSpan _duration;
        private PedometerStepKind _kind;
        private DateTimeOffset _timestamp;

        internal PedometerReading(int steps, TimeSpan duration, PedometerStepKind kind, DateTimeOffset timestamp)
        {
            _steps = steps;
            _duration = duration;
            _kind = kind;
            _timestamp = timestamp;
        }
#endif


        /// <summary>
        /// Gets the total number of steps taken for this pedometer reading.
        /// </summary>
        public int CumulativeSteps
        {
            get
            {
#if WINDOWS_UWP || WINDOWS_APP || WINDOWS_PHONE_APP
                return _pedometerReading.CumulativeSteps;
#elif __IOS__
                return _accelerometerData.Acceleration.X;
#elif TIZEN
                return _steps;
#else
                throw new PlatformNotSupportedException();
#endif
            }
        }

        /// <summary>
        /// Gets the amount of time that has elapsed for this pedometer reading.
        /// </summary>
        public TimeSpan CumulativeStepsDuration
        {
            get
            {
#if WINDOWS_UWP || WINDOWS_APP || WINDOWS_PHONE_APP
                return _pedometerReading.CumulativeStepsDuration;
#elif __IOS__
                return _accelerometerData.Acceleration.Y;
#elif TIZEN
                return _duration;
#else
                throw new PlatformNotSupportedException();
#endif
            }
        }

        /// <summary>
        /// Indicates the type of steps taken for this pedometer reading.
        /// </summary>
        public PedometerStepKind StepKind
        {
            get
            {
#if WINDOWS_UWP || WINDOWS_APP || WINDOWS_PHONE_APP
                return (PedometerStepKind)((int)_pedometerReading.StepKind);
#elif __IOS__
                                return _accelerometerData.Acceleration.Z;
#elif TIZEN
                return _kind;
#else
                                throw new PlatformNotSupportedException();
#endif
            }
        }

        /// <summary>
        /// Gets the time for the most recent pedometer reading.
        /// </summary>
        public DateTimeOffset Timestamp
        {
            get
            {
#if WINDOWS_UWP || WINDOWS_APP || WINDOWS_PHONE_APP
                return _pedometerReading.Timestamp;
#elif __IOS__
                //TODO: convert time since boot to actual timestamp
                return Accelerometer._timestampOffset.AddSeconds(_accelerometerData.Timestamp);
#elif TIZEN
                return _timestamp;
#else
                throw new PlatformNotSupportedException();
#endif
            }
        }
    }
}
