Shader "Custom/PixelateShader"
{
    Properties{
        _MainTex("Texture", 2D) = "white" {}
        _PixelSize("Pixel Size", Range(1, 100)) = 10
    }

        SubShader{
            Tags { "Queue" = "Transparent" "RenderType" = "Opaque" }

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
                float _PixelSize;

                v2f vert(appdata v) {
                    v2f o;
                    o.vertex = UnityObjectToClipPos(v.vertex);
                    o.uv = v.uv;
                    return o;
                }

                fixed4 frag(v2f i) : SV_Target{
                   // float2 center = float2(0.5, 0.5) * _MainTex_TexelSize.xy + _MainTex_TexelSize.xy * 0.5;
                    float2 pixelCoord = floor(i.uv * _ScreenParams.zw * _PixelSize) / _PixelSize;
                    return tex2D(_MainTex, pixelCoord);
                }
                ENDCG
            }
        }
            FallBack "Diffuse"
}
