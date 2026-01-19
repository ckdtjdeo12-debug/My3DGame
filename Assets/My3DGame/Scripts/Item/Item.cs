using UnityEngine;
using System;

namespace My3DGame
{
    /// <summary>
    /// 아이템을 관리하는 클래스
    /// 속성: 아이디, 이름, 능력치[]
    /// 아이디: 아이템 데이터베이스의 아이디(인덱스)로 필요한 아이템SO 정보에 접근한다
    /// 기능: 아이템 생성하기
    /// </summary>
    [Serializable]
    public class Item
    {
        #region Variablse
        public int id;
        public string name;

        //능력치[]
        public ItemBuff[] buffs;
        #endregion

        #region Constructor
        public Item()
        {
            id = -1;            //아이템이 없다
            name = null;        //아이템이 없다
        }

        //아이템 생성한다
        public Item(ItemObjectSO itemObject)
        {
            id = itemObject.data.id;
            name = itemObject.data.name;

            buffs = new ItemBuff[itemObject.data.buffs.Length];
            for (int i = 0; i < buffs.Length; i++)
            {
                buffs[i] = new ItemBuff(itemObject.data.buffs[i].min, itemObject.data.buffs[i].max)
                {
                    stat = itemObject.data.buffs[i].stat
                };
            }
        }
        #endregion
    }
}