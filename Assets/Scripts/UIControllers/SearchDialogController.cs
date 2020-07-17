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
        WebAPIClient webApi;
        [SerializeField]
        Identifier identifier;
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

        public void Show()
        {
            content.SetActive(true);
        }

        public void Dismiss()
        {
            content.SetActive(false);
        }

        public void CallSearch()
        {
            StartCoroutine(Search());
        }

        IEnumerator Search()
        {
            yield return null;
            panelSwitcher.PopLoadingDialog();
            yield return null;
            //panelSwitcher.Fade(PanelType.Regist);
            yield return null;
            panelSwitcher.DepopLoadingDialog();
            yield return null;
        }

        public void CallDepopSearchDialog()
        {
            StartCoroutine(DepopSearchDialog());
        }

        IEnumerator DepopSearchDialog()
        {
            panelSwitcher.DepopSearchDialog();
            yield return null;
        }
    }
}