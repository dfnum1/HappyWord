using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TopGame.UI
{
    public interface IGuideScroll
    {
        Transform GetItemByIndex(int index);
        /// <summary>
        /// ����go��������
        /// ��0��ʼ
        /// </summary>
        /// <param name="go"></param>
        /// <returns>�Ҳ�������-1</returns>
        int GetIndexByItem(GameObject go);
        bool GetIsLoadCompleted();
        T GetComponent<T>();
    }
}

