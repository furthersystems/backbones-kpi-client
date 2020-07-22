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
        GameObject content;

        PanelSwitcher panelSwitcher;

        public PanelType GetPanelType()
        {
            return PanelType.VendorSetting;
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

        public void CallSetVendor()
        {
            StartCoroutine(SetVendor());
        }

        IEnumerator SetVendor()
        {
            panelSwitcher.PopLoadingDialog();
            var nonce = Instance.WebAPIClient.GetTimestamp();
            var (seed, ticks) = Instance.Ident.GetSeed();
            yield return StartCoroutine(Instance.WebAPI.SetVendor(SetName.text, SetCaption.text, Instance.Ident.SessionId, Instance.Ident.GetPlatformIdentifier(), ticks, nonce));
            if (Instance.WebAPI.Result)
            {
                var data = Instance.WebAPI.DequeueResultData<Messages.Response.VendorSetting>();
                Debug.Log("data.ResponseCode: " + data.ResponseCode.ToString());
                Debug.Log("data.VendorCode: " + data.VendorCode);
                Debug.Log("data.Ticks:" + data.Ticks.ToString());

                Storage.Save(Storage.Type.VendorCode, data.VendorCode);

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