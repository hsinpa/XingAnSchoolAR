using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Hsinpa.Shader;
using Hsinpa.Input;

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

        [SerializeField]
        private Transform targetModel;

        private int toolIndex;
        private int toolCount;
        private int layerMask = 1 << 9;

        public System.Action OnTargetDirtIsClear; 


private void Start()
        {
            drawToTexture.SetUp(targetMaterial);
            toolCount = _toolSRP.tools.Length;
            toolIndex = -1;
        }

        private void Update()
        {
            if (InputWrapper.instance.platformInput.GetMouse() && toolIndex >= 0)
            {
                Vector3 diretion = InputWrapper.instance.platformInput.faceDir;
                diretion.y = -diretion.y;

                //Physics.Raycast
                RaycastHit hit;
                if (Physics.Raycast(InputWrapper.instance.platformInput.GetRay(), out hit, 100, layerMask))
                {
                    drawToTexture.DrawOnMesh(hit.textureCoord, _toolSRP.tools[toolIndex].mask_color);
                }
            }

            if (InputWrapper.instance.platformInput.GetMouseUp() && toolIndex >= 0) {
                CheckIfSocreIsMeet();
            }

            if (UnityEngine.Input.GetKeyDown(KeyCode.Space))
            {
                drawToTexture.ResetBuffer();
            }

            CheckIfToolIsPick();
            RotateTargetModelManually();
        }

        private async void CheckIfSocreIsMeet() {
            float colorScore = await drawToTexture.CalScoreOnDrawMat(_toolSRP.tools[toolIndex].mask_color);
            bool dirtIsClear = colorScore < _ResidualThreshold;

            if (dirtIsClear && OnTargetDirtIsClear != null) {
                OnTargetDirtIsClear();
            }
            Debug.Log("Color Score " + colorScore +", pass " + (dirtIsClear));
        }

        private void CheckIfToolIsPick() {
            if (UnityEngine.Input.GetKeyDown(KeyCode.Alpha1))
                EquipTool(ToolSRP.ToolEnum.Tool_1);

            if (UnityEngine.Input.GetKeyDown(KeyCode.Alpha2))
                EquipTool(ToolSRP.ToolEnum.Tool_2);

            if (UnityEngine.Input.GetKeyDown(KeyCode.Alpha3))
                EquipTool(ToolSRP.ToolEnum.Tool_3);
        }

        private void RotateTargetModelManually() {
            int speed = 100;
            if (InputWrapper.instance.platformInput.SwipeRight())
                targetModel.transform.Rotate(Vector3.up * Time.deltaTime * speed);

            if (InputWrapper.instance.platformInput.SwipeLeft())
                targetModel.transform.Rotate(-Vector3.up * Time.deltaTime * speed);
        }

        public void EquipTool(ToolSRP.ToolEnum toolEnum) {
            toolIndex = (int)toolEnum;
        }

        public void UnEquip() {
            toolIndex = -1;
        }
    }
}