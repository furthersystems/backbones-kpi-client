//------------------------------------------------------------------------------
// <copyright file="AgreementDialogController.cs" company="FurtherSystem Co.,Ltd.">
// Copyright (C) 2020 FurtherSystem Co.,Ltd.
// This software is released under the MIT License.
// http://opensource.org/licenses/mit-license.php
// </copyright>
// <author>FurtherSystem Co.,Ltd.</author>
// <email>info@furthersystem.com</email>
// <summary>
// Agreement Dialog Controller
// </summary>
//------------------------------------------------------------------------------
using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

namespace Com.FurtherSystems.vQL.Client
{
    public class AgreementDialogController : MonoBehaviour, PanelControllerInterface
    {
        [SerializeField]
        Toggle checkReaded;
        [SerializeField]
        Button agreeButton;
        [SerializeField]
        GameObject content;

        PanelSwitcher panelSwitcher;

        void Awake()
        {
            agreeButton.interactable = false;
        }

        public PanelType GetPanelType()
        {
            return PanelType.AgreementDialog;
        }

        public void Initialize(PanelSwitcher switcher)
        {
            panelSwitcher = switcher;
            agreeButton.interactable = false;
        }

        public bool IsShowing()
        {
            return content.activeSelf;
        }

        public IEnumerator Show()
        {
            content.SetActive(true);
            agreeButton.interactable = false;
            while (content.activeSelf) yield return new WaitForSeconds(0.5f);
            yield return null;
        }

        public IEnumerator Dismiss()
        {
            content.SetActive(false);
            yield return null;
        }

        public void CallOpenAgreementUrl()
        {
            StartCoroutine(OpenAgreementUrl());
        }

        IEnumerator OpenAgreementUrl()
        {
            Application.OpenURL("https://furthersystem.com");
            yield return null;
        }

        public void CallCheckReaded()
        {
            StartCoroutine(CheckReaded());
        }

        IEnumerator CheckReaded()
        {
            if (checkReaded.isOn)
            {
                agreeButton.interactable = true;
            }
            else
            {
                agreeButton.interactable = false;
            }
            yield return null;
        }

        public void CallSubmitAgree()
        {
            Instance.Ident.SetCheckedAgreement(checkReaded.isOn);
            Instance.Ident.SetAgreementVersion(Instance.AgreementVersion);
            Instance.Ident.SetActivateType(Instance.Identifier.ActivateType.PhoneAuth);
            Instance.Ident.SetActivateKeyword("");
            StartCoroutine(SubmitAgree());
        }

        IEnumerator SubmitAgree()
        {
            yield return Dismiss();
        }
    }
}