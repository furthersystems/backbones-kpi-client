//------------------------------------------------------------------------------
// <copyright file="VendorManagePanelController.cs" company="FurtherSystem Co.,Ltd.">
// Copyright (C) 2020 FurtherSystem Co.,Ltd.
// This software is released under the MIT License.
// http://opensource.org/licenses/mit-license.php
// </copyright>
// <author>FurtherSystem Co.,Ltd.</author>
// <email>info@furthersystem.com</email>
// <summary>
// Vendor Manage Panel Controller
// </summary>
//------------------------------------------------------------------------------
using System;
using System.Text;
using System.Security.Cryptography;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using System.IO;

namespace Com.FurtherSystems.vQL.Client
{
    public class VendorManagePanelController : MonoBehaviour, PanelControllerInterface
    {
        [SerializeField]
        GameObject content;

        PanelSwitcher panelSwitcher;

        public PanelType GetPanelType()
        {
            return PanelType.VendorManage;
        }

        public void Initialize(PanelSwitcher switcher)
        {
            panelSwitcher = switcher;
        }

        public bool IsShowing()
        {
            return content.activeSelf;
        }

        public void Show()
        {
            content.SetActive(true);
        }

        public void Dismiss()
        {
            content.SetActive(false);
        }

        public void CallPopSearchDialog()
        {
            StartCoroutine(PopSearchDialog());
        }

        IEnumerator PopSearchDialog()
        {
            yield return null;
            panelSwitcher.PopSearchDialog();
        }

        public void CallFadeVendorRegist()
        {
            StartCoroutine(FadeVendorRegist());
        }

        IEnumerator FadeVendorRegist()
        {
            yield return null;
            panelSwitcher.PopLoadingDialog();
            yield return null;
            panelSwitcher.Fade(PanelType.VendorSetting);
            yield return null;
            panelSwitcher.DepopLoadingDialog();
            yield return null;
        }

        public void CallFadeVendorMain()
        {
            StartCoroutine(FadeVendorMain());
        }

        IEnumerator FadeVendorMain()
        {
            yield return null;
            panelSwitcher.PopLoadingDialog();
            yield return null;
            panelSwitcher.Fade(PanelType.VendorMain);
            yield return null;
            panelSwitcher.DepopLoadingDialog();
            yield return null;
        }
    }
}