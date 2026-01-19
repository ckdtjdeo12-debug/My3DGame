using UnityEngine;

namespace MySample
{
    /// <summary>
    /// 캐릭터의 머리 뒤쪽 위에서 캐릭터를 쫒아가는 카메라 구현
    /// </summary>
    public class TopDownCamera : MonoBehaviour
    {
        #region Variables
        public Transform target;                //플레이어
        [SerializeField] protected float height = 5f;            //플레이어로 부터의 높이
        [SerializeField] protected float distance = 10f;         //플레이어로 부터의 거리
        [SerializeField] protected float angle = 45f;            //플레이어로 부터의 회전 각도
        [SerializeField] protected float smoothSpeed = 0.5f;     //플레이어를 쫓아가는 속도
        [SerializeField] protected float lookAtHeight = 2f;      //플레이어의 머리 위치, 카메라가 실제 바라보는 위치

        private Vector3 refVelocity;    //속도
        #endregion

        #region Unity Event Method
        private void Start()
        {
            HandleTopDownCamera();
        }

        private void LateUpdate()
        {
            HandleTopDownCamera();
        }

        //Gizmo 그리기
        private void OnDrawGizmos()
        {
            Gizmos.color = new Color(1f, 0f, 0f, 0.5f);
            if (target != null)
            {
                Vector3 lookAtPosition = target.position;
                lookAtPosition.y += lookAtHeight;
                Gizmos.DrawLine(transform.position, lookAtPosition);    //카메라 위치 -> 캐릭터 머리까지
                Gizmos.DrawSphere(lookAtPosition, 0.25f);
            }
            Gizmos.DrawSphere(transform.position, 0.25f);
        }
        #endregion

        #region Custom Method
        private void HandleTopDownCamera()
        {
            //타겟 체크
            if (target == null)
                return;

            //카메라 위치 설정
            Vector3 worldPosion = (target.forward * -distance) + Vector3.up * height;
            Vector3 rotateVector = Quaternion.AngleAxis(angle, Vector3.up) * worldPosion;

            Vector3 finalTargetPositon = target.position;
            finalTargetPositon.y += lookAtHeight;
            Vector3 finalPosition = finalTargetPositon + rotateVector;

            //카메라 이동
            //transform.position = finalPosition;
            transform.position = Vector3.SmoothDamp(transform.position, finalPosition,
                ref refVelocity, smoothSpeed);

            //플레이어 바라보기
            transform.LookAt(finalTargetPositon);
        }
        #endregion
    }
}