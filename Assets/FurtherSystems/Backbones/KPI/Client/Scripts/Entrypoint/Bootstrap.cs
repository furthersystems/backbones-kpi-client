//------------------------------------------------------------------------------
// <copyright file="Bootstrap.cs" company="FurtherSystem Co.,Ltd.">
// Copyright (C) 2020 FurtherSystem Co.,Ltd.
// This software is released under the MIT License.
// http://opensource.org/licenses/mit-license.php
// </copyright>
// <author>FurtherSystem Co.,Ltd.</author>
// <email>info@furthersystem.com</email>
// <summary>
// Bootstrap
// </summary>
//------------------------------------------------------------------------------
using System.Collections;
using UnityEngine;

namespace Com.FurtherSystems.Backbones.KPI.Client
{
    public class Bootstrap : MonoBehaviour
    {
        IEnumerator Start()
        {
            Instance.Initialize();
            var ticks = Instance.WebAPIClient.GetUnixTime();
            var nonce = Instance.WebAPIClient.GetTimestamp();
            var seed = string.Empty;
            var priv = string.Empty;
            yield return StartCoroutine(Instance.WebAPI.CreateScheduled(nonce));
            if (!Instance.WebAPI.Result)
            {
                Debug.Log("send failed.");
                yield return new WaitForSeconds(3.1f);

            }
            yield return StartCoroutine(Instance.WebAPI.SendScheduled(nonce));
            if (!Instance.WebAPI.Result)
            {
                Debug.Log("send failed.");
                yield return new WaitForSeconds(3.1f);

            }
            yield return StartCoroutine(Instance.WebAPI.SearchScheduled(nonce));
            if (!Instance.WebAPI.Result)
            {
                Debug.Log("send failed.");
                yield return new WaitForSeconds(3.1f);

            }

        }
    }
}