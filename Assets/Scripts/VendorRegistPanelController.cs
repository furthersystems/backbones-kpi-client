//------------------------------------------------------------------------------
// <copyright file="VendorRegistPanelController.cs" company="FurtherSystem Co.,Ltd.">
// Copyright (C) 2020 FurtherSystem Co.,Ltd.
// This software is released under the MIT License.
// http://opensource.org/licenses/mit-license.php
// </copyright>
// <author>FurtherSystem Co.,Ltd.</author>
// <email>info@furthersystem.com</email>
// <summary>
// Vendor Regist Panel Controller
// </summary>
//------------------------------------------------------------------------------
using System;
using System.Text;
using System.Security.Cryptography;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using System.IO;

namespace Com.FurtherSystems.vQL.Client
{
    public class VendorRegistPanelController : MonoBehaviour, PanelControllerInterface
    {
        const string MAGIC_KEY = "KIWIKIWIKIWIKIWIKIWIKIWIKIWIKIWI";
        const string FILE = "seed";

        [SerializeField]
        WebAPIClient webApi;
        [SerializeField]
        Text RegistName;
        [SerializeField]
        Text RegistCaption;

        PanelSwitcher panelSwitcher;

        public void Initialize(PanelSwitcher switcher)
        {
            panelSwitcher = switcher;
        }

        public bool IsShowing()
        {
            return gameObject.activeSelf;
        }

        public void Show()
        {
            gameObject.SetActive(true);
        }

        public void Dismiss()
        {
            gameObject.SetActive(false);
        }

        public void CallSubmitVendorRegist()
        {
            StartCoroutine(SubmitVendorRegist());
        }

        IEnumerator SubmitVendorRegist()
        {
            panelSwitcher.PopLoadingDialog();
            var ticks = WebAPIClient.GetUnixTime();
            var nonce = WebAPIClient.GetTimestamp();
            var savedSeed = GetSeed(GetIdentifier(), WebAPIClient.GetPlatform(), ticks);
            Debug.Log(savedSeed);
            var seed = savedSeed.Split(',')[0];
            long originTicks = 0;
            long.TryParse(savedSeed.Split(',')[1], out originTicks);
            yield return StartCoroutine(webApi.Create(RegistName.text, RegistCaption.text, AddNonce(seed,nonce), GetIdentifier(), originTicks, nonce));
            if (webApi.CreateResult)
            {
                // TODO check validate
                panelSwitcher.FadeVendorManage();
                panelSwitcher.DepopLoadingDialog();
            }
            else
            {
                panelSwitcher.PopErrorDialog();
                panelSwitcher.DepopLoadingDialog();
            }
        }

        string GenerateHash(string plane, string key)
        {
            var encode = new UTF8Encoding();
            var planeBytes = encode.GetBytes(plane);
            var keyBytes = encode.GetBytes(key);
            var sha = new HMACSHA256(keyBytes);
            var hashBytes = sha.ComputeHash(planeBytes);
            return BitConverter.ToString(hashBytes).Replace("-", string.Empty).ToLower();
        }

        string GetSeed(string ident, string platform, long ticks)
        {
            Debug.Log(Application.persistentDataPath);
            var seed = LoadSeed(Application.persistentDataPath, FILE);
            if (seed.Equals(string.Empty))
            {
                Debug.Log(ident + " " + platform + " " + ticks.ToString() + "," + MAGIC_KEY);
                seed = GenerateHash(ident + platform + ticks.ToString(), MAGIC_KEY) + "," + ticks.ToString();
                SaveSeed(Application.persistentDataPath, FILE, seed);
            }

            return seed;
        }

        string AddNonce(string seed, long nonce)
        {
            Debug.Log(seed + " " + nonce.ToString() + "," + MAGIC_KEY);
            return GenerateHash(seed + nonce.ToString(), MAGIC_KEY);
        }

        void SaveSeed(string path, string file, string data)
        {
            using (var writer = new StreamWriter(Path.Combine(path, file)))
            {
                writer.WriteLine(data);
                writer.Flush();
            }
        }

        string LoadSeed(string path, string file)
        {
            var str = string.Empty;
            try
            {
                using (var reader = new StreamReader(Path.Combine(path, file)))
                {
                    str = reader.ReadToEnd();
                }
            }
            catch (Exception e)
            {
                Debug.Log("error: " + e.Message);
            }
            return str;
        }

        string GetIdentifier()
        {
            return GenerateHash(SystemInfo.deviceUniqueIdentifier, MAGIC_KEY);
        }
    }
}