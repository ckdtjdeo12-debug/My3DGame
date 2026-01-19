using My3DGame.AI;
using Unity.Cinemachine;
using UnityEngine;

namespace My3DGame
{
    /// <summary>
    /// 플레이어 캐릭터를 관리하는 클래스
    /// </summary>
    public class PlayerController : MonoBehaviour
    {
        #region Variables
        //참조
        protected PlayerInputReader m_Input;
        protected CharacterController m_CharCtrl;
        protected Animator m_Animator;
        protected CameraSettings m_CameraSettings;
        protected Damageable m_Damageable;

        public MeleeWeapon meleeWeapon;

        //애니메이터
        protected AnimatorStateInfo m_CurrentStateInfo;     //현재 애니메이터 상태 정보
        protected AnimatorStateInfo m_NextStateInfo;        //다음 애니메이터 상태 정보
        protected bool m_IsAnimatorTransition;              //상태 변경 체크
        protected AnimatorStateInfo m_PreviousCurrentStateInfo;  //이전 상태
        protected AnimatorStateInfo m_PreviousNextStateInfo;     //이전 상태
        protected bool m_PreviousIsAnimatorTransition;           //이전 상태 변경 체크

        //Move, Jump
        [SerializeField] protected float maxForwardSpeed = 8f;     //최대 이동 속도
        [SerializeField] protected float gravity = 20f;                            //중력 값
        [SerializeField] protected float minTurnSpeed = 400f;
        [SerializeField] protected float maxTurnSpeed = 1200f;
        [SerializeField] protected float jumpSpeed = 10f;

        protected bool m_IsGrounded = false;
        protected float m_ForwardSpeed;
        protected float m_VerticalSpeed;
        protected float m_DesiredForwardSpeed;
        protected bool m_ReadyJump = false;

        //회전
        protected Quaternion m_TargetRotation;        //회전할 값
        protected float m_AngleDiff;                //

        //대기
        [SerializeField] protected float idleTimeOut = 5f;  //로코모션에서 5초 타임아웃 되면 대기로 보낸다
        protected float m_IdleTime = 0f;

        //공격
        protected bool m_InAttack;                          //현재 공격중인지 체크

        //상수 값
        const float k_GroundAcceleration = 20f;     //이동 시작할때 가속도 값
        const float k_GroundDeceleration = 25f;     //멈출때 감속도 값
        const float k_InverseOnEighty = 1f / 180f;      //공중 회전시 연산 계수값
        const float k_AirbornTurnSpeedProportion = 1.0f; //공중 회전시 연산 계수값
        const float k_StickingGravityProportion = 0.3f;
        const float k_GroundedRayDistance = 1f;             //그라운드 체크
        const float k_JumpAbortSpeed = 10f;             //공중에서 아래로 내려오는 속도


        //Animator Parameters Hash값
        readonly int m_HashForwardSpeed = Animator.StringToHash("ForwardSpeed");
        readonly int m_HashAirborneVerticalSpeed = Animator.StringToHash("AirborneVerticalSpeed");
        readonly int m_HashAngleDelatRad = Animator.StringToHash("AngleDeltaRad");
        readonly int m_HashInputDetected = Animator.StringToHash("InputDetected");
        readonly int m_HashTimeoutToIlde = Animator.StringToHash("TimeoutToIdle");
        readonly int m_HashGrounded = Animator.StringToHash("Grounded");
        readonly int m_HashMeleeAttack = Animator.StringToHash("MeleeAttack");
        readonly int m_HashHurt = Animator.StringToHash("Hurt");
        readonly int m_HashDeath = Animator.StringToHash("Death");

        //Animator State Hash값
        readonly int m_HashLocomotion = Animator.StringToHash("Locomotion");
        readonly int m_HashAirborn = Animator.StringToHash("Airborn");
        readonly int m_HashLanding = Animator.StringToHash("Landing");

        readonly int m_HashStateTime = Animator.StringToHash("StateTime");

        //Animator State Tag Hash값
        readonly int m_HashBlockInput = Animator.StringToHash("BlockInput");
        #endregion

        #region Property
        //이동 입력 값 체크
        protected bool IsMoveInput
        {
            get { return !Mathf.Approximately(m_Input.Movement.sqrMagnitude, 0f); }
        }
        #endregion

