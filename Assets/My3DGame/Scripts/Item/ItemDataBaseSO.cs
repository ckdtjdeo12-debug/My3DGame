using UnityEngine;

namespace My3DGame
{
    /// <summary>
    /// 아이템 정보를 관리하는 ItemObjectSO들을 등록해서 관리하는 스크립터블 오브젝트
    /// </summary>
    [CreateAssetMenu(fileName = "New ItemDataBase", menuName = "Inventory System/Items/ItemDataBase")]
    public class ItemDataBaseSO : ScriptableObject
    {
        #region Variables
        public ItemObjectSO[] itemObjects;
        #endregion

        #region Unity Event Method
        //인스펙터창의 value값이 변경될때마다 호출되는 함수
        private void OnValidate()
        {
            //itemObjects 배열을 반복문으로 돌려 id값 셋팅
            for (int i = 0; i < itemObjects.Length; i++)
            {
                //itemObjects 널 체크
                if (itemObjects[i] == null)
                    continue;

                itemObjects[i].data.id = i;
            }
        }
        #endregion
    }
}