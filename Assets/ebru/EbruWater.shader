Shader "Hidden/EbruWater"
{
    Properties
    {
        _MainTex ("Base (RGB)", 2D) = "white" {}
        _TimeScale ("Wave Speed", Range(0.0, 2.0)) = 0.3
        _Distortion ("Distortion", Range(0.0, 0.05)) = 0.01
    }
    SubShader
    {
        Tags { "RenderType"="Opaque" }
        Pass
        {
            ZTest Always
            Cull Off
            ZWrite Off

            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #include "UnityCG.cginc"

            sampler2D _MainTex;
            float _TimeScale;
            float _Distortion;
            float4 _MainTex_ST;

            struct appdata
            {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
            };

            struct v2f
            {
                float4 pos : SV_POSITION;
                float2 uv : TEXCOORD0;
            };

            v2f vert (appdata v)
            {
                v2f o;
                o.pos = UnityObjectToClipPos(v.vertex);
                o.uv = TRANSFORM_TEX(v.uv, _MainTex);
                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {
                float wave = sin(_Time.y * _TimeScale + i.uv.x * 10.0) * cos(_Time.y * _TimeScale + i.uv.y * 10.0);
                i.uv += wave * _Distortion;

                return tex2D(_MainTex, i.uv);
            }
            ENDCG
        }
    }
}
