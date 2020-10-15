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
        private ToolSRP _toolSRP;

        [SerializeField, Range(0, 0.1f)]
        private float _ResidualThreshold;

        private int toolIndex;
        private int toolCount;

        int layerMask = 1 << 0;

        private void Start()
        {
            drawToTexture.SetUp(targetMaterial);
            toolCount = _toolSRP.tools.Length;
            toolIndex = -1;
        }

        private void Update()
        {
            if (Input.GetMouseButton(0) && toolIndex >= 0)
            {
                Vector3 diretion = (GetMouseWorldPos() - _camera.transform.position).normalized;
                diretion.y = -diretion.y;

                //Physics.Raycast
                RaycastHit hit;
                if (Physics.Raycast(_camera.ScreenPointToRay(Input.mousePosition), out hit, 100, layerMask))
                {
                    drawToTexture.DrawOnMesh(hit.textureCoord, _toolSRP.tools[toolIndex].mask_color);
                }
            }

            if (Input.GetMouseButtonUp(0) && toolIndex >= 0) {
                CheckIfSocreIsMeet();
            }

            if (Input.GetKeyDown(KeyCode.Space))
            {
                drawToTexture.ResetBuffer();
            }

            CheckIfToolIsPick();
        }

        private async void CheckIfSocreIsMeet() {
            float colorScore = await drawToTexture.CalScoreOnDrawMat(_toolSRP.tools[toolIndex].mask_color);

            Debug.Log("Color Score " + colorScore +", pass " + (colorScore < _ResidualThreshold));
        }

        private void CheckIfToolIsPick() {
            if (Input.GetKeyDown(KeyCode.Alpha1))
                EquipTool(ToolSRP.ToolEnum.Tool_1);

            if (Input.GetKeyDown(KeyCode.Alpha2))
                EquipTool(ToolSRP.ToolEnum.Tool_2);

            if (Input.GetKeyDown(KeyCode.Alpha3))
                EquipTool(ToolSRP.ToolEnum.Tool_3);
        }

        public void EquipTool(ToolSRP.ToolEnum toolEnum) {
            toolIndex = (int)toolEnum;
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