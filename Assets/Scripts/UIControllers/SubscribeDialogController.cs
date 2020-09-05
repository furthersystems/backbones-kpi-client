//------------------------------------------------------------------------------
// <copyright file="SubscribeDialogController.cs" company="FurtherSystem Co.,Ltd.">
// Copyright (C) 2020 FurtherSystem Co.,Ltd.
// This software is released under the MIT License.
// http://opensource.org/licenses/mit-license.php
// </copyright>
// <author>FurtherSystem Co.,Ltd.</author>
// <email>info@furthersystem.com</email>
// <summary>
// Subscribe Dialog Controller
// </summary>
//------------------------------------------------------------------------------
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

namespace Com.FurtherSystems.vQL.Client
{
    public class SubscribeDialogController : MonoBehaviour, PanelControllerInterface
    {
        [SerializeField]
        GameObject freePlanButton;
        [SerializeField]
        GameObject freePlanSelected;
        [SerializeField]
        GameObject freePlanSelect;
        [SerializeField]
        GameObject proPlanButton;
        [SerializeField]
        GameObject proPlanSelected;
        [SerializeField]
        GameObject proPlanSelect;
        [SerializeField]
        GameObject enterprisePlanButton;
        [SerializeField]
        GameObject enterprisePlanSelected;
        [SerializeField]
        GameObject enterprisePlanSelect;

        [SerializeField]
        GameObject content;

        PanelSwitcher panelSwitcher;

        public PanelType GetPanelType()
        {
            return PanelType.SubscribeDialog;
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
            SelectFreePlan();
            while (content.activeSelf) yield return new WaitForSeconds(0.5f);
            yield return null;
        }

        public IEnumerator Dismiss()
        {
            content.SetActive(false);
            yield return null;
        }

        public void DeselectButtons()
        {
            freePlanButton.GetComponent<Button>().interactable = true;
            freePlanSelected.SetActive(false);
            freePlanSelect.SetActive(true);
            proPlanButton.GetComponent<Button>().interactable = true;
            proPlanSelected.SetActive(false);
            proPlanSelect.SetActive(true);
            enterprisePlanButton.GetComponent<Button>().interactable = false;
            enterprisePlanSelected.SetActive(false);
            enterprisePlanSelect.SetActive(true);
        }

        public void InactiveAll()
        {
            freePlanButton.GetComponent<Button>().interactable = false;
            proPlanButton.GetComponent<Button>().interactable = false;
            enterprisePlanButton.GetComponent<Button>().interactable = false;
        }

        public void SelectFreePlan()
        {
            InactiveAll();
            DeselectButtons();
            freePlanSelected.SetActive(true);
            freePlanSelect.SetActive(false);
        }

        public void SelectProPlan()
        {
            InactiveAll();
            DeselectButtons();
            proPlanSelected.SetActive(true);
            proPlanSelect.SetActive(false);
        }

        public void SelectEntPlan()
        {
            InactiveAll();
            DeselectButtons();
            enterprisePlanSelected.SetActive(true);
            enterprisePlanSelect.SetActive(false);
        }

        public void Next()
        {
            StartCoroutine(Dismiss());
        }
    }
}