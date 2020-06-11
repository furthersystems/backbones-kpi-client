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

        DateTime UNIX_EPOCH = new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc);

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
            // TODO check validate
            var ticks = ToUnixTime(DateTime.UtcNow);
            var savedSeed = GetSeed(GetUuid(), WebAPIClient.GetPlatform(), ticks);
            Debug.Log(savedSeed);
            var seed = savedSeed.Split(',')[0];
            long originTicks = 0;
            long.TryParse(savedSeed.Split(',')[1], out originTicks);
            yield return StartCoroutine(webApi.Create(RegistName.text, RegistCaption.text, seed, GetUuid(), originTicks));
            if (webApi.CreateResult)
            {
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
            return BitConverter.ToString(hashBytes).Replace("-", string.Empty);
        }

        string GetSeed(string uuid, string platform, long ticks)
        {
            Debug.Log(Application.persistentDataPath);
            var seed = LoadSeed(Application.persistentDataPath, FILE);
            if (seed.Equals(string.Empty))
            {
                seed = GenerateHash(uuid + platform + ticks.ToString(), MAGIC_KEY) + "," + ticks.ToString();
                SaveSeed(Application.persistentDataPath, FILE, seed);
            }

            return seed;
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

        string GetUuid()
        {
            return GenerateHash(SystemInfo.deviceUniqueIdentifier, MAGIC_KEY);
        }

        long ToUnixTime(DateTime targetTime)
        {
            targetTime = targetTime.ToUniversalTime();
            TimeSpan elapsedTime = targetTime - UNIX_EPOCH;
            return (long)elapsedTime.TotalSeconds;
        }
    }
}