//------------------------------------------------------------------------------
// <copyright file="Vendor.cs" company="FurtherSystem Co.,Ltd.">
// Copyright (C) 2020 FurtherSystem Co.,Ltd.
// This software is released under the MIT License.
// http://opensource.org/licenses/mit-license.php
// </copyright>
// <author>FurtherSystem Co.,Ltd.</author>
// <email>info@furthersystem.com</email>
// <summary>
// Vendor
// </summary>
//------------------------------------------------------------------------------
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Com.FurtherSystems.vQL.Client
{
    [Serializable]
    public class Vendor
    {
        public Vendor()
        {

        }
        public string VendorCode;
        public string QueueCode;
        public string VendorName;
        public string VendorCaption;
        public string KeyCodePrefix;
        public string KeyCodeSuffix;
        public int PersonsWaitingBefore;
        public int TotalWaiting;
    }

    public class Vendors
    {
        private Dictionary<string, Vendor> vendors = new Dictionary<string, Vendor>();
        public Vendors()
        {
            Initialize();
        }

        public void Initialize()
        {
            if (Storage.Exists(Storage.Type.Vendors))
            {
                var json = Storage.Load(Storage.Type.Vendors);
                var data = JsonUtility.FromJson<Vendor[]>(json);
                foreach (var ele in data)
                {
                    vendors[ele.VendorCode] = ele;
                }
            }
        }

        public void SetVendor(string vendorCode, Vendor vendor)
        {
            vendors[vendorCode] = vendor;
            var array = new Vendor[vendors.Keys.Count];
            vendors.Values.CopyTo(array, 0);
            var jsoned = JsonUtility.ToJson(array);
            Storage.Save(Storage.Type.Vendors, jsoned);
        }

        public Vendor GetVendor(string vendorCode)
        {
            return vendors[vendorCode];
        }
    }
}