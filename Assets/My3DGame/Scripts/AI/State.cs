
namespace My3DGame.AI
{
    /// <summary>
    /// 적 상태를 관리하는 클래스, 모든 상태의 부모 추상 클래스
    /// 속성(멤버): 현재 상태가 등록되어 상태머신, 상태머신의 소유주(Enemy, T) 
    /// 기능(함수): 상태 속성값 셋팅, 상태 들어가기, 상태 업데이트, 상태 나기가
    /// </summary>
    public abstract class State
    {
        #region Variables
        protected Enemy enemy;                  //상태머신의 소유주
        protected StateMachine stateMachine;    //현재 상태가 등록되어 상태머신
        #endregion

        #region Construct
        public State() { }
        #endregion

        #region Custom Method
        //상태 속성값(상태 머신, 소유주) 셋팅: 상태머신에 상태를 등록할때 매개변수로 받아와서 셋팅
        public void SetState(Enemy _enemy, StateMachine _stateMachine)
        {
            this.enemy = _enemy;
            this.stateMachine = _stateMachine;

            //상태 초기화
            OnInitalize();
        }

        //재정의 사용
        public virtual void OnInitalize() { }           //상태 초기화 함수, 상태 생성시 1회 호출
        public virtual void OnEnter() { }               //상태 들어가기, 상태 들어갈때 마다 1회 호출
        public abstract void OnUpdate(float deltaTime); //상태 업데이트, 추상메서드, 강제로 구현, 매 프레임 마다 호출
        public virtual void OnExit() { }                //상태 나가기, 상태를 나갈때 마다 1회 호출
        #endregion
    }
}