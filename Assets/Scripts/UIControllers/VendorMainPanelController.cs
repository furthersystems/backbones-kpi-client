//------------------------------------------------------------------------------
// <copyright file="VendorMainPanelController.cs" company="FurtherSystem Co.,Ltd.">
// Copyright (C) 2020 FurtherSystem Co.,Ltd.
// This software is released under the MIT License.
// http://opensource.org/licenses/mit-license.php
// </copyright>
// <author>FurtherSystem Co.,Ltd.</author>
// <email>info@furthersystem.com</email>
// <summary>
// Vendor Main Panel Controller
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
    public class VendorMainPanelController : MonoBehaviour, PanelControllerInterface
    {
        [SerializeField]
        GameObject content;
        [SerializeField]
        VendorManageRowController[] rows;
        [SerializeField]
        VendorQueueQRCode QrCode;

        PanelSwitcher panelSwitcher;

        public PanelType GetPanelType()
        {
            return PanelType.VendorMain;
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
            foreach (var row in rows)
            {
                yield return row.Initialize();
            }
            var nonce = Instance.WebAPIClient.GetTimestamp();
            var ticks = Instance.WebAPIClient.GetUnixTime();

            var vendorQueueCode = Storage.Load(Storage.Type.VendorQueueCode);
            var codeArray = vendorQueueCode.Split(',');
            var vendorCode = string.Empty;
            var queueCode = string.Empty;
            if (codeArray.Length > 0) vendorCode = codeArray[0];
            if (codeArray.Length > 1) queueCode = codeArray[1];

            if (!string.IsNullOrEmpty(vendorCode) && !string.IsNullOrEmpty(queueCode))
            {
                QrCode.Create(vendorQueueCode, 256, 256);
            }

            yield return StartCoroutine(Instance.WebAPI.VendorManage(queueCode, ticks, nonce));
            if (Instance.WebAPI.Result)
            {
                var data = Instance.WebAPI.DequeueResultData<Messages.Response.VendorManage>();
                if (data.ResponseCode == ResponseCode.ResponseOk)
                {
                    for (int index = 0; index < 20; index++)
                    {
                        rows[index].SetRow(data.Rows[index].KeyCodePrefix, string.Empty);
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

        public IEnumerator Dismiss()
        {
            content.SetActive(false);
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

        public void CallFadeVendorManage()
        {
            StartCoroutine(FadeVendorManage());
        }

        IEnumerator FadeVendorManage()
        {
            yield return panelSwitcher.PopLoadingDialog();
            yield return panelSwitcher.Fade(PanelType.VendorManage);
            yield return panelSwitcher.DepopLoadingDialog();
        }
    }
}