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
    public class ViewPanelController : MonoBehaviour, PanelControllerInterface
    {

        [SerializeField]
        WebAPIClient webApi;
        [SerializeField]
        Identifier identifier;

        PanelSwitcher panelSwitcher;

        public void Initialize(PanelSwitcher switcher)
        {
            panelSwitcher = switcher;
        }

        public bool IsShowing()
        {
            return gameObject.activeSelf;
        }

        public void Show()
        {
            gameObject.SetActive(true);
        }

        public void Dismiss()
        {
            gameObject.SetActive(false);
        }

        public void CallFadeRegist()
        {
            StartCoroutine(FadeRegist());
        }

        IEnumerator FadeRegist()
        {
            yield return null;
            panelSwitcher.PopLoadingDialog();
            yield return null;
            panelSwitcher.FadeRegist();
            yield return null;
            panelSwitcher.DepopLoadingDialog();
            yield return null;
        }
    }
}