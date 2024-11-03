Shader "Custom/SineWaveVertexShader"
{
    Properties
    {
        _HeightMap ("Wave Texture", 2D) = "white" {}
        _Jacobian ("Jacobian", 2D) = "blue" {}
        _ShallowColor ("Shallow Color", Color) = (0, 0, 1, 1)
        _DeepColor ("Deep Color", Color) = (0, 1, 0, 1)
        _Metallic ("Metallic", Range(0, 1)) = 0.0
        _Smoothness ("Smoothness", Range(0, 1)) = 0.5
        _FoamColor("Foam Color", Color) = (1,1,1,1)
        _MaxGloss("Max Gloss", Range(0,1)) = 0
        _Roughness("Distant Roughness", Range(0,1)) = 0
        _RoughnessScale("Roughness Scale", Range(0, 0.01)) = 0.1
        _SSSStrength("SSSStrength", Range(0,1)) = 0.2
        
    }
        SubShader
    {
        Tags {"Queue" = "Transparent" "RenderType" = "Opaque" }
            CGPROGRAM
            #pragma surface surf Standard fullforwardshadows vertex:vert addshadow
            #pragma target 4.0


            struct Input
            {
                float2 uv_HeightMap; 
                float2 uv_Jacobian;
                float3 viewVector;
            };
            fixed4 _Color, _FoamColor, _SSSColor;
            float _SSSStrength;
            float _Roughness, _RoughnessScale, _MaxGloss;
            sampler2D _HeightMap;
            sampler2D _Jacobian;
            float4 _ShallowColor;
            float4 _DeepColor;
          

            void vert(inout appdata_full vertex)
            {
                 
                vertex.vertex.y +=  20.0*tex2Dlod(_HeightMap, float4(vertex.texcoord.xy,0,0)).r;

            }

            float3 WorldToTangentNormalVector(Input IN, float3 normal) {
                float3 t2w0 = WorldNormalVector(IN, float3(1, 0, 0));
                float3 t2w1 = WorldNormalVector(IN, float3(0, 1, 0));
                float3 t2w2 = WorldNormalVector(IN, float3(0, 0, 1));
                float3x3 t2w = float3x3(t2w0, t2w1, t2w2);
                return normalize(mul(t2w, normal));
            }
    
            float pow5(float f)
            {
                return f * f * f * f * f;
            }
    

        

            void surf(Input IN, inout SurfaceOutputStandard o)
            {
     
            float jacobian = tex2D(_Jacobian, IN.uv_Jacobian).r;
          
            float3 worldNormal = normalize(float3(-jacobian, 1, +jacobian));

            
            

            o.Albedo = lerp(0, _FoamColor, jacobian);
            float distanceGloss = lerp(1 - _Roughness, _MaxGloss, 1 / (1 + length(IN.viewVector) * _RoughnessScale));
            o.Smoothness = lerp(distanceGloss, 0, jacobian);
            o.Metallic = 0;

            float3 viewDir = normalize(IN.viewVector);
            float3 H = normalize(-worldNormal + _WorldSpaceLightPos0);
            float ViewDotH = pow5(saturate(dot(viewDir, -H))) * 30 * _SSSStrength;
            fixed3 color = lerp(_Color, saturate(_Color + _SSSColor.rgb * ViewDotH), 1.0);

            float fresnel = dot(worldNormal, viewDir);
            fresnel = saturate(1 - fresnel);
            fresnel = pow5(fresnel);

            o.Emission = lerp(color * (1 - fresnel), 0, jacobian);
            }
            ENDCG
        }
        FallBack "Diffuse"
}