//------------------------------------------------------------------------------
// <copyright file="MainPanelController.cs" company="FurtherSystem Co.,Ltd.">
// Copyright (C) 2020 FurtherSystem Co.,Ltd.
// This software is released under the MIT License.
// http://opensource.org/licenses/mit-license.php
// </copyright>
// <author>FurtherSystem Co.,Ltd.</author>
// <email>info@furthersystem.com</email>
// <summary>
// Main Panel Controller
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
    public class MainPanelController : MonoBehaviour, PanelControllerInterface
    {
        [SerializeField]
        GameObject content;
        [SerializeField]
        Text VendorName;
        [SerializeField]
        Text totalEnqueue;
        [SerializeField]
        Text queueLength;
        [SerializeField]
        Text QRText;
        [SerializeField]
        Text beforePersons;
        [SerializeField]
        Text keyCodePrefix;
        [SerializeField]
        Text keyCodeSuffix;
        [SerializeField]
        GameObject beforePersonIcon1;
        [SerializeField]
        GameObject beforePersonIcon2;

        PanelSwitcher panelSwitcher;
        bool breakInterval = false;

        public PanelType GetPanelType()
        {
            return PanelType.Main;
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
            yield return ShowQueue();
            content.SetActive(true);
            yield return null;
            StartCoroutine(ShowQueueInterval());
        }

        public IEnumerator ShowQueueInterval()
        {
            breakInterval = false;
            while (true)
            {
                yield return new WaitForSeconds(3.1f);
                // modal on
                yield return ShowQueue();
                // modal off
                if (breakInterval) break;
            }
        }

        IEnumerator ShowQueue()
        {
            var nonce = Instance.WebAPIClient.GetTimestamp();
            var ticks = Instance.WebAPIClient.GetUnixTime();
            var currentVendor = Instance.Vendors.GetVendor();
            yield return StartCoroutine(Instance.WebAPI.Get(currentVendor.VendorCode, currentVendor.QueueCode, ticks, nonce));
            if (Instance.WebAPI.Result)
            {
                var data = Instance.WebAPI.DequeueResultData<Messages.Response.Queue>();

                var v = Instance.Vendors.GetVendor(currentVendor.VendorCode);
                v.VendorCode = currentVendor.VendorCode;
                v.QueueCode = currentVendor.QueueCode;
                v.PersonsWaitingBefore = data.PersonsWaitingBefore;
                v.TotalWaiting = data.TotalWaiting;
                Instance.Vendors.SetVendor(currentVendor.VendorCode, v);

                var vendor = Instance.Vendors.GetVendor();
                queueLength.text = "総入場数" + vendor.TotalWaiting + "\n総行列待ち" + vendor.TotalWaiting;
                //totalEnqueue.text = "総入場数" + vendor.TotalWaiting + "\n総行列待ち" + vendor.TotalWaiting;

                if (vendor.PersonsWaitingBefore > 1)
                {
                    beforePersonIcon1.SetActive(true);
                    beforePersonIcon2.SetActive(true);
                    beforePersons.text = "\nあなたの前に" + vendor.PersonsWaitingBefore + "人並んでいます。";
                    keyCodePrefix.text = "";
                    keyCodeSuffix.text = "";
                }
                else if (vendor.PersonsWaitingBefore == 1)
                {
                    beforePersonIcon2.SetActive(true);
                    beforePersons.text = "\nあなたの前に" + vendor.PersonsWaitingBefore + "人並んでいます。";
                    keyCodePrefix.text = "";
                    keyCodeSuffix.text = "";
                }
                else
                {
                    beforePersonIcon1.SetActive(false);
                    beforePersonIcon2.SetActive(false);
                    beforePersons.text = "順番が来ました。\n以下のコードを提示してください。";
                    keyCodePrefix.text = vendor.KeyCodePrefix;
                    keyCodeSuffix.text = vendor.KeyCodeSuffix;
                }
            }
            else
            {
                // error
            }
        }

        public IEnumerator Dismiss()
        {
            breakInterval = true;
            content.SetActive(false);
            yield return null;
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