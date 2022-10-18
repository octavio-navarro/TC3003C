Shader "Custom/FireShader"
{
    Properties
    {
        [HDR] _Color ("Color", Color) = (1,1,1,1)

        _MainTex ("Albedo (RGB)", 2D) = "white" {}
        _NoiseTex ("Noise", 2D) = "white" {}
        _DisolveTex ("Disolve", 2D) = "white" {}

        _Strength ("Strength", Range(0.1, 2)) = 1

        _NoiseScale ("Noise Scale", Range(0.1,2)) = 1

        _ScrollDirection ("Scroll Direction", Vector) = (0, 0, 0, 1)
        _ScrollSpeedX ("Scroll speed X", Range(0,3)) = 0.1
        _ScrollSpeedY ("Scroll speed Y", Range(0,3)) = 0.1

        _DisolveScale ("Disolve Scale", Range(0.1,2)) = 1
        _DisolveStrength ("Disolve Strength", Range(0.1,1)) = 1
        _DisolveDirection ("Disolve Direction", Vector) = (0, 0, 0, 1)
        _DisolveSpeedX ("Disolve speed X", Range(0,3)) = 0.1
        _DisolveSpeedY ("Disolve speed Y", Range(0,3)) = 0.1
    }

    SubShader
    {
        Tags { "RenderType"="Transparent" "Queue"="Transparent"}
        
        LOD 200
        // To get the material to be double sided
        Cull off

        CGPROGRAM
        // Physically based Standard lighting model, and enable shadows on all light types
        #pragma surface surf Standard fullforwardshadows alpha:fade

        // Use shader model 3.0 target, to get nicer looking lighting
        #pragma target 3.0

        sampler2D _MainTex, _NoiseTex, _DisolveTex;

        float _ScrollSpeedX, _ScrollSpeedY, _NoiseScale, _Strength;
        float _DisolveSpeedX, _DisolveSpeedY, _DisolveStrength, _DisolveScale;

        float4 _ScrollDirection, _DisolveDirection;
        float4 _Color;

        struct Input
        {
            float2 uv_MainTex;
            float2 uv_NoiseTex;
            float2 uv_DisolveTex;
        };

        void surf (Input IN, inout SurfaceOutputStandard o)
        {
            float4 color = float4(1, 1, 1, 1);
            o.Albedo = color.rgb;
            o.Alpha = color.a;
        }
        ENDCG
    }
    FallBack "Diffuse"
}
