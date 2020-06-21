//------------------------------------------------------------------------------
// <copyright file="PanelSwitcher.cs" company="FurtherSystem Co.,Ltd.">
// Copyright (C) 2020 FurtherSystem Co.,Ltd.
// This software is released under the MIT License.
// http://opensource.org/licenses/mit-license.php
// </copyright>
// <author>FurtherSystem Co.,Ltd.</author>
// <email>info@furthersystem.com</email>
// <summary>
// Screen Panel Switcher 
// </summary>
//------------------------------------------------------------------------------
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Com.FurtherSystems.vQL.Client
{
    public class PanelSwitcher : MonoBehaviour
    {
        [SerializeField]
        WebAPIClient webApi;
        [SerializeField]
        Transform registPanel;
        [SerializeField]
        Transform mainPanel;
        [SerializeField]
        Transform vendorRegistPanel;
        [SerializeField]
        Transform vendorManagePanel;
        [SerializeField]
        Transform vendorMainPanel;
        [SerializeField]
        Transform searchDialog;
        [SerializeField]
        Transform loadingDialog;
        [SerializeField]
        Transform errorDialog;
        [SerializeField]
        Transform fadePanel;

        List<Transform> panels = new List<Transform>();

        void Start()
        {
            panels.Add(registPanel);
            panels.Add(mainPanel);
            panels.Add(vendorRegistPanel);
            panels.Add(vendorManagePanel);
            panels.Add(vendorMainPanel);
            panels.Add(searchDialog);
            panels.Add(loadingDialog);
            panels.Add(errorDialog);
            panels.Add(fadePanel);

            panels.ForEach(p => p.GetComponent<PanelControllerInterface>().Initialize(this));
        }

        public void FadeRegist()
        {
            AllDeactive();
            registPanel.gameObject.SetActive(true);
        }

        public void FadeMain()
        {
            AllDeactive();
            mainPanel.gameObject.SetActive(true);
        }

        public void FadeVendorRegist()
        {
            AllDeactive();
            vendorRegistPanel.gameObject.SetActive(true);
        }

        public void FadeVendorManage()
        {
            AllDeactive();
            vendorManagePanel.gameObject.SetActive(true);
        }

        public void FadeVendorMain()
        {
            AllDeactive();
            vendorMainPanel.gameObject.SetActive(true);
        }

        public void PopSearchDialog()
        {
            searchDialog.gameObject.SetActive(true);
        }

        public void PopLoadingDialog()
        {
            loadingDialog.gameObject.SetActive(true);
        }

        public void PopErrorDialog()
        {
            loadingDialog.gameObject.SetActive(true);
        }

        public void DepopSearchDialog()
        {
            searchDialog.gameObject.SetActive(false);
        }

        public void DepopLoadingDialog()
        {
            loadingDialog.gameObject.SetActive(false);
        }

        public void DepopErrorDialog()
        {
            loadingDialog.gameObject.SetActive(false);
        }

        void AllDeactive()
        {
            registPanel.gameObject.SetActive(false);
            mainPanel.gameObject.SetActive(false);
            vendorRegistPanel.gameObject.SetActive(false);
            vendorManagePanel.gameObject.SetActive(false);
            vendorMainPanel.gameObject.SetActive(false);
        }
    }
}