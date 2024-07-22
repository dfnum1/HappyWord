using System;
using System.Collections.Generic;
using UnityEngine;

namespace SDK
{
    [System.Serializable]
    public struct TopOnCfg : ISDKConfig
    {
        public string appId;
        public string appKey;
        public GameObject pMainGO;
    }
    public class TopOnUnity : ISDKAgent
    {
#if USE_TOPON
        TopOnCfg m_Config;
        static bool ms_bInited = false;
#endif
        //------------------------------------------------------
        public static AdmobileUnity StartUp(ISDKConfig config, ISDKCallback callback = null)
        {
#if USE_TOPON
            ms_bInited = false;
            AdmobileUnity talkingData = new AdmobileUnity();
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
        protected override bool Init(ISDKConfig cfg)
        {
#if USE_TOPON
            m_Config = (TopOnCfg)cfg;
            Debug.Log("TopOn begin init");
            if (m_Config.pMainGO == null)
            {
                Debug.LogWarning("unity go is null");
                return false;
            }
//����ѡ���ã������Զ����Map��Ϣ����ƥ���̨���õĹ����˳����б�Appγ�ȣ�
//ע�⣺���ô˷��������setChannel()��setSubChannel()�������õ���Ϣ�������������Щ��Ϣ�����ڵ��ô˷�������������
ATSDKAPI.initCustomMap(new Dictionary<string, string> { { "unity3d_data", "test_data" } }); 

//����ѡ���ã������Զ����Map��Ϣ����ƥ���̨���õĹ����˳����б�Placementγ�ȣ�
ATSDKAPI.setCustomDataForPlacementID(new Dictionary<string, string> { { "unity3d_data_pl", "test_data_pl" } } ,placementId);

//����ѡ���ã�������������Ϣ�������߿���ͨ����������Ϣ�ں�̨�����ֿ����������Ĺ������
//ע�⣺�����ʹ��initCustomMap()������������initCustomMap()����֮����ô˷���
ATSDKAPI.setChannel("unity3d_test_channel"); 

//����ѡ���ã���������������Ϣ�������߿���ͨ����������Ϣ�ں�̨�����ֿ������������������������
//ע�⣺�����ʹ��initCustomMap()������������initCustomMap()����֮����ô˷���
ATSDKAPI.setSubChannel("unity3d_test_subchannel"); 

//���ÿ���Debug��־��ǿ�ҽ�����Խ׶ο����������Ų����⣩
ATSDKAPI.setLogDebug(true);

//���������ã�SDK�ĳ�ʼ��
ATSDKAPI.initSDK(m_Config.appId, m_Config.appKey);//Use your own app_id & app_key here
            return true;
#else
            return false;
#endif
        }
        //------------------------------------------------------
        public static void ShowAd(string type, string posId)
        {
#if USE_TOPON
            if (!ms_bInited) return;
            if(type.CompareTo("rewardAd") == 0)
            {
                ATRewardedVideo.Instance.client.onAdLoadEvent += onAdLoad; 
                ATRewardedVideo.Instance.client.onAdLoadFailureEvent += onAdLoadFail;
                ATRewardedVideo.Instance.client.onAdVideoStartEvent  += onAdVideoStartEvent;
                ATRewardedVideo.Instance.client.onAdVideoEndEvent  += onAdVideoEndEvent;
                ATRewardedVideo.Instance.client.onAdVideoFailureEvent += onAdVideoPlayFail;
                ATRewardedVideo.Instance.client.onAdClickEvent += onAdClick;
                ATRewardedVideo.Instance.client.onRewardEvent += onReward;
                ATRewardedVideo.Instance.client.onAdVideoCloseEvent += onAdVideoClosedEvent;

                Dictionary<string,string> jsonmap = new Dictionary<string,string>();
                //�����Ҫͨ�������ߵķ��������н������·������ֹ��ƽ̨֧�ִ˷�����������������Ҫ������������key
                //ATConst.USERID_KEY�ش������ڱ�ʶÿ���û�;ATConst.USER_EXTRA_DATAΪ��ѡ�����������͸���������ߵķ�����
                jsonmap.Add(ATConst.USERID_KEY, "test_user_id");
                jsonmap.Add(ATConst.USER_EXTRA_DATA, "test_user_extra_data");

                ATRewardedVideo.Instance.loadVideoAd(mPlacementId_rewardvideo_all,jsonmap);
            }
#endif
        }
#if USE_TOPON
        class SDKCallback : MonoBehaviour
        {
            void OnAdReceive(string msg)
            {

            }
            void OnAdExpose(string msg)
            {

            }
            void OnAdClick(string msg)
            {

            }
            void OnAdClose(string msg)
            {

            }
            void OnAdFailed(string msg)
            {

            }
            void OnAdComplete(string msg)
            {

            }
            void OnAdReward(string msg)
            {

            }
        }
#endif
    }
}