using UnityEngine;

namespace My3DGame
{
    /// <summary>
    /// 플레이어 인풋을 관리하는 클래스, InputReader 이용
    /// </summary>
    public class PlayerInputReader : MonoBehaviour
    {
        #region Variables
        //참조
        [SerializeField] protected InputReader inputReader;

        //인풋 제어
        [HideInInspector]
        public bool playerControllerInputBlocked;       //애니메이터 상태 태그에 따라 인풋 블록 제어
        protected bool m_ExternalInputBlocked;          //인풋 블록 제어

        //Move, Jump 인풋 값
        protected Vector2 m_Movement;
        protected bool m_Jump;
        protected bool m_Attack;
        #endregion

        #region Property
        public Vector2 Movement
        {
            get
            {
                //블록 체크
                if (playerControllerInputBlocked || m_ExternalInputBlocked)
                {
                    return Vector2.zero;
                }
                return m_Movement;
            }
            private set
            {
                m_Movement = value;
            }
        }

        public bool Jump
        {
            get
            {
                //블록 체크
                if (playerControllerInputBlocked || m_ExternalInputBlocked)
                {
                    return false;
                }
                return m_Jump;
            }
            private set
            {
                m_Jump = value;
            }
        }

        public bool Attack
        {
            get
            {
                //블록 체크
                if (playerControllerInputBlocked || m_ExternalInputBlocked)
                {
                    return false;
                }
                return m_Attack;
            }
            set
            {
                m_Attack = value;
            }
        }
        #endregion

        #region Unity Event Method
        private void OnEnable()
        {
            //플레이 액션 맵 활성화
            inputReader.EnableGamePlayInput();

            //액션 이벤트 함수 등록
            inputReader.MoveEvent += OnMove;
            inputReader.JumpEvent += OnJump;
            inputReader.JumpCanceledEvent += OnJumpCanceled;
            inputReader.AttackEvent += OnAttack;
        }


        private void OnDisable()
        {
            //액션 이벤트 함수 제거
            inputReader.MoveEvent -= OnMove;
            inputReader.JumpEvent -= OnJump;
            inputReader.JumpCanceledEvent -= OnJumpCanceled;
            inputReader.AttackEvent -= OnAttack;
        }
        #endregion

        #region Custom Method
        private void OnMove(Vector2 movement)
        {
            Movement = movement;
        }

        private void OnJump()
        {
            Jump = true;
        }

        private void OnJumpCanceled()
        {
            Jump = false;
        }

        private void OnAttack()
        {
            Attack = true;
        }
        #endregion
    }
}