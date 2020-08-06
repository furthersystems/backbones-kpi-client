//------------------------------------------------------------------------------
// <copyright file="Storage.cs" company="FurtherSystem Co.,Ltd.">
// Copyright (C) 2020 FurtherSystem Co.,Ltd.
// This software is released under the MIT License.
// http://opensource.org/licenses/mit-license.php
// </copyright>
// <author>FurtherSystem Co.,Ltd.</author>
// <email>info@furthersystem.com</email>
// <summary>
// Storage
// </summary>
//------------------------------------------------------------------------------
using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

namespace Com.FurtherSystems.vQL.Client
{
    public static class Storage
    {
        public enum Type
        {
            Seed,
            PrivateCode,
            SSOs,
            Vendors,
            VendorQueueCode,
            Latest,
            Errors,
        }
        public const string ServiceUniqueKey = "KIWIKIWIKIWIKIWIKIWIKIWIKIWIKIWI";
        private static readonly Dictionary<Type, string> storageTypes = new Dictionary<Type, string>()
        {
            { Type.Seed, "seed" },
            { Type.PrivateCode, "priv" },
            { Type.SSOs, "ssos" },
            { Type.Vendors, "vendors" },
            { Type.VendorQueueCode, "vendor" },
            { Type.Latest, "latest" },
            { Type.Errors, "errors" },

        };
        private static string path = Application.persistentDataPath;

        public static string Load(Type type)
        {
            Debug.Log(Application.persistentDataPath);
            var str = string.Empty;
            try
            {
                using (var reader = new StreamReader(Path.Combine(path, storageTypes[type])))
                {
                    str = reader.ReadToEnd();
                }
            }
            catch (Exception e)
            {
                Debug.Log("error: " + e.Message);
            }
            return str;
        }

        public static void Save(Type type, string data)
        {
            Debug.Log(Application.persistentDataPath);
            using (var writer = new StreamWriter(Path.Combine(path, storageTypes[type])))
            {
                writer.Write(data);
                writer.Flush();
            }
        }

        public static bool Exists(Type type)
        {
            return File.Exists(Path.Combine(path, storageTypes[type]));
        }
    }
}