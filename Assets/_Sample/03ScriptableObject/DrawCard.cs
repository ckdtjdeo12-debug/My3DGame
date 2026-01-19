using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace MySample
{
    /// <summary>
    /// 카드 정보(스크립터블 오브젝트)를 읽어서 카드 그리기
    /// </summary>
    public class DrawCard : MonoBehaviour
    {
        #region Variables
        public CardSO card;

        //UI
        public TextMeshProUGUI nameText;
        public TextMeshProUGUI descriptionText;

        public TextMeshProUGUI manaCostText;
        public TextMeshProUGUI attackText;
        public TextMeshProUGUI healthText;

        public Image artWork;
        #endregion

        #region Unity Event Method
        private void OnEnable()
        {
            //카드 정보 UI 셋팅
            UpdateCard();
        }
        #endregion

        #region Custom Method
        //카드 정보 UI 셋팅
        private void UpdateCard()
        {
            nameText.text = card.name;
            descriptionText.text = card.description;

            manaCostText.text = card.manaCost.ToString();
            attackText.text = card.attack.ToString();
            healthText.text = card.health.ToString();

            artWork.sprite = card.argImage;

        }
        #endregion


    }
}