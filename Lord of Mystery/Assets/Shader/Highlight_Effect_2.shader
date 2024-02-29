Shader "Shader/Highlight_Effect_2" {
    Properties {
        _MainTex ("Base (RGB)", 2D) = "white" {}
        _OutlineColor ("Outline Color", Color) = (1,0,0,1)
        _OutlineThickness ("Outline Thickness", Float) = 1.0
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

            struct appdata_t {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
            };

            struct v2f {
                float2 uv : TEXCOORD0;
                float4 vertex : SV_POSITION;
            };

            sampler2D _MainTex;
            float4 _OutlineColor;
            float _OutlineThickness;

            v2f vert(appdata_t v) {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = v.uv;
                return o;
            }

            fixed4 frag(v2f i) : SV_Target {
                float alpha = tex2D(_MainTex, i.uv).a;
                float outline = 0.0;
                for(float x = -_OutlineThickness; x <= _OutlineThickness; x++)
                {
                    for(float y = -_OutlineThickness; y <= _OutlineThickness; y++)
                    {
                        float2 offset = float2(x, y) * _ScreenParams.zw * 0.5;
                        outline = max(outline, tex2D(_MainTex, i.uv + offset).a);
                    }
                }
                fixed4 resultColor = tex2D(_MainTex, i.uv);
                resultColor.rgb = lerp(_OutlineColor.rgb, resultColor.rgb, alpha);
                resultColor.a = outline * alpha;
                return resultColor;
            }
            ENDCG
        }
    }
}
