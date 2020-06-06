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
using System.Collections;
using UnityEngine;
using UnityEngine.Android;
using UnityEngine.iOS;
using UnityEngine.Windows;
using UnityEngine.UI;
using ZXing;

public class QRScanCamera : MonoBehaviour
{
    enum ScanStatus {
        None,
        Initialize,
        Scanning,
    }

    [SerializeField]
    public RawImage rawImage;
    [SerializeField]
    public Text text;
    [SerializeField]
    public int timeoutSeconds;

    private ScanStatus status = ScanStatus.None;
    private WebCamTexture webCamTexture;
    private bool isAbort;

    void Start()
    {
        status = ScanStatus.None;
        StartScan();// temporary check
    }

    void Update()
    {
        switch (status)
        {
            case ScanStatus.Initialize:
                InitializeScan();
                break;
            case ScanStatus.Scanning:
                LoopScan();
                break;
            case ScanStatus.None:
            default:
                break;
        }
    }

    public void StartScan()
    {
        Permission.RequestUserPermission(Permission.Camera);
        isAbort = false;
        status = ScanStatus.Initialize;
    }

    void InitializeScan()
    {
        if (Permission.HasUserAuthorizedPermission(Permission.Camera))
        {
            var rectTransform = GetComponent<RectTransform>();
            webCamTexture = new WebCamTexture((int)rectTransform.rect.width, (int)rectTransform.rect.height);
            webCamTexture.Play();
            rawImage.texture = webCamTexture;
            isAbort = false;
            status = ScanStatus.Scanning;
        }
        // TODO check timeout here.
        // TODO check abort here.
    }

    void LoopScan()
    {
        var reader = new BarcodeReader();
        var readed = reader.Decode(webCamTexture.GetPixels32(), webCamTexture.width, webCamTexture.height)?.Text;
        if (!string.IsNullOrEmpty(readed)) {
            text.text = readed;
            webCamTexture.Stop();
            // TODO check readed text.
            status = ScanStatus.None;
        }
        // TODO check timeout here.
        // TODO check abort here.
    }

    public void Cancel()
    {
        isAbort = true;
    }
}