using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
namespace Expect.View
{
    public class ToolItem : MonoBehaviour
    {
        [SerializeField]
        private Text tipText;

        private Vector3 _originalPos;
        private Quaternion _originalRot;
        private Transform _originalParent;

        private float recordTime;
        private int delayTime = 5;

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

        public void ShowTipIntruction(string message) {
            tipText.enabled = true;
            tipText.text = message;

            recordTime = Time.time + delayTime - 0.1f;

            _ = Hsinpa.Utility.UtilityMethod.DoDelayWork(delayTime, () =>
            {
                if (Time.time > recordTime)
                    tipText.enabled = false;
            });
        }

    }
}