Shader "Shader/Highlight_Effect" {
    Properties {
        _MainTex ("Texture", 2D) = "white" {}
        _EdgeColor ("Edge Color", Color) = (1,1,0,1)
        _EdgeWidth ("Edge Width", float) = 1.0
    }
    SubShader {
        Tags { "RenderType"="Transparent" "Queue"="Transparent"}
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
            fixed4 _EdgeColor;
            float _EdgeWidth;

            v2f vert (appdata v) {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = TRANSFORM_TEX(v.uv, _MainTex);
                return o;
            }

            fixed4 frag (v2f i) : SV_Target {
                fixed4 col = tex2D(_MainTex, i.uv);

                // 简单的边缘检测逻辑
                float dx = _EdgeWidth * _ScreenParams.y / _ScreenParams.x;
                float dy = _EdgeWidth;
                float4 horizEdge = tex2D(_MainTex, i.uv + float2(dx, 0)) - tex2D(_MainTex, i.uv - float2(dx, 0));
                float4 vertEdge = tex2D(_MainTex, i.uv + float2(0, dy)) - tex2D(_MainTex, i.uv - float2(0, dy));
                float edge = length(horizEdge.rgb) + length(vertEdge.rgb);

                if(edge > 0.1) { // 边缘阈值，可根据需要调整
                    col.rgb = _EdgeColor.rgb;
                }
                return col;
            }
            ENDCG
        }
    }
}
