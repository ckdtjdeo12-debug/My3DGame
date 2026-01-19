using UnityEngine;
using UnityEngine.InputSystem;

namespace My3DGame
{
    /// <summary>
    /// 플레이어 인풋을 관리하는 클래스
    /// </summary>
    public class PlayerInput : MonoBehaviour
    {
        #region Variables
        //참조
        protected MyInput myInput;

        //인풋 액션
        //protected InputAction moveAction;

        //인풋 제어
        [HideInInspector]
        public bool playerControllerInputBlocked;       //애니메이터 상태 태그에 따라 인풋 블록 제어
        protected bool m_ExternalInputBlocked;          //인풋 블록 제어

        //Move, Jump 인풋 값
        protected Vector2 m_Movement;        
        protected bool m_Jump;
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
        #endregion

        #region Unity Event Method
        private void Awake()
        {
            //참조
            myInput = new MyInput();

            myInput.GamePlay.Move.performed += ctx => {
                Movement = ctx.ReadValue<Vector2>();
            };
            myInput.GamePlay.Move.canceled += ctx => {
                Movement = ctx.ReadValue<Vector2>();
            };
            myInput.GamePlay.Jump.performed += ctx =>
            {
                Jump = ctx.ReadValueAsButton();
            };
            myInput.GamePlay.Jump.canceled += ctx =>
            {
                Jump = ctx.ReadValueAsButton();
            };


            //[1]인풋 액션 가져오기
            //moveAction = myInput.GamePlay.Move;
        }

        private void OnEnable()
        {
            //인풋 시스템 액션맵 활성화
            myInput.GamePlay.Enable();

            //[1]
            //moveAction.performed += Move_Preformed;
            //moveAction.started += Move_Started;
            //moveAction.canceled += Move_Canceled;
        }

        private void OnDisable()
        {
            //인풋 시스템 액션맵 비 활성화
            myInput.GamePlay.Disable();

            //[1]
            //moveAction.performed -= Move_Preformed;
            //moveAction.started -= Move_Started;
            //moveAction.canceled -= Move_Canceled;
        }
        #endregion

        #region Custom Method
        //인풋 블록 제어
        public bool HaveControl()
        {
            return !m_ExternalInputBlocked;
        }
        //인풋 제어를 잃는다
        public void ReleaseControl()
        {
            m_ExternalInputBlocked = true;
        }
        //인풋 제어를 얻는다
        public void GainControl()
        {
            m_ExternalInputBlocked = false;
        }

        //1
        /*
        private void Move_Preformed(InputAction.CallbackContext context)
        {
            Debug.Log("Move_Preformed");
            Movement = context.ReadValue<Vector2>();
        }

        private void Move_Started(InputAction.CallbackContext context)
        {
            Debug.Log("Move_Started");
        }

        private void Move_Canceled(InputAction.CallbackContext context)
        {
            Debug.Log("Move_Canceled");
            Movement = context.ReadValue<Vector2>();
        }
        */
        #endregion
    }
}