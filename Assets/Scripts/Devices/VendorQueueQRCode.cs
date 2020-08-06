//------------------------------------------------------------------------------
// <copyright file="VendorQueueQRCode.cs" company="FurtherSystem Co.,Ltd.">
// Copyright (C) 2020 FurtherSystem Co.,Ltd.
// This software is released under the MIT License.
// http://opensource.org/licenses/mit-license.php
// </copyright>
// <author>FurtherSystem Co.,Ltd.</author>
// <email>info@furthersystem.com</email>
// <summary>
// Vendor Queue QRCode
// </summary>
//------------------------------------------------------------------------------
using UnityEngine;
using ZXing;
using ZXing.QrCode;

namespace Com.FurtherSystems.vQL.Client
{
    public class VendorQueueQRCode : MonoBehaviour
    {
        [SerializeField]
        SVGImage QRImage;
        Sprite sprite;
        Texture2D texture;

        public void Create(string data, int height, int width)
        {
            Destroy(texture);
            Destroy(sprite);
            texture = new Texture2D(height, width);
            var barcode = new BarcodeWriter
            {
                Format = BarcodeFormat.QR_CODE,
                Options = new QrCodeEncodingOptions
                {
                    Height = height,
                    Width = width
                }
            };
            var encoded = barcode.Write(data);
            texture.SetPixels32(encoded);
            texture.Apply();
            sprite = Sprite.Create(texture, new Rect(0, 0, height, width), Vector2.zero);
            QRImage.sprite = sprite;
        }
    }
}