using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Hsinpa.Shader;

namespace Hsinpa.App {
    public class PaintingManager : MonoBehaviour
    {
        [SerializeField]
        private Material targetMaterial;

        [SerializeField]
        private DrawToTexture drawToTexture;

        [SerializeField]
        private Camera _camera;

        [SerializeField]
        private Color _PaintColor;

        int layerMask = 1 << 0;

        private void Start()
        {
            drawToTexture.SetUp(targetMaterial);
        }

        private void Update()
        {
            if (Input.GetMouseButton(0))
            {
                Vector3 diretion = (GetMouseWorldPos() - _camera.transform.position).normalized;
                diretion.y = -diretion.y;

                //Physics.Raycast
                RaycastHit hit;
                if (Physics.Raycast(_camera.ScreenPointToRay(Input.mousePosition), out hit, 100, layerMask))
                {
                    drawToTexture.DrawOnMesh(hit.textureCoord, _PaintColor);
                }
            }

            if (Input.GetKeyDown(KeyCode.Space))
            {
                drawToTexture.ResetBuffer();
            }
        }

        private Vector3 GetMouseWorldPos()
        {
            Vector2 mousePos = new Vector2();

            // Get the mouse position from Event.
            // Note that the y position from Event is inverted.
            mousePos.x = Input.mousePosition.x;
            mousePos.y = _camera.pixelHeight - Input.mousePosition.y;

            return _camera.ScreenToWorldPoint(new Vector3(mousePos.x, mousePos.y, _camera.nearClipPlane));
        }


    }
}