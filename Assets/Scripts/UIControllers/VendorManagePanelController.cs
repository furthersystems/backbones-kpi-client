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
        Text vendorInfo;
        [SerializeField]
        Text queueAccessCode;
        [SerializeField]
        Text resetButtonText;
        [SerializeField]
        Text rowHeader;
        [SerializeField]
        VendorManageRowController[] rows;

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

        public void CallFadeNewQueue()
        {
            StartCoroutine(NewQueue());
        }

        IEnumerator NewQueue()
        {
            yield return panelSwitcher.PopLoadingDialog();
            var nonce = Instance.WebAPIClient.GetTimestamp();
            var ticks = Instance.WebAPIClient.GetUnixTime();
            yield return StartCoroutine(Instance.WebAPI.NewQueue(true, ticks, nonce));
            if (Instance.WebAPI.Result)
            {
                var data = Instance.WebAPI.DequeueResultData<Messages.Response.NewQueue>();
                var vendorQueueCode = Storage.Load(Storage.Type.VendorQueueCode);
                var vendorQueue = vendorQueueCode.Split(',')[0];
                Storage.Save(Storage.Type.VendorQueueCode, vendorQueue + "," + data.QueueCode);
            }
            else
            {
                // error
            }
            yield return panelSwitcher.DepopLoadingDialog();
        }

        public void CallFadeAddDummy()
        {
            StartCoroutine(AddDummy());
        }

        IEnumerator AddDummy()
        {
            yield return panelSwitcher.PopLoadingDialog();
            yield return panelSwitcher.DepopLoadingDialog();
        }
    }
}