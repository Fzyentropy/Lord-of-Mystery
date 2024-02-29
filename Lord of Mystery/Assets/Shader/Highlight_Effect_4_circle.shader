Shader "Shader/Highlight_Effect_4_circle" {
    Properties {
        _MainTex ("Texture", 2D) = "white" {}
        _HighlightColor ("Highlight Color", Color) = (1,1,0,1)
        _EdgeWidth ("Edge Width", float) = 0.1
        _EdgeIntensity ("Edge Intensity", float) = 1.0
    }
    SubShader {
        Tags { "Queue"="Transparent" "RenderType"="Transparent" }
        LOD 100

        Blend SrcAlpha OneMinusSrcAlpha
        ZWrite Off
        Cull Off

        Pass {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #include "UnityCG.cginc"

            struct appdata {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
            };

            struct v2f {
                float2 uv : TEXCOORD0;
                float4 vertex : SV_POSITION;
            };

            sampler2D _MainTex;
            float4 _MainTex_ST;
            fixed4 _HighlightColor;
            float _EdgeWidth;
            float _EdgeIntensity;

            v2f vert (appdata v) {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = TRANSFORM_TEX(v.uv, _MainTex);
                return o;
            }

            fixed4 frag (v2f i) : SV_Target {
                fixed4 col = tex2D(_MainTex, i.uv);
                float dist = distance(i.uv, float2(0.5, 0.5)); // 假设精灵纹理的圆心在UV坐标(0.5, 0.5)
                float edge = smoothstep(0.5 - _EdgeWidth, 0.5, dist); // 边缘宽度控制
                col.rgb = lerp(col.rgb, _HighlightColor.rgb * _EdgeIntensity, edge);
                return col;
            }
            ENDCG
        }
    }
}
