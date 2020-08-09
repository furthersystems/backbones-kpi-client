//------------------------------------------------------------------------------
// <copyright file="EnqueuePanelController.cs" company="FurtherSystem Co.,Ltd.">
// Copyright (C) 2020 FurtherSystem Co.,Ltd.
// This software is released under the MIT License.
// http://opensource.org/licenses/mit-license.php
// </copyright>
// <author>FurtherSystem Co.,Ltd.</author>
// <email>info@furthersystem.com</email>
// <summary>
// Enqueue Panel Controller
// </summary>
//------------------------------------------------------------------------------
using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using ZXing;
namespace Com.FurtherSystems.vQL.Client
{
    
    public class EnqueuePanelController : MonoBehaviour, PanelControllerInterface
    {
        [SerializeField]
        GameObject content;
        [SerializeField]
        RawImage qrScreen;
        string qrCodeText = null;
        WebCamTexture webCam;
        Quaternion baseRotation;
        bool qrLoaded = false;
        PanelSwitcher panelSwitcher;
        int rotationZ = 0;
        RectTransform qrScreenRectTransform;
        WebCamDevice webCamDevice;
        [SerializeField]
        Text rotateDebugLabel;

        enum RotateStatus
        {
            Default,
            BeforePortrait,
            BeforeLandscape,
        }
        RotateStatus qrRotateStatus = RotateStatus.Default;

        public PanelType GetPanelType()
        {
            return PanelType.Enqueue;
        }

        public void Initialize(PanelSwitcher switcher)
        {
            panelSwitcher = switcher;
        }

        public bool IsShowing()
        {
            return content.activeSelf;
        }

        public IEnumerator Show()
        {
            content.SetActive(true);
            StartCoroutine(Initialize());
            yield return null;
        }


        public IEnumerator Dismiss()
        {
            content.SetActive(false);
            yield return null;
        }

        public void CallEnqueue()
        {
            StartCoroutine(Enqueue());
        }

        IEnumerator Enqueue()
        {
            yield return panelSwitcher.PopLoadingDialog();
            var nonce = Instance.WebAPIClient.GetTimestamp();
            var ticks = Instance.WebAPIClient.GetUnixTime();
            var vendorQueueCode = qrCodeText;
            var codeArray = vendorQueueCode.Split(',');
            var vendorCode = string.Empty;
            var queueCode = string.Empty;
            if (codeArray.Length > 0) vendorCode = codeArray[0];
            if (codeArray.Length > 1) queueCode = codeArray[1];
            yield return StartCoroutine(Instance.WebAPI.Enqueue(vendorCode, queueCode, ticks, nonce));
            if (Instance.WebAPI.Result)
            {
                var data = Instance.WebAPI.DequeueResultData<Messages.Response.Enqueue>();

                var vendor = new Vendor();
                vendor.VendorCode = vendorCode;
                vendor.QueueCode = queueCode;
                vendor.KeyCodePrefix = data.KeyCodePrefix;
                vendor.KeyCodeSuffix = data.KeyCodeSuffix;
                vendor.PersonsWaitingBefore = data.PersonsWaitingBefore;
                vendor.TotalWaiting = data.TotalWaiting;
                Instance.Vendors.SetVendor(vendorCode, vendor);

                yield return panelSwitcher.Fade(PanelType.Main);
                yield return panelSwitcher.DepopLoadingDialog();
            }
            else
            {
                yield return panelSwitcher.PopErrorDialog();
                yield return panelSwitcher.DepopLoadingDialog();
            }
        }

        public void CallFadeView()
        {
            StartCoroutine(FadeView());
        }

        IEnumerator FadeView()
        {
            yield return panelSwitcher.PopLoadingDialog();
            yield return panelSwitcher.Fade(PanelType.View);
            yield return panelSwitcher.DepopLoadingDialog();
        }

