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
            if (this._inputModule.Controller.DominantController == null) return Vector3.zero;
            return _inputModule.Controller.DominantController.transform.forward;
        }
    }

    public InputWave(WaveVR_InputModuleManager inputModule) {
        this._inputModule = inputModule; 
    }

    public bool GetMouse()
    {
        return WaveVR_Controller.Input(wvr.WVR_DeviceType.WVR_DeviceType_Controller_Right).GetPress(WVR_InputId.WVR_InputId_Alias1_Trigger);
    }

    public bool GetMouseDown()
    {
        return WaveVR_Controller.Input(wvr.WVR_DeviceType.WVR_DeviceType_Controller_Right).GetPressDown(WVR_InputId.WVR_InputId_Alias1_Bumper);
    }

    public bool GetMouseUp()
    {
        return WaveVR_Controller.Input(wvr.WVR_DeviceType.WVR_DeviceType_Controller_Right).GetPressUp(WVR_InputId.WVR_InputId_Alias1_Bumper);
    }

    public Ray GetRay()
    {
        Debug.LogError("Has Ctrl " + this._inputModule.Controller == null);
        if (this._inputModule.Controller.DominantController == null) return new Ray();
        var d = _inputModule.Controller.DominantController;
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
}
