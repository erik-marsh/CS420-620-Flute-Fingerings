// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'

// Upgrade NOTE: replaced '_Object2World' with 'unity_ObjectToWorld'

// Shader created with Shader Forge v1.30 
// Shader Forge (c) Neat Corporation / Joachim Holmer - http://www.acegikmo.com/shaderforge/
// Note: Manually altering this data may prevent you from opening it in Shader Forge
/*SF_DATA;ver:1.30;sub:START;pass:START;ps:flbk:,iptp:0,cusa:False,bamd:0,lico:1,lgpr:1,limd:0,spmd:1,trmd:0,grmd:0,uamb:True,mssp:True,bkdf:False,hqlp:False,rprd:False,enco:False,rmgx:True,rpth:0,vtps:0,hqsc:True,nrmq:1,nrsp:0,vomd:0,spxs:False,tesm:0,olmd:1,culm:0,bsrc:0,bdst:1,dpts:2,wrdp:True,dith:0,rfrpo:True,rfrpn:Refraction,coma:15,ufog:False,aust:True,igpj:False,qofs:0,qpre:1,rntp:1,fgom:False,fgoc:False,fgod:False,fgor:False,fgmd:0,fgcr:0.5,fgcg:0.5,fgcb:0.5,fgca:1,fgde:0.01,fgrn:0,fgrf:300,stcl:False,stva:128,stmr:255,stmw:255,stcp:6,stps:0,stfa:0,stfz:0,ofsf:0,ofsu:0,f2p0:False,fnsp:False,fnfb:False;n:type:ShaderForge.SFN_Final,id:3138,x:33907,y:32728,varname:node_3138,prsc:2|emission-5628-OUT,voffset-6631-OUT;n:type:ShaderForge.SFN_Slider,id:2486,x:31385,y:32733,ptovrint:False,ptlb:node_5349_copy,ptin:_node_5349_copy,varname:_node_5349_copy,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,min:0,cur:0.78639,max:1;n:type:ShaderForge.SFN_RemapRange,id:4672,x:31790,y:32740,varname:node_4672,prsc:2,frmn:0,frmx:1,tomn:0,tomx:4|IN-2486-OUT;n:type:ShaderForge.SFN_Multiply,id:5583,x:32649,y:32598,varname:node_5583,prsc:2|A-4108-OUT,B-2006-RGB;n:type:ShaderForge.SFN_Color,id:2006,x:32425,y:32788,ptovrint:False,ptlb:Color_copy,ptin:_Color_copy,varname:_Color_copy,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,c1:0.7720588,c2:0.873366,c3:1,c4:1;n:type:ShaderForge.SFN_Multiply,id:2807,x:33100,y:32650,varname:node_2807,prsc:2|A-5583-OUT,B-946-OUT;n:type:ShaderForge.SFN_Vector1,id:946,x:32773,y:32805,varname:node_946,prsc:2,v1:3.5;n:type:ShaderForge.SFN_Time,id:4408,x:31163,y:32719,varname:node_4408,prsc:2;n:type:ShaderForge.SFN_Panner,id:2966,x:31373,y:32487,varname:node_2966,prsc:2,spu:1,spv:1|UVIN-37-UVOUT,DIST-4408-TSL;n:type:ShaderForge.SFN_Tex2d,id:6132,x:31609,y:32427,ptovrint:False,ptlb:node_5255,ptin:_node_5255,varname:node_5255,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,tex:22563b23cd1cc8b489e8cde0546750e1,ntxv:0,isnm:False|UVIN-2966-UVOUT;n:type:ShaderForge.SFN_Add,id:7930,x:31919,y:32360,varname:node_7930,prsc:2|A-6132-R,B-9747-OUT;n:type:ShaderForge.SFN_Posterize,id:5497,x:32177,y:32367,varname:node_5497,prsc:2|IN-7930-OUT,STPS-8201-OUT;n:type:ShaderForge.SFN_Vector1,id:8201,x:31919,y:32512,varname:node_8201,prsc:2,v1:3;n:type:ShaderForge.SFN_Vector1,id:9747,x:31542,y:32606,varname:node_9747,prsc:2,v1:0.4;n:type:ShaderForge.SFN_Multiply,id:8960,x:32407,y:32181,varname:node_8960,prsc:2|A-9104-R,B-5497-OUT;n:type:ShaderForge.SFN_Vector1,id:423,x:33306,y:32754,varname:node_423,prsc:2,v1:0.5;n:type:ShaderForge.SFN_TexCoord,id:37,x:31053,y:32492,varname:node_37,prsc:2,uv:0;n:type:ShaderForge.SFN_Lerp,id:5628,x:33505,y:32600,varname:node_5628,prsc:2|A-4771-OUT,B-2807-OUT,T-423-OUT;n:type:ShaderForge.SFN_Multiply,id:6410,x:32850,y:32055,varname:node_6410,prsc:2|A-8960-OUT,B-4962-OUT;n:type:ShaderForge.SFN_Vector1,id:4962,x:32557,y:32424,varname:node_4962,prsc:2,v1:30;n:type:ShaderForge.SFN_Tex2d,id:9104,x:31744,y:32197,ptovrint:False,ptlb:node_9104,ptin:_node_9104,varname:node_9104,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,tex:d60a400ee342fd3448e0727e41a1032e,ntxv:0,isnm:False;n:type:ShaderForge.SFN_Multiply,id:9192,x:33074,y:32191,varname:node_9192,prsc:2|A-6410-OUT,B-1352-RGB;n:type:ShaderForge.SFN_Color,id:1352,x:32730,y:32227,ptovrint:False,ptlb:node_1352,ptin:_node_1352,varname:node_1352,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,c1:0.7647059,c2:0.8645277,c3:1,c4:1;n:type:ShaderForge.SFN_FragmentPosition,id:5875,x:31723,y:33277,varname:node_5875,prsc:2;n:type:ShaderForge.SFN_Vector4Property,id:3044,x:31735,y:33470,ptovrint:False,ptlb:bullet_pos,ptin:_bullet_pos,varname:node_3044,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,v1:0,v2:0,v3:0,v4:0;n:type:ShaderForge.SFN_Vector4Property,id:9037,x:32117,y:33699,ptovrint:False,ptlb:bullet_dir,ptin:_bullet_dir,varname:node_9037,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,v1:0,v2:0,v3:0,v4:0;n:type:ShaderForge.SFN_Subtract,id:3204,x:32172,y:33429,varname:node_3204,prsc:2|A-5875-XYZ,B-3044-XYZ;n:type:ShaderForge.SFN_Dot,id:4861,x:32651,y:33567,varname:node_4861,prsc:2,dt:0|A-7279-OUT,B-2998-OUT;n:type:ShaderForge.SFN_Normalize,id:7279,x:32407,y:33429,varname:node_7279,prsc:2|IN-3204-OUT;n:type:ShaderForge.SFN_Normalize,id:2998,x:32407,y:33687,varname:node_2998,prsc:2|IN-9037-XYZ;n:type:ShaderForge.SFN_Multiply,id:4043,x:33351,y:33642,varname:node_4043,prsc:2|A-2551-OUT,B-7054-OUT;n:type:ShaderForge.SFN_Vector1,id:9051,x:32743,y:33718,varname:node_9051,prsc:2,v1:3;n:type:ShaderForge.SFN_Slider,id:4373,x:32664,y:33861,ptovrint:False,ptlb:node_4373,ptin:_node_4373,varname:node_4373,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,min:0,cur:0,max:1;n:type:ShaderForge.SFN_NormalVector,id:9675,x:33092,y:33375,prsc:2,pt:False;n:type:ShaderForge.SFN_Multiply,id:6631,x:33341,y:33412,varname:node_6631,prsc:2|A-9675-OUT,B-4043-OUT;n:type:ShaderForge.SFN_Power,id:2551,x:33054,y:33611,varname:node_2551,prsc:2|VAL-7688-OUT,EXP-9051-OUT;n:type:ShaderForge.SFN_OneMinus,id:7688,x:32833,y:33501,varname:node_7688,prsc:2|IN-4861-OUT;n:type:ShaderForge.SFN_RemapRange,id:7054,x:33111,y:33808,varname:node_7054,prsc:2,frmn:0,frmx:1,tomn:0,tomx:0.005|IN-4373-OUT;n:type:ShaderForge.SFN_Fresnel,id:4108,x:32066,y:32711,varname:node_4108,prsc:2|EXP-4672-OUT;n:type:ShaderForge.SFN_Multiply,id:4771,x:33341,y:32297,varname:node_4771,prsc:2|A-9192-OUT,B-6132-RGB;proporder:2486-2006-6132-9104-1352-3044-9037-4373;pass:END;sub:END;*/

