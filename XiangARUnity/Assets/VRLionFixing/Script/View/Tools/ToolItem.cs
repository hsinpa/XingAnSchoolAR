using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Expect.View
{
    public class ToolItem : MonoBehaviour
    {
        private Vector3 _originalPos;
        private Quaternion _originalRot;

        private Transform _originalParent;

        private void Start()
        {
            _originalPos = transform.position;
            _originalRot = transform.rotation;
            _originalParent = transform.parent;
        }

        public void Return() {
            transform.SetParent(_originalParent);
            transform.position = _originalPos;
            transform.rotation = _originalRot;
        }

        public void PairToParent(Transform parentObject) {
            this.transform.SetParent(parentObject);
            this.transform.localPosition = Vector3.zero;
            this.transform.localRotation = parentObject.rotation;
        }
    }
}