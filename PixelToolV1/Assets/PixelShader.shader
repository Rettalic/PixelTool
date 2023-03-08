Shader "Custom/PixelateWithKernel" {
    Properties {
        _MainTex ("Texture", 2D) = "white" {}
        _PixelSize ("Pixel Size", Range(1, 100)) = 10
        _UseKernel ("Use Kernel", Range(0, 1)) = 1
        _KernelSize ("Kernel Size", Range(1, 25)) = 3
        _KernelRadius ("Kernel Radius", Range(0, 12)) = 1
        _KernelLength ("Kernel Length", Float) = 9
        _TexWidth ("Texture Width", Float) = 1
        _TexHeight ("Texture Height", Float) = 1
        _Kernel ("Kernel", Float) = 0 
    }

    SubShader {
        Tags { "RenderType"="Opaque" }
        LOD 100

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
            int _UseKernel;
            int _KernelSize;
            int _KernelRadius;
            float _KernelLength;
            float _TexWidth;
            float _TexHeight;
            float _Kernel[625];

            v2f vert (appdata v) {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = v.uv;
                return o;
            }

            fixed4 frag (v2f i) : SV_Target {
                float2 pixelCoord = float2(i.uv.x * _TexWidth, i.uv.y * _TexHeight);
                float2 pixelIndex = floor(pixelCoord / _PixelSize) * _PixelSize;

                if (_UseKernel == 1) {
                    float4 colorSum = float4(0, 0, 0, 0);
                    float4 kernelWeightSum = float4(0, 0, 0, 0);
                    for (int y = -_KernelRadius; y <= _KernelRadius; y++) {
                        for (int x = -_KernelRadius; x <= _KernelRadius; x++) {
                            float2 sampleCoord = pixelIndex + float2(x, y) * _PixelSize;
                            float2 sampleUV = sampleCoord / float2(_TexWidth, _TexHeight);
                            float kernelWeight = _Kernel[(y + _KernelRadius) * _KernelSize + (x + _KernelRadius)];
                            colorSum += tex2D(_MainTex, sampleUV) * kernelWeight;
                            kernelWeightSum += kernelWeight;
                        }
                    }
                    return colorSum / kernelWeightSum;
                } else {
                    return tex2D(_MainTex, pixelIndex / float2(_TexWidth, _TexHeight));
                }
            }
            ENDCG
        }
    }
    FallBack "Diffuse"
}