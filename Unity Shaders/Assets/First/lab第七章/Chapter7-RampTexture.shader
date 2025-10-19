Shader "Custom/Chapter7-RampTexture"
{
    Properties
    {
        _Color ("Color", Color) = (1,1,1,1)
        _RampTex ("Ramp Texture", 2D) = "white" {}
        _Gloss("Gloss", Range(8,256)) = 20
        _Specular ("Specular", Color) = (1,1,1,1)
    }
    SubShader
    {
    Pass
    {
        Tags { "LightMode"="ForwardBase" }
        
        CGPROGRAM
        #pragma vertex vert
        #pragma fragment frag
        #include "Lighting.cginc"
        #include "UnityCG.cginc"

        fixed4 _Color;
        sampler2D _RampTex;
        float4 _RampTex_ST;
        fixed4 _Specular;
        float _Gloss;

        struct a2v
        {
            float4 vertex : POSITION;
            float3 normal : NORMAL;
            float4 texcoord : TEXCOORD0;
        };

        struct v2f
        {
            float4 pos : SV_POSITION;
            float3 worldNormal : TEXCOORD0;
            float3 worldPos : TEXCOORD1;
            float2 uv : TEXCOORD2;
        };

        v2f vert(a2v v)
        {
            v2f o;
            o.pos = UnityObjectToClipPos(v.vertex);
            o.worldNormal = UnityObjectToWorldNormal(v.normal);
            o.worldPos = mul(unity_ObjectToWorld, v.vertex).xyz;
            o.uv = TRANSFORM_TEX(v.texcoord, _RampTex);
            return o;
        }

        fixed4 frag(v2f i) : SV_Target
        {
            fixed3 worldNormal = normalize(i.worldNormal);
            fixed3 worldLightDir = normalize(UnityWorldSpaceLightDir(i.worldPos));
            fixed3 worldViewDir = normalize(UnityWorldSpaceViewDir(i.worldPos));
            fixed3 halfVector = normalize(worldLightDir + worldViewDir);

            // ʹ�ð�������ģ�ͼ������ǿ��
            fixed halfLambert = 0.5 * dot(worldNormal, worldLightDir) + 0.5;
            
            // �ӽ������������ɫ
            fixed3 rampColor = tex2D(_RampTex, fixed2(halfLambert, halfLambert)).rgb;
            
            // ����������
            fixed3 albedo = rampColor * _Color.rgb;
            fixed3 diffuse = _LightColor0.rgb * albedo;

            // ����߹�
            fixed3 specular = _LightColor0.rgb * _Specular.rgb * 
                            pow(max(0, dot(worldNormal, halfVector)), _Gloss);

            // ������
            fixed3 ambient = UNITY_LIGHTMODEL_AMBIENT.xyz * albedo;

            fixed3 finalColor = diffuse + specular + ambient;
            return fixed4(finalColor, 1.0);
        }
        ENDCG
    }
    }
    FallBack "Diffuse"
}