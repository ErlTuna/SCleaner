Shader "Custom/SpriteOutline"
{
    Properties
    {
        [PerRendererData] _MainTex ("Sprite Texture", 2D) = "white" {}
        _OutlineColor ("Outline Color", Color) = (1,1,0,1)
        _OutlineSize ("Outline Size", Float) = 0.01
    }

    SubShader
    {
        Tags { "Queue"="Transparent" "RenderType"="Transparent" }
        Blend SrcAlpha OneMinusSrcAlpha
        Cull Off
        Lighting Off
        ZWrite Off

        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #include "UnityCG.cginc"

            sampler2D _MainTex;
            float4 _OutlineColor;
            float _OutlineSize;

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

            v2f vert(appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = v.uv;
                return o;
            }

            float4 frag(v2f i) : SV_Target
            {
                float alpha = tex2D(_MainTex, i.uv).a;

                if (alpha > 0.1)
                {
                    return tex2D(_MainTex, i.uv); // Draw sprite normally
                }

                // Look around the pixel in all directions for nearby alpha
                float outline = 0.0;
                float2 offset = float2(_OutlineSize, _OutlineSize);

                outline += tex2D(_MainTex, i.uv + float2( offset.x,  0)).a;
                outline += tex2D(_MainTex, i.uv + float2(-offset.x,  0)).a;
                outline += tex2D(_MainTex, i.uv + float2(0,  offset.y)).a;
                outline += tex2D(_MainTex, i.uv + float2(0, -offset.y)).a;

                if (outline > 0.1)
                {
                    return _OutlineColor;
                }

                return float4(0, 0, 0, 0); // Transparent
            }
            ENDCG
        }
    }
}
