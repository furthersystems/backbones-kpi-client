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

            const string Url = "http://192.168.1.30:7000";
            const string UserAgent = "vQLClient Unity";
            const string ClientVersion = "v1.0.0";

            const string traditionalKey = "KIWIKIWIKIWIKIWIKIWIKIWIKIWIKIWI";

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

            string Encode(object obj, long iv)
            {
                var postDataJsonBytes = Encoding.UTF8.GetBytes(JsonUtility.ToJson(obj));
                //Debug.Log("send json bytes: " + BitConverter.ToString(postDataJsonBytes));
                //var encoded = new BouncyCastleWrapper(new AesEngine(), new Pkcs7Padding()).Encrypt(postDataJsonBytes, traditionalKey);
                //Debug.Log("send encrypted bytes: " + BitConverter.ToString(encoded));
                //Debug.Log("send base64 bytes: " + BitConverter.ToString(Encoding.UTF8.GetBytes(Convert.ToBase64String(encoded))));
                return Convert.ToBase64String(postDataJsonBytes);
            }

            public T Decode<T>(string value, long iv)
            {
                var base64Decoded = Convert.FromBase64String(value);
                var json = Encoding.UTF8.GetString(base64Decoded);
                return JsonUtility.FromJson<T>(json);
            }

            private string ToSafe(string base64str)
            {
                return base64str.Replace('=', '-').Replace('+', '.').Replace('/', '_');
            }

            public bool Result { get; set; }

            private class ResultData
            {
                public string Data;
                public long IV;
                public ResultData(string data, long iv)
                {
                    Data = data;
                    IV = iv;
                }
            }

            ConcurrentQueue<ResultData> resultQueue = null;
            public T DequeueResultData<T>()
            {
                ResultData result = null;
                Debug.Log("result data queue count: " + resultQueue.Count.ToString());
                if (resultQueue.Count > 0)
                {
                    resultQueue.TryDequeue(out result);
                }
                // decode here.
                return Decode<T>(result.Data, result.IV);
            }

            private void enqueueResultData(ResultData value)
            {
                resultQueue.Enqueue(value);
            }

            private void clearResultData()
            {
                if (resultQueue == null)
                {
                    resultQueue = new ConcurrentQueue<ResultData>();
                }
                ResultData result;
                while (resultQueue.Count > 0)
                {
                    resultQueue.TryDequeue(out result);
                }
            }

            public IEnumerator CreateAccount(string seed, string ident, long ticks, long nonce)
            {
                Debug.Log("Create start");
                clearResultData();

                var reqBody = new Messages.Request.Create
                {
                    IdentifierType = (byte)IdentifierType.None,
                    Identifier = ident,
                    Seed = seed,
                    Ticks = ticks
                };

                return Request(RequestType.Post, "/new", reqBody, nonce);
            }

            public IEnumerator Logon(string privateCode, long ticks, long nonce)
            {
                Debug.Log("Logon start");
                clearResultData();

                var reqBody = new Messages.Request.Logon
                {
                    PrivateCode = privateCode,
                    Ticks = ticks
                };

                return Request(RequestType.Post, "/logon", reqBody, nonce);
            }

            public IEnumerator Enqueue(string vendorCode, string queueCode, long ticks, long nonce)
            {
                Debug.Log("Enqueue start");
                clearResultData();

                var reqBody = new Messages.Request.Enqueue
                {
                    VendorCode = vendorCode,
                    QueueCode = queueCode,
                    Ticks = ticks
                };

                return Request(RequestType.Post, "/on/queue", reqBody, nonce);
            }

            public IEnumerator Get(string vendorCode, string queueCode, long ticks, long nonce)
            {
                Debug.Log("Get start");
                clearResultData();

                return Request(RequestType.Get, "/on/queue/"+ ToSafe(vendorCode) + "/"+ ToSafe(queueCode), null, nonce);
            }

            public IEnumerator SetVendor(string name, string caption, string sessionId, string ident, long ticks, long nonce)
            {
                Debug.Log("SetVendor start");
                clearResultData();

                var reqBody = new Messages.Request.VendorSetting
                {
                    Name = name,
                    Caption = caption,
                    Ticks = ticks
                };

                return Request(RequestType.Post, "/on/vendor/upgrade", reqBody, nonce);
            }

            public IEnumerator VendorManage(string queueCode, long ticks, long nonce)
            {
                Debug.Log("VendorManage start");
                clearResultData();
                if (!string.IsNullOrEmpty(queueCode))
                {
                    queueCode = ToSafe(queueCode);
                }
                return Request(RequestType.Get, "/on/vendor/manage/" + queueCode, null, nonce);
            }

            public IEnumerator NewQueue(bool requireAdmit, long ticks, long nonce)
            {
                Debug.Log("new queue start");
                clearResultData();

                var reqBody = new Messages.Request.NewQueue
                {
                    RequireAdmit = requireAdmit,
                    RequireTimeEstimate = false,
                    KeyCodeType = 0,
                    KeyCodePrefix = "",
                    Ticks = ticks
                };

                return Request(RequestType.Post, "/on/vendor/queue/new", reqBody, nonce);
            }

            private IEnumerator Request(RequestType type, string path, object postData, long nonce)
            {

                Debug.Log(path + " start");
                clearResultData();
                
                UnityWebRequest req;
                var counter = 1;
                do
                {
                    var timer = 0f;
                    long ivReq = 0;
                    if (type == RequestType.Post)
                    {
                        req = UnityWebRequest.Post(Url + path, Encode(postData, ivReq));
                    }
                    else if (type == RequestType.Put)
                    {
                        req = UnityWebRequest.Put(Url + path, Encode(postData, ivReq));
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
                    req.SetRequestHeader("Platform", GetPlatform());
                    req.SetRequestHeader("Nonce", nonce.ToString());
                    req.SetRequestHeader("IV", ivReq.ToString());
                    req.SetRequestHeader("Session", Ident.SessionId);
                    req.SetRequestHeader("Hash", Ident.GenerateHash(Ident.SessionPrivate + nonce.ToString(), Storage.ServiceUniqueKey));
                    
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
                enqueueResultData(new ResultData(req.downloadHandler.text, ivRes));
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