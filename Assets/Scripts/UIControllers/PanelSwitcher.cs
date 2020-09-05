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
using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Com.FurtherSystems.vQL.Client
{
    public class PanelSwitcher : MonoBehaviour
    {
        [SerializeField]
        Component[] PanelObjects;

        Dictionary<PanelType, PanelControllerInterface> panels;
        List<PanelType> panelTypes;
        PanelType[] dialogTypes = new PanelType[]
        {
            PanelType.SearchDialog,
            PanelType.LoadingDialog,
            PanelType.ErrorDialog,
            PanelType.AuthDialog,
            PanelType.SubscribeDialog,
            PanelType.AgreementDialog,
        };
        PanelType currentPanel;
        PanelType beforePanel;

        public void Initialize()
        {
            panels = new Dictionary<PanelType, PanelControllerInterface>();
            foreach (var component in PanelObjects)
            {
                var panel = component.GetComponent<PanelControllerInterface>();
                if (panel != null)
                {
                    panel.Initialize(this);
                    panels.Add(panel.GetPanelType(), panel);
                }
                else
                {
                    throw new Exception("component is null.");
                }
            }
            panelTypes = Enum.GetValues(typeof(PanelType)).OfType<PanelType>().ToList();
            foreach (var value in panelTypes)
            {
                if (!panels.ContainsKey((PanelType)value))
                {
                    throw new Exception("panel type " + ((PanelType)value).ToString() + " component is not found.");
                }
            }
        }

        public IEnumerator Fade(PanelType type)
        {
            yield return AllDeactive();
            beforePanel = currentPanel;
            currentPanel = type;
            yield return panels[currentPanel].Show();
            Storage.Save(Storage.Type.Latest, ((int)currentPanel).ToString());
        }

        public IEnumerator FadeBack()
        {
            yield return AllDeactive();
            currentPanel = beforePanel;
            yield return panels[currentPanel].Show();
            Storage.Save(Storage.Type.Latest, ((int)currentPanel).ToString());
        }

        public IEnumerator PopSearchDialog()
        {
            yield return panels[PanelType.SearchDialog].Show();
        }

        public IEnumerator PopLoadingDialog()
        {
            yield return panels[PanelType.LoadingDialog].Show();
        }

        public IEnumerator PopErrorDialog()
        {
            yield return panels[PanelType.ErrorDialog].Show();
        }

        public IEnumerator DepopSearchDialog()
        {
            yield return panels[PanelType.SearchDialog].Dismiss();
        }

        public IEnumerator DepopLoadingDialog()
        {
            yield return panels[PanelType.LoadingDialog].Dismiss();
        }

        public IEnumerator DepopErrorDialog()
        {
            yield return panels[PanelType.ErrorDialog].Dismiss();
        }

        public IEnumerator ModalAuthDialog()
        {
            yield return panels[PanelType.AuthDialog].Show();
        }

        public IEnumerator ModalSubscribeDialog()
        {
            yield return panels[PanelType.SubscribeDialog].Show();
        }

        public IEnumerator ModalAgreementDialog()
        {
            yield return panels[PanelType.AgreementDialog].Show();
        }

        public IEnumerator ProgressAuthDialog()
        {
            yield return panels[PanelType.AuthDialog].Dismiss();
        }

        public IEnumerator ProgressSubscribeDialog()
        {
            yield return panels[PanelType.SubscribeDialog].Dismiss();
        }

        public IEnumerator ProgressAgreementDialog()
        {
            yield return panels[PanelType.AgreementDialog].Dismiss();
        }

        IEnumerator AllDeactive()
        {
            foreach(var p in panelTypes)
            {
                if (!dialogTypes.Contains(p))
                {
                    yield return panels[p].Dismiss();
                }
            }
            yield return null;
        }
    }
}