//------------------------------------------------------------------------------
// <copyright file="EnqueuePanelController.cs" company="FurtherSystem Co.,Ltd.">
// Copyright (C) 2020 FurtherSystem Co.,Ltd.
// This software is released under the MIT License.
// http://opensource.org/licenses/mit-license.php
// </copyright>
// <author>FurtherSystem Co.,Ltd.</author>
// <email>info@furthersystem.com</email>
// <summary>
// Enqueue Panel Controller
// </summary>
//------------------------------------------------------------------------------
using System.Collections;
using UnityEngine;

namespace Com.FurtherSystems.vQL.Client
{
    public class EnqueuePanelController : MonoBehaviour, PanelControllerInterface
    {
        [SerializeField]
        GameObject content;

        PanelSwitcher panelSwitcher;

        public PanelType GetPanelType()
        {
            return PanelType.Regist;
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

        public void CallEnqueue()
        {
            StartCoroutine(Enqueue());
        }

        IEnumerator Enqueue()
        {
            panelSwitcher.PopLoadingDialog();
            var nonce = Instance.WebAPIClient.GetTimestamp();
            var ticks = Instance.WebAPIClient.GetUnixTime();
            var vendorCode = "uboyz1bW5PSgTNIeC7ZkOBq4mkdvxpOefNklaNn88Fs=";
            var queueCode = "c2FtcGxlX3F1ZXVlX2NvZGU=";
            yield return StartCoroutine(Instance.WebAPI.Enqueue(Instance.Ident.SessionId, vendorCode, queueCode, ticks, nonce));
            if (Instance.WebAPI.Result)
            {
                var data = Instance.WebAPI.DequeueResultData<Messages.Response.Enqueue>();

                var vendor = new Vendor();
                vendor.VendorCode = vendorCode;
                vendor.QueueCode = queueCode;
                vendor.KeyCodePrefix = data.KeyCodePrefix;
                vendor.KeyCodeSuffix = data.KeyCodeSuffix;
                vendor.PersonsWaitingBefore = data.PersonsWaitingBefore;
                vendor.TotalWaiting = data.TotalWaiting;
                Instance.Vendors.SetVendor(vendorCode, vendor);

                panelSwitcher.Fade(PanelType.Main);
                panelSwitcher.DepopLoadingDialog();
            }
            else
            {
                panelSwitcher.PopErrorDialog();
                panelSwitcher.DepopLoadingDialog();
            }
        }

        public void CallFadeView()
        {
            StartCoroutine(FadeView());
        }

        IEnumerator FadeView()
        {
            yield return null;
            panelSwitcher.PopLoadingDialog();
            yield return null;
            panelSwitcher.Fade(PanelType.View);
            yield return null;
            panelSwitcher.DepopLoadingDialog();
            yield return null;
        }
    }
}