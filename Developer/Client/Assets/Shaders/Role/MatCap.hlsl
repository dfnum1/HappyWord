#ifndef MATCAP_HLSH
#define MATCAP_HLSH

#include "PBRLib.hlsl"
#include "../Includes/WorldCurvedCG.hlsl"

//�������(ƬԪ����)�ṹ
struct VertexToFragment
{
	float4 diffuseUVAndMatCapCoords : TEXCOORD0;
	float4 position : SV_POSITION;
	float3 worldSpaceReflectionVector : TEXCOORD1;
	float3 worldPos : TEXCOORD2;
	float4 TtoW0 : TEXCOORD3;
	float4 TtoW1 : TEXCOORD4;
	float4 TtoW2 : TEXCOORD5;//xyz �洢�� �����߿ռ䵽����ռ�ľ���w�洢����������
#if USE_DISSOLVE
	float3 dissolve : TEXCOORD6;
	UNITY_FOG_COORDS(7)
#else
	UNITY_FOG_COORDS(6)
#endif
	
};

VertexToFragment MatCapVS(appdata_tan v)
{
	VertexToFragment output;

	//������UV����׼�����洢��TEXCOORD1��ǰ��������xy��
	output.diffuseUVAndMatCapCoords.xy = TRANSFORM_TEX(v.texcoord, _DiffuseTex);

	//MatCap����׼���������ߴ�ģ�Ϳռ�ת�����۲�ռ䣬�洢��TEXCOORD1�ĺ�������������zw
	output.diffuseUVAndMatCapCoords.z = dot(normalize(UNITY_MATRIX_IT_MV[0].xyz), normalize(v.normal));
	output.diffuseUVAndMatCapCoords.w = dot(normalize(UNITY_MATRIX_IT_MV[1].xyz), normalize(v.normal));
	//��һ���ķ���ֵ����[-1,1]ת�������������������[0,1]
	output.diffuseUVAndMatCapCoords.zw = output.diffuseUVAndMatCapCoords.zw * 0.5 + 0.5;

	//����任
	CURVED_WORLD_TRANSFORM_POINT(v.vertex,_CurveFactor);
	output.position = UnityObjectToClipPos(v.vertex);

	//����ռ�λ��
	output.worldPos = mul(UNITY_MATRIX_M, v.vertex).xyz;

	//����ռ䷨��
	float3 worldNormal = normalize(mul((float3x3)UNITY_MATRIX_M, v.normal));
	half3 worldTangent = UnityObjectToWorldDir(v.tangent.xyz).xyz;
	half3 worldBinormal = cross(worldNormal, worldTangent) * v.tangent.w;

#if USE_DISSOLVE
	output.dissolve.xy = TRANSFORM_TEX(v.texcoord, _DissolveNoise);
	output.dissolve.z = (output.worldPos.y - _DissolveHeight*(1 - _DissolveAmount*_DissolveHeightSpeed))*_DissolveHeightSpeed;
#endif

	//����ռ䷴������
	output.worldSpaceReflectionVector = reflect(output.worldPos - _WorldSpaceCameraPos.xyz, worldNormal);


	//ǰ3x3�洢�Ŵ����߿ռ䵽����ռ�ľ��󣬺�3x1�洢����������
	output.TtoW0 = float4(worldTangent.x, worldBinormal.x, worldNormal.x, output.worldPos.x);
	output.TtoW1 = float4(worldTangent.y, worldBinormal.y, worldNormal.y, output.worldPos.y);
	output.TtoW2 = float4(worldTangent.z, worldBinormal.z, worldNormal.z, output.worldPos.z);

	UNITY_TRANSFER_FOG(output, output.position);
	return output;
}

