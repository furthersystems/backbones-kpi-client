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
        Text WaitInfo;
        [SerializeField]
        Text keyCodeSuffix;
        [SerializeField]
        GameObject beforePersonIcon1;
        [SerializeField]
        GameObject beforePersonIcon2;
        [SerializeField]
        GameObject beforePersonIcon3;
        [SerializeField]
        GameObject beforePersonIcon4;
        [SerializeField]
        GameObject beforePersonIcon5;
        [SerializeField]
        GameObject dotDot;
        [SerializeField]
        Text beforePersonText1;
        [SerializeField]
        Text beforePersonText2;
        [SerializeField]
        Text beforePersonText3;
        [SerializeField]
        Text beforePersonText4;
        [SerializeField]
        Text beforePersonText5;

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
            yield return StartCoroutine(Instance.WebAPI.GetQueue(currentVendor.VendorCode, currentVendor.QueueCode, ticks, nonce));
            if (Instance.WebAPI.Result)
            {
                var data = Instance.WebAPI.DequeueResultData<Messages.Response.Queue>();

                var v = Instance.Vendors.GetVendor(currentVendor.VendorCode);
                v.VendorCode = currentVendor.VendorCode;
                v.QueueCode = currentVendor.QueueCode;
                v.PersonsWaitingBefore = data.PersonsWaitingBefore;
                v.TotalWaiting = data.TotalWaiting;
                Instance.Vendors.SetVendor(currentVendor.VendorCode, v);

                VendorName.text = data.Name;

                var vendor = Instance.Vendors.GetVendor();
                //queueLength.text = "行列人数: " + vendor.TotalWaiting;
                //totalEnqueue.text = "総積算数: " + vendor.TotalWaiting;
                var keyCodePrefix = int.Parse(vendor.KeyCodePrefix);

                beforePersonText1.text = (vendor.PersonsWaitingBefore - 1).ToString();
                beforePersonText2.text = (vendor.PersonsWaitingBefore).ToString();
                beforePersonText3.text = (vendor.PersonsWaitingBefore + 1).ToString();
                beforePersonText4.text = (vendor.PersonsWaitingBefore + 2).ToString();
                beforePersonText5.text = (vendor.PersonsWaitingBefore + 3).ToString();

                beforePersonIcon4.SetActive(false);
                beforePersonIcon5.SetActive(false);
                dotDot.SetActive(false);

                Debug.Log("total waiting" + vendor.TotalWaiting.ToString());

                if (vendor.PersonsWaitingBefore == vendor.TotalWaiting + 2)
                {
                    beforePersonIcon4.SetActive(true);
                    beforePersonIcon5.SetActive(false);
                    dotDot.SetActive(false);
                }
                else if (vendor.PersonsWaitingBefore == vendor.TotalWaiting + 3)
                {
                    beforePersonIcon4.SetActive(true);
                    beforePersonIcon5.SetActive(true);
                    dotDot.SetActive(false);
                }
                else if (vendor.PersonsWaitingBefore < vendor.TotalWaiting + 3)
                {
                    beforePersonIcon4.SetActive(true);
                    beforePersonIcon5.SetActive(true);
                    dotDot.SetActive(true);
                }

                if (vendor.PersonsWaitingBefore > 1)
                {
                    beforePersonIcon1.SetActive(true);
                    beforePersonIcon2.SetActive(true);
                    beforePersons.text = $"\nあなたの順番は <size=30> {vendor.PersonsWaitingBefore + 1} </size> 番目です。";

                    WaitInfo.text = $"受付番号:{keyCodePrefix:0000} 総待ち人数:{vendor.TotalWaiting}";
                    keyCodeSuffix.text = "";
                }
                else if (vendor.PersonsWaitingBefore == 1)
                {
                    beforePersonIcon1.SetActive(false);
                    beforePersonIcon2.SetActive(true);
                    beforePersons.text = $"\nあなたの順番は <size=30> {vendor.PersonsWaitingBefore + 1} </size> 番目です。";
                    WaitInfo.text = $"受付番号:{keyCodePrefix:0000} 総待ち人数:{vendor.TotalWaiting}";
                    keyCodeSuffix.text = "";
                }
                else
                {
                    beforePersonIcon1.SetActive(false);
                    beforePersonIcon2.SetActive(false);
                    beforePersons.text = "順番が来ました。\n受付に入場用コードを提示してください。";
                    WaitInfo.text = $"受付番号:{keyCodePrefix:0000} 総待ち人数:{vendor.TotalWaiting}";
                    keyCodeSuffix.text = "入場用コード\n<size=120>"+vendor.KeyCodeSuffix.ToUpper()+"</size>";
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