using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TopGame.UI
{
    public class MainAdventurePanelSerialize : UserInterface
    {
        [Header("����ս��ʱ�Ķ���ʱ��")]
        public float EnterBattleAniTime = 0.3f;

        [Header("���Ϲ�ͷ������ƫ������")]
        public Vector2 Type4HeadOffsetPosition;

        [Header("Boss��ͷ������ƫ������")]
        public Vector2 Type3HeadOffsetPosition;

        [Header("��Ӣ��ͷ������ƫ������")]
        public Vector2 Type2HeadOffsetPosition;
    }
}