/********************************************************************
生成日期:	26:11:2019 
类    名: 	SettingConfig
作    者:	
描    述:	游戏品质配置
*********************************************************************/
using System;
using Framework.Data;
using Framework.URP;
using UnityEngine;
using UnityEngine.Rendering;
namespace TopGame.Data
{
    [System.Serializable]
    public struct QualityConfig : AConfig
    {
        public static QualityConfig DEFAULT = new QualityConfig()
        {
            nQualityLevel = 4,
            ThresholdSystemMemory = 1024,   //MB
            TargetFrameRate = 30,
            ResolutionScaler = Vector2.one,
            Tier = GraphicsTier.Tier1,

            LOD = 100,

            EnableFog = true,
            SoftParticles = false,

            eShadowQuality = ShadowQuality.Disable,
            ShadowLevel = ShadowResolution.Low,
            ShadowDistance = 5,

            SyncCount = 0,

            allowThreadedTextureCreation = true,
            textureQuality = ETextureQuality.Full,
            anisotropicFiltering =AnisotropicFiltering.Disable,

            AntiAliasing = EAntiSamplingType.Disable,
            MSAA = false,
            HDR = false,

            BoneWeight = SkinWeights.TwoBones,

            //! 逻辑相关
            LimitParticleMaxCount = false,
            nMaxParticleCount = 3,

            bForceLOWLOD = false,

            TerrainBrushCheckVisible = 0,
            ProjectorLightCheckGap = 0.5f,

            OneFrameCost = 300,
            MaxInstanceCount = 30,
            DestroyDelayTime = 60,

            AsyncUploadTimeSlice = 4,
            AsyncUploadBuffSize = 32,
            ReadAbStreamBuffSize = 32*1024,

            LowerFpsThreshold = 20,
            bInstanceBrush = true,

            nURPPassFlags = 0xffffffff,
        };
        public enum EAntiSamplingType : byte
        {
            [Framework.Plugin.PluginDisplay("禁用")]
            Disable = 0,
            [Framework.Plugin.PluginDisplay("x2")]
            Samplingx2 = 2,
            [Framework.Plugin.PluginDisplay("x4")]
            Samplingx4 = 4,
            [Framework.Plugin.PluginDisplay("x8")]
            Samplingx8 = 8
        }
        public enum ETextureQuality : byte
        {
            [Framework.Plugin.PluginDisplay("全品质")]
            Full = 0,
            [Framework.Plugin.PluginDisplay("半品质")]
            Half = 1,
            [Framework.Plugin.PluginDisplay("四分之一品质")]
            Quarter = 2,
            [Framework.Plugin.PluginDisplay("八分之一品质")]
            Eighth = 3,
        }

        [Framework.Plugin.PluginDisplay("图形渲染等级")]
        [Framework.Plugin.DisableGUI]
        public int                  nQualityLevel;
        [Framework.Plugin.PluginDisplay(">=内存阀值大小(MB)")]
        public int                  ThresholdSystemMemory;   //MB
        [Framework.Plugin.PluginDisplay("限制帧数")]
        public int                  TargetFrameRate ;
        [Framework.Plugin.PluginDisplay("降屏")]
        public Vector2      ResolutionScaler;

        [Framework.Plugin.DisableGUI]
        [Framework.Plugin.PluginDisplay("图形参数")]
        public GraphicsTier         Tier;

        [Framework.Plugin.PluginDisplay("LOD等级")]
        public int                  LOD;

        [Framework.Plugin.PluginDisplay("启用雾效")]
        public bool                 EnableFog;
        [Framework.Plugin.PluginDisplay("启用软粒子")]
        public bool                 SoftParticles;

        [Framework.Plugin.PluginDisplay("阴影品质"), Framework.Data.StateGUIByField("urpAsset", "null")]
        public ShadowQuality        eShadowQuality;
        [Framework.Plugin.PluginDisplay("阴影等级")]
        [Framework.Data.StateGUIByField("eShadowQuality", new string[]{ "HardOnly", "All" }), Framework.Data.StateGUIByField("urpAsset", "null")]
        public ShadowResolution     ShadowLevel;
        [Framework.Plugin.PluginDisplay("阴影投影接受距离"), Framework.Data.StateGUIByField("eShadowQuality", new string[] { "HardOnly", "All" }), Framework.Data.StateGUIByField("urpAsset", "null")]
        public int                  ShadowDistance;

        [Framework.Plugin.PluginDisplay("垂直同步")]
        public int                  SyncCount;

        [Framework.Plugin.PluginDisplay("贴图品质")]
        public ETextureQuality      textureQuality;
        [Framework.Plugin.PluginDisplay("允许异步创建纹理(有设备限制)")]
        public bool allowThreadedTextureCreation;

        [Framework.Plugin.PluginDisplay("各向异性纹理")]
        public AnisotropicFiltering anisotropicFiltering;

        [Framework.Plugin.PluginDisplay("抗锯齿级别")]
        public EAntiSamplingType    AntiAliasing;
        [Framework.Plugin.PluginDisplay("启用MAAA抗锯齿")]
        public bool                 MSAA;
        [Framework.Plugin.PluginDisplay("启用HDR")]
        public bool                 HDR;

