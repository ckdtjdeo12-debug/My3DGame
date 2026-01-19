using UnityEngine;
using System.Collections.Generic;

namespace My3DGame.AI
{
    /// <summary>
    /// (등록된) 적 상태들을 관리하는 봉인 클래스
    /// 속성: 상태머신의 소유주, 상태들을 등록하는 변수(Dictionary), 현재 상태
    /// 기능: 상태머신에 상태 등록하기, 현재 상태 업데이트, 상태 변경
    /// </summary>
    public sealed class StateMachine
    {
        #region Variables
        private Enemy enemy;                //상태머신의 소유주

        //상태들을 등록하는 변수
        private Dictionary<System.Type, State> states = new Dictionary<System.Type, State>();

        private State m_CurrentState;       //현재 상태
        private State m_PreviousState;      //이전 상태
        private float m_ElapseTime = 0f;    //현재 상태가 진행된 누적 시간 카운트
        #endregion

        #region Property
        public State CurrentState => m_CurrentState;
        public State PreviousState => m_PreviousState;
        public float ElapseTime => m_ElapseTime;
        #endregion

        #region Constructor
        //생성자, 매개변수로 소유주, 생태머신의 초기 상태
        public StateMachine(Enemy _enemy, State initalState)
        {
            //소유주 저장
            this.enemy = _enemy;

            //초기 상태 설정 - 등록
            RegisterState(initalState);

            //현재 상태로 설정
            m_CurrentState = initalState;
            m_CurrentState.OnEnter();
            m_ElapseTime = 0f;

            Debug.Log($"{initalState} 상태로 처음 시작");
        }
        #endregion

        #region Custom Method
        //상태머신에 매개변수로 들어온 상태 등록
        public void RegisterState(State state)
        {
            //상태 셋팅
            state.SetState(this.enemy, this);

            //상태 저장
            states[state.GetType()] = state;
        }

        //현재 상태 업데이트
        public void Update(float delaTime)
        {
            m_ElapseTime += delaTime;
            m_CurrentState.OnUpdate(delaTime);
        }

        //상태 변경
        public State ChangeState(State newState)
        {
            //새로운 상태의 타입받아와서 타입체크
            var newType = newState.GetType();
            //현재 생태 체크
            if(newType == m_CurrentState.GetType())
            {
                //새로운 상태가 현재 상태와 같은면 변경하지 않는다
                return m_CurrentState;
            }

            //현재 상태 나가기
            m_CurrentState.OnExit();

            //상태변경
            m_PreviousState = m_CurrentState;
            m_CurrentState = states[newType];

            //새로운 상태 들어가기
            m_CurrentState.OnEnter();
            m_ElapseTime = 0f;

            return m_CurrentState;
        }
        #endregion
    }
}