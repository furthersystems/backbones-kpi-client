//------------------------------------------------------------------------------
// <copyright file="PanelControllerInterface.cs" company="FurtherSystem Co.,Ltd.">
// Copyright (C) 2020 FurtherSystem Co.,Ltd.
// This software is released under the MIT License.
// http://opensource.org/licenses/mit-license.php
// </copyright>
// <author>FurtherSystem Co.,Ltd.</author>
// <email>info@furthersystem.com</email>
// <summary>
// Panel Controller Interface
// </summary>
//------------------------------------------------------------------------------

using System.Collections;

namespace Com.FurtherSystems.vQL.Client
{
    public enum PanelType : int
    {
        Enqueue,
        View,
        Setting,
        Main,
        VendorSetting,
        VendorManage,
        VendorMain,
        SearchDialog,
        LoadingDialog,
        ErrorDialog,
        Fade,
    }

    public interface PanelControllerInterface
    {
        PanelType GetPanelType();

        void Initialize(PanelSwitcher switcher);

        bool IsShowing();

        //IEnumerator PreShow();

        IEnumerator Show();

        //IEnumerator PostShow();

        //IEnumerator PreDismiss();

        IEnumerator Dismiss();

        //IEnumerator PostDismiss();
    }
}