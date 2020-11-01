#if UNITY_EDITOR || UNITY_ANDROID
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Hsinpa.Input;
using wvr;

public class InputWave : InputInterface
{
    WaveVR_InputModuleManager _inputModule;

    public Vector3 faceDir
    {
        get
        {
            var d = WaveVR_EventSystemControllerProvider.Instance.GetControllerModel(WaveVR_Controller.EDeviceType.Dominant);

            if (d == null) return Vector3.zero;
            return d.transform.forward;
        }
    }

    public float raycastLength => 1.5f;

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
        if (d == null) return new Ray();

        return new Ray(d.transform.position, d.transform.forward);
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
}
#endif