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

namespace Com.FurtherSystems.Backbones.KPI.Client
{
    public partial class Instance
    {
        public class Identifier
        {
            public enum ActivateType : byte
            {
                PhoneAuth = 0,
                EmailiAuth = 1,
            }
            public Identifier()
            {
            }

            public UInt16 AgreementVersion { private set; get; } = 0;

            public void SetAgreementVersion(UInt16 version)
            {
                AgreementVersion = version;
            }

            public bool CheckedAgreement { private set; get; } = false;

            public void SetCheckedAgreement(bool flag)
            {
                CheckedAgreement = flag;
            }
            
            public ActivateType CurrentActivateType { private set; get; } = 0;

            public void SetActivateType(ActivateType activate)
            {
                CurrentActivateType = activate;
            }

            public string ActivateKeyword { private set; get; } = string.Empty;

            public void SetActivateKeyword(string keyword)
            {
                ActivateKeyword = keyword;
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


            public string GenerateHash(string plane, string key)
            {
                var encode = new UTF8Encoding();
                var planeBytes = encode.GetBytes(plane);
                var keyBytes = encode.GetBytes(key);
                var sha = new HMACSHA256(keyBytes);
                var hashBytes = sha.ComputeHash(planeBytes);
                return BitConverter.ToString(hashBytes).Replace("-", string.Empty).ToLower();
            }


            public bool VendorInitializingFlow { get; set; } = false;
        }
    }
}