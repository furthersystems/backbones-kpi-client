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
        public string CurrentCode = string.Empty;
        [SerializeField]
        public Vendor[] vendors = null;
        public Vendors()
        {
            CurrentCode = string.Empty;
            vendors = new Vendor[] { };
        }
    }

    public class VendorsInstance
    {
        private Vendors instance; 

        public VendorsInstance()
        {
            instance = new Vendors();
            Initialize();
        }

        private void Initialize()
        {
            if (Storage.Exists(Storage.Type.Vendors))
            {
                var json = Storage.Load(Storage.Type.Vendors);
                instance = JsonUtility.FromJson<Vendors>(json);
            }
        }

        private string ToJson()
        {
            return JsonUtility.ToJson(instance);
        }

        public bool SetCurrentKey(string vendorCode)
        {
            var notFound = true;
            foreach (var v in instance.vendors)
            {
                if (v.VendorCode == vendorCode)
                {
                    notFound = false;
                    break;
                }
            }
            if (notFound) return false;

            instance.CurrentCode = vendorCode;
            return true;
        }

        public void SetVendor(string vendorCode, Vendor vendor)
        {
            var index = GetIndex(vendorCode);
            if (index >= 0)
            {
                instance.CurrentCode = vendorCode;
                instance.vendors[index] = vendor;
            }
            else
            {
                instance.CurrentCode = vendorCode;
                instance.vendors = instance.vendors.Concat(new Vendor[] { vendor }).ToArray();
            }
            var jsoned = ToJson();
            Storage.Save(Storage.Type.Vendors, jsoned);
        }

        public void PargeVendor(string vendorCode)
        {
            if (instance.vendors.Length == 0)
            {
                return;
            }
            else if (instance.vendors.Length == 1)
            {
                instance = new Vendors();
            }
            else
            {
                var newVendors = new Vendor[instance.vendors.Length - 1];
                int newIndex = 0;
                for (int i = 0; i < instance.vendors.Length; i++)
                {
                    if (instance.vendors[i].VendorCode == vendorCode) continue;

                    newVendors[newIndex] = instance.vendors[i];
                    newIndex++;
                }
                instance.vendors = newVendors;
            }
            var jsoned = ToJson();
            Storage.Save(Storage.Type.Vendors, jsoned);
        }

        public Vendor GetVendor()
        {
            return GetVendor(instance.CurrentCode);
        }

        public Vendor GetVendor(string vendorCode)
        {
            var index = GetIndex(vendorCode);
            if (index >= 0)
            {
                return instance.vendors[index];
            }
            else
            {
                return new Vendor();
            }
        }

        public Vendor[] GetVendors()
        {
            return instance.vendors;
        }

        private int GetIndex(string vendorCode)
        {
            int targetIndex = -1;
            for (int index = 0; index < instance.vendors.Length; index++)
            {
                if (instance.vendors[index].VendorCode == vendorCode)
                {
                    targetIndex = index;
                    break;
                }
            }
            return targetIndex;
        }
    }
}