// Upgrade NOTE: replaced '_World2Object' with 'unity_WorldToObject'

Shader "Unity Shaders Book/Chapter 6/Chapter_DiffuseVertexLevel"
{
    Properties
    {
       _Color {"_Color",Color}=(1,1,1,1) 
    }
    SubShader
    {
      

        Pass
        {
        Tags{"LightMode"="ForwardBase"}
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            // make fog work
            #pragma multi_compile_fog

            #include "UnityCG.cginc"
            
            fixed4 _Color;
            struct a2v
            {
            float4 vertex:POSITION;
            float3 normal:NORMAL;
            }
            struct v2f 
            {
            float4 pos:SV_POSITION
            fixed3 color:COLOR;
            
            }
          
          v2f vert(a2v v):SV_POSITION
          {
          v2f o;
          o.pos = UnityObjectToCilpPos(v.vector);
          fixed3 ambient =UNITY_LIGHTMODEL_AMBIENT.xyz;
          fixed3 worldNormal=normalize(mul(v.normal,(float3x3)unity_WorldToObject));
          fixed3 worldLight = normalize(_WorldSpaceLightPos0.xyz);
          fixed3 diffuse =_LightColor0.rgb*saturate(dot(worldNormal,worldLight))*_Color.rgb;
          o.color=diffuse+ambient;
          return o;
          
         }
         
         fixed4 frag(v2f i):SV_Target
         {
         return fixed4(i.color,1.0);
         }
           
            ENDCG
        }
    }
}
