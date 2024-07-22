#if UNITY_EDITOR
using System.Collections.Generic;
using UnityEngine;

namespace Framework.Plugin.Guide
{
    //------------------------------------------------------
    public partial class DataSearcher : GuideSearcher
    {
        //------------------------------------------------------
        protected override bool OnDragItem(AssetTree.ItemData item)
        {
            return true;
        }
        //------------------------------------------------------
        protected override bool OnDrawItem(Rect cellRect, AssetTree.TreeItemData item, int column, bool bSelected, bool focused)
        {
            AssetTree.ItemData itemData = item.data as AssetTree.ItemData;
            item.displayName = itemData.name;

            GUI.Label(new Rect(cellRect.x, cellRect.y, cellRect.width - 40, cellRect.height), item.displayName);
            if(GUI.Button(new Rect(cellRect.xMax-40, cellRect.y,40, cellRect.height), "移除"))
            {
                GuideSystem.getInstance().datas.Remove(item.id);
                Search(m_assetTree.searchString);
            }
            return true;
        }
        //------------------------------------------------------
        protected override void OnSawpDatas()
        {
            if (GuideSystem.getInstance().datas == null) return;
            Dictionary<int, GuideGroup> datas = GuideSystem.getInstance().datas;
            datas.Clear();
            List<AssetTree.ItemData> vItems = m_assetTree.GetDatas();
            for(int i = 0; i < vItems.Count; ++i)
            {
                ItemEvent temp = vItems[i] as ItemEvent;
                GuideEditor.DataParam dataParam = (GuideEditor.DataParam)temp.param;
                datas[vItems[i].id] = dataParam.Data;
            }
        }
        //------------------------------------------------------
        protected override void OnSearch(string query)
        {
            if (GuideSystem.getInstance().datas == null) return;
            GuideEditor pEditor = GuideEditor.Instance;
            foreach (var db in GuideSystem.getInstance().datas)
            {
                bool bQuerty = IsQuery(query, db.Value.Guid+db.Value.Name);
                if (!bQuerty) continue;
                GuideEditor.DataParam param = new GuideEditor.DataParam();
                param.Data = db.Value;

                ItemEvent item = new ItemEvent();
                item.param = param;
                item.callback = pEditor.LoadData;

                item.id = db.Key;
                item.name = db.Value.Name + "[" + db.Value.Guid + "]";
                m_assetTree.AddData(item);
            }
        }
        //------------------------------------------------------
        protected override void OnClose()
        {
            GuideEditor.Instance.Save(false);
        }
    }
}
#endif