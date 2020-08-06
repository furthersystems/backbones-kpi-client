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
using UnityEngine.UI;

namespace Com.FurtherSystems.vQL.Client
{
    public class EnqueuePanelController : MonoBehaviour, PanelControllerInterface
    {
        [SerializeField]
        GameObject content;
        [SerializeField]
        Text qrCode;

        PanelSwitcher panelSwitcher;

        public PanelType GetPanelType()
        {
            return PanelType.Enqueue;
        }

        public void Initialize(PanelSwitcher switcher)
        {
            panelSwitcher = switcher;
            qrCode.text = Storage.Load(Storage.Type.VendorQueueCode);
        }

        public bool IsShowing()
        {
            return content.activeSelf;
        }

        public IEnumerator Show()
        {
            content.SetActive(true);
            yield return null;
        }


        public IEnumerator Dismiss()
        {
            content.SetActive(false);
            yield return null;
        }

        public void CallEnqueue()
        {
            StartCoroutine(Enqueue());
        }

        IEnumerator Enqueue()
        {
            yield return panelSwitcher.PopLoadingDialog();
            var nonce = Instance.WebAPIClient.GetTimestamp();
            var ticks = Instance.WebAPIClient.GetUnixTime();
            var vendorQueueCode = qrCode.text;
            var codeArray = vendorQueueCode.Split(',');
            var vendorCode = string.Empty;
            var queueCode = string.Empty;
            if (codeArray.Length > 0) vendorCode = codeArray[0];
            if (codeArray.Length > 1) queueCode = codeArray[1];
            yield return StartCoroutine(Instance.WebAPI.Enqueue(vendorCode, queueCode, ticks, nonce));
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

                yield return panelSwitcher.Fade(PanelType.Main);
                yield return panelSwitcher.DepopLoadingDialog();
            }
            else
            {
                yield return panelSwitcher.PopErrorDialog();
                yield return panelSwitcher.DepopLoadingDialog();
            }
        }

        public void CallFadeView()
        {
            StartCoroutine(FadeView());
        }

        IEnumerator FadeView()
        {
            yield return panelSwitcher.PopLoadingDialog();
            yield return panelSwitcher.Fade(PanelType.View);
            yield return panelSwitcher.DepopLoadingDialog();
        }
    }
}