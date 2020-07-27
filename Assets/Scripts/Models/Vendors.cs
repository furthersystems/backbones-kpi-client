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
using System.Linq;
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

    [Serializable]
    public class Vendors
    {
        [SerializeField]
        private string currentCode = string.Empty;
        [SerializeField]
        private string[] codes = new string[] { };
        [SerializeField]
        private Vendor[] vendors = new Vendor[] { };

        public Vendors()
        {
            Initialize();
        }

        public void Initialize()
        {
            if (Storage.Exists(Storage.Type.Vendors))
            {
                var json = Storage.Load(Storage.Type.Vendors);
                FillFromJson(json);
            }
        }

        private string ToJson()
        {
            return JsonUtility.ToJson(this);
        }

        private void FillFromJson(string js)
        {
            var data = JsonUtility.FromJson<Vendors>(js);
            codes = data.codes;
            vendors = data.vendors;
        }

        public void SetVendor(string vendorCode, Vendor vendor)
        {
            currentCode = vendorCode;
            codes = codes.Concat(new string[] { vendorCode }).ToArray();
            vendors = vendors.Concat(new Vendor[] { vendor }).ToArray();
            var jsoned = ToJson();
            Storage.Save(Storage.Type.Vendors, jsoned);
        }

        public Vendor GetVendor()
        {
            return GetVendor(currentCode);
        }

        public Vendor GetVendor(string vendorCode)
        {
            var index = Array.IndexOf(codes, vendorCode);
            return vendors[index];
        }
    }
}