using UnityEngine;

namespace My3DGame
{
    /// <summary>
    /// 아이템 기본 데이터를 관리하는 스크립터블 오브젝트 클래스
    /// </summary>
    [CreateAssetMenu(fileName = "New Item", menuName = "Inventory System/Items/Item")]
    public class ItemObjectSO : DescriptionBaseSO
    {
        #region Variables
        //아이템 아이디, 이름, 능력치
        public Item data = new Item();          //아이템 생성할때 필요한 데이터

        public ItemType itemType;               //아이템 타입
        public bool stackable;                  //인벤 저장시 하나의 슬롯에 다수를 누적 저장 가능 여부

        public int shopPrice;                   //유저가 상점에서 구매하는 금액
        //public int sellPrice;                 //유저가 상점에서 판매하는 금액

        public Sprite icon;                     //아이템 아이콘 이미지
        public GameObject modlePrefab;          //장이 아이템 모델 오브젝트
        #endregion

        #region Custom Method
        //아이템 생성하기
        public Item CreateItem()
        {
            Item newItem = new Item(this);
            return newItem;
        }
        #endregion
    }
}