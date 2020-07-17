//------------------------------------------------------------------------------
// <copyright file="VendorRegistPanelController.cs" company="FurtherSystem Co.,Ltd.">
// Copyright (C) 2020 FurtherSystem Co.,Ltd.
// This software is released under the MIT License.
// http://opensource.org/licenses/mit-license.php
// </copyright>
// <author>FurtherSystem Co.,Ltd.</author>
// <email>info@furthersystem.com</email>
// <summary>
// Vendor Regist Panel Controller
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
    public class VendorRegistPanelController : MonoBehaviour, PanelControllerInterface
    {
        [SerializeField]
        WebAPIClient webApi;
        [SerializeField]
        Text RegistName;
        [SerializeField]
        Text RegistCaption;
        [SerializeField]
        Identifier identifier;
        [SerializeField]
        GameObject content;

        PanelSwitcher panelSwitcher;

        public PanelType GetPanelType()
        {
            return PanelType.VendorRegist;
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

        public void CallRegistVendor()
        {
            StartCoroutine(RegistVendor());
        }

        IEnumerator RegistVendor()
        {
            panelSwitcher.PopLoadingDialog();
            var ticks = WebAPIClient.GetUnixTime();
            var nonce = WebAPIClient.GetTimestamp();
            var savedSeed = identifier.GetSeed(WebAPIClient.GetPlatform(), ticks);
            Debug.Log(savedSeed);
            var seed = savedSeed.Split(',')[0];
            long originTicks = 0;
            long.TryParse(savedSeed.Split(',')[1], out originTicks);
            yield return StartCoroutine(webApi.RegistVendor(RegistName.text, RegistCaption.text, identifier.AddNonce(seed,nonce), identifier.GetPlatformIdentifier(), originTicks, nonce));
            if (webApi.Result)
            {
                var data = webApi.DequeueResultData<WebAPIClient.ResponseBodyVendorCreate>();
                Debug.Log("data.ResponseCode: " + data.ResponseCode.ToString());
                Debug.Log("data.VendorCode: " + data.VendorCode);
                Debug.Log("data.PrivateKey: " + data.PrivateCode);
                Debug.Log("data.SessionId: " + data.SessionId);
                Debug.Log("data.Ticks:" + data.Ticks.ToString());
                identifier.SetPrivateCode(data.PrivateCode);
                identifier.SetSessionId(data.SessionId);
                panelSwitcher.Fade(PanelType.VendorManage);
                panelSwitcher.DepopLoadingDialog();
            }
            else
            {
                panelSwitcher.PopErrorDialog();
                panelSwitcher.DepopLoadingDialog();
            }
        }

        public void CallFadeBack()
        {
            StartCoroutine(FadeBack());
        }

        IEnumerator FadeBack()
        {
            yield return null;
            panelSwitcher.PopLoadingDialog();
            yield return null;
            panelSwitcher.FadeBack();
            yield return null;
            panelSwitcher.DepopLoadingDialog();
            yield return null;
        }
    }
}