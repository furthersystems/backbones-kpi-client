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
        Text total;
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
            var nonce = Instance.WebAPIClient.GetTimestamp();
            var ticks = Instance.WebAPIClient.GetUnixTime();
            //var vendorCode = "RxLkzOB9/Jn6G7J1CPucotixrO7EZhuteI82DorvE0M=";
            var vendorCode = "6FLFuoN2FVuHlaYdIxPgBwlanSm7m3/0IPZOCqRZZRI=";
            var queueCode = "c2FtcGxlX3F1ZXVlX2NvZGU=";
            yield return StartCoroutine(Instance.WebAPI.Get(vendorCode, queueCode, ticks, nonce));
            if (Instance.WebAPI.Result)
            {
                var data = Instance.WebAPI.DequeueResultData<Messages.Response.Queue>();
                
                var v = Instance.Vendors.GetVendor(vendorCode);
                v.VendorCode = vendorCode;
                v.QueueCode = queueCode;
                v.PersonsWaitingBefore = data.PersonsWaitingBefore;
                v.TotalWaiting = data.TotalWaiting;
                Instance.Vendors.SetVendor(vendorCode, v);

                var vendor = Instance.Vendors.GetVendor();
                total.text = vendor.TotalWaiting + "人待ち";
                if (vendor.PersonsWaitingBefore > 0)
                {
                    beforePersonIcon1.SetActive(true);
                    beforePersonIcon2.SetActive(true);
                    beforePersons.text = "\nあなたの前に" + vendor.PersonsWaitingBefore + "人並んでいます。";
                    keyCodePrefix.text = vendor.KeyCodePrefix;
                    keyCodeSuffix.text = "";
                }
                else
                {
                    beforePersonIcon1.SetActive(false);
                    beforePersonIcon2.SetActive(false);
                    beforePersons.text = "\nあなたの順番が来ました。以下のコードを提示してください。";
                    keyCodePrefix.text = vendor.KeyCodePrefix;
                    keyCodeSuffix.text = vendor.KeyCodeSuffix;
                }
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