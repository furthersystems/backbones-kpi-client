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
using System;
using System.Collections;
using System.Collections.Concurrent;
using System.Security.Cryptography;
using System.Text;
using UnityEngine;
using UnityEngine.Networking;

namespace Com.FurtherSystems.vQL.Client
{
    public partial class Instance
    {
        public enum IdentifierType : byte
        {
            None = 0,
            GSuite = 1,
            Twitter = 2,
            Facebook = 3,
            LINE = 4,
        }
        public partial class WebAPIClient
        {
            enum RequestType {
                Get,
                Post,
                Put,
                Delete,
            }
            //const string Url = "https://s.srvs.cc:443";
            const string Url = "http://192.168.1.38:9201";
            const string UserAgent = "vQLClient Unity";
            const string ClientVersion = "v1.0.0";

            private static DateTime UNIX_EPOCH = new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc);

            const int RetryCount = 3;
            const float Timeout = 15f;

            public WebAPIClient()
            {
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

            private string ToSafe(string base64str)
            {
                return base64str.Replace('=', '-').Replace('+', '.').Replace('/', '_');
            }

            public bool Result { get; set; }


            public IEnumerator SendScheduled(long nonce)
            {
                Debug.Log("send start");
                var reqBody = @"{
  ""uuid"" : ""XXXX - XXXX - XXXX - XXXX - XXXX"",
  ""timestamp"" : ""2021-09-16T10:00:00Z"",
  ""typename"": ""scheduled send"",
  ""location"": [141.3534235,43.0644717],
  ""OS"": ""Windows"",
  ""region"": ""Japan"",
  ""GDPR"": ""yes""
}";
                return Request(RequestType.Post, "/flexent20210523/_doc/user", reqBody);
            }

            private IEnumerator Request(RequestType type, string path, string postData)
            {
                Debug.Log(path + " start");
                UnityWebRequest req;
                var counter = 1;
                do
                {
                    Debug.Log("url"+Url + path);
                    Debug.Log("reqbody" + postData);
                    byte[] pdata = System.Text.Encoding.UTF8.GetBytes(postData);
                    var timer = 0f;
                    long ivReq = 0;
                    if (type == RequestType.Post)
                    {
                        //req = UnityWebRequest.Post(Url + path, Encode(postData, ivReq));
                        //req = UnityWebRequest.Post(Url + path, postData);
                        req = new UnityWebRequest(Url + path);
                    }
                    else if (type == RequestType.Put)
                    {
                        //req = UnityWebRequest.Put(Url + path, Encode(postData, ivReq));
                        req = UnityWebRequest.Put(Url + path, postData);
                    }
                    else if (type == RequestType.Delete)
                    {
                        req = UnityWebRequest.Delete(Url + path);
                    }
                    else if (type == RequestType.Get)
                    {
                        req = UnityWebRequest.Get(Url + path);
                    }
                    else
                    {
                        req = UnityWebRequest.Get(Url + path);
                    }
                    req.SetRequestHeader("User-Agent", UserAgent + " " + ClientVersion);
                    req.SetRequestHeader("Content-Type", "application/json");
                    req.uploadHandler = (UploadHandler)new UploadHandlerRaw(pdata);
                    req.downloadHandler = (DownloadHandler)new DownloadHandlerBuffer();
                    req.timeout = 10;
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
                    Result = false;
                    yield break;
                }

                if (req.isNetworkError)
                {
                    Debug.LogError(req.error);
                    Result = false;
                    yield break;
                }
                long ivRes = 0;
                Debug.Log("result"+req.downloadHandler.text);
                //enqueueResultData(new ResultData(req.downloadHandler.text, ivRes));
                if (req.responseCode != 200)
                {
                    Debug.Log("http status code:" + req.responseCode);
                    Result = false;
                    yield break;
                }

                Debug.Log(path + " end " + req.downloadHandler.text);
                Result = true;
            }
        }
    }
}