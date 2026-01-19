using UnityEngine;
using UnityEngine.AI;

namespace My3DGame.AI
{
    /// <summary>
    /// 등록 되어 있는 Waypoints들을 순회하는 상태를 관리하는 클래스
    /// </summary>
    public class PatrolState : State
    {
        #region Variables
        //참조
        private Animator m_Animator;
        private NavMeshAgent m_Agent;

        private EnemyPatrol m_EnemyPatrol;

        //패트롤
        private Transform m_TargetWayPoint = null;  //현재 목표 웨이포인트
        private int m_WayPointIndex = -1;            //현재 목표 웨이포인트 인덱스

        //애니메이터 파라미터
        readonly int m_HashForwardSpeed = Animator.StringToHash("ForwardSpeed");
        #endregion

        #region Property
        public Transform[] WayPoints => m_EnemyPatrol?.wayPoints;
        #endregion

        //상태 초기화 함수, 상태 생성시 1회 호출
        public override void OnInitalize()
        {
            //참조
            m_Animator = enemy.GetComponent<Animator>();
            m_Agent = enemy.GetComponent<NavMeshAgent>();

            //부모 객체로 부터 자식 객체 가져오기
            m_EnemyPatrol = enemy as EnemyPatrol;
        }

        //상태 들어가기, 상태 들어갈때 마다 1회 호출
        public override void OnEnter()
        {
            //NavMeshAgent 설정
            m_Agent.stoppingDistance = 0.2f;

            //다음 목표 웨이포인트 찾기
            if(m_TargetWayPoint == null)
            {
                FindNextWayPoint();
            }

            //다음 목표를 찾으면
            if (m_TargetWayPoint)
            {
                m_Agent.SetDestination(m_TargetWayPoint.position);
            }
            else
            {
                stateMachine.ChangeState(new IdleState());
            }
        }

        //상태 업데이트, 매 프레임 마다 호출
        public override void OnUpdate(float deltaTime)
        {
            //타겟 체크
            if (enemy.Target)
            {
                //공격 가능 여부
                if (enemy.IsAttackable)
                {
                    stateMachine.ChangeState(new AttackState());
                }
                else
                {
                    stateMachine.ChangeState(new WalkState());
                }
            }
            else //적이 감지 안되면 계속 패트롤
            {
                //m_TargetWayPoint에 도착 판정
                if (m_Agent.remainingDistance <= m_Agent.stoppingDistance)
                {
                    //다음 목표 웨이포인트 찾기
                    FindNextWayPoint();

                    //대기 상태로 보내고 대기 상태에서 대기 타임후에 다시 패트롤로 돌아온다
                    stateMachine.ChangeState(new IdleState());
                }
                else
                {
                    //애니메이션 적용
                    m_Animator.SetFloat(m_HashForwardSpeed, m_Agent.velocity.magnitude);
                }
            }
        }

        //상태 나가기, 상태를 나갈때 마다 1회 호출
        public override void OnExit()
        {
            //NavMeshAgent 길찾기 초기화
            m_Agent.ResetPath();
        }

        #region Custom Method
        //다음 목표 웨이포인트 찾기
        private void FindNextWayPoint()
        {
            m_TargetWayPoint = null;

            if(WayPoints != null && WayPoints.Length > 0)
            {
                m_WayPointIndex = (m_WayPointIndex + 1) % WayPoints.Length;
                m_TargetWayPoint = WayPoints[m_WayPointIndex];
            }
        }
        #endregion
    }
}
