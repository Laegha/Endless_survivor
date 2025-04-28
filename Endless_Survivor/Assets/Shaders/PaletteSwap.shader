Shader "Custom/PaletteSwap"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
        _InputPalette ("Input Palette", 2D) = "white" {}
        _OutputPalette ("Output Palette", 2D) = "white" {}
        _PaletteSize ("Palette Size", Float) = 12
    }
    SubShader
    {
        Tags { "Queue"="Transparent" "RenderType"="Transparent" }
        LOD 100

        Pass
        {
            Blend SrcAlpha OneMinusSrcAlpha
            ZWrite Off
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #pragma multi_compile_fog

            #include "UnityCG.cginc"

            struct appdata
            {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
            };

            struct v2f
            {
                float2 uv : TEXCOORD0;
                UNITY_FOG_COORDS(1)
                float4 vertex : SV_POSITION;
            };

            sampler2D _MainTex;
            sampler2D _InputPalette;
            sampler2D _OutputPalette;
            float _PaletteSize;
            float4 _MainTex_ST;

            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = TRANSFORM_TEX(v.uv, _MainTex);
                UNITY_TRANSFER_FOG(o, o.vertex);
                return o;
            }

            float2 PaletteIndex(float3 color)
            {
                float minDist = 9999.0;
                float2 bestUV = float2(0, 0.5);

                for (int i = 0; i < (int)_PaletteSize; i++)
                {
                    float2 uv = float2((i + 0.5) / _PaletteSize, 0.5);
                    float3 paletteColor = tex2D(_InputPalette, uv).rgb;
                    float dist = distance(color, paletteColor);

                    if (dist < minDist)
                    {
                        minDist = dist;
                        bestUV = uv;
                    }
                }

                return bestUV;
            }

            fixed4 frag (v2f i) : SV_Target
            {
                fixed4 col = tex2D(_MainTex, i.uv);

                float2 paletteUV = PaletteIndex(col.rgb);
                float3 newColor = tex2D(_OutputPalette, paletteUV).rgb;

                col.rgb = newColor; // Swap RGB

                UNITY_APPLY_FOG(i.fogCoord, col);
                return col;
            }

            ENDCG
        }
    }
}
