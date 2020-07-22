//------------------------------------------------------------------------------
// <copyright file="ViewPanelController.cs" company="FurtherSystem Co.,Ltd.">
// Copyright (C) 2020 FurtherSystem Co.,Ltd.
// This software is released under the MIT License.
// http://opensource.org/licenses/mit-license.php
// </copyright>
// <author>FurtherSystem Co.,Ltd.</author>
// <email>info@furthersystem.com</email>
// <summary>
// View Panel Controller
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
        GameObject content;

        PanelSwitcher panelSwitcher;

        public PanelType GetPanelType()
        {
            return PanelType.View;
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

        public void CallFadeRegist()
        {
            StartCoroutine(FadeRegist());
        }

        IEnumerator FadeRegist()
        {
            yield return null;
            panelSwitcher.PopLoadingDialog();
            yield return null;
            panelSwitcher.Fade(PanelType.Regist);
            yield return null;
            panelSwitcher.DepopLoadingDialog();
            yield return null;
        }

        public void CallFadeSetting()
        {
            StartCoroutine(FadeSetting());
        }

        IEnumerator FadeSetting()
        {
            yield return null;
            panelSwitcher.PopLoadingDialog();
            yield return null;
            panelSwitcher.Fade(PanelType.Setting);
            yield return null;
            panelSwitcher.DepopLoadingDialog();
            yield return null;
        }
    }
}