﻿//-----------------------------------------------------------------------
// <copyright file="FileOpenPicker.cs" company="In The Hand Ltd">
//     Copyright © 2016 In The Hand Ltd. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

using System;
using System.IO;

using System.Threading.Tasks;
using InTheHand.Storage.FileProperties;
using System.Collections.Generic;

namespace InTheHand.Storage.Pickers
{
    /// <summary>
    /// Represents a UI element that lets the user choose and open files. 
    /// </summary>
    /// <remarks>
    /// <para/><list type="table">
    /// <listheader><term>Platform</term><description>Version supported</description></listheader>
    /// <item><term>Windows UWP</term><description>Windows 10</description></item>
    /// <item><term>Windows Store</term><description>Windows 8.1 or later</description></item>
    /// <item><term>Windows (Desktop Apps)</term><description>Windows Vista or later</description></item></list>
    /// </remarks>
    public sealed partial class FileOpenPicker
    {
        /// <summary>
        /// Shows the file picker so that the user can pick one file.
        /// </summary>
        /// <returns>When the call to this method completes successfully, it returns a <see cref="StorageFile"/> object that represents the file that the user picked.</returns>
        public Task<StorageFile> PickSingleFileAsync()
        {
            return PickSingleFileAsyncImpl();
        }

        /// <summary>
        /// Gets the collection of file types that the file open picker displays.
        /// </summary>
        public IList<string> FileTypeFilter
        {
            get
            {
                return GetFileTypeFilter();
            }
        }
    }
}