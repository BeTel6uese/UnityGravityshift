// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'

Shader"Custom/OutlineThroughWalls"
{
    Properties
    {
        _OutlineWidth ("Outline Width", Float) = 1.0
        _OutlineColor ("Outline Color", Color) = (0, 1, 0, 1)
        _MainColor ("Main Color", Color) = (1, 1, 1, 1)
    }
    SubShader
    {
        Tags { "Queue" = "Overlay" } // Force this shader to render on top of everything

        // Pass 1: Render the outline
        Pass
        {
            // Outline pass
Cull Front // Cull front faces to generate the outline effect

ZTest Always // Always pass the depth test (render regardless of depth)

ZWrite Off // Disable depth writes to avoid affecting depth buffer

            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
#include "UnityCG.cginc"

struct appdata
{
    float4 vertex : POSITION;
    float3 normal : NORMAL;
};

struct v2f
{
    float4 pos : SV_POSITION;
};

uniform float _OutlineWidth;
uniform float4 _OutlineColor;

            // Vertex shader (offset vertices to create outline)
v2f vert(appdata v)
{
    v2f o;
    float3 offset = v.normal * _OutlineWidth;
    o.pos = UnityObjectToClipPos(v.vertex + float4(offset, 0.0));
    return o;
}

            // Fragment shader (apply outline color)
fixed4 frag(v2f i) : SV_Target
{
    return _OutlineColor; // Output the outline color
}

            ENDCG
        }

        // Pass 2: Render the object normally
        Pass
        {
Cull Back // Cull back faces to render the object normally

ZTest LEqual // Normal depth testing (renders correctly with depth)

ZWrite On // Enable depth writes for normal object rendering

            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
#include "UnityCG.cginc"

struct appdata
{
    float4 vertex : POSITION;
};

struct v2f
{
    float4 pos : SV_POSITION;
};

uniform float4 _MainColor;

v2f vert(appdata v)
{
    v2f o;
    o.pos = UnityObjectToClipPos(v.vertex);
    return o;
}

fixed4 frag(v2f i) : SV_Target
{
    return _MainColor; // Output the main object color
}

            ENDCG
        }
    }
FallBack"Diffuse"
}
