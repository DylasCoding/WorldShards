// Shader created with Shader Forge v1.38 
// Shader Forge (c) Neat Corporation / Joachim Holmer - http://www.acegikmo.com/shaderforge/
// Note: Manually altering this data may prevent you from opening it in Shader Forge
/*SF_DATA;ver:1.38;sub:START;pass:START;ps:flbk:,iptp:0,cusa:False,bamd:0,cgin:,lico:1,lgpr:1,limd:0,spmd:1,trmd:0,grmd:0,uamb:True,mssp:True,bkdf:False,hqlp:False,rprd:False,enco:False,rmgx:True,imps:True,rpth:0,vtps:0,hqsc:True,nrmq:1,nrsp:0,vomd:0,spxs:False,tesm:0,olmd:1,culm:0,bsrc:3,bdst:7,dpts:2,wrdp:True,dith:0,atcv:False,rfrpo:True,rfrpn:Refraction,coma:15,ufog:True,aust:True,igpj:False,qofs:0,qpre:2,rntp:3,fgom:False,fgoc:False,fgod:False,fgor:False,fgmd:0,fgcr:0.5,fgcg:0.5,fgcb:0.5,fgca:1,fgde:0.01,fgrn:0,fgrf:300,stcl:False,atwp:False,stva:128,stmr:255,stmw:255,stcp:6,stps:0,stfa:0,stfz:0,ofsf:0,ofsu:0,f2p0:False,fnsp:False,fnfb:False,fsmp:False;n:type:ShaderForge.SFN_Final,id:9361,x:33209,y:32712,varname:node_9361,prsc:2|custl-106-OUT,clip-5046-A;n:type:ShaderForge.SFN_Tex2d,id:5046,x:32650,y:32656,ptovrint:False,ptlb:node_5046,ptin:_node_5046,varname:node_5046,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,ntxv:0,isnm:False;n:type:ShaderForge.SFN_Color,id:2799,x:32640,y:32882,ptovrint:False,ptlb:Color,ptin:_Color,varname:node_2799,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,c1:0,c2:0,c3:0,c4:0;n:type:ShaderForge.SFN_Add,id:106,x:32933,y:32649,varname:node_106,prsc:2|A-5046-RGB,B-2799-RGB;proporder:5046-2799;pass:END;sub:END;*/

Shader "Shader Forge/FX_Diffuse" {
    Properties {
        _node_5046 ("node_5046", 2D) = "white" {}
        _Color1 ("Color1", Color) = (0,0,0,0)
		_Color2("Color2", Color) = (0,0,0,0)
		_Color3("Color3", Color) = (0,0,0,0)
        _Cutoff ("Alpha cutoff", Range(0,1)) = 0.5
        _TransVal ("Transparency Value", Range(0,1)) = 1
		_Change_Value1("Change_Value1", Range(0, 1)) = 0
		_Change_Value2("Change_Value2", Range(0, 1)) = 0
		_ColorSign ("ColorSign", Float) = 1
    }
    SubShader {
        Tags {
            "Queue"="AlphaTest"
            "RenderType"="TransparentCutout"
        }
        Pass {
            Name "FORWARD"
            Tags {
                "LightMode"="ForwardBase"
            }
            Blend SrcAlpha OneMinusSrcAlpha
			Cull off
			//Blend OneMinusDstColor One
			//BlendOp Min
            
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #define UNITY_PASS_FORWARDBASE
            #include "UnityCG.cginc"
            #pragma multi_compile_fwdbase_fullshadows
            #pragma multi_compile_fog
            #pragma only_renderers d3d9 d3d11 glcore gles metal
            #pragma target 3.0
            uniform sampler2D _node_5046; uniform float4 _node_5046_ST;
            uniform float4 _Color1;
			uniform float4 _Color2;
			uniform float4 _Color3;
			float _Change_Value2;
			float _Change_Value1;
			fixed _Cutoff;
			float _TransVal;
			float _ColorSign;
            struct VertexInput {
                float4 vertex : POSITION;
                float2 texcoord0 : TEXCOORD0;
            };
            struct VertexOutput {
                float4 pos : SV_POSITION;
                float2 uv0 : TEXCOORD0;
                UNITY_FOG_COORDS(1)
            };
            VertexOutput vert (VertexInput v) {
                VertexOutput o = (VertexOutput)0;
                o.uv0 = v.texcoord0;
                o.pos = UnityObjectToClipPos( v.vertex );
                UNITY_TRANSFER_FOG(o,o.pos);
                return o;
            }
            float4 frag(VertexOutput i) : COLOR {
                float4 _node_5046_var = tex2D(_node_5046,TRANSFORM_TEX(i.uv0, _node_5046));
                clip(_node_5046_var.a - _Cutoff);

				float3 emissive1 = lerp(_Color1.rgb, _Color3.rgb, _Change_Value1);
				float3 emissive2 = lerp(emissive1.rgb, _Color2.rgb, _Change_Value2) * _ColorSign;
                float3 finalColor = (_node_5046_var.rgb+ emissive2.rgb);
                fixed4 finalRGBA = fixed4(finalColor,_TransVal);
                UNITY_APPLY_FOG(i.fogCoord, finalRGBA);
                return finalRGBA;
            }
            ENDCG
        }
     
    }
    FallBack "Diffuse"
    CustomEditor "ShaderForgeMaterialInspector"
}
