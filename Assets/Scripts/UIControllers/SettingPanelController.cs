//------------------------------------------------------------------------------
// <copyright file="SettingPanelController.cs" company="FurtherSystem Co.,Ltd.">
// Copyright (C) 2020 FurtherSystem Co.,Ltd.
// This software is released under the MIT License.
// http://opensource.org/licenses/mit-license.php
// </copyright>
// <author>FurtherSystem Co.,Ltd.</author>
// <email>info@furthersystem.com</email>
// <summary>
// Setting Panel Controller
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
    public class SettingPanelController : MonoBehaviour, PanelControllerInterface
    {
        [SerializeField]
        GameObject content;

        PanelSwitcher panelSwitcher;

        public PanelType GetPanelType()
        {
            return PanelType.Setting;
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

        public void CallFadeVendorManage()
        {
            StartCoroutine(FadeVendorManage());
        }

        IEnumerator FadeVendorManage()
        {
            yield return null;
            panelSwitcher.PopLoadingDialog();
            yield return null;
            // if not regist?
            //panelSwitcher.FadeRegist();
            panelSwitcher.Fade(PanelType.VendorManage);
            yield return null;
            panelSwitcher.DepopLoadingDialog();
            yield return null;
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