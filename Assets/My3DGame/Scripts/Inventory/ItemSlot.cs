using UnityEngine;
using System;
using UnityEngine.Events;

namespace My3DGame
{
    /// <summary>
    /// 인벤토리에 들어가는 아이템 슬롯을 관리하는 클래스
    /// 속성: 아이템, 아이템 갯수, 장착가능 타입
    /// 기능: 슬롯 업데이트, 슬롯 비우기, 아이템 수량 연산, 슬롯 장착 가능 여부 체크
    /// </summary>
    [Serializable]
    public class ItemSlot
    {
        #region Variables
        public Item item;
        public int amount;

        public ItemType[] allowedItems = new ItemType[0];   //장착 가능 타입 조건들

        [NonSerialized]
        public InventorySO parents;                         //현재 슬롯을 가진 인벤토리 오브젝트
        [NonSerialized]
        public GameObject slotUI;                           //슬롯 데이터가 보일 UI 오브젝트

        [NonSerialized]
        public Action<ItemSlot> OnPreUpdate;                //슬롯 갱신하기전에 등록된 함수 호출해서 실행
        [NonSerialized]
        public Action<ItemSlot> OnPostUpdate;               //슬롯 갱신후에 등록된 함수 호출해서 실행
        #endregion

        #region Property
        public ItemObjectSO ItemObject
        {
            get
            {
                return item.id >= 0 ? parents.database.itemObjects[item.id] : null;
            }
        }
        #endregion

        #region Contructor
        //빈 슬롯 만들기
        public ItemSlot()
        {   
            UpdateSlot(new Item(), 0);
        }

        //매개변수로 들어온 아이템과 수량으로 슬롯 채우기
        public ItemSlot(Item _item, int _amount)
        {
            UpdateSlot(_item, _amount);
        }
        #endregion

        #region Custom Method
        //슬롯 내용 갱신하기: 아이템 변경, 수량 변경
        public void UpdateSlot(Item _item, int _amount)
        {
            //수량 체크
            if(_amount <= 0)
            {
                _item = new Item();
            }

            if (OnPreUpdate != null)
            {
                OnPreUpdate.Invoke(this);
            }

            this.item = _item;
            this.amount = _amount;

            if(OnPostUpdate != null)
            {
                OnPostUpdate.Invoke(this);
            }
        }

        //슬롯 비우기, 아이템 제거하기
        public void RemoveItem()
        {
            UpdateSlot(new Item(), 0);
        }

        //아이템 수량 연산
        public void AddAmount(int value)
        {
            UpdateSlot(this.item, this.amount += value);
        }

        //매개변수로 들어온 아이템의 슬롯 장착 가능 여부 체크
        public bool CanPalceInSlot(ItemObjectSO itemObject)
        {
            //들어온 아이템을 무조건 장착 가능 조건 체크
            if(allowedItems.Length <= 0 ||
                itemObject == null || itemObject.data.id < 0)
            {
                return true;
            }

            //장착 가능 조건이 있으면
            foreach (var itemType in allowedItems)
            {
                if (itemObject.itemType == itemType)
                {
                    return true;
                }
            }

            return false;
        }
        #endregion

    }
}