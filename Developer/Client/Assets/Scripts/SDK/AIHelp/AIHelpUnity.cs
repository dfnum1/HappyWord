#define TD_GAME
using System;
using System.Collections.Generic;
using UnityEngine;

namespace SDK
{
    [System.Serializable]
    public struct AIHelpCfg : ISDKConfig
    {
        public string appKey;
        public string domain;
        public string android_appId;
        public string ios_appid;
        public string GetAppID()
        {
#if UNITY_ANDROID
            return android_appId;
#elif UNITY_IPHONE
            return ios_appid;
#else
            return null;
#endif
        }
    }
    public class AIHelpUnity : ISDKAgent
    {
#if USE_AIHELP
        AIHelpCfg m_Config;
        ISDKAgent m_pAgent = null;
        static bool ms_bInited = false;
#endif
        //------------------------------------------------------
        public static AIHelpUnity StartUp(ISDKConfig config, ISDKCallback callback = null)
        {
#if USE_AIHELP
            ms_bInited = false;
            AIHelpUnity talkingData = new AIHelpUnity();
            if (talkingData.Init(config))
            {
                talkingData.SetCallback(callback);
                ms_bInited = true;
                return talkingData;
            }
            return null;
#else
            return null;
#endif
        }
        //------------------------------------------------------
        public static void ShowHelp(string enter)
        {
#if USE_AIHELP
            if (ms_bInited)
            {
                AIHelp.AIHelpSupport.Show(enter);
            }
#endif
        }
        //------------------------------------------------------
        protected override bool Init(ISDKConfig cfg)
        {
#if USE_AIHELP
            m_Config = (AIHelpCfg)cfg;
            if (string.IsNullOrEmpty(m_Config.GetAppID()))
            {
                Debug.Log("AIHelp init failed, appid is null");
                return false;
            }
            Debug.Log("AIHelp begin init");
            AIHelp.AIHelpSupport.Init(m_Config.appKey, m_Config.domain, m_Config.GetAppID(), GetSystemLanuage());
            AIHelp.AIHelpSupport.SetOnAIHelpInitializedCallback(OnAIHelpInitializedCallback);
            return true;
#else
            return false;
#endif
        }
        //------------------------------------------------------
        void OnAIHelpInitializedCallback()
        {
            Debug.Log("AIHelp init Ok");
        }
        //------------------------------------------------------
        public string GetSystemLanuage()
        {
            switch (Application.systemLanguage)
            {
                case SystemLanguage.English: return "en";
                case SystemLanguage.Belarusian: return "ru"; //����˹
                case SystemLanguage.Japanese: return "ja"; //�ձ�
                case SystemLanguage.Chinese:
                case SystemLanguage.ChineseSimplified: return "zh-CH";
                case SystemLanguage.ChineseTraditional: return "zh-TW";
                case SystemLanguage.Arabic: return "ar";
                case SystemLanguage.German: return "de";    //����
                case SystemLanguage.French: return "fr";    //����
                case SystemLanguage.Korean: return "ko";    //����
                case SystemLanguage.Portuguese: return "pt";    //��������
                case SystemLanguage.Thai: return "th";    //̩��
                case SystemLanguage.Turkish: return "tr";    //�������
                case SystemLanguage.Indonesian: return "id";    //ӡ����������
                case SystemLanguage.Spanish: return "es";    //��������
                case SystemLanguage.Vietnamese: return "vi";    //Խ����
                case SystemLanguage.Italian: return "it";    //�������
                case SystemLanguage.Polish: return "pl";    //������
                case SystemLanguage.Dutch: return "nl";    //������
                case SystemLanguage.Faroese: return "fa";    //��˹��
                case SystemLanguage.Romanian: return "ro";    //����������
                case SystemLanguage.Estonian: return "tl";    //���ɱ���
                case SystemLanguage.Czech: return "cs";    //�ݿ���
                case SystemLanguage.Greek: return "el";    //ϣ����
                case SystemLanguage.Hungarian: return "hu";    //��������
                case SystemLanguage.Swedish: return "sv";    //�����
                case SystemLanguage.Hebrew: return "hi";    //ӡ����
                case SystemLanguage.Norwegian: return "nb";    //Ų����
//              case SystemLanguage.xxx: return "te";    //̩¬����
//              case SystemLanguage.xxx: return "bn";    //�ϼ�����
//              case SystemLanguage.xxx: return "ta";    //̩�׶���
//              case SystemLanguage.xx: return "ms";    //������
//              case SystemLanguage.Bulgarian: return "my";    //�����
            }
            return "en";
        }
    }
}