Shader "Custom/SurfaceWater"
{
    Properties
    {
        _Color ("Color", Color) = (1,1,1,1)
        _MainTex ("Albedo (RGB)", 2D) = "white" {}
        _NoiseTex ("Noise Texture", 2D) = "white" {}
    }

    SubShader
    {
        Tags { "RenderType"="Transparent" "Queue"="Transparent" }
        LOD 200

        CGPROGRAM
        // Physically based Standard lighting model, and enable shadows on all light types
        #pragma surface surf Standard fullforwardshadows alpha:fade

        // Use shader model 3.0 target, to get nicer looking lighting
        #pragma target 3.0

        sampler2D _MainTex, _NoiseTex;

        struct Input
        {
            float2 uv_MainTex;
        };

        fixed4 _Color;

        void surf (Input IN, inout SurfaceOutputStandard o)
        {
            float4 noise = tex2D(_NoiseTex, IN.uv_MainTex);
            float2 translation = IN.uv_MainTex + float2(0.5, 0) * _Time;

            translation.x += noise.r;

            if(noise.g < 0.5)
                translation.y -= noise.g;
            else
                translation.y += noise.g;

            fixed4 c = tex2D (_MainTex, translation) * _Color;

            o.Albedo = c.rgb;
            o.Alpha = 0.8;
        }
        ENDCG
    }
    FallBack "Diffuse"
}
