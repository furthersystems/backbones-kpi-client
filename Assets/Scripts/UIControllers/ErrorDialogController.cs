//------------------------------------------------------------------------------
// <copyright file="ErrorDialogController.cs" company="FurtherSystem Co.,Ltd.">
// Copyright (C) 2020 FurtherSystem Co.,Ltd.
// This software is released under the MIT License.
// http://opensource.org/licenses/mit-license.php
// </copyright>
// <author>FurtherSystem Co.,Ltd.</author>
// <email>info@furthersystem.com</email>
// <summary>
// Error Dialog Controller
// </summary>
//------------------------------------------------------------------------------
using System;
using System.Collections;
using UnityEngine;

namespace Com.FurtherSystems.vQL.Client
{
    public class ErrorDialogController : MonoBehaviour, PanelControllerInterface
    {
        [SerializeField]
        GameObject content;

        PanelSwitcher panelSwitcher;

        public string Message { get; set; }

        public string Debug { get; set; }

        public PanelType GetPanelType()
        {
            return PanelType.ErrorDialog;
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
            while (content.activeSelf) yield return new WaitForSeconds(0.5f);
            yield return null;

        }

        public IEnumerator Dismiss()
        {
            content.SetActive(false);
            yield return null;
        }

        void Update()
        {
            if (IsShowing())
            {
                if (Application.platform == RuntimePlatform.Android
                    || Application.platform == RuntimePlatform.IPhonePlayer)
                {
                    var touch = TouchInfo.Get(0);
                    if (touch != TouchType.None) StartCoroutine(Dismiss());
                }
                else
                {
                    if (Input.GetMouseButtonDown(0)) StartCoroutine(Dismiss());
                }
            }
        }
    }
}