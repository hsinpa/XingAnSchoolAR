using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Expect.ARTour
{
    public class ARThemeAnchor : MonoBehaviour
    {
        [SerializeField]
        private Transform target;

        public void SetLocalData(Vector3 pos, Quaternion rot, Vector3 scale) {
            target.transform.localPosition = pos;
            target.transform.localRotation = rot;
            target.transform.localScale = scale;
        } 
    }
}