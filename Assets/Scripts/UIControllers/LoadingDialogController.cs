//------------------------------------------------------------------------------
// <copyright file="LoadingDialogController.cs" company="FurtherSystem Co.,Ltd.">
// Copyright (C) 2020 FurtherSystem Co.,Ltd.
// This software is released under the MIT License.
// http://opensource.org/licenses/mit-license.php
// </copyright>
// <author>FurtherSystem Co.,Ltd.</author>
// <email>info@furthersystem.com</email>
// <summary>
// Loading Dialog Controller
// </summary>
//------------------------------------------------------------------------------
using UnityEngine;
using UnityEngine.UI;

namespace Com.FurtherSystems.vQL.Client
{
    public class LoadingDialogController : MonoBehaviour, PanelControllerInterface
    {
        [SerializeField]
        Image bar1;
        [SerializeField]
        Image bar2;
        [SerializeField]
        Image bar3;
        [SerializeField]
        Image bar4;
        [SerializeField]
        Image bar5;
        [SerializeField]
        GameObject content;

        float baseFill;
        bool boundFlag;
        const float bar1fill = 1f;
        const float bar2fill = 0.75f;
        const float bar3fill = 0.5f;
        const float bar4fill = 0.25f;
        const float bar5fill = 0f;

        [SerializeField]
        float Volume = 2f;

        PanelSwitcher panelSwitcher;

        void Start()
        {
            baseFill = 1f;
            bar1.fillAmount = bar1fill;
            bar2.fillAmount = bar2fill;
            bar3.fillAmount = bar3fill;
            bar4.fillAmount = bar4fill;
            bar5.fillAmount = bar5fill;
            boundFlag = false;
        }

        void Update()
        {
            if (IsShowing())
            {
                UpdateBaseFillAmount(ref baseFill, ref boundFlag, Time.deltaTime);
                bar1.fillAmount = UpdateFillAmount(baseFill, bar1fill, boundFlag);
                bar2.fillAmount = UpdateFillAmount(baseFill, bar2fill, boundFlag);
                bar3.fillAmount = UpdateFillAmount(baseFill, bar3fill, boundFlag);
                bar4.fillAmount = UpdateFillAmount(baseFill, bar4fill, boundFlag);
                bar5.fillAmount = UpdateFillAmount(baseFill, bar5fill, boundFlag);
            }
        }

        float UpdateBaseFillAmount(ref float inAmount, ref bool flag , float delta)
        {
            inAmount = (flag) ? inAmount - Volume * delta : inAmount + Volume * delta;
            if (inAmount > 1f) { inAmount = 1f; flag = true; }
            else if (inAmount < 0f) { inAmount = 0f; flag = false; }

            return inAmount;
        }

        float UpdateFillAmount(float baseAmount, float inAmount, bool flag)
        {
            var amount = 0f;
            if (flag) amount = baseAmount + inAmount;
            else amount = baseAmount - inAmount;

            if (flag && amount >= 1f) amount = 2f - amount;
            //else if (flag && amount < 1f) ;
            //else if (!flag && amount >= 0f) ;
            else if (!flag && amount < 0f) amount = - amount;

            return amount;
        }

        public PanelType GetPanelType()
        {
            return PanelType.LoadingDialog;
        }

        public void Initialize(PanelSwitcher switcher)
        {
            panelSwitcher = switcher;
        }

        public bool IsShowing()
        {
            return content.activeSelf;
        }

        public void Show()
        {
            content.SetActive(true);
        }

        public void Dismiss()
        {
            content.SetActive(false);
        }
    }
}