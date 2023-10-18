Shader "Custom/OutlineShader"
{
    Properties
    {
        _OutlineColor("Outline Color", Color) = (0,1,0,1)
        _Outline("Outline width", Range(0.002, 0.03)) = 0.005
    }

        SubShader
    {
        Tags {"Queue" = "Overlay" }
        LOD 100

        CGINCLUDE
        #include "UnityCG.cginc"

        struct v2f
        {
            float4 pos : POSITION;
            float4 color : COLOR;
        };

        float _Outline;
        float4 _OutlineColor;

        v2f vert(appdata_t v)
        {
            v2f o;
            o.pos = UnityObjectToClipPos(v.vertex);
            o.color = v.color;

            float normOffset = mul(unity_ObjectToWorld, v.vertex).z;
            o.pos.xy += UnityObjectToClipPos(v.vertex).zw * normOffset * _Outline;

            return o;
        }
        ENDCG

        CGPROGRAM
        #pragma surface surf Lambert

        sampler2D _MainTex;

        fixed4 LightingLambert_PrePass(SurfaceOutput s, fixed3 lightDir, fixed atten)
        {
            fixed4 c;
            c.rgb = s.Albedo;
            c.a = s.Alpha;
            return c;
        }

        ENDCG
    }

        FallBack "Diffuse"
}
