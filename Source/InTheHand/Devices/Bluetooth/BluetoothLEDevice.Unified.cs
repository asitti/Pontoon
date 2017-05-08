﻿//-----------------------------------------------------------------------
// <copyright file="BluetoothLEDevice.Unified.cs" company="In The Hand Ltd">
//   Copyright (c) 2015-17 In The Hand Ltd, All rights reserved.
//   This source code is licensed under the MIT License - see License.txt
// </copyright>
//-----------------------------------------------------------------------

using CoreBluetooth;
using Foundation;
using InTheHand.Devices.Bluetooth.GenericAttributeProfile;
using InTheHand.Devices.Enumeration;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace InTheHand.Devices.Bluetooth
{
    partial class BluetoothLEDevice
    {
        private CBPeripheral _peripheral;

        private BluetoothLEDevice(CBPeripheral peripheral)
        {
            _peripheral = peripheral;
            
        }

        public static implicit operator CBPeripheral(BluetoothLEDevice device)
        {
            return device._peripheral;
        }

        public static implicit operator BluetoothLEDevice(CBPeripheral peripheral)
        {
            return new BluetoothLEDevice(peripheral);
        }

        private void Manager_DisconnectedPeripheral(object sender, CBPeripheralErrorEventArgs e)
        {
            if (e.Peripheral == _peripheral)
            {
                _connectionStatusChanged?.Invoke(this, null);
            }
        }

        private void Manager_ConnectedPeripheral(object sender, CBPeripheralEventArgs e)
        {
            if (e.Peripheral == _peripheral)
            {
                _connectionStatusChanged?.Invoke(this, null);
            }
        }

        private static Task<BluetoothLEDevice> FromBluetoothAddressAsyncImpl(ulong bluetoothAddress)
        {
            return Task.FromResult<BluetoothLEDevice>(null);
        }

        private static async Task<BluetoothLEDevice> FromIdAsyncImpl(string deviceId)
        {
            var peripherals = DeviceInformation.Manager.RetrievePeripheralsWithIdentifiers(new NSUuid(deviceId));

            if (peripherals.Length > 0)
            {
                return new BluetoothLEDevice(peripherals[0]);
            }

            return null;
        }
    
        private static async Task<BluetoothLEDevice> FromDeviceInformationAsyncImpl(DeviceInformation deviceInformation)
        {
            return deviceInformation._peripheral;
        }

        private static string GetDeviceSelectorImpl()
        {
            return "btle";
        }

        private void NameChangedAdd()
        {
            _peripheral.UpdatedName += _peripheral_UpdatedName;
        }

        private void NameChangedRemove()
        {
            _peripheral.UpdatedName -= _peripheral_UpdatedName;
        }

        private void _peripheral_UpdatedName(object sender, EventArgs e)
        {
            _nameChanged?.Invoke(this, e);
        }

        private ulong GetBluetoothAddress()
        {
            return ulong.MaxValue;
        }

        private BluetoothConnectionStatus GetConnectionStatus()
        {
            return _peripheral.State == CBPeripheralState.Connected ? BluetoothConnectionStatus.Connected : BluetoothConnectionStatus.Disconnected;
        }

        private void ConnectionStatusChangedAdd()
        {
            DeviceInformation.Manager.ConnectedPeripheral += Manager_ConnectedPeripheral;
            DeviceInformation.Manager.DisconnectedPeripheral += Manager_DisconnectedPeripheral;
        }

        private void ConnectionStatusChangedRemove()
        {
            DeviceInformation.Manager.ConnectedPeripheral -= Manager_ConnectedPeripheral;
            DeviceInformation.Manager.DisconnectedPeripheral -= Manager_DisconnectedPeripheral;
        }

        private string GetDeviceId()
        {
            return _peripheral.Identifier.ToString();
        }

        private EventWaitHandle _servicesHandle = new EventWaitHandle(false, EventResetMode.AutoReset);
        private List<GattDeviceService> _services = new List<GattDeviceService>();

        private IReadOnlyList<GattDeviceService> GetGattServices()
        {
                if (_services.Count == 0)
                {
                    _peripheral.DiscoveredService += _peripheral_DiscoveredService;
                    var state = _peripheral.State;
                    if(state == CBPeripheralState.Disconnected)
                    {
                        InTheHand.Devices.Enumeration.DeviceInformation.Manager.ConnectPeripheral(_peripheral);
                        Thread.Sleep(1000);
                    }

                    if (_peripheral.State == CBPeripheralState.Connected)
                    {
                        _peripheral.DiscoverServices();
                        Task.Run(() =>
                        {
                            Task.Delay(6000);
                            _servicesHandle.Set();
                        });
                        _servicesHandle.WaitOne();
                        foreach (CBService service in _peripheral?.Services)
                        {
                            _services.Add(new GattDeviceService(service, _peripheral));
                        }
                    }
                }
                

                return _services.AsReadOnly();
        }
        
        private void _peripheral_DiscoveredService(object sender, NSErrorEventArgs e)
        {
            _servicesHandle.Set();
        }
    }
}