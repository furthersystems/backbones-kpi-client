//------------------------------------------------------------------------------
// <copyright file="Message.cs" company="FurtherSystem Co.,Ltd.">
// Copyright (C) 2020 FurtherSystem Co.,Ltd.
// This software is released under the MIT License.
// http://opensource.org/licenses/mit-license.php
// </copyright>
// <author>FurtherSystem Co.,Ltd.</author>
// <email>info@furthersystem.com</email>
// <summary>
// Message
// </summary>
//------------------------------------------------------------------------------
using System;

namespace Com.FurtherSystems.vQL.Client.Messages.Request
{
    [Serializable]
    public class Create
    {
        public Create()
        {

        }
        public byte IdentifierType;
        public string Identifier;
        public string Seed;
        public long Ticks;
    }

    [Serializable]
    public class Logon
    {
        public Logon()
        {

        }
        public string PrivateCode;
        public long Ticks;
    }

    [Serializable]
    public class Enqueue
    {
        public Enqueue()
        {

        }
        public string SessionId;
        public string VendorCode;
        public string QueueCode;
        public long Ticks;
    }

    [Serializable]
    public class VendorSetting
    {
        public VendorSetting()
        {

        }
        public string SessionId;
        public string Name;
        public string Caption;
        public long Ticks;
    }
}

namespace Com.FurtherSystems.vQL.Client.Messages.Response
{

    [Serializable]
    public class Create
    {
        public Create()
        {

        }
        public ResponseCode ResponseCode;
        public string PrivateCode;
        public string SessionId;
        public long Ticks;
    }

    [Serializable]
    public class Logon
    {
        public Logon()
        {

        }
        public ResponseCode ResponseCode;
        public string SessionId;
        public long Ticks;
    }

    [Serializable]
    public class Enqueue
    {
        public Enqueue()
        {

        }
        public string KeyCodePrefix;
        public string KeyCodeSuffix;
        public int PersonsWaitingBefore;
        public int TotalWaiting;
        public long Ticks;
    }

    [Serializable]
    public class VendorSetting
    {
        public VendorSetting()
        {

        }
        public ResponseCode ResponseCode;
        public string VendorCode;
        public long Ticks;
    }
}