        [Framework.Plugin.PluginDisplay("骨骼权重级别")]
        public SkinWeights          BoneWeight;

        //! 逻辑相关
        [Framework.Plugin.PluginDisplay("是否限制特效个数")]
        public bool                 LimitParticleMaxCount;
        [Framework.Plugin.PluginDisplay("限制最大个数")]
        [Framework.Data.StateGUIByField("LimitParticleMaxCount", "true")]
        public int                  nMaxParticleCount;

        [Framework.Plugin.PluginDisplay("实例化地表笔刷")]
        public bool bInstanceBrush;

        [Framework.Plugin.PluginDisplay("地表笔刷检测调节")]
        public float TerrainBrushCheckVisible;
        [Framework.Plugin.PluginDisplay("探照灯检测频率(次/秒)")]
        public float ProjectorLightCheckGap;

        [Framework.Plugin.PluginDisplay("是否强制低LOD")]
        public bool bForceLOWLOD;

        [Framework.Plugin.PluginDisplay("资源实例化单帧耗时限制")]
        public int                  OneFrameCost;
        [Framework.Plugin.PluginDisplay("资源实例化单帧最大可实例化个数")]
        public int                  MaxInstanceCount;
        [Framework.Plugin.PluginDisplay("实例化资源延迟销毁时长(s)")]
        public int                  DestroyDelayTime;

        [Framework.Plugin.PluginDisplay("异步加载时间切片(s)")]
        public int AsyncUploadTimeSlice;
        [Framework.Plugin.PluginDisplay("异步加载缓存大小(Mb)")]
        public int AsyncUploadBuffSize;
        [Framework.Plugin.PluginDisplay("AB流缓存池大小(Kb)")]
        public uint ReadAbStreamBuffSize;

        [Framework.Plugin.PluginDisplay("低帧检测")]
        public int LowerFpsThreshold;

        [Framework.Plugin.PluginDisplay("URP")]
        public UnityEngine.Rendering.Universal.UniversalRenderPipelineAsset urpAsset;

        [Framework.Plugin.PluginDisplay("后处理文件")]
        public UnityEngine.Rendering.VolumeProfile postProcess;

        [Framework.Plugin.PluginDisplay("URPPass开关"), Framework.Data.DisplayEnumBitGUI(typeof(EPostPassType), true)]
        public uint nURPPassFlags;

