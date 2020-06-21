//------------------------------------------------------------------------------
// <copyright file="WebAPIClient.cs" company="FurtherSystem Co.,Ltd.">
// Copyright (C) 2020 FurtherSystem Co.,Ltd.
// This software is released under the MIT License.
// http://opensource.org/licenses/mit-license.php
// </copyright>
// <author>FurtherSystem Co.,Ltd.</author>
// <email>info@furthersystem.com</email>
// <summary>
// vQL WebAPI Client
// </summary>
//------------------------------------------------------------------------------
using Org.BouncyCastle.Crypto.Engines;
using Org.BouncyCastle.Crypto.Paddings;
using System;
using System.Collections;
using System.Security.Cryptography;
using System.Text;
using UnityEngine;
using UnityEngine.Networking;

namespace Com.FurtherSystems.vQL.Client
{
    public enum IdentiferType : byte
    {
        None = 0,
        GSuite = 1,
        Twitter = 2,
        Facebook = 3,
        LINE = 4,
    }
    public class WebAPIClient : MonoBehaviour
    {
        const string Url = "http://192.168.1.30:7000";
        const string UserAgent = "vQLClient Unity";
        const string ClientVersion = "v1.0.0";

        const string traditionalKey = "KIWIKIWIKIWIKIWIKIWIKIWIKIWIKIWI";

        private static DateTime UNIX_EPOCH = new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc);

        const int RetryCount = 3;
        const float Timeout = 15f;

        [Serializable]
        struct ReqBodyCreate
        {
            public byte IdentifierType;
            public string Identifier;
            public string Seed;
            public string Name;
            public string Caption;
            public long Ticks;
        }

        public static string GetPlatform()
        {
#if UNITY_STANDALONE_WIN
            return "Windows";
#elif UNITY_STANDALONE_LINUX
            return "Linux";
#elif UNITY_STANDALONE_OSX
            return "MacOSX";
#elif UNITY_IOS
            return "iOS";
#elif UNITY_ANDROID
            return "Android";
#endif
        }

        public static long GetTimestamp()
        {
            return DateTime.UtcNow.Ticks;
        }

        public static long GetUnixTime()
        {
            var targetTime = DateTime.UtcNow.ToUniversalTime();
            TimeSpan elapsedTime = targetTime - UNIX_EPOCH;
            return (long)elapsedTime.TotalSeconds;
        }

        string Encode(object obj)
        {
            var postDataJsonBytes = Encoding.UTF8.GetBytes(JsonUtility.ToJson(obj));
            //Debug.Log("send json bytes: " + BitConverter.ToString(postDataJsonBytes));
            //var encoded = new BouncyCastleWrapper(new AesEngine(), new Pkcs7Padding()).Encrypt(postDataJsonBytes, traditionalKey);
            //Debug.Log("send encrypted bytes: " + BitConverter.ToString(encoded));
            //Debug.Log("send base64 bytes: " + BitConverter.ToString(Encoding.UTF8.GetBytes(Convert.ToBase64String(encoded))));
            return Convert.ToBase64String(postDataJsonBytes);
        }

        public bool CreateResult { get; set; }
        public string CreateResultData { get; private set; }
        public IEnumerator Create(string name, string caption, string seed, string ident, long ticks, long nonce)
        {
            Debug.Log("Create start");
            ReqBodyCreate reqBody;
            reqBody.IdentifierType = (byte) IdentiferType.None;
            reqBody.Identifier = ident;
            reqBody.Seed = seed;
            reqBody.Name = name;
            reqBody.Caption = caption;
            reqBody.Ticks = ticks;

            Debug.Log("Create req send");
            UnityWebRequest req;
            var counter = 1;
            do
            {
                var timer = 0f;
                req = UnityWebRequest.Post(Url + "/vendor/new", Encode(reqBody));
                req.SetRequestHeader("User-Agent", UserAgent + " " + ClientVersion);
                req.SetRequestHeader("Platform", GetPlatform());
                req.SetRequestHeader("Nonce", nonce.ToString());
                req.SendWebRequest();
                while (true)
                {
                    if (req.isDone) break;
                    else if (timer > Timeout) break;

                    yield return new WaitForSeconds(0.5f);
                    timer += 0.5f;
                }

                if (req.isDone) break;

                if (counter < RetryCount)
                {
                    counter++;
                    Debug.Log("Create req send retry " + counter.ToString());
                    continue;
                }

                break;
            } while (true);

            if (!req.isDone || counter >= RetryCount)
            {
                Debug.Log("Create req retry over failed");
                CreateResult = false;
                yield break;
            }

            if (req.isNetworkError)
            {
                Debug.LogError(req.error);
                CreateResult = false;
                yield break;
            }
            CreateResultData = req.downloadHandler.text;
            if (req.responseCode != 200)
            {
                Debug.Log("http status code:" + req.responseCode);
                CreateResult = false;
                yield break;
            }

            Debug.Log("Create end" + CreateResultData);
            CreateResult = true;
        }
    }
}