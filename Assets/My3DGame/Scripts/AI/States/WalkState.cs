using UnityEngine;
using UnityEngine.AI;

namespace My3DGame.AI
{
    /// <summary>
    /// 적을 추격하여 이동하는 상태를 관리하는 클래스
    /// </summary>
    public class WalkState : State
    {
        #region Variables
        //참조
        private Animator m_Animator;
        private NavMeshAgent m_Agent;

        //애니메이터 파라미터
        readonly int m_HashForwardSpeed = Animator.StringToHash("ForwardSpeed");
        #endregion

        //상태 초기화 함수, 상태 생성시 1회 호출
        public override void OnInitalize()
        {
            //참조
            m_Animator = enemy.GetComponent<Animator>();
            m_Agent = enemy.GetComponent<NavMeshAgent>();
        }

        //상태 들어가기, 상태 들어갈때 마다 1회 호출
        public override void OnEnter()
        {
            //타겟 체크
            if(enemy.Target)
            {
                m_Agent.stoppingDistance = 1.5f;
                m_Agent.SetDestination(enemy.Target.position);
            }
        }

        //상태 업데이트, 매 프레임 마다 호출
        public override void OnUpdate(float deltaTime)
        {
            //타겟 체크
            if (enemy.Target)
            {
                if (enemy.IsAttackable)
                {
                    stateMachine.ChangeState(new AttackState());
                }
                else
                {
                    m_Agent.SetDestination(enemy.Target.position);
                }

                //애니메이션 적용
                m_Animator.SetFloat(m_HashForwardSpeed, m_Agent.velocity.magnitude);
            }
            else //타겟을 잃어버리면
            {
                stateMachine.ChangeState(new IdleState());
            }
        }

        //상태 나가기, 상태를 나갈때 마다 1회 호출
        public override void OnExit()
        {
            //NavMeshAgent 길찾기 초기화
            m_Agent.ResetPath();
        }

        #region Custom Method
        #endregion
    }
}