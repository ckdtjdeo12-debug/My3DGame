using UnityEngine;
using System.Linq;
using System;

namespace My3DGame
{
    /// <summary>
    /// 아이템 슬롯들을 관리하는 클래스
    /// 속성: 슬롯 목록
    /// 기능: 인벤토리 전체 비우기, 인벤토리에 아이템 존재 여부 체크
    /// </summary>
    [Serializable]
    public class Inventory
    {
        #region Variables
        public ItemSlot[] slots = new ItemSlot[16];
        #endregion

        #region Custom Method
        //인벤토리 전체 비우기
        public void Clear()
        {
            foreach (var slot in slots)
            {
                slot.RemoveItem();
            }
        }

        //인벤토리에 아이템 존재 여부 체크, 아이템오브젝트 체크
        public bool IsContain(ItemObjectSO itemObect)
        {
            return IsContain(itemObect.data.id);
        }

        //인벤토리에 아이템 존재 여부 체크, 아이디 체크
        public bool IsContain(int id)
        {
            return slots.FirstOrDefault(i => i.item.id  == id) != null;
        }
        #endregion
    }
}