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

        public void Show()
        {
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
            content.SetActive(true);
        }

        public void Dismiss()
        {
            content.SetActive(false);
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