Shader "Shader Forge/new_head" {
    Properties {
        _node_5349_copy ("node_5349_copy", Range(0, 1)) = 0.78639
        _Color_copy ("Color_copy", Color) = (0.7720588,0.873366,1,1)
        _node_5255 ("node_5255", 2D) = "white" {}
        _node_9104 ("node_9104", 2D) = "white" {}
        _node_1352 ("node_1352", Color) = (0.7647059,0.8645277,1,1)
        _bullet_pos ("bullet_pos", Vector) = (0,0,0,0)
        _bullet_dir ("bullet_dir", Vector) = (0,0,0,0)
        _node_4373 ("node_4373", Range(0, 1)) = 0
    }
    SubShader {
        Tags {
            "RenderType"="Opaque"
        }
        Pass {
            Name "FORWARD"
            Tags {
                "LightMode"="ForwardBase"
            }
            
            
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #define UNITY_PASS_FORWARDBASE
            #include "UnityCG.cginc"
            #pragma multi_compile_fwdbase_fullshadows
            #pragma exclude_renderers gles3 metal d3d11_9x xbox360 xboxone ps3 ps4 psp2 
            #pragma target 3.0
            uniform float4 _TimeEditor;
            uniform float _node_5349_copy;
            uniform float4 _Color_copy;
            uniform sampler2D _node_5255; uniform float4 _node_5255_ST;
            uniform sampler2D _node_9104; uniform float4 _node_9104_ST;
            uniform float4 _node_1352;
            uniform float4 _bullet_pos;
            uniform float4 _bullet_dir;
            uniform float _node_4373;
            struct VertexInput {
                float4 vertex : POSITION;
                float3 normal : NORMAL;
                float2 texcoord0 : TEXCOORD0;
            };
            struct VertexOutput {
                float4 pos : SV_POSITION;
                float2 uv0 : TEXCOORD0;
                float4 posWorld : TEXCOORD1;
                float3 normalDir : TEXCOORD2;
            };
            VertexOutput vert (VertexInput v) {
                VertexOutput o = (VertexOutput)0;
                o.uv0 = v.texcoord0;
                o.normalDir = UnityObjectToWorldNormal(v.normal);
                v.vertex.xyz += (v.normal*(pow((1.0 - dot(normalize((mul(unity_ObjectToWorld, v.vertex).rgb-_bullet_pos.rgb)),normalize(_bullet_dir.rgb))),3.0)*(_node_4373*0.005+0.0)));
                o.posWorld = mul(unity_ObjectToWorld, v.vertex);
                o.pos = UnityObjectToClipPos(v.vertex );
                return o;
            }
            float4 frag(VertexOutput i) : COLOR {
                i.normalDir = normalize(i.normalDir);
                float3 viewDirection = normalize(_WorldSpaceCameraPos.xyz - i.posWorld.xyz);
                float3 normalDirection = i.normalDir;
////// Lighting:
////// Emissive:
                float4 _node_9104_var = tex2D(_node_9104,TRANSFORM_TEX(i.uv0, _node_9104));
                float4 node_4408 = _Time + _TimeEditor;
                float2 node_2966 = (i.uv0+node_4408.r*float2(1,1));
                float4 _node_5255_var = tex2D(_node_5255,TRANSFORM_TEX(node_2966, _node_5255));
                float node_8201 = 3.0;
                float3 emissive = lerp(((((_node_9104_var.r*floor((_node_5255_var.r+0.4) * node_8201) / (node_8201 - 1))*30.0)*_node_1352.rgb)*_node_5255_var.rgb),((pow(1.0-max(0,dot(normalDirection, viewDirection)),(_node_5349_copy*4.0+0.0))*_Color_copy.rgb)*3.5),0.5);
                float3 finalColor = emissive;
                return fixed4(finalColor,1);
            }
            ENDCG
        }
        Pass {
            Name "ShadowCaster"
            Tags {
                "LightMode"="ShadowCaster"
            }
            Offset 1, 1
            
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #define UNITY_PASS_SHADOWCASTER
            #include "UnityCG.cginc"
            #include "Lighting.cginc"
            #pragma fragmentoption ARB_precision_hint_fastest
            #pragma multi_compile_shadowcaster
            #pragma exclude_renderers gles3 metal d3d11_9x xbox360 xboxone ps3 ps4 psp2 
            #pragma target 3.0
            uniform float4 _bullet_pos;
            uniform float4 _bullet_dir;
            uniform float _node_4373;
            struct VertexInput {
                float4 vertex : POSITION;
                float3 normal : NORMAL;
            };
            struct VertexOutput {
                V2F_SHADOW_CASTER;
                float4 posWorld : TEXCOORD1;
                float3 normalDir : TEXCOORD2;
            };
            VertexOutput vert (VertexInput v) {
                VertexOutput o = (VertexOutput)0;
                o.normalDir = UnityObjectToWorldNormal(v.normal);
                v.vertex.xyz += (v.normal*(pow((1.0 - dot(normalize((mul(unity_ObjectToWorld, v.vertex).rgb-_bullet_pos.rgb)),normalize(_bullet_dir.rgb))),3.0)*(_node_4373*0.005+0.0)));
                o.posWorld = mul(unity_ObjectToWorld, v.vertex);
                o.pos = UnityObjectToClipPos(v.vertex );
                TRANSFER_SHADOW_CASTER(o)
                return o;
            }
            float4 frag(VertexOutput i) : COLOR {
                i.normalDir = normalize(i.normalDir);
                float3 normalDirection = i.normalDir;
                SHADOW_CASTER_FRAGMENT(i)
            }
            ENDCG
        }
    }
    FallBack "Diffuse"
    CustomEditor "ShaderForgeMaterialInspector"
}
