//------------------------------------------------------------------------------
// <copyright file="Identifier.cs" company="FurtherSystem Co.,Ltd.">
// Copyright (C) 2020 FurtherSystem Co.,Ltd.
// This software is released under the MIT License.
// http://opensource.org/licenses/mit-license.php
// </copyright>
// <author>FurtherSystem Co.,Ltd.</author>
// <email>info@furthersystem.com</email>
// <summary>
// Identifier
// </summary>
//------------------------------------------------------------------------------
using System;
using System.Security.Cryptography;
using System.Text;
using UnityEngine;

namespace Com.FurtherSystems.vQL.Client
{
    public partial class Instance
    {
        public class Identifier
        {
            public Identifier()
            {
            }

            public string SessionId { private set; get; } = "";

            public void SetSessionId(string sessionId)
            {
                SessionId = sessionId;
            }

            public string SessionPrivate { private set; get; } = "";

            public void SetSessionPrivate(string sessionPrivate)
            {
                SessionPrivate = sessionPrivate;
            }

            public void SetPrivateCode(string priv)
            {
                Storage.Save(Storage.Type.PrivateCode, priv);
            }

            public string GetPrivateKey()
            {
                return Storage.Load(Storage.Type.PrivateCode);
            }

            public (string, long) GetSeed()
            {
                var saveSeed = Storage.Load(Storage.Type.Seed);
                Debug.Log(saveSeed);
                var seed = saveSeed.Split(',')[0];
                long originTicks = 0;
                long.TryParse(saveSeed.Split(',')[1], out originTicks);
                return (seed, originTicks);
            }

            public (string, long) CreateSeed(string platform, long ticks)
            {
                Debug.Log(GetPlatformIdentifier() + " " + platform + " " + ticks.ToString() + "," + Storage.ServiceUniqueKey);
                string saveSeed = GenerateHash(GetPlatformIdentifier() + platform + ticks.ToString(), Storage.ServiceUniqueKey) + "," + ticks.ToString();
                Storage.Save(Storage.Type.Seed, saveSeed);

                var seed = saveSeed.Split(',')[0];
                long originTicks = 0;
                long.TryParse(saveSeed.Split(',')[1], out originTicks);
                return (seed, ticks);
            }

            public string AddNonce(string seed, long nonce)
            {
                Debug.Log(seed + " " + nonce.ToString() + "," + Storage.ServiceUniqueKey);
                return GenerateHash(seed + nonce.ToString(), Storage.ServiceUniqueKey);
            }

            public string GenerateHash(string plane, string key)
            {
                var encode = new UTF8Encoding();
                var planeBytes = encode.GetBytes(plane);
                var keyBytes = encode.GetBytes(key);
                var sha = new HMACSHA256(keyBytes);
                var hashBytes = sha.ComputeHash(planeBytes);
                return BitConverter.ToString(hashBytes).Replace("-", string.Empty).ToLower();
            }

            public string GetPlatformIdentifier()
            {
                return GenerateHash(SystemInfo.deviceUniqueIdentifier, Storage.ServiceUniqueKey);
            }
        }
    }
}