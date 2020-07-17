//------------------------------------------------------------------------------
// <copyright file="ImplementFitting.cs" company="FurtherSystem Co.,Ltd.">
// Copyright (C) 2020 FurtherSystem Co.,Ltd.
// This software is released under the MIT License.
// http://opensource.org/licenses/mit-license.php
// </copyright>
// <author>FurtherSystem Co.,Ltd.</author>
// <email>info@furthersystem.com</email>
// <summary>
// ImplementFitting
// </summary>
//------------------------------------------------------------------------------
using System;
using UnityEngine;
using UnityEditor;

namespace Com.FurtherSystems.vQL.Client
{
    [System.AttributeUsage(System.AttributeTargets.Field, Inherited = true, AllowMultiple = false)]
    public class ImplementFittingAttribute : PropertyAttribute
    {
        public Type type;
        public ImplementFittingAttribute(Type type)
        {
            this.type = type;
        }
    }

    [CustomPropertyDrawer(typeof(ImplementFittingAttribute))]
    public class ImplementFittingDrawer : PropertyDrawer
    {
        public override void OnGUI(Rect pos, SerializedProperty prop, GUIContent label)
        {
            var fitting = (ImplementFittingAttribute)attribute;
            if (prop.propertyType == SerializedPropertyType.ObjectReference)
            {
                EditorGUI.ObjectField(pos, prop, fitting.type);
            }
            else
            {
                EditorGUI.PropertyField(pos, prop);
            }
        }
    }
}