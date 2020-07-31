//------------------------------------------------------------------------------
// <copyright file="SearchDialogController.cs" company="FurtherSystem Co.,Ltd.">
// Copyright (C) 2020 FurtherSystem Co.,Ltd.
// This software is released under the MIT License.
// http://opensource.org/licenses/mit-license.php
// </copyright>
// <author>FurtherSystem Co.,Ltd.</author>
// <email>info@furthersystem.com</email>
// <summary>
// Search Dialog Controller
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
    public class SearchDialogController : MonoBehaviour, PanelControllerInterface
    {
        [SerializeField]
        GameObject content;

        PanelSwitcher panelSwitcher;

        public PanelType GetPanelType()
        {
            return PanelType.SearchDialog;
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

        public void CallSearch()
        {
            StartCoroutine(Search());
        }

        IEnumerator Search()
        {
            yield return panelSwitcher.PopLoadingDialog();
            //panelSwitcher.Fade(PanelType.Regist);
            yield return panelSwitcher.DepopLoadingDialog();
        }

        public void CallDepopSearchDialog()
        {
            StartCoroutine(DepopSearchDialog());
        }

        IEnumerator DepopSearchDialog()
        {
            yield return panelSwitcher.DepopSearchDialog();
        }
    }
}