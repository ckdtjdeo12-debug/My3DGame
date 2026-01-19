using UnityEngine;

namespace My3DGame.AI
{
    /// <summary>
    /// 패트롤 하는 적을 관리하는 클래스, Enemy를 상속 받는다
    /// Enemy 기능 + 패트롤 기능
    /// </summary>
    public class EnemyPatrol : Enemy
    {
        #region Variables
        //웨이포인트
        public Transform[] wayPoints;
        #endregion

        #region Unity Event Method
        protected override void Start()
        {
            base.Start();

            //상속 받은 후 추가로 새로운 상태 등록
            stateMachine.RegisterState(new PatrolState());
        }
        #endregion
    }
}