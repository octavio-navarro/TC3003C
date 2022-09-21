//
//  "InspectorPath/shaderName"
//   Refers to the place where we will select our shader to apply it to a material. This selection is made through the Unity Inspector
//
Shader "Custom/BaseUnlit"
{
    // Properties that the shader can control
    // Properties allow you to expose GUI elements in a material's Inspector tab
    // These are written in shaderlab declarative language
    Properties
    {
        // _Name of the property ("Name in the inspector", Type of the property) = Default value
        // Type of properties: Color, Vector, 2D - Texture, Range(min, max), Float, Int
        [Header(Base properties)]
        [Space(10)]
        // Textures
        _MainTex ("Texture", 2D) = "white" {}

        // Numbers
        _Specular ("Specular", Range(0.0, 1.1)) = 0.3
        _Factor ("Color Factor", Float) = 0.3
        _Cid ("Color id", Int) = 2

        // Vectors 
        _Color ("Tint", Color) = (1, 1, 1, 1)
        _VPos ("Vertex Position", Vector) = (0, 0, 0, 1)

        // Cubemap: This type of texture is quite useful for generating reflection maps, e.g., reflections in our character’s armor or to metallic elements in general.
        _Reflection ("Reflection", Cube) = "black" {}

        // Drawers allow us to define custom properties in the unity inspector.
        // There are 7 drawers: toggle, enum, keywordenum, intrange, space, header, powerslider

        [Header(Drawers)]
        [Space(10)]
        // We do not have boolean type properties, but we have the toggle that does the same thing.
        [Toggle] _Enable ("Enable color?", Float) = 0

        [Toggle] _Flip_Texture("Flip texture?", Float) = 0

        [KeywordEnum(Off, Red, Blue)] _Options ("Options", Float) = 0

        // This drawer is very similar to the KeywordEnum with the difference that it can define a “value/id” as an argument and pass this property to a command in our shader to change its functionality dynamically from the inspector.
        [Enum(Off, 0, Front, 1, Back, 2)] _Face("Face Culling", Float) = 0

        // PowerSlider and IntRange are quite useful when working with numerical ranges and precision.
        // PowerSlider allow us to generate a non-linear slider with curve control
        [PowerSlider(3.0)] _Brightness("Brightness", Range(0.01, 1)) = 0.08

        // IntRange adds a numerical range of integer values.
        [IntRange] _Samples ("Samples", Range (0, 255)) = 100

        // Space and Header help in organization tasks. 
        // Space allows us to add space between one property and another, while header adds a header in the unity inspector.
    }

    // Each shader is made up of at least one SubShader for the perfect loading of the program. When there is more than one SubShader, Unity will process each of them and take the most  suitable according to the hardware characteristics, starting from the first to the last on the list.
    SubShader
    {
        // SubShader configuration

        // Tags are labels that show how and when our shaders are processed.
        Tags { "RenderType"="Opaque" }
        LOD 100

        Pass
        {
            // HLSL or Cg Program
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag

            // Needed so that the toggle works. These are declared as constant. If the property is called "_Enable", this constant will be called "_ENABLE".
            #pragma shader_feature _ENABLE_ON

            #pragma shader_feature _FLIP_TEXTURE_ON

            // Used to link the KeywordEnum
            #pragma multi_compile _OPTIONS_OFF _OPTIONS_RED _OPTIONS_BLUE

            // make fog work
            #pragma multi_compile_fog

            #include "UnityCG.cginc"

            struct appdata
            {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
            };

            struct v2f
            {
                float2 uv : TEXCOORD0;
                float4 vertex : SV_POSITION;
            };

            // Connection variables are used to connect the shader variables to the shaderlab variables so that they can be used. They must have the same name in both sections so that the program recognizes them.
            sampler2D _MainTex;
            float4 _Color;

            // For the powerSlider and intRange we need connection variables
            float _Brightness;
            int _Samples;

            float4 _MainTex_ST;

            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = TRANSFORM_TEX(v.uv, _MainTex);

                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {
                // sample the texture
                #if _FLIP_TEXTURE_ON
                    i.uv.y *= -1;
                #endif

                fixed4 col = tex2D(_MainTex, i.uv);

                #if _ENABLE_ON
                    return col * _Color;
                #else
                    #if _OPTIONS_OFF
                        return col;
                    #elif _OPTIONS_RED
                        return col * float4(1, 0, 0, 1);
                    #elif _OPTIONS_BLUE
                        return col * float4(0, 0, 1, 1);
                    #endif
                #endif
            }
            ENDCG
        }
    }
}
