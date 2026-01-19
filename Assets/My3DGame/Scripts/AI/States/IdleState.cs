using UnityEngine;

namespace My3DGame.AI
{
    /// <summary>
    /// 적의 대기 상태를 관리하는 클래스
    /// 디텍션하다가 타겟이 잡이면 걷기 상태 전환
    /// 공격 범위에 들어오면 공격 상태 전환
    /// 공격 상태가 가능해도 공격 딜레이 시간 체크
    /// </summary>
    public class IdleState : State
    {
        #region Variables
        //참조
        private Animator m_Animator;

        //패트롤 설정
        private bool m_IsPatrol = false;
        private float m_MinTime = 0f;
        private float m_MaxTime = 3f;
        private float m_IdleTime = 0;   //대기 시간

        //애니메이터 파라미터
        readonly int m_HashForwardSpeed = Animator.StringToHash("ForwardSpeed");
        #endregion

        //상태 초기화 함수, 상태 생성시 1회 호출
        public override void OnInitalize()
        {
            //참조
            m_Animator = enemy.GetComponent<Animator>();
        }

        //상태 들어가기, 상태 들어갈때 마다 1회 호출
        public override void OnEnter()
        {
            //애니메이션 상태 설정
            m_Animator.SetFloat(m_HashForwardSpeed, 0f);

            //패트롤 설정
            if(enemy is EnemyPatrol)
            {
                m_IsPatrol = true;
                m_IdleTime = Random.Range(m_MinTime, m_MaxTime);
            }
        }

        //상태 업데이트, 매 프레임 마다 호출
        public override void OnUpdate(float deltaTime)
        {
            //타겟 체크
            if(enemy.Target)
            {
                //공격 가능 여부
                if(enemy.IsAttackable)
                {
                    if(stateMachine.ElapseTime >= enemy.AttackDelayTime)
                    {
                        stateMachine.ChangeState(new AttackState());
                    }
                }
                else
                {
                    stateMachine.ChangeState(new WalkState());
                }
            }
            else if (m_IsPatrol)
            {
                //대기 시간 체크
                if(stateMachine.ElapseTime >= m_IdleTime)
                {
                    stateMachine.ChangeState(new PatrolState());
                }
            }
        }

        //상태 나가기, 상태를 나갈때 마다 1회 호출
        public override void OnExit()
        {
            
        }

        #region Custom Method
        #endregion
    }
}