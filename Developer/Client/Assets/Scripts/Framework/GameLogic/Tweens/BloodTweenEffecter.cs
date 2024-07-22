/********************************************************************
生成日期:	1:11:2020 10:06
类    名: 	TweenEffecter
作    者:	HappLI
描    述:	晃动表现
*********************************************************************/
using TopGame.UI;
using UnityEngine;

namespace TopGame.Logic
{
    [ExecuteInEditMode]
    public class BloodTweenEffecter : TweenEffecter
    {
        public bool bRandomEnd = true;
        public bool bRandomStart = true;
        public int nRandomRange = 130;
        public int nStartXRandomRange = 20;
        public int nStartYRandomRange = 20;
        public UnityEngine.UI.Text pTex;
        public UnityEngine.UI.Image pImg;
        //------------------------------------------------------
        public override void Play(Vector3 startPos, bool bEditor = false,float fromTime = 0)
        {
            base.Play(startPos, bEditor, fromTime);
            if (fDuration <= 0) return;
            if (pTex != null)
            {
                Color color = pTex.color;
                color.a = alpha.Evaluate(m_fTime);
                pTex.color = color;
            }

            if (pImg != null)
            {
                Color color = pImg.color;
                color.a = alpha.Evaluate(m_fTime);
                pImg.color = color;
            }
            m_pTransform.localScale = Vector3.zero;
        }
        //------------------------------------------------------
        public void SetText(string text)
        {
            if (pTex == null) return;
            pTex.text = text;
        }
        //------------------------------------------------------
        public void SetImg(string path)
        {
            if (pImg == null) return;
            pImg.gameObject.SetActive(!string.IsNullOrEmpty(path));
            (pImg as ImageEx)?.SetAssetByPath(path, null);
        }
        //------------------------------------------------------
        public void SetColor(Color color)
        {
            if (pTex == null) return;
            pTex.color = color;
        }
        //------------------------------------------------------
        public override void ForceUpdate(float fFrameTime)
        {
            if (!m_bPlaying) return;
            base.ForceUpdate(fFrameTime);
            {
                if (pTex != null)
                {
                    Color color = pTex.color;
                    color.a = alpha.Evaluate(m_fTime);
                    pTex.color = color;                   
                }

                if (pImg != null)
                {
                    Color color = pImg.color;
                    color.a = alpha.Evaluate(m_fTime);
                    pImg.color = color;
                }
            }
        }
    }
}
