Shader "UI/RadialRingFill"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
        _Fill ("Fill Amount", Range(0,1)) = 1
        _GreenColor ("Green", Color) = (0,1,0,1)
        _GrayColor ("Gray", Color) = (0.5,0.5,0.5,1)
        _RedColor ("Red", Color) = (1,0,0,1)
        _WarningFill ("Warning Fill Threshold", Range(0,1)) = 0.4
    }

    SubShader
    {
        Tags { "Queue"="Transparent" "RenderType"="Transparent" }
        Blend SrcAlpha OneMinusSrcAlpha
        ZWrite Off
        Cull Off

        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #include "UnityCG.cginc"

            sampler2D _MainTex;
            float4 _GreenColor;
            float4 _GrayColor;
            float _Fill;
            float4 _RedColor;
            float _WarningFill;

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

            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = v.uv;
                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {
                float4 tex = tex2D(_MainTex, i.uv);
                if (tex.a == 0) discard;

                float2 center = float2(0.5, 0.5);
                float2 dir = i.uv - center;

                float angle = atan2(dir.y, dir.x);
                angle -= UNITY_PI * 0.5;
                angle = (angle + UNITY_PI * 2) / (UNITY_PI * 2);
                angle = frac(angle);

                float4 activeColor = (_Fill <= _WarningFill) ? _RedColor : _GreenColor;
                float4 color = angle <= _Fill ? activeColor : _GrayColor;

                color.a *= tex.a;
                return color;
            }
            ENDCG
        }
    }
}
