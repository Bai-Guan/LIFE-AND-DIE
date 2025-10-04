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
            float4 pos : SV_POSITION;  // 修正了语义，应该是SV_POSITION而不是VS_POSITION
            fixed4 color : COLOR0;
        };
        
        v2f vert(appdata_full v)
        {
            v2f o;
            o.pos = UnityObjectToClipPos(v.vertex);
            
            // 法线可视化
            o.color = fixed4(v.normal * 0.5 + fixed3(0.5, 0.5, 0.5), 1.0);
            
            // 切线可视化
            o.color = fixed4(v.tangent.xyz * 0.5 + fixed3(0.5, 0.5, 0.5), 1.0);
            
            // 副法线计算和可视化
            fixed3 binormal = cross(v.normal, v.tangent.xyz) * v.tangent.w;
            o.color = fixed4(binormal * 0.5 + fixed3(0.5, 0.5, 0.5), 1.0); // 修正了多余的括号和参数
            
            // 第一组纹理坐标可视化
            o.color = fixed4(v.texcoord.xy, 0.0, 1.0);
            
            // 第二组纹理坐标可视化
            o.color = fixed4(v.texcoord1.xy, 0.0, 1.0);
            
            // 纹理坐标的小数部分可视化
            o.color = frac(v.texcoord);
            
            // 检查纹理坐标是否在[0,1]范围内
            if(any(saturate(v.texcoord) - v.texcoord))
            {
                o.color.b = 0.5;
            }
            
            o.color.a = 1.0;
            
            // 可视化顶点颜色
            // o.color = v.color;
            
            return o;
        }
        
        fixed4 frag(v2f i) : SV_Target // 修正了语义，应该是SV_Target而不是SV_target
        {
            return i.color;
        }
        ENDCG
    }
}
}
