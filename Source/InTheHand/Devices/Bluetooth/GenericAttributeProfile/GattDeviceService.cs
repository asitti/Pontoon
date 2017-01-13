﻿//-----------------------------------------------------------------------
// <copyright file="GattDeviceService.cs" company="In The Hand Ltd">
//   32feet.NET - Personal Area Networking for .NET
//   This source code is licensed under the MIT License - see License.txt
// </copyright>
//-----------------------------------------------------------------------

using System;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Collections.ObjectModel;
#if __UNIFIED__
using CoreBluetooth;
#elif WINDOWS_UWP || WINDOWS_APP || WINDOWS_PHONE_APP
using Windows.Devices.Enumeration;
using Windows.Foundation;
#endif

namespace InTheHand.Devices.Bluetooth.GenericAttributeProfile
{
    /// <summary>
    ///
    /// </summary>
    public sealed class GattDeviceService
    {
#if WINDOWS_UWP || WINDOWS_APP || WINDOWS_PHONE_APP
        private Windows.Devices.Bluetooth.GenericAttributeProfile.GattDeviceService  _service;

        private GattDeviceService(Windows.Devices.Bluetooth.GenericAttributeProfile.GattDeviceService service)
        {
            _service = service;
        }

        public static implicit operator Windows.Devices.Bluetooth.GenericAttributeProfile.GattDeviceService(GattDeviceService service)
        {
            return service._service;
        }

        public static implicit operator GattDeviceService(Windows.Devices.Bluetooth.GenericAttributeProfile.GattDeviceService service)
        {
            return new GattDeviceService(service);
        }

#elif __UNIFIED__
        internal CBService _service;
      
        internal GattDeviceService(CBService service)
        {
            _service = service;
        }
#endif

        public static async Task<GattDeviceService> FromIdAsync(string deviceId)
        {
#if WINDOWS_UWP || WINDOWS_APP || WINDOWS_PHONE_APP
            return await Windows.Devices.Bluetooth.GenericAttributeProfile.GattDeviceService.FromIdAsync(deviceId).AsTask();
#else
            return null;
#endif
        }

        public static string GetDeviceSelectorFromShortId(ushort serviceShortId)
        {
#if __UNIFIED__
            return CBUUID.FromPartial(serviceShortId).ToString();
#elif WINDOWS_UWP || WINDOWS_APP || WINDOWS_PHONE_APP
            return Windows.Devices.Bluetooth.GenericAttributeProfile.GattDeviceService.GetDeviceSelectorFromShortId(serviceShortId);
#else
            return string.Empty;
#endif
        }

        public static string GetDeviceSelectorFromUuid(Guid serviceUuid)
        {
#if __UNIFIED__
            return CBUUID.FromBytes(serviceUuid.ToByteArray()).ToString();
#elif WINDOWS_UWP || WINDOWS_APP || WINDOWS_PHONE_APP
            return Windows.Devices.Bluetooth.GenericAttributeProfile.GattDeviceService.GetDeviceSelectorFromUuid(serviceUuid);
#else
            return string.Empty;
#endif
        }


        public IReadOnlyList<GattCharacteristic> GetAllCharacteristics()
        {
            List<GattCharacteristic> characteristics = new List<GattCharacteristic>();
#if __UNIFIED__
            foreach (CBCharacteristic characteristic in _service.Characteristics)
            {
                characteristics.Add(characteristic);
            }

            return characteristics.AsReadOnly();
#elif WINDOWS_UWP || WINDOWS_APP || WINDOWS_PHONE_APP
            foreach (Windows.Devices.Bluetooth.GenericAttributeProfile.GattCharacteristic characteristic in _service.GetAllCharacteristics())
            {
                characteristics.Add(characteristic);
            }

            return new ReadOnlyCollection<GattCharacteristic>(characteristics);
#else
            return new ReadOnlyCollection<GattCharacteristic>(new List<GattCharacteristic>());
#endif
        }

        public IReadOnlyList<GattCharacteristic> GetCharacteristics(Guid characteristicUuid)
        {
            List<GattCharacteristic> chars = new List<GenericAttributeProfile.GattCharacteristic>();
#if __UNIFIED__
            foreach (CBCharacteristic characteristic in _service.Characteristics)
            {
                if (characteristic.UUID.ToGuid() == characteristicUuid)
                {
                    chars.Add(characteristic);
                }
            }

#elif WINDOWS_UWP || WINDOWS_APP || WINDOWS_PHONE_APP
            foreach (Windows.Devices.Bluetooth.GenericAttributeProfile.GattCharacteristic characteristic in _service.GetCharacteristics(characteristicUuid))
            {
                chars.Add(characteristic);
            }
#endif
            return chars.AsReadOnly();
        }

        /// <summary>
        /// The GATT Service UUID associated with this GattDeviceService.
        /// </summary>
        public Guid Uuid
        {
            get
            {
#if __UNIFIED__
                return _service.UUID.ToGuid();
#elif WINDOWS_UWP || WINDOWS_APP || WINDOWS_PHONE_APP
                return _service.Uuid;
#else
                return Guid.Empty;
#endif
            }
        }
    }
}