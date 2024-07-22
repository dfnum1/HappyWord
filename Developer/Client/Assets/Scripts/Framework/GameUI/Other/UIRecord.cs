using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TopGame.UI;
using UnityEngine.UI;
#if UNITY_EDITOR
using UnityEditor;
#endif

namespace TopGame.UI
{
    public class UIRecord : MonoBehaviour
    {
        [Serializable]
        public class RecordData
        {
            public RectTransform ui;
            [Header("ê��λ��")]
            public Vector2 anchorPos;
            [Header("ê��")]
            public Vector4 anchors;
            [Header("��С")]
            public Vector2 size;
            [Header("����")]
            public Vector3 scale;

            public RecordData(RectTransform rectTransform)
            {
                this.ui = rectTransform ?? throw new ArgumentNullException(nameof(ui));
                UpdateData();
            }
            //------------------------------------------------------
            /// <summary>
            /// ����¼������ͬ������ǰUI��
            /// </summary>
            public void SynData()
            {
                if (ui == null)
                {
                    Debug.LogError("ui is  null!");
                    return;
                }

                ui.anchoredPosition= anchorPos;
                ui.anchorMin = new Vector2(anchors.x, anchors.y);
                ui.anchorMax = new Vector2(anchors.z, anchors.w);
                ui.sizeDelta = size;
                ui.localScale= scale;
            }
            //------------------------------------------------------
            /// <summary>
            /// ����ǰUI������µ���¼������
            /// </summary>
            public void UpdateData()
            {
                if (ui == null)
                {
                    Debug.LogError("ui is  null!");
                    return;
                }

                anchorPos = ui.anchoredPosition;
                anchors = new Vector4(ui.anchorMin.x, ui.anchorMin.y, ui.anchorMax.x, ui.anchorMax.y);
                this.size = ui.sizeDelta;
                this.scale = ui.localScale;
            }
            //------------------------------------------------------

            public override bool Equals(object obj)
            {
                //����:UI,ê��λ��,ê��,����,��С����ͬ,����ͬ
                if (obj is RecordData)
                {
                    RecordData other = (RecordData)obj;

                    if (ui && other.ui && ui.GetInstanceID() != other.ui.GetInstanceID())//����ͬһ��UI�����бȽ�
                    {
                        return false;
                    }

                    if (anchorPos != other.anchorPos || anchors != other.anchors || size != other.size || scale != other.scale)
                    {
                        return false;
                    }
                    return true;
                }
                return base.Equals(obj);
            }
            //------------------------------------------------------
            public override int GetHashCode()
            {
                return base.GetHashCode();
            }
        }
        [NonReorderable]
        /// <summary>
        /// ����ui����
        /// </summary>
        public List<RecordData> vDataLandscape = new List<RecordData>();
        [NonReorderable]
        /// <summary>
        /// ����ui����
        /// </summary>
        public List<RecordData> vDataPortrait = new List<RecordData>();

