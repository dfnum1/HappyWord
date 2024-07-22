/********************************************************************
��������:	2:6:2023   10:46
��    ��: 	AddEventUtil
��    ��:	Ywm
��    ��:	AddEventUtil
*********************************************************************/


using UnityEngine;

public static class AddEventUtil
{
    //------------------------------------------------------ ��ӹ����ֶ�
    public static void LogEvent(string eventName, bool syncServer = false, bool bImmde = false, bool isFirst = false)
    {
        if (TopGame.SvrData.UserManager.MySelf != null && TopGame.SvrData.UserManager.MySelf.userID != 0)
        {
            //�����ֶ�
            TopGame.Core.AUserActionManager.AddActionKV("date", System.DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
            TopGame.Core.AUserActionManager.AddActionKV("user_level", TopGame.SvrData.UserManager.MySelf.PlayerLevel);
//            TopGame.Core.AUserActionManager.AddActionKV("chapter", TopGame.SvrData.UserManager.MySelf.GetBattleDB().GetPVEChapterId());
            TopGame.Core.AUserActionManager.AddActionKV("user_name", TopGame.SvrData.UserManager.MySelf.GetUserName());
            TopGame.Core.AUserActionManager.LogActionEvent(eventName,syncServer,bImmde,isFirst);
        }
    }
}