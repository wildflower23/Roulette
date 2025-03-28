using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class MultiResolutionScript : MonoBehaviour
{

    private float MultiResoluation_finalScaler;
 void Start()
    {
        // if (GlobalScript.MultiResoluation_finalScaler == -1)
        // {
         MultiResoluation_finalScaler = GetMultiResScaleFactor();
        // }
        CanvasScaler canvasScaler = GetComponent<CanvasScaler>();
        canvasScaler.scaleFactor = MultiResoluation_finalScaler;
    }
    private float GetMultiResScaleFactor()
    {
        bool isIpad = false;
        //if (Application.platform == RuntimePlatform.IPhonePlayer)
        //{
        //    // Check if the device is an iPad
        //    if (SystemInfo.deviceModel.ToUpper().Contains("iPad".ToUpper()))
        //    {
        //        isIpad = true;
        //    }
        //}
        var currentWidth = Screen.width;
        var currentHight = Screen.height;
        var wRatio = currentWidth / 2556f;
        var hRatio = currentHight / 1179f;
        if (wRatio > hRatio)
        {
            MultiResoluation_finalScaler = hRatio;
        }
        else
        {
            MultiResoluation_finalScaler = wRatio;
        }
        if (isIpad)
        {
            MultiResoluation_finalScaler = 1;
        }
        return MultiResoluation_finalScaler;
    }
}

