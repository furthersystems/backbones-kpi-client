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
using UnityEngine;

public class QRScanCamera : MonoBehaviour
{
    int fps = 20;
    WebCamTexture webcamTexture;

    void Start()
    {
        var rectTransform = GetComponent<RectTransform>();
        if (0 < WebCamTexture.devices.Length)
        {
            WebCamDevice[] devices = WebCamTexture.devices;
            webcamTexture = new WebCamTexture(devices[0].name, (int)rectTransform.rect.width, (int)rectTransform.rect.height, this.fps);
            GetComponent<Renderer>().material.mainTexture = webcamTexture;
            webcamTexture.Play();
        }
    }
}