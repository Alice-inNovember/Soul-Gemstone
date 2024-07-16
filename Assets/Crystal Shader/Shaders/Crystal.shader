Shader "Custom/URP_Crystal"
{
    Properties
    {
        [Header(Noise Params)]
        _CellDensity("Cell Density", Range(0.01, 0.5)) = 0.25
        _Strength("Displacement Strength", Range(0.0, 2.0)) = 1.0
        _MainTex("Texture", 2D) = "white" {}
    }

    SubShader
    {
        Tags { "RenderType" = "Opaque" "Queue" = "Geometry" }

        Pass
        {
            Name "ForwardLit"
            Tags { "LightMode" = "UniversalForward" }

            HLSLPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #include "Utils.cginc"
            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"
            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Lighting.hlsl"

            TEXTURE2D(_MainTex);
            SAMPLER(sampler_MainTex);
            float _CellDensity;
            float _Strength;

            struct Attributes
            {
                float4 positionOS : POSITION;
                float3 normalOS : NORMAL;
            };

            struct Varyings
            {
                float4 positionHCS : SV_POSITION;
                float3 uv : TEXCOORD0;
                float3 local_uv : TEXCOORD1;
            };

            Varyings vert(Attributes v)
            {
                Varyings o;
                float3 worldPos = TransformObjectToWorld(v.positionOS);
                o.local_uv = v.positionOS.xyz;

                // Displacement stuff
                float3 noise = voronoiNoise(o.local_uv / _CellDensity);
                worldPos += TransformObjectToWorldNormal(v.normalOS) * noise.z * _Strength;
                o.positionHCS = TransformWorldToHClip(worldPos);
                o.uv = worldPos;
                return o;
            }

            half4 frag(Varyings i) : SV_Target
            {
                half4 col = SAMPLE_TEXTURE2D(_MainTex, sampler_MainTex, i.uv.xy);
                col.rgb = 1 - col.rgb;
                return col;
            }

            ENDHLSL
        }
    }
    FallBack "Hidden/InternalErrorShader"
}
