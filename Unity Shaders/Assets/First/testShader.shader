Shader "Unlit/NewUnlitShader"
{
 
    SubShader
{
    Pass
    {
        CGPROGRAM
        #pragma vertex vert
        #pragma fragment frag
        #include "UnityCG.cginc"
        
        struct v2f
        {
            float4 pos : SV_POSITION;  // ���������壬Ӧ����SV_POSITION������VS_POSITION
            fixed4 color : COLOR0;
        };
        
        v2f vert(appdata_full v)
        {
            v2f o;
            o.pos = UnityObjectToClipPos(v.vertex);
            
            // ���߿��ӻ�
            o.color = fixed4(v.normal * 0.5 + fixed3(0.5, 0.5, 0.5), 1.0);
            
            // ���߿��ӻ�
            o.color = fixed4(v.tangent.xyz * 0.5 + fixed3(0.5, 0.5, 0.5), 1.0);
            
            // �����߼���Ϳ��ӻ�
            fixed3 binormal = cross(v.normal, v.tangent.xyz) * v.tangent.w;
            o.color = fixed4(binormal * 0.5 + fixed3(0.5, 0.5, 0.5), 1.0); // �����˶�������źͲ���
            
            // ��һ������������ӻ�
            o.color = fixed4(v.texcoord.xy, 0.0, 1.0);
            
            // �ڶ�������������ӻ�
            o.color = fixed4(v.texcoord1.xy, 0.0, 1.0);
            
            // ���������С�����ֿ��ӻ�
            o.color = frac(v.texcoord);
            
            // ������������Ƿ���[0,1]��Χ��
            if(any(saturate(v.texcoord) - v.texcoord))
            {
                o.color.b = 0.5;
            }
            
            o.color.a = 1.0;
            
            // ���ӻ�������ɫ
            // o.color = v.color;
            
            return o;
        }
        
        fixed4 frag(v2f i) : SV_Target // ���������壬Ӧ����SV_Target������SV_target
        {
            return i.color;
        }
        ENDCG
    }
}
}
