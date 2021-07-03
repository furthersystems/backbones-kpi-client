//------------------------------------------------------------------------------
// <copyright file="Instance.cs" company="FurtherSystem Co.,Ltd.">
// Copyright (C) 2020 FurtherSystem Co.,Ltd.
// This software is released under the MIT License.
// http://opensource.org/licenses/mit-license.php
// </copyright>
// <author>FurtherSystem Co.,Ltd.</author>
// <email>info@furthersystem.com</email>
// <summary>
// Instance
// </summary>
//------------------------------------------------------------------------------
using System;

namespace Com.FurtherSystems.vQL.Client
{
    public partial class Instance
    {
        public static UInt16 ClientVersion = 1;
        public static UInt16 AgreementVersion = 1;
        public static WebAPIClient WebAPI = null;
        public static Identifier Ident = null;
        public static void Initialize()
        {
            WebAPI = new WebAPIClient();
            Ident = new Identifier();
        }
    }
}