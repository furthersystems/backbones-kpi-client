//------------------------------------------------------------------------------
// <copyright file="TouchInfo.cs" company="FurtherSystem Co.,Ltd.">
// Copyright (C) 2020 FurtherSystem Co.,Ltd.
// This software is released under the MIT License.
// http://opensource.org/licenses/mit-license.php
// </copyright>
// <author>FurtherSystem Co.,Ltd.</author>
// <email>info@furthersystem.com</email>
// <summary>
// Touch Info util class
// </summary>
//------------------------------------------------------------------------------
using UnityEngine;

namespace Com.FurtherSystems.vQL.Client
{
    public enum TouchType
    {
        None = 99,
        Began = 0,
        Moved = 1,
        Stayed = 2,
        Ended = 3,
        Canceled = 4,
    }

    public static class TouchInfo
    {
        private static Vector3 TouchPosition = Vector3.zero;

        public static TouchType Get(int index)
        {
            var touch = TouchType.None;
#if UNITY_STANDALONE_WIN || UNITY_STANDALONE_LINUX || UNITY_STANDALONE_OSX
            if (Input.GetMouseButtonDown(index)) { touch = TouchType.Began; }
            else if (Input.GetMouseButton(index)) { touch = TouchType.Moved; }
            else if (Input.GetMouseButtonUp(index)) { touch = TouchType.Ended; }
#elif UNITY_IOS || UNITY_ANDROID
        if (Input.touchCount > 0) touch = (TouchType)((int)Input.GetTouch(index).phase);
#endif
            return touch;
        }

        public static Vector3 GetPosition(int index)
        {
            var position = Vector3.zero;
#if UNITY_STANDALONE_WIN || UNITY_STANDALONE_LINUX || UNITY_STANDALONE_OSX
            var touch = Get(index);
            if (touch != TouchType.None) position = Input.mousePosition;
#elif UNITY_IOS || UNITY_ANDROID
        if (Input.touchCount > 0)
        {
            var touch = Input.GetTouch(index).position;
            position.x = touch.x;
            position.y = touch.y;
        }
#endif
            return position;
        }

        public static Vector3 GetWorldPosition(Camera camera, int index)
        {
            return camera.ScreenToWorldPoint(GetPosition(index));
        }
    }
}