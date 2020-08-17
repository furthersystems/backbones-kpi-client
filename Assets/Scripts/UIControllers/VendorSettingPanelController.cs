//------------------------------------------------------------------------------
// <copyright file="VendorSettingPanelController.cs" company="FurtherSystem Co.,Ltd.">
// Copyright (C) 2020 FurtherSystem Co.,Ltd.
// This software is released under the MIT License.
// http://opensource.org/licenses/mit-license.php
// </copyright>
// <author>FurtherSystem Co.,Ltd.</author>
// <email>info@furthersystem.com</email>
// <summary>
// Vendor Setting Panel Controller
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
    public class VendorSettingPanelController : MonoBehaviour, PanelControllerInterface
    {
        [SerializeField]
        Text SetName;
        [SerializeField]
        Text SetCaption;
        [SerializeField]
        Toggle RequireQueueInit;
        [SerializeField]
        Toggle RequireAdmit;
        [SerializeField]
        GameObject content;

        PanelSwitcher panelSwitcher;

        public PanelType GetPanelType()
        {
            return PanelType.VendorUpdate;
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
            yield return null;
        }

        public IEnumerator Dismiss()
        {
            content.SetActive(false);
            yield return null;
        }

        public void CallSubmitVendor()
        {
            if (Instance.Ident.VendorInitializingFlow)
            {
                StartCoroutine(UpgradeVendor());
            }
            else
            {
                StartCoroutine(UpdateVendor());
            }
        }

        IEnumerator UpgradeVendor()
        {
            panelSwitcher.PopLoadingDialog();
            var nonce = Instance.WebAPIClient.GetTimestamp();
            var (seed, ticks) = Instance.Ident.GetSeed();
            var requireQueueInit = RequireQueueInit.isOn;
            var requireAdmit = RequireAdmit.isOn;
            yield return StartCoroutine(Instance.WebAPI.UpgradeVendor(SetName.text, SetCaption.text, requireQueueInit, requireAdmit, Instance.Ident.GetPlatformIdentifier(), ticks, nonce));
            if (Instance.WebAPI.Result)
            {
                var data = Instance.WebAPI.DequeueResultData<Messages.Response.VendorSetting>();
                Debug.Log("data.ResponseCode: " + data.ResponseCode.ToString());
                Debug.Log("data.VendorCode: " + data.VendorCode);
                Debug.Log("data.QueueCode: " + data.QueueCode);
                Debug.Log("data.Ticks:" + data.Ticks.ToString());
                Storage.Save(Storage.Type.VendorQueueCode, data.VendorCode + "," + data.QueueCode);

                Instance.Ident.VendorInitializingFlow = false;
                yield return panelSwitcher.Fade(PanelType.VendorManage);
                yield return panelSwitcher.DepopLoadingDialog();
            }
            else
            {
                yield return panelSwitcher.PopErrorDialog();
                yield return panelSwitcher.FadeBack();
                yield return panelSwitcher.DepopLoadingDialog();
            }
        }

        IEnumerator UpdateVendor()
        {
            panelSwitcher.PopLoadingDialog();
            var nonce = Instance.WebAPIClient.GetTimestamp();
            var (seed, ticks) = Instance.Ident.GetSeed();
            var requireInitQueue = RequireQueueInit.isOn;
            var requireAdmit = RequireAdmit.isOn;
            yield return StartCoroutine(Instance.WebAPI.UpdateVendor(SetName.text, SetCaption.text, requireInitQueue, requireAdmit, Instance.Ident.GetPlatformIdentifier(), ticks, nonce));
            if (Instance.WebAPI.Result)
            {
                if (requireInitQueue)
                {
                    var data = Instance.WebAPI.DequeueResultData<Messages.Response.VendorSetting>();
                    Debug.Log("data.ResponseCode: " + data.ResponseCode.ToString());
                    Debug.Log("data.QueueCode: " + data.QueueCode);
                    Debug.Log("data.Ticks:" + data.Ticks.ToString());
                    var vendorQueueCode = Storage.Load(Storage.Type.VendorQueueCode);
                    var codeArray = vendorQueueCode.Split(',');
                    var vendorCode = string.Empty;
                    if (codeArray.Length > 0) vendorCode = codeArray[0];
                    Storage.Save(Storage.Type.VendorQueueCode, vendorCode + "," + data.QueueCode);
                }

                yield return panelSwitcher.Fade(PanelType.VendorManage);
                yield return panelSwitcher.DepopLoadingDialog();
            }
            else
            {
                yield return panelSwitcher.PopErrorDialog();
                yield return panelSwitcher.FadeBack();
                yield return panelSwitcher.DepopLoadingDialog();
            }
        }

        public void CallFadeBack()
        {
            StartCoroutine(FadeBack());
        }

        IEnumerator FadeBack()
        {
            yield return panelSwitcher.PopLoadingDialog();
            Instance.Ident.VendorInitializingFlow = false;
            yield return panelSwitcher.FadeBack();
            yield return panelSwitcher.DepopLoadingDialog();
        }
    }
}