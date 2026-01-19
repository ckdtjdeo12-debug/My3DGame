using UnityEngine;

namespace MySample
{
    /// <summary>
    /// 카드 정보를 담는 스크립터블오브젝트 클래스
    /// </summary>

    [CreateAssetMenu(fileName = "New Card", menuName = "Card/CardSO")]
    public class CardSO : ScriptableObject
    {
        new public string name;
        public string description;

        public int manaCost;
        public int attack;
        public int health;

        public Sprite argImage;
    }
}