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
            this.transform.localRotation = Quaternion.Euler(parentObject.transform.forward);
        }

        public void ShowTipIntruction(string message) {
            if (string.IsNullOrEmpty(message)) {
                tipText.enabled = false;
                return;
            }

            tipText.enabled = true;
            tipText.text = message;

            recordTime = Time.time + delayTime - 0.1f;

            CloseSiblingMsg();

            _ = Hsinpa.Utility.UtilityMethod.DoDelayWork(delayTime, () =>
            {
                if (Time.time > recordTime)
                    tipText.enabled = false;
            });
        }

        private void CloseSiblingMsg() {
            foreach (Transform child in transform.parent) {
                var toolItem = child.GetComponent<ToolItem>();
                if (toolItem != null && toolItem.name != this.name)
                    toolItem.ShowTipIntruction("");
            }
        }

    }
}