Shader "Unlit/Chapter6-DiffusePixelLevel"
{
     Properties
    {
       _Color ("_Color",Color)=(1,1,1,1)
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
            #include "Lighting.cginc"
            fixed4 _Color;

            struct a2v
            {
            float4 vertex:POSITION;
            float3 normal:NORMAL;
            };

            struct v2f 
            {
            float4 pos:SV_POSITION;
            fixed3 worldNormal:TEXCOORD;
            
            };
          
          v2f vert(a2v v)
          {
          v2f o;
          o.pos = UnityObjectToClipPos(v.vertex);
          o.worldNormal=normalize(mul(v.normal,(float3x3)unity_WorldToObject));
          return o;
          
         }
         
         fixed4 frag(v2f i):SV_Target
         {
         fixed3 ambient = UNITY_LIGHTMODEL_AMBIENT.xyz;
         fixed3 worldNormal = normalize(i.worldNormal);
         fixed3 worldLightDir = normalize(_WorldSpaceLightPos0.xyz);
         fixed3 diffuse = _LightColor0.rgb * _Color.rgb *saturate(dot(worldNormal,worldLightDir));
         fixed3 color =ambient+diffuse;
         
         return fixed4(color,1.0);
         }
           
          
            ENDCG
             
        }
    }
}
