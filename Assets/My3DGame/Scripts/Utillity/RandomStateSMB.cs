using UnityEngine;

namespace My3DGame.Utillity
{
    /// <summary>
    /// 타이머를 돌려 랜덤하게 설정된 상태로 보낸다
    /// </summary>
    public class RandomStateSMB : StateMachineBehaviour
    {
        #region Variables
        public int numberOfStates = 3;  //랜덤 상태 갯수
        public float minNormalTime = 0f;    //기본 상태 플레이 최소 시간
        public float maxNormalTime = 3f;    //기본 상태 플레이 최대 시간

        protected float m_NormalTime; //기본 상태 플레이 시간

        //애니메이터 파라미터 값
        readonly int m_HashRandomIdle = Animator.StringToHash("RandomIdle");
        #endregion

        // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
        override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            //기본 상태 플레이 타이머 시간
            m_NormalTime = Random.Range(minNormalTime, maxNormalTime);
        }

        // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
        override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            //파라미터 초기화
            if(animator.IsInTransition(0) 
                && animator.GetCurrentAnimatorStateInfo(0).fullPathHash == stateInfo.fullPathHash)
            {
                animator.SetInteger(m_HashRandomIdle, -1);
            }

            //램덤 상태로 보내기
            if(stateInfo.normalizedTime > m_NormalTime && !animator.IsInTransition(0))
            {
                animator.SetInteger(m_HashRandomIdle, Random.Range(0, numberOfStates));
            }
        }

        // OnStateExit is called when a transition ends and the state machine finishes evaluating this state
        //override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        //{
        //    
        //}

        // OnStateMove is called right after Animator.OnAnimatorMove()
        //override public void OnStateMove(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        //{
        //    // Implement code that processes and affects root motion
        //}

        // OnStateIK is called right after Animator.OnAnimatorIK()
        //override public void OnStateIK(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        //{
        //    // Implement code that sets up animation IK (inverse kinematics)
        //}
    }
}