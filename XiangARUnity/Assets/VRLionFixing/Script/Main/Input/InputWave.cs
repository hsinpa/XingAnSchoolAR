﻿#if UNITY_EDITOR || UNITY_ANDROID
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Hsinpa.Input;
using wvr;

public class InputWave : InputInterface
{
    WaveVR_InputModuleManager _inputModule;
    Ray ray = new Ray();

    public Vector3 faceDir
    {
        get
        {
            var d = WaveVR_EventSystemControllerProvider.Instance.GetControllerModel(WaveVR_Controller.EDeviceType.Dominant);

            if (d == null) return Vector3.zero;
            return d.transform.forward;
        }
    }


    //0.3 => official length
    public float raycastLength => 1f;

    public InputWave(WaveVR_InputModuleManager inputModule) {
        this._inputModule = inputModule; 
    }

    public bool GetMouse()
    {
        return WaveVR_Controller.Input(wvr.WVR_DeviceType.WVR_DeviceType_Controller_Right).GetPress(WVR_InputId.WVR_InputId_Alias1_Trigger);
    }

    public bool GetMouseDown()
    {
        return WaveVR_Controller.Input(wvr.WVR_DeviceType.WVR_DeviceType_Controller_Right).GetPressDown(WVR_InputId.WVR_InputId_Alias1_Trigger);
    }

    public bool GetMouseUp()
    {
        return WaveVR_Controller.Input(wvr.WVR_DeviceType.WVR_DeviceType_Controller_Right).GetPressUp(WVR_InputId.WVR_InputId_Alias1_Trigger);
    }

    public Ray GetRay()
    {
        var d = WaveVR_EventSystemControllerProvider.Instance.GetControllerModel(WaveVR_Controller.EDeviceType.Dominant);
        if (d == null) return ray;

        ray.direction = d.transform.forward;
        ray.origin = d.transform.position;

        return ray;
    }

    public bool SwipeLeft()
    {
        return WaveVR_Controller.Input(wvr.WVR_DeviceType.WVR_DeviceType_Controller_Right).GetPress(WaveVR_ButtonList.EButtons.DPadLeft);
    }

    public bool SwipeRight()
    {
        return WaveVR_Controller.Input(wvr.WVR_DeviceType.WVR_DeviceType_Controller_Right).GetPress(WaveVR_ButtonList.EButtons.DPadRight);
    }

    public Transform GetParent()
    {
        return WaveVR_EventSystemControllerProvider.Instance.GetControllerModel(WaveVR_Controller.EDeviceType.Dominant).transform;
    }

    public void SwitchControllerModel(bool isOn) {
        Transform controlParent = GetParent();

        foreach (Transform child in controlParent) {
            child.gameObject.SetActive(isOn);
        }
    }

    public bool ClickOnMenuKey()
    {
        return WaveVR_Controller.Input(wvr.WVR_DeviceType.WVR_DeviceType_Controller_Right).GetPressDown(WaveVR_ButtonList.EButtons.Menu);
    }
}
#endif