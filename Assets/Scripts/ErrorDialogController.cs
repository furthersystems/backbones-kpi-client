//------------------------------------------------------------------------------
// <copyright file="PanelControllerDefault.cs" company="FurtherSystem Co.,Ltd.">
// Copyright (C) 2020 FurtherSystem Co.,Ltd.
// This software is released under the MIT License.
// http://opensource.org/licenses/mit-license.php
// </copyright>
// <author>FurtherSystem Co.,Ltd.</author>
// <email>info@furthersystem.com</email>
// <summary>
// QR Scan Camera Script
// </summary>
//------------------------------------------------------------------------------
using System;
using UnityEngine;

namespace Com.FurtherSystems.vQL.Client
{
    public class ErrorDialogController : MonoBehaviour, PanelControllerInterface
    {
        PanelSwitcher panelSwitcher;

        public string Message { get; set; }

        public string Debug { get; set; }

        public void Initialize(PanelSwitcher switcher)
        {
            panelSwitcher = switcher;
        }

        public bool IsShowing()
        {
            return gameObject.activeSelf;
        }

        public void Show()
        {
            gameObject.SetActive(true);
        }

        public void Dismiss()
        {
            gameObject.SetActive(false);
        }

        void Update()
        {
            if (IsShowing())
            {
                var touch = TouchInfo.Get(0);
                if (touch != TouchType.None) Dismiss();
            }
        }
    }
}