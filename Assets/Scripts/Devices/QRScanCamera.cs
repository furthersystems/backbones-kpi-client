//------------------------------------------------------------------------------
// <copyright file="QRScanCamera.cs" company="FurtherSystem Co.,Ltd.">
// Copyright (C) 2020 FurtherSystem Co.,Ltd.
// This software is released under the MIT License.
// http://opensource.org/licenses/mit-license.php
// </copyright>
// <author>FurtherSystem Co.,Ltd.</author>
// <email>info@furthersystem.com</email>
// <summary>
// QR Scan Camera Script
// </summary>
//------------------------------------------------------------------------------
using System.Threading;
using UnityEngine;
//using UnityEngine.Android;
//using UnityEngine.iOS;
//using UnityEngine.Windows;
using UnityEngine.UI;
using ZXing;
using ZXing.QrCode;

namespace Com.FurtherSystems.vQL.Client
{
    public class QRScanCamera : MonoBehaviour
    {
        enum ScanStatus
        {
            None,
            Initialize,
            Scanning,
        }

        [SerializeField]
        RawImage rawImage;
        [SerializeField]
        Text text;
        [SerializeField]
        int timeoutSeconds;

        WebCamTexture camTexture;
        Thread qrThread;
        bool isQuit;
        int W, H;
        //string LastResult;
        //bool shouldEncodeNow;
        Color32[] c;
        Rect screenRect;

        ScanStatus status = ScanStatus.None;
        WebCamTexture webCamTexture;
        bool isAbort;

        void OnGUI()
        {
            GUI.DrawTexture(screenRect, camTexture, ScaleMode.ScaleToFit);
        }

        void OnEnable()
        {
            if (camTexture != null)
            {
                camTexture.Play();
                W = camTexture.width;
                H = camTexture.height;
            }
        }

        void Start()
        {
            //LastResult = "http://www.google.com";
            //shouldEncodeNow = true;

            screenRect = new Rect(0, 0, Screen.width, Screen.height);

            camTexture = new WebCamTexture();
            camTexture.requestedHeight = Screen.height; // 480;
            camTexture.requestedWidth = Screen.width; //640;
            OnEnable();
            qrThread = new Thread(DecodeQR);
            qrThread.Start();
        }

        void Update()
        {
            if (c == null)
            {
                c = camTexture.GetPixels32();
            }
        }
        
        void OnApplicationQuit()
        {
            isQuit = true;
        }

        void DecodeQR()
        {
            var barcodeReader = new BarcodeReader { AutoRotate = false, TryInverted = false };
            while (true)
            {
                if (isQuit)
                    break;

                try
                {
                    var result = barcodeReader.Decode(c, W, H);
                    if (result != null)
                    {
                        print(result.Text);
                    }
                    Thread.Yield();
                    Thread.Sleep(200);
                    c = null;
                }
                catch
                {
                }
            }
        }

        void OnDestroy()
        {
            qrThread.Abort();
            camTexture.Stop();
        }

        void OnDisable()
        {
            if (camTexture != null)
            {
                camTexture.Pause();
            }
        }

    }
}