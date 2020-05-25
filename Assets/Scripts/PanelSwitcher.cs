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
        private Transform vendorRegistPanel;

        [SerializeField]
        private Transform vendorManagePanel;

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

        public void SwitchVendorRegist()
        {
            vendorRegistPanel.gameObject.SetActive(true);
            vendorManagePanel.gameObject.SetActive(false);
        }

        public void SwitchVendorManage()
        {
            vendorManagePanel.gameObject.SetActive(true);
            vendorRegistPanel.gameObject.SetActive(false);
        }
    }
}