using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Hsinpa.Shader {
    public class DrawToTexture : MonoBehaviour
    {
        [SerializeField, Range(0, 1)]
        private float _Power = 0.1f;

        [SerializeField, Range(0, 1)]
        private float _Range = 0.1f;

        [SerializeField]
        private UnityEngine.Shader DrawShader;

        public RenderTexture buffer;

        private string ShaderPowerKey = "_Power";
        private string ShaderPositionKey = "_MousePosition";
        private string ShaderColorKey = "_Color";
        private string ShaderRangeKey = "_Range";

        private Material drawMaterial;

        public void SetUp(Material targetMaterial)
        {
            drawMaterial = new Material(DrawShader);

            ResetBuffer();

            targetMaterial.SetTexture("_EraseTex", buffer);
        }

        public void DrawOnMesh(Vector2 textureCoord, Color paintColor) {
            drawMaterial.SetFloat(ShaderPowerKey, _Power);
            drawMaterial.SetFloat(ShaderRangeKey, _Range);
            drawMaterial.SetVector(ShaderPositionKey, textureCoord);
            drawMaterial.SetColor(ShaderColorKey, paintColor);

            RenderTexture temp = RenderTexture.GetTemporary(buffer.width, buffer.height, 0, RenderTextureFormat.ARGBFloat);
            Graphics.Blit(buffer, temp);
            Graphics.Blit(temp, buffer, drawMaterial);
            RenderTexture.ReleaseTemporary(temp);
        }

        private void OnGUI()
        {
            GUI.DrawTexture(new Rect(0, 0, 256, 256), buffer, ScaleMode.ScaleToFit, false, 1);
        }

        public void ResetBuffer() {
            buffer = new RenderTexture(256, 256, 0, RenderTextureFormat.ARGBFloat);
        }

    }
}
