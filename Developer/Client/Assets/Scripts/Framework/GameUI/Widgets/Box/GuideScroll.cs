/********************************************************************
��������:	7:10:2020 18:16
��    ��: 	DynamicListView
��    ��:	zdq
��    ��:	������������������
*********************************************************************/
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TopGame.UI
{
    public class GuideScroll : MonoBehaviour, IGuideScroll
    {
        List<GameObject> m_vInstantiateItems = null;
        bool m_bInit = false;

        /// <summary>
        /// ���б�������Ʒ��������Ϻ�,���øĺ������г�ʼ��,���ɽ�����������
        /// </summary>
        public void Init()
        {
            if (m_vInstantiateItems == null)
            {
                m_vInstantiateItems = new List<GameObject>();
            }
            m_vInstantiateItems.Clear();

            int count = transform.childCount;
            for (int i = 0; i < count; i++)
            {
                Transform child = transform.GetChild(i);
                m_vInstantiateItems.Add(child.gameObject);
            }

            m_bInit = true;
        }
        //------------------------------------------------------
        public void Clear()
        {
            m_bInit = false;
            if (m_vInstantiateItems != null)
            {
                m_vInstantiateItems.Clear();
                m_vInstantiateItems = null;
            }
            //�ýű�ֻ�ǻ�ȡ�������������������,����¼,����ʱ��������������
        }
        //------------------------------------------------------
        public int GetIndexByItem(GameObject go)
        {
            if (m_vInstantiateItems == null)
            {
                return -1;
            }

            GameObject item = null;
            for (int i = 0; i < m_vInstantiateItems.Count; i++)
            {
                item = m_vInstantiateItems[i];
                if (item != null && go == item)
                {
                    return i;
                }
            }
            return -1;
        }
        //------------------------------------------------------
        public bool GetIsLoadCompleted()
        {
            return m_bInit;
        }
        //------------------------------------------------------
        public Transform GetItemByIndex(int index)
        {
            if (m_vInstantiateItems == null || index >= m_vInstantiateItems.Count)
            {
                return null;
            }

            return m_vInstantiateItems[index].transform;
        }
    }
}