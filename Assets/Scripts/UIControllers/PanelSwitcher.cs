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
        Component[] PanelObjects;

        Dictionary<PanelType, PanelControllerInterface> panels;
        List<PanelType> panelTypes;
        PanelType[] dialogTypes = new PanelType[]
        {
            PanelType.SearchDialog,
            PanelType.LoadingDialog,
            PanelType.ErrorDialog,
        };
        PanelType currentPanel;
        PanelType beforePanel;

        void Start()
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
                    throw new Exception("panel type "+ ((PanelType)value).ToString() + " component is not found.");
                }
            }
        }

        public void Fade(PanelType type)
        {
            AllDeactive();
            beforePanel = currentPanel;
            currentPanel = type;
            panels[currentPanel].Show();
        }

        public void FadeBack()
        {
            AllDeactive();
            currentPanel = beforePanel;
            panels[currentPanel].Show();
        }

        public void PopSearchDialog()
        {
            panels[PanelType.SearchDialog].Show();
        }

        public void PopLoadingDialog()
        {
            panels[PanelType.LoadingDialog].Show();
        }

        public void PopErrorDialog()
        {
            panels[PanelType.ErrorDialog].Show();
        }

        public void DepopSearchDialog()
        {
            panels[PanelType.SearchDialog].Dismiss();
        }

        public void DepopLoadingDialog()
        {
            panels[PanelType.LoadingDialog].Dismiss();
        }

        public void DepopErrorDialog()
        {
            panels[PanelType.ErrorDialog].Dismiss();
        }

        void AllDeactive()
        {
            panelTypes.ForEach(p => { if (!dialogTypes.Contains(p)) panels[p].Dismiss(); } );
        }
    }
}