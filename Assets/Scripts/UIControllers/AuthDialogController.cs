//------------------------------------------------------------------------------
// <copyright file="AuthDialogController.cs" company="FurtherSystem Co.,Ltd.">
// Copyright (C) 2020 FurtherSystem Co.,Ltd.
// This software is released under the MIT License.
// http://opensource.org/licenses/mit-license.php
// </copyright>
// <author>FurtherSystem Co.,Ltd.</author>
// <email>info@furthersystem.com</email>
// <summary>
// Auth Dialog Controller
// </summary>
//------------------------------------------------------------------------------
using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

namespace Com.FurtherSystems.vQL.Client
{
    public class AuthDialogController : MonoBehaviour, PanelControllerInterface
    {
        [SerializeField]
        GameObject authPhoneLabel;
        [SerializeField]
        GameObject authPhoneCaption;
        [SerializeField]
        GameObject authPhoneInputField;
        [SerializeField]
        GameObject authPhoneSubmit;

        [SerializeField]
        GameObject authCodeLabel;
        [SerializeField]
        GameObject authCodeCaption;
        [SerializeField]
        GameObject authCodeInputField;
        [SerializeField]
        GameObject authCodeSubmit;
        [SerializeField]
        GameObject authCodeToPhoneSwitch;

        [SerializeField]
        GameObject authCompleteLabel;
        [SerializeField]
        GameObject authCompleteCaption;
        [SerializeField]
        GameObject content;

        PanelSwitcher panelSwitcher;

        public PanelType GetPanelType()
        {
            return PanelType.AuthDialog;
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
            ActivePhoneForm();
            while (content.activeSelf) yield return new WaitForSeconds(0.5f);
            yield return null;
        }

        public IEnumerator Dismiss()
        {
            content.SetActive(false);
            yield return null;
        }

        private void ActivePhoneForm()
        {
            authPhoneLabel.SetActive(true);
            authPhoneCaption.SetActive(true);
            authPhoneInputField.SetActive(true);
            authPhoneSubmit.SetActive(true);

            authCodeLabel.SetActive(false);
            authCodeCaption.SetActive(false);
            authCodeInputField.SetActive(false);
            authCodeSubmit.SetActive(false);
            authCodeToPhoneSwitch.SetActive(false);

            authCompleteLabel.SetActive(false);
            authCompleteCaption.SetActive(false);
    }

        private void ActiveCodeForm()
        {
            authPhoneLabel.SetActive(false);
            authPhoneCaption.SetActive(false);
            authPhoneInputField.SetActive(false);
            authPhoneSubmit.SetActive(false);

            authCodeLabel.SetActive(true);
            authCodeCaption.SetActive(true);
            authCodeInputField.SetActive(true);
            authCodeSubmit.SetActive(true);
            authCodeToPhoneSwitch.SetActive(true);

            authCompleteLabel.SetActive(false);
            authCompleteCaption.SetActive(false);
        }

        private void ActiveCompleteForm()
        {
            authPhoneLabel.SetActive(false);
            authPhoneCaption.SetActive(false);
            authPhoneInputField.SetActive(false);
            authPhoneSubmit.SetActive(false);

            authCodeLabel.SetActive(false);
            authCodeCaption.SetActive(false);
            authCodeInputField.SetActive(false);
            authCodeSubmit.SetActive(false);
            authCodeToPhoneSwitch.SetActive(false);

            authCompleteLabel.SetActive(true);
            authCompleteCaption.SetActive(true);
        }

        public void CallSwitchCodeToPhoneForm()
        {
            StartCoroutine(SwitchCodeToPhoneForm());
        }

        IEnumerator SwitchCodeToPhoneForm()
        {
            // TODO RASH LOCK
            //yield return new WaitForSeconds(3f);
            ActivePhoneForm();
            yield break;
        }

        public void CallSend()
        {
            // TODO fix dummy logic.
            StartCoroutine(Send());
        }

        IEnumerator Send()
        {
            // TODO fix dummy logic.
            yield return new WaitForSeconds(3f);
            CallBackSend();
        }

        private void CallBackSend()
        {
            // TODO fix dummy logic.
            // Switch UI

            // OK case
            ActiveCodeForm();
        }

        public void CallCheck()
        {
            // TODO fix dummy logic.
            StartCoroutine(Check());
        }

        IEnumerator Check()
        {
            // TODO fix dummy logic.
            yield return new WaitForSeconds(3f);
            CallBackCheck();
        }

        private void CallBackCheck()
        {
            // TODO fix dummy logic.
            // NG case failed.

            // OK case
            ActiveCompleteForm();
            CallPassAuth();
        }

        private void CallPassAuth()
        {
            Instance.Ident.SetActivateType(Instance.Identifier.ActivateType.PhoneAuth);
            Instance.Ident.SetActivateKeyword(authPhoneInputField.GetComponent<InputField>().text);
            StartCoroutine(PassAuth());
        }

        IEnumerator PassAuth()
        {
            yield return new WaitForSeconds(3f);
            yield return Dismiss();
        }
    }
}