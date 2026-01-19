using My3DGame;
using My3DGame.GameData;
using UnityEngine;
using UnityEngine.AI;

namespace MySample
{
    /// <summary>
    /// 캐릭터 클릭앤 무브 움직임 구현
    /// </summary>
    public class PlayerControllerAgent : MonoBehaviour
    {
        #region Variables
        //참조
        protected PlayerInputAgent m_Input;
        protected CharacterController m_CharCtrl;
        protected Animator m_Animator;

        protected NavMeshAgent m_Agent;
        protected Camera m_Camera;

        //애니메이터
        protected AnimatorStateInfo m_CurrentStateInfo;     //현재 애니메이터 상태 정보
        protected AnimatorStateInfo m_NextStateInfo;        //다음 애니메이터 상태 정보
        protected bool m_IsAnimatorTransition;              //상태 변경 체크
        protected AnimatorStateInfo m_PreviousCurrentStateInfo;  //이전 상태
        protected AnimatorStateInfo m_PreviousNextStateInfo;     //이전 상태
        protected bool m_PreviousIsAnimatorTransition;           //이전 상태 변경 체크

        [Header("Broadcasting on")]
        [SerializeField] protected EffectDataChannelSO _EffectOneShot = default;

        //이동
        [SerializeField] protected LayerMask groundLayerMast;

        protected bool isArrive = false;
        protected bool m_IsGrounded = false;
        
        //대기
        [SerializeField] protected float idleTimeOut = 5f;  //로코모션에서 5초 타임아웃 되면 대기로 보낸다
        protected float m_IdleTime = 0f;

        //Animator Parameters Hash값
        readonly int m_HashForwardSpeed = Animator.StringToHash("ForwardSpeed");
        readonly int m_HashAirbornVerticalSpeed = Animator.StringToHash("AirbornVerticalSpeed");
        readonly int m_HashAngleDelatRad = Animator.StringToHash("AngleDelatRad");
        readonly int m_HashInputDetected = Animator.StringToHash("InputDetected");
        readonly int m_HashTimeoutToIlde = Animator.StringToHash("TimeoutToIlde");
        readonly int m_HashGrounded = Animator.StringToHash("Grounded");

        //Animator State Hash값
        readonly int m_HashLocomotion = Animator.StringToHash("Locomotion");
        readonly int m_HashAirborn = Animator.StringToHash("Airborn");
        readonly int m_HashLanding = Animator.StringToHash("Landing");

        //Animator State Tag Hash값
        readonly int m_HashBlockInput = Animator.StringToHash("BlockInput");
        #endregion

        #region Property
        //이동 입력 값 체크
        protected bool IsMoveInput
        {
            get { return !Mathf.Approximately(m_Agent.velocity.sqrMagnitude, 0f); }
        }
        #endregion

        #region Unity Event Method
        private void Awake()
        {
            //참조
            m_Input = GetComponent<PlayerInputAgent>();
            m_CharCtrl = GetComponent<CharacterController>();
            m_Animator = GetComponent<Animator>();

            m_Camera = Camera.main;
            m_Agent = GetComponent<NavMeshAgent>();
            m_Agent.updatePosition = false;             //계산은 하되 이동은 하지 않는다
            m_Agent.updateRotation = true;              //계산도 하고 회전도 한다
            m_Agent.SetDestination(transform.position);

        }

        private void FixedUpdate()
        {
            CacheAnimatorState();
            UpdateInputBlocking();

            CalculateForwardMovement();

            TimeOutToIdle();
        }

        private void OnAnimatorMove()
        {
            //캐릭터 위치 보정
            Vector3 position = m_Agent.nextPosition;
            m_Animator.rootPosition = position;
            transform.position = position;

            //캐릭터 컨트롤러를 이용하여 이동
            if (m_Agent.remainingDistance > m_Agent.stoppingDistance)
            {
                m_CharCtrl.Move(m_Agent.velocity * Time.deltaTime);
            }
            else
            {
                m_CharCtrl.Move(Vector3.zero);
            }

            //그라운드
            m_IsGrounded = true;// m_CharCtrl.isGrounded;

            //애니메이션 적용
            m_Animator.SetFloat(m_HashForwardSpeed, m_Agent.velocity.magnitude);
            m_Animator.SetBool(m_HashGrounded, m_IsGrounded);
        }
        #endregion

        #region Custom Method
        //애니메이션 정보 얻어오기
        private void CacheAnimatorState()
        {
            //이전 상태값 셋팅
            m_PreviousCurrentStateInfo = m_CurrentStateInfo;
            m_PreviousNextStateInfo = m_NextStateInfo;
            m_PreviousIsAnimatorTransition = m_IsAnimatorTransition;

            //layerIndex(0) - baselayer 의 상태 얻어오기
            m_CurrentStateInfo = m_Animator.GetCurrentAnimatorStateInfo(0);
            m_NextStateInfo = m_Animator.GetNextAnimatorStateInfo(0);
            m_IsAnimatorTransition = m_Animator.IsInTransition(0);
        }

        //인풋 블록 체크
        private void UpdateInputBlocking()
        {
            bool inputBlock = m_CurrentStateInfo.tagHash == m_HashBlockInput && !m_IsAnimatorTransition;
            inputBlock |= m_NextStateInfo.tagHash == m_HashBlockInput;
            m_Input.playerControllerInputBlocked = inputBlock;
        }

        //마우스 클릭한 지점으로 이동값 설정
        private void CalculateForwardMovement()
        {
            if(m_Input.ClickMove)
            {
                //마우스의 위치에서 맵 좌표를 얻어온다
                Ray ray = m_Camera.ScreenPointToRay(m_Input.MousePosion);
                RaycastHit hit;
                if(Physics.Raycast(ray, out hit, 100f, groundLayerMast))
                {
                    //hit 오브젝트 체크
                    if(hit.transform.tag == "Ground")
                    {
                        m_Agent.stoppingDistance = 0.05f;
                        m_Agent.SetDestination(hit.point);

                        //그라운드 클릭 이펙트 효과
                        Vector3 effectPostion = hit.point + new Vector3(0f, 0.05f, 0f);
                        GameObject effectGo =
                            _EffectOneShot.RaiseEvent(EffectList.ClickEffect, effectPostion);
                            //EffectManager.Instance.EffectOneShot((int)EffectList.ClickEffect, effectPostion);
                        Destroy(effectGo, 2f);
                    }

                    isArrive = false;
                }
                else
                {
                    isArrive = true; 
                }

                m_Input.ClickMove = false;
            }

            //도착 체크
            if(isArrive == false)
            {
                if(m_Agent.remainingDistance <= m_Agent.stoppingDistance)
                {
                    isArrive = true;
                    m_Agent.SetDestination(transform.position);
                }
            }
        }

        //대기 동작 처리
        private void TimeOutToIdle()
        {
            bool inputDetected = IsMoveInput;

            if (m_IsGrounded && inputDetected == false)
            {
                m_IdleTime += Time.deltaTime;
                if (m_IdleTime >= idleTimeOut)
                {
                    m_Animator.SetTrigger(m_HashTimeoutToIlde);

                    //타이머초기화
                    m_IdleTime = 0f;
                }
            }
            else
            {
                //아이들 타이머 리셋
                m_IdleTime = 0f;
                m_Animator.ResetTrigger(m_HashTimeoutToIlde);
            }

            //애니메이션 처리
            m_Animator.SetBool(m_HashInputDetected, inputDetected);
        }
        #endregion
    }
}