        public void Used(EGameQulity quality, Vector2Int SourceResolution)
        {
            QualitySettings.SetQualityLevel(nQualityLevel, true);
            OnResetResolution(quality,SourceResolution);
            Application.targetFrameRate = TargetFrameRate;
            Shader.globalMaximumLOD = LOD;
            RenderSettings.fog = EnableFog;

            if(UnityEngine.Rendering.GraphicsSettings.renderPipelineAsset == null)
            {
                QualitySettings.shadows = eShadowQuality;
                QualitySettings.shadowResolution = ShadowLevel;
                QualitySettings.shadowDistance = ShadowDistance;

                if (MSAA)
                {
                    QualitySettings.antiAliasing = (int)AntiAliasing;
                }
                else
                    QualitySettings.antiAliasing = 0;

                if (quality == EGameQulity.Low)
                {
                    Tier = GraphicsTier.Tier1;
                }
                else
                {
                    if (SystemInfo.SupportsRenderTextureFormat(RenderTextureFormat.RGB111110Float)) //                //R11G11B10
                    {
                        Tier = GraphicsTier.Tier3;
                    }
                    else if (SystemInfo.SupportsRenderTextureFormat(RenderTextureFormat.ARGBHalf)) //FP16
                    {
                        Tier = GraphicsTier.Tier2;
                    }
                    else
                        Tier = GraphicsTier.Tier1;
                }
                Graphics.activeTier = Tier;
            }

            QualitySettings.softParticles = SoftParticles;
            QualitySettings.vSyncCount = SyncCount;

            QualitySettings.masterTextureLimit = (int)textureQuality;
            Texture.allowThreadedTextureCreation = allowThreadedTextureCreation;

            QualitySettings.skinWeights = BoneWeight;

            QualitySettings.anisotropicFiltering = anisotropicFiltering;

            QualitySettings.asyncUploadTimeSlice = AsyncUploadTimeSlice;
            QualitySettings.asyncUploadBufferSize = AsyncUploadBuffSize;

            QualitySettings.renderPipeline = urpAsset;

#if UNITY_EDITOR || UNITY_STANDALONE
            QualitySettings.antiAliasing = 8;
#endif
            if (Framework.Module.ModuleManager.mainModule != null)
            {
                Framework.Module.ModuleManager.mainModule.OnGameQuality((int)quality, this);
            }
#if !UNITY_EDITOR && UNITY_IOS
            QualitySettings.vSyncCount = 0;
            QualitySettings.antiAliasing = 0;
            if (UnityEngine.Rendering.GraphicsSettings.renderPipelineAsset)
            {
                UnityEngine.Rendering.Universal.UniversalRenderPipelineAsset pipelineAsset = UnityEngine.Rendering.GraphicsSettings.renderPipelineAsset as UnityEngine.Rendering.Universal.UniversalRenderPipelineAsset;
                if(pipelineAsset)
                {
                    pipelineAsset.msaaSampleCount = 0;
                    pipelineAsset.supportsCameraDepthTexture = false;
                }
            }
#endif
        }
        //--------------------------------------------------------------------------
        public void OnResetResolution(EGameQulity quality, Vector2Int SourceResolution, float fFactor = -1)
        {
            //#if UNITY_ANDROID && !UNITY_EDITOR
            //            if (SourceResolution.y >= 720 * 2)
            //            {
            //                Screen.SetResolution(SourceResolution.x / 2, SourceResolution.y / 2, true);
            //            }
            //            else if (SourceResolution.y > 720)
            //            {
            //                Screen.SetResolution(SourceResolution.x * 720 / SourceResolution.y, 720, true);
            //            }
            //            return;
            //#endif
#if UNITY_STANDALONE
            return;
#endif
            if (fFactor > 0)
            {
                Screen.SetResolution(Mathf.RoundToInt(SourceResolution.x * fFactor), Mathf.RoundToInt(SourceResolution.y * fFactor), true);
                //QualitySettings.resolutionScalingFixedDPIFactor = fFactor;
                return;
            }
            bool bPortrait = (Screen.orientation == ScreenOrientation.Portrait || Screen.orientation == ScreenOrientation.PortraitUpsideDown);
            float fFactorX = ResolutionScaler.x;
            float fFactorY = ResolutionScaler.y;
            if (fFactorX <= 0)  fFactorX = 0.8f;
            if (fFactorY <= 0) fFactorY = 0.8f;
            if (fFactorX >= 1 && fFactorY >= 1)
            {
                Screen.SetResolution(SourceResolution.x, SourceResolution.y, true);
                Debug.Log("降屏:" + fFactorX + "  分辨率:" + Screen.currentResolution.width + "," + Screen.currentResolution.height);
                return;
            }

            int finalW = Mathf.RoundToInt(SourceResolution.x * fFactorX);
            int finalH = Mathf.RoundToInt(SourceResolution.y * fFactorY);
            if(bPortrait)
            {
                if (finalW < 720)
                {
                    fFactorY = 720.0f / (float)SourceResolution.x;
                    finalH = Mathf.RoundToInt(SourceResolution.y * fFactorY);
                    finalW = 720;
                }
                else if (finalW > 1080)
                {
                    fFactorY = 1080.0f / (float)SourceResolution.x;
                    finalH = Mathf.RoundToInt(SourceResolution.y * fFactorY);
                    finalW = 1080;
                }
                if (quality >= EGameQulity.High)
                {
                    if (finalW < 1080)
                    {
                        fFactorY = 1080.0f / (float)SourceResolution.x;
                        finalH = Mathf.RoundToInt(SourceResolution.y * fFactorY);
                        finalW = 1080;
                    }
                }
                else
                {
                    if (finalW < 720)
                    {
                        fFactorX = 720.0f / (float)SourceResolution.x;
                        finalH = Mathf.RoundToInt(SourceResolution.y * fFactorY);
                        finalW = 720;
                    }
                }
            }
            else
            {
                if (finalH < 720)
                {
                    fFactorX = 720.0f / (float)SourceResolution.y;
                    finalW = Mathf.RoundToInt(SourceResolution.x * fFactorX);
                    finalH = 720;
                }
                else if (finalH > 1080)
                {
                    fFactorX = 1080.0f / (float)SourceResolution.y;
                    finalW = Mathf.RoundToInt(SourceResolution.x * fFactorX);
                    finalH = 1080;
                }
                if (quality >= EGameQulity.High)
                {
                    if (finalH < 1080)
                    {
                        fFactorX = 1080.0f / (float)SourceResolution.y;
                        finalW = Mathf.RoundToInt(SourceResolution.x * fFactorX);
                        finalH = 1080;
                    }
                }
                else
                {
                    if (finalH < 720)
                    {
                        fFactorX = 720.0f / (float)SourceResolution.y;
                        finalW = Mathf.RoundToInt(SourceResolution.x * fFactorX);
                        finalH = 720;
                    }
                }
            }
            
            Screen.SetResolution(finalW, finalH, true);
            //QualitySettings.resolutionScalingFixedDPIFactor = fFactorX;

            Resolution resolution = Screen.currentResolution;
            Debug.Log("降屏:" + fFactorX + "  分辨率:" + resolution.width + "," + resolution.height);
        }
        //------------------------------------------------------
        public void Init(Framework.Module.AFrameworkBase pFramewok)
        {
        }
        //------------------------------------------------------
        public void Apply()
        {
        }
#if UNITY_EDITOR
        public void Save() { }
        //------------------------------------------------------
        public void OnInspector(System.Object param = null)
        {
        }
#endif
    }

}
