using UnityEngine;
using My3DGame;

namespace My3DGame.UI
{
    /// <summary>
    /// 게임 UI들을 관리하는 클래스
    /// </summary>
    public class UIManager : MonoBehaviour
    {
        #region Variables
        public DialogueManager dialogueManager;
        public DialogueUIManager dialogueUIManager;
        #endregion

        #region Unity Event Method
        private void OnEnable()
        {
            if (dialogueManager != null)
            {
                //이벤트 등록
                dialogueManager.openUIDialogEvent += OpenUIDialogue;
                dialogueManager.closeUIDialogEvent += CloseUIDialog;
            }
        }

        private void OnDisable()
        {
            if (dialogueManager != null)
            {
                //이벤트 제거
                dialogueManager.openUIDialogEvent += OpenUIDialogue;
                dialogueManager.closeUIDialogEvent += CloseUIDialog;
            }   
        }
        #endregion

        #region Custom Method
        //대화창 열기
        private void OpenUIDialogue(Dialog dialog)
        {
            dialogueUIManager.gameObject.SetActive(true);
            //dialog 대화창 셋팅
            dialogueUIManager.SetDialogue(dialog);
        }

        //대화창 닫기
        private void CloseUIDialog()
        {
            dialogueUIManager.gameObject.SetActive(false);
        }
        #endregion
    }
}