        IEnumerator Initialize()
        {
            yield return Application.RequestUserAuthorization(UserAuthorization.WebCam);
            if (Application.HasUserAuthorization(UserAuthorization.WebCam) == false)
            {
                Debug.Log("no camera.");
                yield break;
            }
            Debug.Log("camera ok.");
            WebCamDevice[] devices = WebCamTexture.devices;
            if (devices == null || devices.Length == 0)
                yield break;
            var outCameraIndex = -1;
            for (int index = 0; index < devices.Length; index++)
            {
                if (!devices[index].isFrontFacing) {
                    outCameraIndex = index;
                }
            }
            if (outCameraIndex == -1) yield break;

            webCamDevice = devices[outCameraIndex];

            qrRotateStatus = RotateStatus.Default;
            qrScreenRectTransform = qrScreen.GetComponent<RectTransform>();
            //webCam = new WebCamTexture(devices[0].name, (int)qrScreenRectTransform.sizeDelta.x, (int)qrScreenRectTransform.sizeDelta.y, 10);
            webCam = new WebCamTexture(webCamDevice.name);
            qrScreen.texture = webCam;
            webCam.Play();
            qrLoaded = false;
        }

        void Update()
        {
            if (qrLoaded) return;
            if (webCam != null)
            {
                UpdateQRCameraRotation();
                qrCodeText = Read(webCam);
                if (qrCodeText != null)
                {
                    Debug.Log("result : " + qrCodeText);
                    qrLoaded = true;
                    StartCoroutine(Enqueue());
                }
            }
        }

        void UpdateQRCameraRotation()
        {
            var orientation = Input.deviceOrientation;
            switch (orientation)
            {
                case DeviceOrientation.Unknown:
                case DeviceOrientation.FaceUp:
                case DeviceOrientation.FaceDown:
                    if (qrRotateStatus != RotateStatus.Default) return;
                    orientation = DeviceOrientation.Portrait;
                    break;
                default:
                    break;
            }
            rotationZ = 0;
            switch (orientation)
            {
                case DeviceOrientation.Portrait:
                    rotationZ = -90;
                    qrRotateStatus = RotateStatus.BeforePortrait;
                    break;
                case DeviceOrientation.LandscapeRight:
                    rotationZ = -180;
                    qrRotateStatus = RotateStatus.BeforeLandscape;
                    break;
                case DeviceOrientation.PortraitUpsideDown:
                    rotationZ = 90;
                    qrRotateStatus = RotateStatus.BeforePortrait;
                    break;
                case DeviceOrientation.LandscapeLeft:
                    rotationZ = 0;
                    qrRotateStatus = RotateStatus.BeforeLandscape;
                    break;
                default: break;
            }

            switch (orientation)
            {
                case DeviceOrientation.Portrait:
                case DeviceOrientation.PortraitUpsideDown:
                    qrScreenRectTransform.sizeDelta = new Vector2(800, 600);
                    if (Application.platform == RuntimePlatform.IPhonePlayer) qrScreenRectTransform.localScale = new Vector3(1f, -1f, 1f);
                    break;
                case DeviceOrientation.LandscapeRight:
                case DeviceOrientation.LandscapeLeft:
                    qrScreenRectTransform.sizeDelta = new Vector2(800, 600);
                    if (Application.platform == RuntimePlatform.IPhonePlayer) qrScreenRectTransform.localScale = new Vector3(-1f, 1f, 1f);
                    break;
                default: break;
            }
            var isCameraType = webCamDevice.isFrontFacing ? "isFaceCamera" : "isOutCamera" ;
            rotateDebugLabel.text = isCameraType + ":" + Input.deviceOrientation.ToString();
            qrScreenRectTransform.rotation = Quaternion.Euler(qrScreenRectTransform.rotation.x, qrScreenRectTransform.rotation.y, rotationZ);
        }

        string Read(WebCamTexture tex)
        {
            BarcodeReader reader = new BarcodeReader();
            int w = tex.width;
            int h = tex.height;
            var pixel32s = tex.GetPixels32();
            var r = reader.Decode(pixel32s, w, h);
            return r?.Text;
        }
    }
}