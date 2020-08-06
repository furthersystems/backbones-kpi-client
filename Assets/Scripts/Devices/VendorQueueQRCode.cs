using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ZXing;
using ZXing.QrCode;

[RequireComponent(typeof(SVGImage))]
public class VendorQueueQRCode : MonoBehaviour
{
    SVGImage QRImage;
    Sprite sprite;
    Texture2D texture;
    void Start()
    {
        QRImage = GetComponent<SVGImage>();
        Create("test", 200, 200);
    }

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