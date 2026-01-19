using My3DGame;
using UnityEngine;
using UnityEngine.AI;

namespace MySample
{
    /// <summary>
    /// 캐릭터 클릭앤 무브 입력 처리
    /// </summary>
    public class PlayerInputAgent : MonoBehaviour
    {
        #region Variables
        //참조
        [SerializeField] protected InputReader inputReader;

        //인풋 제어
        [HideInInspector]
        public bool playerControllerInputBlocked;       //애니메이터 상태 태그에 따라 인풋 블록 제어
        protected bool m_ExternalInputBlocked;          //인풋 블록 제어

        //Move, Jump 인풋 값
        protected bool m_ClickMove;
        protected Vector2 m_MousePosion;
        #endregion

        #region Property
        public Vector2 MousePosion
        {
            get
            {
                //블록 체크
                if (playerControllerInputBlocked || m_ExternalInputBlocked)
                {
                    return Vector2.zero;
                }
                return m_MousePosion;
            }
            private set
            {
                m_MousePosion = value;
            }
        }

        public bool ClickMove
        {
            get
            {
                //블록 체크
                if (playerControllerInputBlocked || m_ExternalInputBlocked)
                {
                    return false;
                }
                return m_ClickMove;
            }
            set
            {
                m_ClickMove = value;
            }
        }
        #endregion

        #region Unity Event Method
        private void OnEnable()
        {
            //클릭앤무브 액션 맵 활성화
            inputReader.EnableClickNMoveInput();

            inputReader.ClickEvent += MouseClick;
            inputReader.MousePositionEvent += MovePosion;
        }
        #endregion

        #region Custom Method
        private void MouseClick()
        {
            ClickMove = true;
        }

        private void MovePosion(Vector2 position)
        {
            MousePosion = position;
        }
        #endregion

    }
}