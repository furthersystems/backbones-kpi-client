//------------------------------------------------------------------------------
// <copyright file="VendorMainRowController.cs" company="FurtherSystem Co.,Ltd.">
// Copyright (C) 2020 FurtherSystem Co.,Ltd.
// This software is released under the MIT License.
// http://opensource.org/licenses/mit-license.php
// </copyright>
// <author>FurtherSystem Co.,Ltd.</author>
// <email>info@furthersystem.com</email>
// <summary>
// Vendor Main Row Controller
// </summary>
//------------------------------------------------------------------------------
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

namespace Com.FurtherSystems.vQL.Client
{
    [RequireComponent(typeof(LayoutElement))]
    public class VendorMainRowController : MonoBehaviour
    {
        [SerializeField]
        GameObject content;
        [SerializeField]
        Text KeyCodePrefix;
        LayoutElement layout;

        public IEnumerator Initialize()
        {
            layout = GetComponent<LayoutElement>();
            yield return ClearRow();
        }

        public void SetRow(params string[] data)
        {
            KeyCodePrefix.text = data[0];
            StartCoroutine(Show());
        }

        public void CallClearRow()
        {
            StartCoroutine(ClearRow());
        }

        IEnumerator ClearRow()
        {
            KeyCodePrefix.text = "";
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
    }
}