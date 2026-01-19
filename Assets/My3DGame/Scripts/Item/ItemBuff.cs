using UnityEngine;

namespace My3DGame
{
    /// <summary>
    /// 아이템의 능력치를 관리하는 클래스
    /// 속성: 아이템 능력치의 속성, 능력치 실제 값, 능력치의 최소값, 능력치의 최대값
    /// 기능: 능력치 실제값 생성하기, 능력치 실제값을 더하기
    /// </summary>
    [System.Serializable]
    public class ItemBuff : IModifier
    {
        #region Variables
        public CharacterAttribute stat;     //아이템 능력치의 속성
        public int value;                   //아이템 능력치

        public int min;                     //능력치의 최소값
        public int max;                     //능력치의 최대값
        #endregion

        #region Constructor
        public ItemBuff() { }

        public ItemBuff(int _min, int _max)
        {
            this.min = _min;
            this.max = _max;
            GenerateValue();
        }
        #endregion

        #region Custom Method
        //능력치(value) 생성하기
        private void GenerateValue()
        {
            if (this.min == this.max)
                value = this.max;
            else
                value = Random.Range(this.min, this.max);
        }

        //매개변수로 들어온 값에 ItemBuff가 가지고 있는 value를 더해서 다시 결과를 반환한다
        public void AddValue(ref int baseValue)
        {            
            baseValue += value;
        }
        #endregion
    }
}
