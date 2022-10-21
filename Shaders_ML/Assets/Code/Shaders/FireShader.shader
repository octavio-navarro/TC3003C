Shader "Custom/FireShader"
{
    Properties
    {
        [HDR] _Color ("Color", Color) = (1,1,1,1)

        _MainTex ("Mask", 2D) = "white" {}
        _NoiseTex ("Noise", 2D) = "white" {}
        _DisolveTex ("Disolve", 2D) = "white" {}
        
        _Strength ("Strength", Range(0.1, 5)) = 1

        _NoiseScale ("Noise Scale", Range(0.1,2)) = 1
        _NoiseStrength ("Noise Strength", Range(0.1, 5)) = 1
        _NoiseMultiplier("Noise Multiplier", Range(0.1, 2)) = 1

        _ScrollDirection ("Scroll Direction", Vector) = (0, 0, 0, 1)
        _ScrollSpeedX ("Scroll speed X", Range(0,3)) = 0.1
        _ScrollSpeedY ("Scroll speed Y", Range(0,3)) = 0.1

        _DisolveScale ("Disolve Scale", Range(0.1, 2)) = 1
        _DisolveStrength ("Disolve Strength", Range(0.1, 5)) = 1
        _DisolveMultiplier ("Disolve Multiplier", Range(0.1, 2)) = 1

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

        float _Strength;
        float _ScrollSpeedX, _ScrollSpeedY, _NoiseScale, _NoiseStrength, _NoiseMultiplier;
        float _DisolveSpeedX, _DisolveSpeedY, _DisolveStrength, _DisolveMultiplier, _DisolveScale;

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
            // Albedo comes from a texture tinted by color
            IN.uv_NoiseTex *= _NoiseScale;
            IN.uv_DisolveTex *= _DisolveScale;

            float2 scrollSpeed = float2(_ScrollSpeedX, _ScrollSpeedY);
            float2 disolveSpeed = float2(_DisolveSpeedX, _DisolveSpeedY);

            float2 translationNoise = IN.uv_NoiseTex + _ScrollDirection.xy * _Time * scrollSpeed;
            float2 translationDisolve = IN.uv_DisolveTex + _DisolveDirection.xy * _Time * disolveSpeed;

            float4 noise = pow(tex2D (_NoiseTex, translationNoise), _NoiseStrength);
            float4 disolve = pow(tex2D (_DisolveTex, translationDisolve), _DisolveStrength);

            float4 mask = tex2D (_MainTex, IN.uv_MainTex);
            float4 color = ((noise * _NoiseMultiplier) * disolve * _DisolveMultiplier) * mask  * _Color * _Strength;

            o.Albedo = color.rgb;
            o.Alpha = color.a;
        }
        ENDCG
    }
    FallBack "Diffuse"
}
