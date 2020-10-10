Shader "Hsinpa/DisplayShader"
{
    Properties
    {
        _MainTex ("Main Texture", 2D) = "white" {}
        _DirtTex ("Dirt Texture", 2D) = "white" {}
        _DirtMaskTex ("Dirt Mask Texture", 2D) = "white" {}
        _DirtTransition ("Transition", Range(0, 1)) = 0
        [MaterialToggle] _OverrideColor ("Override Color", Float) = 0
    }
    SubShader
    {
        Tags {  "RenderType"="Opaque"  "LightMode"="ForwardBase" }

        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #include "UnityCG.cginc"
            #include "UnityLightingCommon.cginc" // for _LightColor0

            struct appdata
            {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
                float3 normal   : NORMAL;    // The vertex normal in model space.
            };

            struct v2f
            {
                float2 uv : TEXCOORD0;
                float4 vertex : SV_POSITION;
                fixed4 diff : COLOR0; // diffuse lighting color
            };

            sampler2D _MainTex;
            float4 _MainTex_ST;

            sampler2D _DirtTex;
            sampler2D _DirtMaskTex;
            sampler2D _EraseTex;

            float _DirtTransition;
            float _OverrideColor;

            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = TRANSFORM_TEX(v.uv, _MainTex);

                half3 worldNormal = UnityObjectToWorldNormal(v.normal);
                // dot product between normal and light direction for
                // standard diffuse (Lambert) lighting
                half nl = max(0, dot(worldNormal, _WorldSpaceLightPos0.xyz));
                // factor in the light color
                o.diff = nl * _LightColor0;

                return o;
            }

            fixed FindTheLargestColor(fixed3 colorCode) {
                return max(max(colorCode.r, colorCode.g), colorCode.b);
            }

            fixed4 frag (v2f i) : SV_Target
            {
                // sample the texture
                fixed4 col = tex2D(_MainTex, i.uv);
                fixed4 dirtTex = tex2D(_DirtTex, i.uv);
                fixed3 dirtMaskTex = tex2D(_DirtMaskTex, i.uv).rgb;
                fixed3 eraseTex = tex2D(_EraseTex, i.uv).rgb;

                if ((length(dirtTex.rgb) * _OverrideColor) > 0) {
                    return dirtTex;
                }

                fixed4 white = fixed4(1,1,1,1);
                fixed4 black = fixed4(0,0,0,1);

                fixed3 maskResult = fixed3(dirtMaskTex - eraseTex);
                maskResult = clamp(black, maskResult, white);

                fixed4 dirtEffect = col - (col * (dirtTex + dirtTex) * FindTheLargestColor(maskResult));

                col =  lerp(col , dirtEffect, _DirtTransition);
                
                col *= i.diff;

                return col;
            }
            ENDCG
        }
    }
}
