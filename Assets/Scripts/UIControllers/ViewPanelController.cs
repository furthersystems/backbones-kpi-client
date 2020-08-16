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
        [SerializeField]
        ViewRowController[] rows;
        [SerializeField]
        GameObject manageIcon;
        [SerializeField]
        GameObject upgradeIcon;

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

        public IEnumerator Show()
        {
            content.SetActive(true);
            foreach (var row in rows)
            {
                yield return row.Initialize();
            }
            yield return ShowVendors();
            yield return null;
        }

        public IEnumerator Dismiss()
        {
            content.SetActive(false);
            yield return null;
        }

        public void CallFadeMain()
        {
            StartCoroutine(FadeMain());
        }

        IEnumerator FadeMain()
        {
            yield return panelSwitcher.PopLoadingDialog();
            yield return panelSwitcher.Fade(PanelType.Main);
            yield return panelSwitcher.DepopLoadingDialog();
        }

        public IEnumerator ShowVendors()
        {
            var index = 0;
            foreach (var v in Instance.Vendors.GetVendors())
            {
                rows[index].SetRow(v.VendorCode, v.VendorName, "");
                index++;
            }
            yield return null;
        }

        public void CallFadeRegist()
        {
            StartCoroutine(FadeRegist());
        }

        IEnumerator FadeRegist()
        {
            yield return panelSwitcher.PopLoadingDialog();
            yield return panelSwitcher.Fade(PanelType.Enqueue);
            yield return panelSwitcher.DepopLoadingDialog();
        }

        public void CallFadeUpgrade()
        {
            StartCoroutine(FadeUpgrade());
        }

        IEnumerator FadeUpgrade()
        {
            yield return panelSwitcher.PopLoadingDialog();
            yield return panelSwitcher.Fade(PanelType.Upgrade);
            yield return panelSwitcher.DepopLoadingDialog();
        }

        public void CallFadeVendorManage()
        {
            StartCoroutine(FadeVendorManage());
        }

        IEnumerator FadeVendorManage()
        {
            yield return panelSwitcher.PopLoadingDialog();
            // if not regist?
            //panelSwitcher.FadeRegist();
            yield return panelSwitcher.Fade(PanelType.VendorManage);
            yield return panelSwitcher.DepopLoadingDialog();
        }
    }
}