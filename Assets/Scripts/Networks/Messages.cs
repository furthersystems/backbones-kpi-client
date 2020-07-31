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
    public class Base
    {
        public long Ticks;
    }

    [Serializable]
    public class Create : Base
    {
        public Create()
        {

        }
        public byte IdentifierType;
        public string Identifier;
        public string Seed;
    }

    [Serializable]
    public class Logon : Base
    {
        public Logon()
        {

        }
        public string PrivateCode;
    }

    [Serializable]
    public class Enqueue : Base
    {
        public Enqueue()
        {

        }
        public string VendorCode;
        public string QueueCode;
    }

    [Serializable]
    public class Queue : Base
    {
        public Queue()
        {

        }
    }

    [Serializable]
    public class VendorSetting : Base
    {
        public VendorSetting()
        {

        }
        public string Name;
        public string Caption;
    }
}

namespace Com.FurtherSystems.vQL.Client.Messages.Response
{
    [Serializable]
    public class Base
    {
        public ResponseCode ResponseCode;
        public long Ticks;
    }

    [Serializable]
    public class Create : Base
    {
        public Create()
        {

        }
        public string PrivateCode;
        public string SessionId;
        public string SessionPrivate;
    }

    [Serializable]
    public class Logon : Base
    {
        public Logon()
        {

        }
        public string SessionId;
        public string SessionPrivate;
    }

    [Serializable]
    public class Enqueue : Base
    {
        public Enqueue()
        {

        }
        public string KeyCodePrefix;
        public string KeyCodeSuffix;
        public int PersonsWaitingBefore;
        public int TotalWaiting;
    }

    [Serializable]
    public class Queue : Base
    {
        public Queue()
        {

        }
        public int PersonsWaitingBefore;
        public int TotalWaiting;
        public int Status;
    }

    [Serializable]
    public class VendorSetting : Base
    {
        public VendorSetting()
        {

        }
        public string VendorCode;
    }
}
