using UnityEngine;

namespace My3DGame.AI
{
    /// <summary>
    /// 적을 공격하는 상태를 관리하는 클래스
    /// 공격 동작이 끝나면 대기 상태 전환
    /// </summary>
    public class AttackState : State
    {
        #region Variables
        //참조
        private Animator m_Animator;

        //애니메이터 파라미터
        readonly int m_HashForwardSpeed = Animator.StringToHash("ForwardSpeed");
        readonly int m_HashAttack = Animator.StringToHash("Attack");
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
            //애니메이터 설정
            m_Animator.SetFloat(m_HashForwardSpeed, 0f);
            m_Animator.SetTrigger(m_HashAttack);
        }

        //상태 업데이트, 매 프레임 마다 호출
        public override void OnUpdate(float deltaTime)
        {
            enemy.FaceToTarget();
        }

        //상태 나가기, 상태를 나갈때 마다 1회 호출
        public override void OnExit()
        {
            //공격 딜레이 타임 조정

        }

        #region Custom Method
        #endregion
    }
}