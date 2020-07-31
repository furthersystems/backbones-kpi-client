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

        public IEnumerator Show()
        {
            var nonce = Instance.WebAPIClient.GetTimestamp();
            var ticks = Instance.WebAPIClient.GetUnixTime();
            var vendorCode = "/tlqq/GzRXTe/wH9w26DZ7M6bYsC9cOW906EN59yG2s=";
            var queueCode = "c2FtcGxlX3F1ZXVlX2NvZGU=";
            yield return StartCoroutine(Instance.WebAPI.VendorManage(vendorCode, queueCode, ticks, nonce));
            if (Instance.WebAPI.Result)
            {
                var data = Instance.WebAPI.DequeueResultData<Messages.Response.Queue>();

                var v = Instance.Vendors.GetVendor(vendorCode);
                v.VendorCode = vendorCode;
                v.QueueCode = queueCode;
                v.PersonsWaitingBefore = data.PersonsWaitingBefore;
                v.TotalWaiting = data.TotalWaiting;
                Instance.Vendors.SetVendor(vendorCode, v);


            }
            else
            {
                // error
            }
            content.SetActive(true);
            yield return null;
        }

        public IEnumerator Dismiss()
        {
            content.SetActive(false);
            yield return null;
        }

        public void CallEnterRow()
        {
            StartCoroutine(EnterRow());
        }

        IEnumerator EnterRow()
        {
            yield return null;
        }

        public void CallNotifyRow()
        {
            StartCoroutine(NotifyRow());
        }

        IEnumerator NotifyRow()
        {
            yield return null;
        }

        public void CallPopSearchDialog()
        {
            StartCoroutine(PopSearchDialog());
        }

        IEnumerator PopSearchDialog()
        {
            yield return panelSwitcher.PopSearchDialog();
        }

        public void CallFadeVendorRegist()
        {
            StartCoroutine(FadeVendorRegist());
        }

        IEnumerator FadeVendorRegist()
        {
            yield return panelSwitcher.PopLoadingDialog();
            yield return panelSwitcher.Fade(PanelType.VendorSetting);
            yield return panelSwitcher.DepopLoadingDialog();
        }

        public void CallFadeVendorMain()
        {
            StartCoroutine(FadeVendorMain());
        }

        IEnumerator FadeVendorMain()
        {
            yield return panelSwitcher.PopLoadingDialog();
            yield return panelSwitcher.Fade(PanelType.VendorMain);
            yield return panelSwitcher.DepopLoadingDialog();
        }
    }
}