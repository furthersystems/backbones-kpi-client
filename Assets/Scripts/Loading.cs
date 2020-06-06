//------------------------------------------------------------------------------
// <copyright file="Loading.cs" company="FurtherSystem Co.,Ltd.">
// Copyright (C) 2020 FurtherSystem Co.,Ltd.
// This software is released under the MIT License.
// http://opensource.org/licenses/mit-license.php
// </copyright>
// <author>FurtherSystem Co.,Ltd.</author>
// <email>info@furthersystem.com</email>
// <summary>
// Loading Action Script
// </summary>
//------------------------------------------------------------------------------

using UnityEngine;
using UnityEngine.UI;

public class Loading : MonoBehaviour
{
    [SerializeField]
    private Image bar1;
    [SerializeField]
    private Image bar2;
    [SerializeField]
    private Image bar3;
    [SerializeField]
    private Image bar4;
    [SerializeField]
    private Image bar5;

    private float bar1fill;
    private float bar2fill;
    private float bar3fill;
    private float bar4fill;
    private float bar5fill;
    private bool bar1BoundFlag;
    private bool bar2BoundFlag;
    private bool bar3BoundFlag;
    private bool bar4BoundFlag;
    private bool bar5BoundFlag;

    public float Volume = 0.01f;
    public float BoundingLine = 0.2f;

    void Start()
    {
        bar1fill = 1f;
        bar2fill = 0.75f;
        bar3fill = 0.5f;
        bar4fill = 0.25f;
        bar5fill = 0f;
        bar1.fillAmount = bar1fill;
        bar2.fillAmount = bar2fill;
        bar3.fillAmount = bar3fill;
        bar4.fillAmount = bar4fill;
        bar5.fillAmount = bar5fill;
        bar1BoundFlag = false;
        bar2BoundFlag = false;
        bar3BoundFlag = false;
        bar4BoundFlag = false;
        bar5BoundFlag = false;
    }

    void Update()
    {
        bar1.fillAmount = UpdateFillAmount(ref bar1fill, ref bar1BoundFlag);
        bar2.fillAmount = UpdateFillAmount(ref bar2fill, ref bar2BoundFlag);
        bar3.fillAmount = UpdateFillAmount(ref bar3fill, ref bar3BoundFlag);
        bar4.fillAmount = UpdateFillAmount(ref bar4fill, ref bar4BoundFlag);
        bar5.fillAmount = UpdateFillAmount(ref bar5fill, ref bar5BoundFlag);
    }

    private float UpdateFillAmount(ref float inAmount, ref bool flag)
    {
        inAmount =  (flag) ? inAmount - Volume : inAmount + Volume;
        if (inAmount > (1f + BoundingLine)) { inAmount = 1f; flag = true; }
        if (inAmount < -(0f + BoundingLine)) { inAmount = 0f; flag = false; }

        return inAmount;
    }
}
