Shader "Custom/Water" {
    Properties {
        _MainTex ("Water Texture", 2D) = "white" {}
        _Speed ("Speed", Range(0.0, 10.0)) = 1.0
        _Distortion ("Distortion", Range(0.0, 1.0)) = 0.1
        _Transparency ("Transparency", Range(0.0, 1.0)) = 1.0
    }

    SubShader {
        Tags {"Queue"="Transparent" "RenderType"="Transparent"}
        LOD 200

        Pass {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #pragma multi_compile_fog
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
            float _Speed;
            float _Distortion;
            float _Transparency;

            float4 _Noise;

            v2f vert (appdata v) {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = v.uv;
                return o;
            }

            float4 frag (v2f i) : SV_Target {
                float2 uv = i.uv;
                uv.x += _Speed * _Time.y * 0.1;
                uv.y += _Speed * _Time.y * 0.1;
                float4 col = tex2D(_MainTex, uv);
                float4 water = float4(0.0, 0.5, 1.0, 1.0);

                _Noise = (tex2D(_MainTex, i.uv * 20.0) * 2.0 - 1.0) * _Distortion;
                float4 noise = float4(_Noise.x, _Noise.y, _Noise.z, 1.0);

                return lerp(col, water, noise.r) * _Transparency;
            }
            ENDCG
        }
    }
    FallBack "Diffuse"
}
