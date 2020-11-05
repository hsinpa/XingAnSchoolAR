using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Hsinpa.Input
{
    public interface InputInterface
    {
        Vector3 faceDir
        {
            get;
        }

        float raycastLength
        {
            get;
        }


        Ray GetRay();

        bool GetMouseDown();
        bool GetMouse();
        bool GetMouseUp();

        bool SwipeLeft();
        bool SwipeRight();

        bool ClickOnMenuKey();

        Transform GetParent();

        void SwitchControllerModel(bool isOn);
    }
}