        #region Unity Event Method
        private void Awake()
        {
            //참조
            m_Input = GetComponent<PlayerInputReader>();
            m_CharCtrl = GetComponent<CharacterController>();
            m_Animator = GetComponent<Animator>();
            m_Damageable = GetComponent<Damageable>();

            m_CameraSettings = GameObject.FindFirstObjectByType<CameraSettings>();
        }

        private void OnEnable()
        {
            m_Damageable.OnDamage += OnDamaged;
            m_Damageable.OnDie += OnDie;
        }

        private void OnDisable()
        {
            m_Damageable.OnDamage -= OnDamaged;
            m_Damageable.OnDie -= OnDie;
        }

        private void Start()
        {
            //초기화
            meleeWeapon.SetOwner(this.gameObject);
        }

        private void FixedUpdate()
        {
            CacheAnimatorState();
            UpdateInputBlocking();

            UpdateAttack();

            CalculateForwardMovement();
            CalculateVerticalMovement();

            SetTargetRotaion();
            if (IsMoveInput)
            {
                UpdateRoation();
            }

            TimeOutToIdle();
        }

        private void OnAnimatorMove()
        {
            Vector3 movement;

            if(m_IsGrounded)
            {
                RaycastHit hit;
                Ray ray = new Ray(transform.position + Vector3.up * k_GroundedRayDistance * 0.5f,
                    -Vector3.up);
                if (Physics.Raycast(ray, out hit, k_GroundedRayDistance, Physics.AllLayers,
                    QueryTriggerInteraction.Ignore))
                {
                    movement = Vector3.ProjectOnPlane(m_Animator.deltaPosition, hit.normal);
                }
                else
                {
                    movement = m_Animator.deltaPosition;
                }   
            }
            else
            {
                movement = m_ForwardSpeed * transform.forward * Time.deltaTime;
            }

            //캐릭터 컨트롤러에 애니메이션 값 적용
            m_CharCtrl.transform.rotation *= m_Animator.deltaRotation;
            movement += m_VerticalSpeed * Vector3.up * Time.deltaTime;
            m_CharCtrl.Move(movement);

            //그라운드
            m_IsGrounded = m_CharCtrl.isGrounded;

            //애니메이션 적용
            if (!m_IsGrounded)
            {
                m_Animator.SetFloat(m_HashAirborneVerticalSpeed, m_VerticalSpeed);
            }
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

        //공격
        private void UpdateAttack()
        {
            m_Animator.ResetTrigger(m_HashMeleeAttack);     //트리거 파라미터 초기화
            m_Animator.SetFloat(m_HashStateTime, Mathf.Repeat(
                m_Animator.GetCurrentAnimatorStateInfo(0).normalizedTime, 1f));
            if (m_Input.Attack)
            {
                m_Animator.SetTrigger(m_HashMeleeAttack);
            }
        }

        //앞뒤좌우 이동
        private void CalculateForwardMovement()
        {
            Vector2 moveInput = m_Input.Movement;
            //moveInput이 1이상이 안되도록 한다
            if (moveInput.sqrMagnitude > 1f)
            {
                moveInput.Normalize();
            }

            //입력에 의한 이동값 얻어오기
            m_DesiredForwardSpeed = moveInput.magnitude * maxForwardSpeed;

            //가속도값 얻어오기
            float accelation = IsMoveInput ? k_GroundAcceleration : k_GroundDeceleration;
            //이동 속도
            m_ForwardSpeed = Mathf.MoveTowards(m_ForwardSpeed, m_DesiredForwardSpeed, accelation * Time.deltaTime);

            //애니메이터 적용
            m_Animator.SetFloat(m_HashForwardSpeed, m_ForwardSpeed);
        }

        //점프 - 위아래 이동
        private void CalculateVerticalMovement()
        {
            //점프 준비 체크
            if(!m_Input.Jump && m_IsGrounded)
                m_ReadyJump = true;

            if(m_IsGrounded)
            {
                m_VerticalSpeed = -gravity * k_StickingGravityProportion;

                if(m_ReadyJump && m_Input.Jump)
                {
                    m_VerticalSpeed = jumpSpeed;
                    m_IsGrounded = false;
                    m_ReadyJump = false;
                }
            }
            else
            {
                //점프버튼을 Hold하지 않으면 추가 속도 적용
                if(!m_Input.Jump && m_VerticalSpeed > 0f)
                {
                    m_VerticalSpeed -= k_JumpAbortSpeed * Time.deltaTime;
                }

                if(Mathf.Approximately(m_VerticalSpeed, 0f))
                {
                    m_VerticalSpeed = 0f;
                }

                //
                m_VerticalSpeed -= gravity * Time.deltaTime;
            }
        }

        //회전 값 얻어오기
        private void SetTargetRotaion()
        {
            Vector2 moveInput = m_Input.Movement;
            Vector3 localMovementDirection = new Vector3(moveInput.x, 0f, moveInput.y).normalized;

            //입력 값에 의한 앞방향 구하기
            //Vector3 forward = Quaternion.Euler(localMovementDirection.x, localMovementDirection.y, 
            //    localMovementDirection.z) * Vector3.forward;
            //카메라의 앞 방향
            Vector3 forward = Quaternion.Euler(0f,
                m_CameraSettings.freeLookCamera.GetComponent<CinemachineOrbitalFollow>().HorizontalAxis.Value,
                0f) * Vector3.forward;
            forward.y = 0f;
            forward.Normalize();

            Quaternion targetRotaion;
            if(Mathf.Approximately(Vector3.Dot(localMovementDirection, Vector3.forward), -1.0f))
            {
                targetRotaion = Quaternion.LookRotation(-forward);
            }
            else
            {
                Quaternion cameraToInputOffset = Quaternion.FromToRotation(Vector3.forward,
                    localMovementDirection);
                targetRotaion = Quaternion.LookRotation(cameraToInputOffset * forward);
            }

            //
            Vector3 resultForword = targetRotaion * Vector3.forward;

            float angleCurrent = Mathf.Atan2(transform.forward.x, transform.forward.z) * Mathf.Rad2Deg;
            float angleTarget = Mathf.Atan2(resultForword.x, resultForword.z) * Mathf.Rad2Deg;

            m_AngleDiff = Mathf.DeltaAngle(angleCurrent, angleTarget);
            m_TargetRotation = targetRotaion;
        }

        private void UpdateRoation()
        {
            //애니메이션 적용
            m_Animator.SetFloat(m_HashAngleDelatRad, m_AngleDiff * Mathf.Deg2Rad);

            Vector3 locaclInput = new Vector3(m_Input.Movement.x, 0f, m_Input.Movement.y);            
            float groundTurnSpeed = Mathf.Lerp(maxTurnSpeed, minTurnSpeed, m_ForwardSpeed / m_DesiredForwardSpeed);
            //실제 회전속도(ground, air)
            float actualTurnSpeed = m_IsGrounded ? groundTurnSpeed : groundTurnSpeed *
                Vector3.Angle(transform.forward, locaclInput) * k_InverseOnEighty * k_AirbornTurnSpeedProportion;
            m_TargetRotation = Quaternion.RotateTowards(transform.rotation, m_TargetRotation, 
                actualTurnSpeed * Time.deltaTime);

            transform.rotation = m_TargetRotation;
        }


        //대기 동작 처리
        private void TimeOutToIdle()
        {
            bool inputDetected = IsMoveInput || m_Input.Jump || m_Input.Attack;

            if (m_Input.Attack)
            {
                m_Input.Attack = false;
            }

            if (m_IsGrounded && inputDetected == false)
            {
                m_IdleTime += Time.deltaTime;
                if(m_IdleTime >= idleTimeOut)
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

        public void MeleeAttackStart(int throwing = 0)
        {
            meleeWeapon.StartAttack(throwing != 0);
            m_InAttack = true;
        }

        public void MeleeAttackEnd()
        {
            meleeWeapon.EndAttack();
            m_InAttack = false;
        }

        private void OnDamaged(float damage)
        {
            m_Animator.SetTrigger(m_HashHurt);
        }

        private void OnDie()
        {
            m_Animator.SetTrigger(m_HashDeath);
        }
        #endregion
    }
}
