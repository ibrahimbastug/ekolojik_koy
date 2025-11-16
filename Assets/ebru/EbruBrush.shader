Shader "Hidden/EbruBrush"
{
    Properties
    {
        _Color ("Color", Color) = (1,1,1,1)
        _Strength ("Brush Strength", Range(0,1)) = 0.5
    }
    SubShader
    {
        Tags { "RenderType"="Opaque" }
        Pass
        {
            ZWrite Off
            ZTest Always
            Blend SrcAlpha OneMinusSrcAlpha

            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #include "UnityCG.cginc"

            sampler2D _MainTex;
            fixed4 _Color;
            float _Strength;

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
                o.uv = v.uv;
                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {
                // mevcut arka plan rengi (su yüzeyi)
                fixed4 baseCol = tex2D(_MainTex, i.uv);
                
                // yeni renk
                fixed4 blendCol = _Color;

                // alpha'ya göre karýþým
                fixed4 result = lerp(baseCol, blendCol, blendCol.a * _Strength);

                return result;
            }
            ENDCG
        }
    }
}
