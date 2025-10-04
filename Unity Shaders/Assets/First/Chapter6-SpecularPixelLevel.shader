Shader "Custom/Chapter6-SpecularPixelLevel"
{
    Properties
    {
        _Diffuse ("Diffuse",Color)=(1,1,1,1)
        _Specular  ("Specular",Color)=(1,1,1,1)//控制高光反射颜色
        _Gloss  ("Gloss",Range(8.0,256))=20//控制高光反射区域大小
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
            #include"Lighting.cginc"
            #include "UnityCG.cginc"

            fixed4 _Diffuse;
            fixed4 _Specular;
            float _Gloss;

            struct a2v{
              float4 vertex :POSITION;
              float3 normal:NORMAL;
              
            };

            struct v2f{
            float4 pos :SV_POSITION;
            fixed3 worldNormal:TEXCOORD0;
            float3 worldPos:TEXCOORD1;
            };
            
            v2f vert (a2v v)
            {
            v2f o;
            o.pos = UnityObjectToClipPos(v.vertex);

           
           
           o.worldNormal = normalize(mul(v.normal, (float3x3)unity_WorldToObject));
           
            o.worldPos =  mul(unity_ObjectToWorld,v.vertex).xyz;
         
            return o;
            }

            fixed4 frag (v2f i) :SV_Target
            {
            //1 计算漫反射 2 计算高光 3 颜色相加

            //漫反射
            fixed3 ambient = UNITY_LIGHTMODEL_AMBIENT.xyz;
            fixed3 worldNormal = normalize(i.worldNormal);
            fixed3 worldLightDir = normalize(_WorldSpaceLightPos0.xyz);
            fixed3 diffuse = _LightColor0.rgb * _Diffuse.rgb *saturate(dot(worldNormal,worldLightDir));
            //高光计算
            //得到当前片元和摄像机的向量 并进行归一化操作
            fixed3 viewDir = normalize( _WorldSpaceCameraPos.xyz -i.worldPos);
            //得到世界坐标下光线反射向量 并归一化
            fixed3 reflectDir = normalize(reflect(-worldLightDir,i.worldNormal));
            //计算代入高光反射公式
            fixed3 specular =  _LightColor0.rgb* _Specular.rgb *pow( saturate(dot(viewDir,reflectDir)),_Gloss);


             
             fixed3 color=specular + diffuse + ambient; 
              return fixed4(color ,1.0);
            }
            ENDCG
        }
    }
    FallBack "Specular"
}
