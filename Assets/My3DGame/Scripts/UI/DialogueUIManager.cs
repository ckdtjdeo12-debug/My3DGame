using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections;

namespace My3DGame.UI
{
    /// <summary>
    /// DialogueUI를 관리하는 클래스
    /// 대화창 UI 셋팅하기
    /// </summary>
    public class DialogueUIManager : MonoBehaviour
    {
        #region Variables
        public TextMeshProUGUI _lineText;           //대화 글
        public TextMeshProUGUI _actorName;          //대화 캐릭터 이름

        public Image _npcImage;                     //대화 캐릭터 이미지
        public GameObject _nextButton;              //다음 액션 버튼 - 다음 대화 보여주기
        #endregion

        #region Custom Method
        //매개변수로 받은 대화로 UI 셋팅
        public void SetDialogue(Dialog dialog)
        {
            //캐릭터 이미지
            if (_npcImage)
            {
                if (dialog.character <= 0)   //대응되는 이미지가 없다
                {
                    _npcImage.gameObject.SetActive(false);
                }
                else //대응되는 이미지가 있다
                {
                    Sprite sprite = ResourcesManager.Load<Sprite>("Dialogue/Npc/npc0"
                        + dialog.character.ToString());
                    if (sprite)
                    {
                        _npcImage.gameObject.SetActive(true);
                        _npcImage.sprite = sprite;
                    }
                }
            }

            //Actor 이름
            _actorName.text = dialog.name;

            //next 버튼
            _nextButton.SetActive(false);

            //대화 글
            //_lineText.text = dialog.sentence;
            StartCoroutine(TypingSentence(dialog.sentence));
        }

        //대화글 타이핑 연출
        IEnumerator TypingSentence(string typingText)
        {
            _lineText.text = "";

            foreach (char latter in typingText)
            {
                _lineText.text += latter;
                yield return new WaitForSeconds(0.03f);
            }

            //next 버튼
            _nextButton.SetActive(true);
        }
        #endregion
    }
}