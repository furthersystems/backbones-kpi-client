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

namespace Com.FurtherSystems.vQL.Client
{
    [RequireComponent(typeof(PanelSwitcher))]
    public class Bootstrap : MonoBehaviour
    {
        PanelSwitcher panelSwitcher;
        IEnumerator Start()
        {
            Instance.Initialize();
            panelSwitcher = GetComponent<PanelSwitcher>();
            panelSwitcher.Initialize();
            yield return panelSwitcher.PopLoadingDialog();

            // TODO check network connectivity.
            // TODO error dialog here & exit

            Screen.autorotateToPortrait = false;
            Screen.autorotateToLandscapeLeft = false;
            Screen.autorotateToLandscapeRight = false;
            Screen.autorotateToPortraitUpsideDown = false;

            do
            {
                var ticks = Instance.WebAPIClient.GetUnixTime();
                var nonce = Instance.WebAPIClient.GetTimestamp();
                var seed = string.Empty;
                var priv = string.Empty;
                if (!Storage.Exists(Storage.Type.Seed))
                {
                    (seed, ticks) = Instance.Ident.CreateSeed(Instance.WebAPIClient.GetPlatform(), ticks);
                    Debug.Log("first boot seed created.");
                }
                else
                {
                    (seed, ticks) = Instance.Ident.GetSeed();
                    Debug.Log("seed loaded.");
                }

                if (!Storage.Exists(Storage.Type.PrivateCode))
                {
                    yield return panelSwitcher.DepopLoadingDialog();
                    yield return panelSwitcher.ModalAgreementDialog();
                    yield return panelSwitcher.ModalAuthDialog();
                    yield return panelSwitcher.ModalSubscribeDialog();
                    yield return panelSwitcher.PopLoadingDialog();

                    // Validate Instance.Ident.CheckedAgreement
                    // Validate Instance.Ident.AgreementVersion
                    // Validate Instance.Ident.CurrentActivateType
                    // Validate Instance.Ident.ActivateKeyword

                    yield return StartCoroutine(Instance.WebAPI.CreateAccount(Instance.Ident.CheckedAgreement, Instance.Ident.AgreementVersion, 
                        Instance.Ident.CurrentActivateType, Instance.Ident.ActivateKeyword,
                        Instance.Ident.AddNonce(seed, nonce), Instance.Ident.GetPlatformIdentifier(), ticks, nonce));
                    if (!Instance.WebAPI.Result)
                    {
                        // TODO error dialog here & exit
                        Debug.Log("logon failed.");
                        yield return panelSwitcher.DepopLoadingDialog();
                        yield return panelSwitcher.PopErrorDialog();

                        yield return panelSwitcher.PopLoadingDialog();
                        yield return new WaitForSeconds(3.1f);
                        continue;
                    }
                    var data = Instance.WebAPI.DequeueResultData<Messages.Response.Create>();
                    Debug.Log("data.ResponseCode: " + data.ResponseCode.ToString());
                    Debug.Log("data.PrivateCode: " + data.PrivateCode);
                    Debug.Log("data.SessionId: " + data.SessionId);
                    Debug.Log("data.SessionPrivate: " + data.SessionPrivate);
                    Debug.Log("data.Ticks:" + data.Ticks.ToString());
                    Instance.Ident.SetPrivateCode(data.PrivateCode);
                    Instance.Ident.SetSessionId(data.SessionId);
                    Instance.Ident.SetSessionPrivate(data.SessionPrivate);
                    Debug.Log("private key created.");
                }
                else
                {
                    priv = Instance.Ident.GetPrivateKey();
                    Debug.Log("data.PrivateCode: " + priv);
                    yield return StartCoroutine(Instance.WebAPI.Logon(priv, ticks, nonce));
                    if (!Instance.WebAPI.Result)
                    {
                        // TODO error dialog here & exit
                        Debug.Log("logon failed.");
                        yield return panelSwitcher.DepopLoadingDialog();
                        yield return panelSwitcher.PopErrorDialog();

                        yield return panelSwitcher.PopLoadingDialog();
                        yield return new WaitForSeconds(3.1f);
                        continue;
                    }
                    var data = Instance.WebAPI.DequeueResultData<Messages.Response.Logon>();
                    Debug.Log("data.ResponseCode: " + data.ResponseCode.ToString());
                    Debug.Log("data.SessionId: " + data.SessionId);
                    Debug.Log("data.SessionPrivate: " + data.SessionPrivate);
                    Debug.Log("data.Ticks:" + data.Ticks.ToString());
                    Instance.Ident.SetSessionId(data.SessionId);
                    Instance.Ident.SetSessionPrivate(data.SessionPrivate);
                    Debug.Log("private key created.");
                }
                Debug.Log("logon ok.");

                if (!Storage.Exists(Storage.Type.Latest))
                {
                    Debug.Log("latest not found. open enqueue.");
                    yield return panelSwitcher.Fade(PanelType.Enqueue);
                    yield return panelSwitcher.DepopLoadingDialog();
                    break;
                }

                var latest = Storage.Load(Storage.Type.Latest);
                yield return panelSwitcher.Fade((PanelType)int.Parse(latest));
                yield return panelSwitcher.DepopLoadingDialog();

                break;
            } while (true);

            yield break;
        }
    }
}