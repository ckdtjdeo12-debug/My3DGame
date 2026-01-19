using UnityEngine;

namespace My3DGame.AI
{
    /// <summary>
    /// 타겟(플레이어)를 찾는 클래스
    /// 속성: 타겟, 디텍팅 범위
    /// 기능: 0.1초마다 적을 디텍팅, 디텍팅 범위를 기즈모 표시
    /// </summary>
    public class DetectionModule : MonoBehaviour
    {
        #region Variables
        private Transform m_Target;
        public LayerMask targetMask;        //타겟의 레이어

        [SerializeField] private float detectionRange = 5f;  //디텍팅 범위
        [SerializeField] private float detectionDelayTime = 0.1f;   //디텍팅 시간 간격

        private float m_DistanceToTarget;   //현재 타겟과의 거리
        #endregion

        #region Property
        public Transform Target => m_Target;
        public float DistanceToTarget => m_DistanceToTarget;
        #endregion

        #region Unity Event Method
        private void Start()
        {
            //0.1초마다 적을 디텍팅
            InvokeRepeating("UpdateDetection", 0f, detectionDelayTime);
        }

        //디텍팅 범위를 기즈모 표시
        private void OnDrawGizmosSelected()
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(transform.position, detectionRange);
        }
        #endregion

        #region Custom Method
        //적을 디텍팅
        private void UpdateDetection()
        {
            m_DistanceToTarget = 0f;

            //가장 가까운 적 찾기
            float shortestDistance = Mathf.Infinity;
            Transform nearestEnemy = null;

            Collider[] enemies = Physics.OverlapSphere(transform.position, detectionRange, 
                targetMask);

            foreach (var enemy in enemies)
            {
                float distance = Vector3.Distance(transform.position, enemy.transform.position);
                if(distance < shortestDistance)
                {
                    shortestDistance = distance;
                    nearestEnemy = enemy.transform;
                }
            }

            if (nearestEnemy != null && shortestDistance <= detectionRange)
            {
                m_DistanceToTarget = shortestDistance;
                m_Target = nearestEnemy;
            }
            else
            {
                m_Target = null;
            }

        }
        #endregion
    }
}