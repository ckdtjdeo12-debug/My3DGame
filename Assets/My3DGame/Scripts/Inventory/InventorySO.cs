using UnityEngine;

namespace My3DGame
{
    /// <summary>
    /// 인벤토리를 관리하는 스크립터블 오브젝트 클래스
    /// 속성: 인벤토리 컨터이너, 아이템 데이터 베이스, 인벤토리 타입
    /// </summary>
    [CreateAssetMenu(fileName = "New Inventory", menuName = "Inventory System/Inventory")]
    public class InventorySO : ScriptableObject
    {
        #region Variables
        public Inventory container = new Inventory();

        public ItemDataBaseSO database;                 //아이템 데이터 베이스
        public InventoryType inventoryType;             //인벤토리 타입
        #endregion
    }
}