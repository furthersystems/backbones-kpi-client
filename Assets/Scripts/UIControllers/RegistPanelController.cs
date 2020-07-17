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
using System.Collections;
using UnityEngine;

namespace Com.FurtherSystems.vQL.Client
{
    public class RegistPanelController : MonoBehaviour, PanelControllerInterface
    {
        [SerializeField]
        WebAPIClient webApi;
        [SerializeField]
        Identifier identifier;
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

        public void CallCreateAccount()
        {
            StartCoroutine(CreateAccount());
        }

        IEnumerator CreateAccount()
        {
            panelSwitcher.PopLoadingDialog();
            var ticks = WebAPIClient.GetUnixTime();
            var nonce = WebAPIClient.GetTimestamp();
            var savedSeed = identifier.GetSeed(WebAPIClient.GetPlatform(), ticks);
            Debug.Log(savedSeed);
            var seed = savedSeed.Split(',')[0];
            long originTicks = 0;
            long.TryParse(savedSeed.Split(',')[1], out originTicks);
            yield return StartCoroutine(webApi.CreateAccount(identifier.AddNonce(seed, nonce), identifier.GetPlatformIdentifier(), originTicks, nonce));
            if (webApi.Result)
            {
                var data = webApi.DequeueResultData<WebAPIClient.ResponseBodyCreate>();
                Debug.Log("data.ResponseCode: " + data.ResponseCode.ToString());
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

        public void CallRegistQueue()
        {
            StartCoroutine(RegistQueue());
        }

        IEnumerator RegistQueue()
        {
            panelSwitcher.PopLoadingDialog();
            var ticks = WebAPIClient.GetUnixTime();
            var nonce = WebAPIClient.GetTimestamp();
            var savedSeed = identifier.GetSeed(WebAPIClient.GetPlatform(), ticks);
            Debug.Log(savedSeed);
            var seed = savedSeed.Split(',')[0];
            long originTicks = 0;
            long.TryParse(savedSeed.Split(',')[1], out originTicks);
            yield return StartCoroutine(webApi.Regist(identifier.AddNonce(seed,nonce), identifier.GetPlatformIdentifier(), originTicks, nonce));
            if (webApi.Result)
            {
                var data = webApi.DequeueResultData<WebAPIClient.ResponseBodyCreate>();

                // TODO create file update "vendors"

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