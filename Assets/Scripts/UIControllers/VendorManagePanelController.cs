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
        [SerializeField]
        Text vendorName;
        [SerializeField]
        Text queingTotalLabel;
        [SerializeField]
        Text TotalLabel;
        [SerializeField]
        Text queueCodeLabel;
        [SerializeField]
        Text rowHeader;
        [SerializeField]
        VendorManageRowController[] rows;
        [SerializeField]
        VendorQueueQRCode QrCode;
        bool breakInterval = false;

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
            content.SetActive(true);
            yield return ShowQueue();
        }

        public IEnumerator Dismiss()
        {
            breakInterval = true;
            content.SetActive(false);
            yield return null;
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

        public IEnumerator ShowQueue()
        {
            foreach (var row in rows)
            {
                yield return row.Initialize();
            }

            var vendorQueueCode = Storage.Load(Storage.Type.VendorQueueCode);
            var codeArray = vendorQueueCode.Split(',');
            var vendorCode = string.Empty;
            var queueCode = string.Empty;
            if (codeArray.Length > 0) vendorCode = codeArray[0];
            if (codeArray.Length > 1) queueCode = codeArray[1];

            if (string.IsNullOrEmpty(vendorCode) || string.IsNullOrEmpty(queueCode)) yield break;

            QrCode.Create(vendorQueueCode, 256, 256);
            var page = 0;
            if (string.IsNullOrEmpty(queueCode)) yield break;

            var nonce = Instance.WebAPIClient.GetTimestamp();
            var ticks = Instance.WebAPIClient.GetUnixTime();
            yield return StartCoroutine(Instance.WebAPI.VendorManage(queueCode, page, ticks, nonce));
            if (Instance.WebAPI.Result)
            {
                var data = Instance.WebAPI.DequeueResultData<Messages.Response.VendorManage>();
                if (data.ResponseCode == ResponseCode.ResponseOk)
                {
                    Debug.Log("data.ResponseCode: " + data.ResponseCode.ToString());
                    Debug.Log("data.Name: " + data.Name);
                    Debug.Log("data.QueingTotal: " + data.QueingTotal);
                    Debug.Log("data.Total: " + data.Total);
                    Debug.Log("data.Ticks:" + data.Ticks.ToString());
                    vendorName.text = data.Name;
                    queueCodeLabel.text = "行列コード: " + queueCode;
                    queingTotalLabel.text = "行列人数: " + data.QueingTotal.ToString();
                    TotalLabel.text = "総積算数: " + data.Total.ToString();

                    var index = 0;
                    foreach (var row in data.Rows)
                    {
                        if (row.Status == 1)
                        {
                            rows[index].SetRow(row.KeyCodePrefix, string.Empty);
                            index++;
                        }
                    }
                }
                else if (data.ResponseCode == ResponseCode.ResponseOkVendorRequireInitialize)
                {
                    // need initialize queue
                }
                else
                {

                }
            }
            else
            {
                // error
            }
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
            yield return panelSwitcher.Fade(PanelType.VendorUpdate);
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

        public void CallFadeAddDummy()
        {
            StartCoroutine(AddDummy());
        }

        IEnumerator AddDummy()
        {
            yield return panelSwitcher.PopLoadingDialog();
            // AddDummy
            yield return ShowQueue();
            yield return panelSwitcher.DepopLoadingDialog();
        }
    }
}