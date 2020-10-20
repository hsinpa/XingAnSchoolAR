using Expect.View;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace Expect.Input
{
    public class FixLionInput : MonoBehaviour
    {

        public System.Action<ToolItem> HoldItemEvent;
        private RaycastHit[] m_Results = new RaycastHit[2];

        private Camera _camera;
        public float raycastlength;

        int layerMask;


        public void SetUp(Camera p_camera) {
            _camera = p_camera;
            layerMask = LayerMask.GetMask("Interactable");
        }

        public void OnUpdate() {
            if (UnityEngine.Input.GetMouseButtonDown(0)) {
                ToolItem toolItem = DetectAvailableTool();

                if (toolItem != null && HoldItemEvent != null)
                    HoldItemEvent(toolItem);
            }
        }

        private ToolItem DetectAvailableTool() {

            
            Ray ray = _camera.ScreenPointToRay(UnityEngine.Input.mousePosition);
            int hits = Physics.RaycastNonAlloc(ray, m_Results, raycastlength, layerMask);

            if (hits > 0)
            {
                Debug.Log("Hit " + m_Results[0].collider.gameObject.name);
                return m_Results[0].collider.GetComponent<ToolItem>();
            }

            return null;
            
        }


    }
}