float4 MatCapFS(VertexToFragment input)
{
	float3 worldPos = float3(input.TtoW0.w,input.TtoW1.w,input.TtoW2.w);//��������

	half3 normal = UnpackNormal(tex2D(_BumpMap, input.diffuseUVAndMatCapCoords.xy));
	half3 normalTangent = normal*_BumpValue;
	normalTangent.z = sqrt(1.0 - saturate(dot(normalTangent.xy, normalTangent.xy)));
	half3 worldNormal = normalize(half3(dot(input.TtoW0.xyz, normalTangent)*(_BumpValue + 1), dot(input.TtoW1.xyz, normalTangent)*(_BumpValue + 1), dot(input.TtoW2.xyz, normalTangent)));
	float3 lightDir = -GetMainLight().direction * _UseDirLight + _ShdowLight*(1- _UseDirLight);

	float3 viewDir = normalize(_WorldSpaceCameraPos.xyz - worldPos);

	half3 refDir = reflect(-viewDir, worldNormal);
	half3 halfDir = normalize(lightDir + viewDir);
	half nv = saturate(dot(worldNormal, viewDir));
	half nl = saturate(dot(worldNormal, lightDir));
	half nh = saturate(dot(worldNormal, halfDir));
	half lv = saturate(dot(lightDir, viewDir));
	half lh = saturate(dot(lightDir, halfDir));

	float roughness = saturate(max(tex2D(_RoughnessMap, input.diffuseUVAndMatCapCoords.xy).r, _RoughnessStrength + EPS));
	float metalness = 0;// tex2D(_MetallicGlossMap, input.diffuseUVAndMatCapCoords.xy).r*_MetallicStrength;
	float occlusion = saturate(tex2D(_AOMap, input.diffuseUVAndMatCapCoords.xy).r);

	//��������ɫ
	float4 diffuseColor = tex2D(_DiffuseTex, input.diffuseUVAndMatCapCoords.xy);
	Util_AmbientColor(diffuseColor,_AmbientStrength);
	diffuseColor.rgb += tex2D(_EmissionMap, input.diffuseUVAndMatCapCoords.xy).rgb * _EmissionStrength;

	half Rim = 1.0 - max(0, nv);
	float rimX = pow(Rim, 1 / _RimEdgePower)*_RimFactor;
	diffuseColor.rgb = diffuseColor.rgb*(1 - rimX) + _RimColor *rimX;

	//����1 - ������,�������ܱ���
	half oneMinusReflectivity = (1 - metalness);// *unity_ColorSpaceDielectricSpec.a;
												//������������
	half3 diffColor = diffuseColor.xyz * oneMinusReflectivity;

	//half3 indirectDiffuse =occlusion;//�����ӹ�������
	half3 indirectDiffuse = lerp(lerp(1.0, occlusion, _AOStrength), 1.0, metalness * (1.0 - roughness) * (1.0 - roughness));

	indirectDiffuse *= diffColor;

	half V = ComputeSmithJointGGXVisibilityTerm(nl, nv, roughness);//����BRDF�߹ⷴ����ɼ���V
	half D = ComputeGGXTerm(nh, roughness);//����BRDF�߹ⷴ����,���߷ֲ�����D
	half3 F = 1;// ComputeFresnelTerm(_FresnelColor, lh);//����BRDF�߹ⷴ�����������F

	half3 specularTerm =  V * D * F;//���㾵�淴����
	half3 diffuseTerm = ComputeDisneyDiffuseTerm(nv, nl, lh, roughness, diffColor);//������������
	specularTerm = clamp(specularTerm, 0, 1);
																				   //���ṩ��MatCap�����У���ȡ����Ӧ������Ϣ
	float3 matCapColor = tex2D(_MatCap, input.diffuseUVAndMatCapCoords.zw).rgb;

	float3 baseColor = M_PI * (diffuseTerm + specularTerm) * nl*matCapColor.rgb*_MatCapStrength*D;
	//ϸ����ɫ������ɫ���в�ֵ����Ϊ�µ�����ɫ
	half3 mainColor = baseColor + indirectDiffuse;

	//������ɫ
	float4 finalColor = float4(mainColor*_MainColor.rgb *_ProjectorColor.rgb, _MainColor.a*_Alpha);
#if USE_DISSOLVE
	fixed dissove = tex2D(_DissolveNoise, input.dissolve.xy).r;
	dissove = dissove + (1 - _DissolveAmount);
	float dissolve_alpha = 1 - input.dissolve.z*dissove;
	clip(dissolve_alpha);
	clip(dissove - 1);
	float edge_area = saturate(1 - saturate((dissove - 1 + _DissolveEdgeWidth) / _DissolveSmoothness));
	edge_area *= _DissolveEdgeColor.a*saturate(_DissolveAmount);
	finalColor.rgb = lerp(finalColor.rgb, _DissolveEdgeColor.rgb * 10, edge_area);
#else
	finalColor = clamp(finalColor, 0, 1.5);
#endif

	UNITY_APPLY_FOG(input.fogCoord, finalColor);
	return finalColor;
}


#endif