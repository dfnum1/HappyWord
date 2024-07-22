#ifndef _PBR_LIB_HLSL_
#define _PBR_LIB_HLSL_
#include "../Includes/BaseDefine/CommonDefine.hlsl"

float fPow5(float v)
{
	return Pow5(1 - v);
}
// Diffuse distribution functions

float3 lambertDiffuse(float3 albedo)
{
    return albedo / M_PI;
}

// Fresnel functions

half3 ComputeFresnelLerp(half3 c0, half3 c1, half cosA)
{
	half t = Pow5(1 - cosA);
	return lerp(c0, c1, t);
}

//���������
inline half3 ComputeFresnelTerm(half3 F0, half cosA)
{
	return F0 + (1 - F0) * Pow5(1 - cosA);
}

float3 fresnel(float3 F0, float NdotV, float roughness)
{
    return F0 + (max(1.0 - roughness, F0) - F0) * fPow5(NdotV);
}

float3 fresnelDisney(float HdotL, float NdotL, float NdotV, float roughness)
{
    float k = 0.5 + 2 * roughness * sqrt(HdotL);
    float firstTerm = (k - 1) * fPow5(NdotV) + 1;
    float secondTerm = (k - 1) * fPow5(NdotL) + 1;
    return firstTerm * secondTerm;
}

float3 F0(float ior)
{
    return Pow2((1.0 - ior) / (1.0 + ior));
}

// Normal distribution functions

float trowbridgeReitzNDF(float NdotH, float roughness)
{
    float alpha = roughness * roughness;
    float alpha2 = alpha * alpha;
    float NdotH2 = NdotH * NdotH;
    float denominator = M_PI * Pow2((alpha2 - 1) * NdotH2 + 1);
    return alpha2 / denominator;
}

float trowbridgeReitzAnisotropicNDF(float NdotH, float roughness, float anisotropy, float HdotT, float HdotB)
{
    float aspect = sqrt(1.0 - 0.9 * anisotropy);
    float alpha = roughness * roughness;

    float roughT = alpha / aspect;
    float roughB = alpha * aspect;

    float alpha2 = alpha * alpha;
    float NdotH2 = NdotH * NdotH;
    float HdotT2 = HdotT * HdotT;
    float HdotB2 = HdotB * HdotB;

    float denominator = M_PI * roughT * roughB * Pow2(HdotT2 / (roughT * roughT) + HdotB2 / (roughB * roughB) + NdotH2);
    return 1 / denominator;
}

// Geometric attenuation functions

float cookTorranceGAF(float NdotH, float NdotV, float HdotV, float NdotL)
{
    float firstTerm = 2 * NdotH * NdotV / HdotV;
    float secondTerm = 2 * NdotH * NdotL / HdotV;
    return min(1, min(firstTerm, secondTerm));
}

float schlickBeckmannGAF(float dotProduct, float roughness)
{
    float alpha = roughness * roughness;
    float k = alpha * 0.797884560803;  // sqrt(2 / PI)
    return dotProduct / (dotProduct * (1 - k) + k);
}

// Helpers
float3 gammaCorrection(float3 v)
{
    return pow(v, 1.0 / 2.2);
}

float3 sRGB2Lin(float3 col)
{
    return pow(col, 2.2);
}

//����Smith-Joint��Ӱ���ں��������ص��ǳ��Ծ��淴�����ĸ�Ŀɼ�����V
inline half ComputeSmithJointGGXVisibilityTerm(half nl, half nv, half roughness)
{
	half ag = roughness * roughness;
	half lambdaV = nl * (nv * (1 - ag) + ag);
	half lambdaL = nv * (nl * (1 - ag) + ag);

	return 0.5f / (lambdaV + lambdaL + 1e-5f);
}
//���㷨�߷ֲ�����
inline half ComputeGGXTerm(half nh, half roughness)
{
	half a = roughness * roughness;
	half a2 = a * a;
	half d = (a2 - 1.0f) * nh * nh + 1.0f;
	return a2 * M_INV_PI / (d * d + 1e-5f);
}

inline half3 ComputeDisneyDiffuseTerm(half nv, half nl, half lh, half roughness, half3 baseColor)
{
	half Fd90 = 0.5f + 2 * roughness * lh * lh;
	return baseColor * M_INV_PI * (1 + (Fd90 - 1) * Pow5(1 - nl)) * (1 + (Fd90 - 1) * Pow5(1 - nv));
}

//���㻷�����ջ������ͼuv����
inline half4 VertexGI(float2 uv1, float2 uv2, float3 worldPos, float3 worldNormal)
{
	half4 ambientOrLightmapUV = 0;

	//�������������ͼ�����������ͼ��uv����
#ifdef LIGHTMAP_ON
	ambientOrLightmapUV.xy = uv1.xy * unity_LightmapST.xy + unity_LightmapST.zw;
	//���Զ�̬�����������̽ͷ,������UnityCG.cginc
#elif UNITY_SHOULD_SAMPLE_SH
	//�������Ҫ�Ķ������
#ifdef VERTEXLIGHT_ON
	//����4��������գ�������UnityCG.cginc
	ambientOrLightmapUV.rgb = Shade4PointLights(
		unity_4LightPosX0, unity_4LightPosY0, unity_4LightPosZ0,
		unity_LightColor[0].rgb, unity_LightColor[1].rgb, unity_LightColor[2].rgb, unity_LightColor[3].rgb,
		unity_4LightAtten0, worldPos, worldNormal);
#endif
	//������г���գ�������UnityCG.cginc
	ambientOrLightmapUV.rgb += ShadeSH9(half4(worldNormal, 1));
#endif

	//��������� ��̬������ͼ�����㶯̬������ͼ��uv����
#ifdef DYNAMICLIGHTMAP_ON
	ambientOrLightmapUV.zw = uv2.xy * unity_DynamicLightmapST.xy + unity_DynamicLightmapST.zw;
#endif

	return ambientOrLightmapUV;
}
//�����ӹ�������
inline half3 ComputeIndirectDiffuse(half4 ambientOrLightmapUV, half occlusion)
{
	half3 indirectDiffuse = 0;

	//����Ƕ�̬���壬��ӹ�������Ϊ�ڶ��㺯���м���ķ���Ҫ��Դ
#if UNITY_SHOULD_SAMPLE_SH
	indirectDiffuse = ambientOrLightmapUV.rgb;
#endif

	//���ھ�̬���壬�����������ͼ��̬������ͼ
#ifdef LIGHTMAP_ON
	//�Թ�����ͼ���в����ͽ���
	//UNITY_SAMPLE_TEX2D������HLSLSupport.cginc
	//DecodeLightmap������UnityCG.cginc
	indirectDiffuse = DecodeLightmap(UNITY_SAMPLE_TEX2D(unity_Lightmap, ambientOrLightmapUV.xy));
#endif
#ifdef DYNAMICLIGHTMAP_ON
	//�Զ�̬������ͼ���в����ͽ���
	//DecodeRealtimeLightmap������UnityCG.cginc
	indirectDiffuse += DecodeRealtimeLightmap(UNITY_SAMPLE_TEX2D(unity_DynamicLightmap, ambientOrLightmapUV.zw));
#endif

	//����ӹ���������Ի��������֣�����
	return indirectDiffuse * occlusion;
}

#endif