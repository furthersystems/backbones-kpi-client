//------------------------------------------------------------------------------
// <copyright file="LoadingTickerPanelController.cs" company="FurtherSystem Co.,Ltd.">
// Copyright (C) 2020 FurtherSystem Co.,Ltd.
// This software is released under the MIT License.
// http://opensource.org/licenses/mit-license.php
// </copyright>
// <author>FurtherSystem Co.,Ltd.</author>
// <email>info@furthersystem.com</email>
// <summary>
// Loading Ticker Panel Controller
// </summary>
//------------------------------------------------------------------------------
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

namespace Com.FurtherSystems.vQL.Client
{
    public class LoadingTickerPanelController : MonoBehaviour
    {
        [SerializeField]
        SVGImage ticker1;
        [SerializeField]
        SVGImage ticker2;
        [SerializeField]
        SVGImage ticker3;
        [SerializeField]
        LoadingDialogController Parent;

        float baseFill;
        bool boundFlag;
        const float bar1fill = 1f;
        const float bar2fill = 0.75f;
        const float bar3fill = 0.5f;

        [SerializeField]
        float Volume = 2f;

        void Start()
        {
            baseFill = 1f;
            ticker1.color = UpdateColor(ticker1.color, bar1fill);
            ticker2.color = UpdateColor(ticker2.color, bar2fill);
            ticker3.color = UpdateColor(ticker3.color, bar3fill);
            boundFlag = false;
        }

        void Update()
        {
            if (Parent.IsShowing())
            {
                UpdateBaseFillAmount(ref baseFill, ref boundFlag, Time.deltaTime);
                ticker1.color = UpdateColor(ticker1.color, UpdateFillAmount(baseFill, bar1fill, boundFlag));
                ticker2.color = UpdateColor(ticker2.color, UpdateFillAmount(baseFill, bar2fill, boundFlag));
                ticker3.color = UpdateColor(ticker3.color, UpdateFillAmount(baseFill, bar3fill, boundFlag));
            }
        }

        Color UpdateColor(Color target, float alpha)
        {
            return new Color(ticker1.color.r, ticker1.color.g, ticker1.color.b, alpha);
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
            else if (!flag && amount < 0f) amount = - amount;

            return amount;
        }
    }
}