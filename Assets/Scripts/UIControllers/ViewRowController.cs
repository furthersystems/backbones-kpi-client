//------------------------------------------------------------------------------
// <copyright file="ViewRowController.cs" company="FurtherSystem Co.,Ltd.">
// Copyright (C) 2020 FurtherSystem Co.,Ltd.
// This software is released under the MIT License.
// http://opensource.org/licenses/mit-license.php
// </copyright>
// <author>FurtherSystem Co.,Ltd.</author>
// <email>info@furthersystem.com</email>
// <summary>
// View Row Controller
// </summary>
//------------------------------------------------------------------------------
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

namespace Com.FurtherSystems.vQL.Client
{
    [RequireComponent(typeof(LayoutElement))]
    public class ViewRowController : MonoBehaviour
    {
        [SerializeField]
        GameObject content;
        [SerializeField]
        ViewPanelController parentPanel;
        [SerializeField]
        string vendorCode;
        [SerializeField]
        Text vendorName;
        [SerializeField]
        Text waitingBefore;
        LayoutElement layout;

        public IEnumerator Initialize()
        {
            layout = GetComponent<LayoutElement>();
            yield return ClearRow();
        }

        public void SetRow(params string[] data)
        {
            vendorCode = data[0];
            vendorName.text = data[1];
            if (!string.IsNullOrEmpty(data[2])) { 
            waitingBefore.text = "あと" + data[2] + "人";
            }
            StartCoroutine(Show());
        }

        public void CallClearRow()
        {
            StartCoroutine(ClearRow());
        }

        IEnumerator ClearRow()
        {
            vendorCode = string.Empty;
            vendorName.text = "";
            waitingBefore.text = "";
            yield return StartCoroutine(Dismiss());
        }

        IEnumerator Show()
        {
            layout.enabled = true;
            content.SetActive(true);
            yield return null;
        }

        IEnumerator Dismiss()
        {
            content.SetActive(false);
            layout.enabled = false;
            yield return null;
        }

        public void CallFadeMain()
        {
            StartCoroutine(FadeMain());
        }

        IEnumerator FadeMain()
        {
            Instance.Vendors.SetCurrentKey(vendorCode);
            parentPanel.CallFadeMain();
            yield return Dismiss();
        }
    }
}