        /// <summary>
        /// ��¼��������
        /// </summary>
        public void RecordLandscapeData()
        {
            //��������������,��¼����,
            vDataLandscape.Clear();
            var uis = transform.GetComponentsInChildren<RectTransform>(true);
            for (int i = 0; i < uis.Length; i++)
            {
                vDataLandscape.Add(new RecordData(uis[i]));
            }

            Debug.Log($"��¼�˺���UI {vDataLandscape.Count} ��");
        }
        //------------------------------------------------------
        /// <summary>
        /// ��¼��������
        /// </summary>
        public void RecordPortraitData()
        {
            vDataPortrait.Clear();
            var uis = transform.GetComponentsInChildren<RectTransform>(true);
            for (int i = 0; i < uis.Length; i++)
            {
                vDataPortrait.Add(new RecordData(uis[i]));
            }
            Debug.Log($"��¼������UI {vDataPortrait.Count} ��");
        }
        //------------------------------------------------------
        /// <summary>
        /// �Ƚ�����,�޳���δ�ı��ui
        /// </summary>
        public void CompareData()
        {
            //ȷ��������¼�����鳤��һ����
            if (vDataLandscape.Count == vDataPortrait.Count)
            {
                for (int i = vDataLandscape.Count-1; i >= 0; i--)
                {
                    var landscape = vDataLandscape[i];
                    var portrait = vDataPortrait[i];
                    if (landscape.Equals(portrait))
                    {
                        vDataLandscape.Remove(landscape);
                        vDataPortrait.Remove(portrait);
                    }
                }
                Debug.Log($"�ȽϺ�,ʣ���޸ĵ�UI {vDataPortrait.Count} ��");
            }
            else
            {
                Debug.LogError("���鳤�Ȳ�һ����!��������");
            }
        }
        //------------------------------------------------------
        /// <summary>
        /// ����
        /// </summary>
        public void OnLandscape()
        {
#if UNITY_EDITOR
            List<UnityEngine.Object> list = new List<UnityEngine.Object>();
            foreach (var item in vDataLandscape)
            {
                if (item.ui == false)
                {
                    continue;
                }

                list.Add(item.ui);
            }
            Undo.RecordObjects(list.ToArray(), "OnLandscape");
#endif

            //��������
            for (int i = 0; i < vDataLandscape.Count; i++)
            {
                vDataLandscape[i].SynData();
            }
        }
        //------------------------------------------------------
        /// <summary>
        /// ��������
        /// </summary>
        public void OnPortrait()
        {
#if UNITY_EDITOR
            List<UnityEngine.Object> list = new List<UnityEngine.Object>();
            foreach (var item in vDataPortrait)
            {
                if (item.ui == false)
                {
                    continue;
                }

                list.Add(item.ui);
            }
            Undo.RecordObjects(list.ToArray(), "OnPortrait");
#endif

            for (int i = 0; i < vDataPortrait.Count; i++)
            {
                vDataPortrait[i].SynData();
            }
        }

#if UNITY_EDITOR
        //------------------------------------------------------
        public void OnExportUI()
        {
            //��ȡ��ǰ��Ļ����
            var canvasScaler =  FindObjectOfType<CanvasScaler>();
            if (!canvasScaler)
            {
                Debug.LogError("��ȡ����Canvas Scaler �޷��жϺ�����,��ִ������UI�������");
                return;
            }

            if (canvasScaler.referenceResolution.x < canvasScaler.referenceResolution.y)//����
            {
                for (int i = 0; i < vDataPortrait.Count; i++)
                {
                    vDataPortrait[i].UpdateData();
                }
            }
            else//����
            {
                for (int i = 0; i < vDataLandscape.Count; i++)
                {
                    vDataLandscape[i].UpdateData();
                }
            }
        }
#endif
    }
}

#if UNITY_EDITOR
[CustomEditor(typeof(UIRecord), true)]
//[CanEditMultipleObjects]
public class UIRecordEditor : Editor
{
    UIRecord m_Targer;

    private SerializedProperty m_vNormalUI;
    private SerializedProperty m_vSpecialUI;

    string m_text1 = "����";
    string m_text2 = "����";

    //------------------------------------------------------
    void OnEnable()
    {
        m_Targer = target as UIRecord;
        m_vNormalUI = serializedObject.FindProperty("vDataLandscape");
        m_vSpecialUI = serializedObject.FindProperty("vDataPortrait");

    }
    //------------------------------------------------------
    public override void OnInspectorGUI()
    {
        serializedObject.Update();

        EditorGUILayout.PrefixLabel(m_text1);
        EditorGUILayout.PropertyField(m_vNormalUI);

        EditorGUILayout.PrefixLabel(m_text2);
        EditorGUILayout.PropertyField(m_vSpecialUI);


        serializedObject.ApplyModifiedProperties();
        //base.OnInspectorGUI();

        if (GUILayout.Button("��¼��������"))
        {
            if (m_Targer != null)
            {
                m_Targer.RecordLandscapeData();
            }
        }
        if (GUILayout.Button("��¼��������"))
        {
            if (m_Targer != null)
            {
                m_Targer.RecordPortraitData();
            }
        }
        if (GUILayout.Button("�Ƚ�����"))
        {
            if (m_Targer != null)
            {
                m_Targer.CompareData();
            }
        }

        if (GUILayout.Button("����Ԥ��"))
        {
            if (m_Targer != null)
            {
                m_Targer.OnLandscape();
            }
        }
        if (GUILayout.Button("����Ԥ��"))
        {
            if (m_Targer != null)
            {
                m_Targer.OnPortrait();
            }
        }

        //if (GUILayout.Button("ˢ�±���"))
        //{
        //    EditorUtility.SetDirty(target);
        //    AssetDatabase.SaveAssets();
        //    AssetDatabase.Refresh(ImportAssetOptions.ForceSynchronousImport);
        //}
    }
}
#endif