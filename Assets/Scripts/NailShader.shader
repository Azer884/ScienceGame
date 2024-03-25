Shader "Custom/SmoothStepMaskedTextureShader"
{
    Properties
    {
        _MainTex ("Main Texture", 2D) = "white" {}
        _MaskTex ("Mask Texture", 2D) = "white" {}
        _BlendThreshold ("Blend Threshold", float) = 0.5
        _Smoothness ("Smoothness", Range(0, 1)) = 0.5
        _MainTexMetallic ("Main Texture Metallic", 2D) = "white" {}
        _MainTexNormal ("Main Texture Normal", 2D) = "bump" {}
        _MaskTexMetallic ("Mask Texture Metallic", 2D) = "white" {}
        _MaskTexNormal ("Mask Texture Normal", 2D) = "bump" {}
    }
    
    SubShader
    {
        Tags { "RenderType"="Opaque" }
        LOD 200
        
        CGPROGRAM
        #pragma surface surf Standard fullforwardshadows
        
        sampler2D _MainTex;
        sampler2D _MaskTex;
        float _BlendThreshold;
        float _Smoothness;
        sampler2D _MainTexMetallic;
        sampler2D _MainTexNormal;
        sampler2D _MaskTexMetallic;
        sampler2D _MaskTexNormal;
        
        struct Input
        {
            float2 uv_MainTex;
            float2 uv_MaskTex;
            float3 worldPos;
            float3 worldNormal;
        };
        
        void surf (Input IN, inout SurfaceOutputStandard o)
        {
            fixed4 mainTex = tex2D (_MaskTex, IN.uv_MainTex);
            fixed4 maskTex = tex2D (_MainTex, IN.uv_MaskTex);
            
            // Determine the blend factor based on Y position and mapped threshold
            float blendFactor = smoothstep(_BlendThreshold - _Smoothness, _BlendThreshold + _Smoothness, IN.worldPos.y);

            // Blend the textures
            fixed4 blendedColor = lerp(mainTex, maskTex, blendFactor);
            
            o.Albedo = blendedColor.rgb;
            o.Alpha = blendedColor.a;
            o.Metallic = lerp(tex2D(_MaskTexMetallic, IN.uv_MainTex).r, tex2D(_MainTexMetallic, IN.uv_MaskTex).r, blendFactor);
            o.Normal = lerp(UnpackNormal(tex2D(_MaskTexNormal, IN.uv_MainTex)), UnpackNormal(tex2D(_MainTexNormal, IN.uv_MaskTex)), blendFactor);
        }
        ENDCG
    }
    FallBack "Diffuse"
}
