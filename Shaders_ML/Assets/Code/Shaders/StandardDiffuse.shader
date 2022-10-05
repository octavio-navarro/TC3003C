//
//  "InspectorPath/shaderName"
//   Refers to the place where we will select our shader to apply it to a material. This selection is made through the Unity Inspector
//
Shader "Custom/StandardDiffuse"
{
    // Properties that the shader can control
    // Properties allow you to expose GUI elements in a material's Inspector tab
    Properties
    {
        // _Name of the property ("Name in the inspector", Type of the property) = Default value
        _Color ("Color", Color) = (1,1,1,1)
        _SpecularColor ("Specular Color", Color) = (1,1,1,1)
        _MainTex ("Albedo (RGB)", 2D) = "white" {}
        _Glossiness ("Smoothness", Range(0,1)) = 0.5
        _Metallic ("Metallic", Range(0,1)) = 0.0
    }

    SubShader
    {
        // Shader configuration
        Tags { "RenderType"="Opaque" }
        LOD 200

        // HLSL Program
        CGPROGRAM
            // Physically based Standard lighting model, and enable shadows on all light types
            // #pragma surface surf Standard fullforwardshadows
            #pragma surface surf StandardSpecular fullforwardshadows

            // Use shader model 3.0 target, to get nicer looking lighting
            #pragma target 3.0

            sampler2D _MainTex;

            struct Input
            {
                float2 uv_MainTex;
            };

            half _Glossiness;
            // half _Metallic;
            fixed4 _SpecularColor;
            fixed4 _Color;

            // Add instancing support for this shader. You need to check 'Enable Instancing' on materials that use the shader.
            // See https://docs.unity3d.com/Manual/GPUInstancing.html for more information about instancing.
            // #pragma instancing_options assumeuniformscaling
            UNITY_INSTANCING_BUFFER_START(Props)
                // put more per-instance properties here
            UNITY_INSTANCING_BUFFER_END(Props)

            void surf (Input IN, inout SurfaceOutputStandardSpecular o)
            {
                // Albedo comes from a texture tinted by color
                fixed4 c = tex2D (_MainTex, IN.uv_MainTex) * _Color;
                o.Albedo = c.rgb;
                // Metallic and smoothness come from slider variables
                // o.Metallic = _Metallic;
                o.Specular = _SpecularColor;
                o.Smoothness = _Glossiness;
                o.Alpha = c.a;
            }
        ENDCG
    }
    FallBack "Diffuse"
}
