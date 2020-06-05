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
using System.Collections;
using UnityEngine;

namespace Com.FurtherSystems.vQL.Client
{
    public class PanelSwitcher : MonoBehaviour
    {
        [SerializeField]
        private WebAPIClient webApi;
        [SerializeField]
        private Transform registPanel;
        [SerializeField]
        private Transform mainPanel;
        [SerializeField]
        private Transform vendorRegistPanel;
        [SerializeField]
        private Transform vendorManagePanel;
        [SerializeField]
        private Transform vendorMainPanel;
        [SerializeField]
        private Transform searchDialog;
        [SerializeField]
        private Transform loadingDialog;

        public IEnumerator SubmitVendorRegist()
        {
            yield return StartCoroutine(webApi.Create("","",""));
            if ( webApi.CreateResult) {
                SwitchVendorManage();
            }
            else
            {
                //failed dialog
            }
        }

        public void SwitchRegist()
        {
            AllDeactive();
            registPanel.gameObject.SetActive(true);
        }

        public void SwitchMain()
        {
            AllDeactive();
            mainPanel.gameObject.SetActive(true);
        }

        public void SwitchVendorRegist()
        {
            AllDeactive();
            vendorRegistPanel.gameObject.SetActive(true);
        }

        public void SwitchVendorManage()
        {
            AllDeactive();
            vendorManagePanel.gameObject.SetActive(true);
        }

        public void SwitchVendorMain()
        {
            AllDeactive();
            vendorMainPanel.gameObject.SetActive(true);
        }

        public void SwitchSearchDialog()
        {
            AllDeactive();
            searchDialog.gameObject.SetActive(true);
        }

        public void SwitchLoadingDialog()
        {
            AllDeactive();
            loadingDialog.gameObject.SetActive(true);
        }

        private void AllDeactive()
        {
            registPanel.gameObject.SetActive(false);
            mainPanel.gameObject.SetActive(false);
            vendorRegistPanel.gameObject.SetActive(false);
            vendorManagePanel.gameObject.SetActive(false);
            vendorMainPanel.gameObject.SetActive(false);
            searchDialog.gameObject.SetActive(false);
            loadingDialog.gameObject.SetActive(false);
        }
    }
}