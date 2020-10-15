﻿using Questionaire.Utility;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
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

        [SerializeField]
        private UnityEngine.Shader ScoreShader;

        [SerializeField]
        private Texture maskTexture;

        private RenderTexture buffer;

        //To Check how many dirt is been clean
        private RenderTexture scoreBuffer;

        private string ShaderMainTex = "_MainTex";
        private string ShaderPowerKey = "_Power";
        private string ShaderPositionKey = "_MousePosition";
        private string ShaderColorKey = "_Color";
        private string ShaderRangeKey = "_Range";

        private string ScoreShaderPaintedTex = "_PaintedTex";


        private Material drawMaterial;
        private Material scoreMaterial;
        private int scoreTexSize = 24;

        public void SetUp(Material targetMaterial)
        {
            drawMaterial = new Material(DrawShader);
            scoreMaterial = new Material(ScoreShader);

            ResetBuffer();


            scoreMaterial.SetTexture(ShaderMainTex, maskTexture);
            scoreMaterial.SetTexture(ScoreShaderPaintedTex, buffer);

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

        public async Task<float> CalScoreOnDrawMat(Color paintColor)
        {
            scoreMaterial.SetColor(ShaderColorKey, paintColor);

            Graphics.Blit(null, scoreBuffer, scoreMaterial, 0);

            return await CalculateOnCPU(paintColor);
        }

        private async Task<float> CalculateOnCPU(Color paintColor) {

            Color[] colors = Utility.UtilityMethod.ToColor(Utility.UtilityMethod.toTexture2D(scoreTexSize, scoreBuffer));
            Color whiteColor = Color.white;
            float allocateColor = 0;

            return await Task.Run<float>(() =>
            {
                for (int x = 0; x < scoreTexSize; x++) 
                {
                    for (int y = 0; y < scoreTexSize; y++)
                    {
                        int index = x + (y * scoreTexSize);
                        Color invertColor = whiteColor - paintColor;
                        Color targetColor = colors[index] - invertColor;

                        allocateColor += Mathf.Clamp(targetColor.r, 0, 1) + Mathf.Clamp(targetColor.g, 0, 1) + Mathf.Clamp(targetColor.b, 0, 1);
                    }
                }

                return allocateColor / (scoreTexSize * scoreTexSize);
            });
        }

        private void OnGUI()
        {
            GUI.DrawTexture(new Rect(0, 0, 256, 256), buffer, ScaleMode.ScaleToFit, false, 1);
            GUI.DrawTexture(new Rect(256, 0, 256, 256), scoreBuffer, ScaleMode.ScaleToFit, false, 1);
        }

        public void ResetBuffer() {
            buffer = new RenderTexture(256, 256, 0, RenderTextureFormat.ARGBFloat);
            scoreBuffer = new RenderTexture(scoreTexSize, scoreTexSize, 0, RenderTextureFormat.ARGBFloat);
        }

    }
}