using UnityEngine;
using System.Collections;

namespace MySample
{
    /// <summary>
    /// 발사체를  관리하는 클래스
    /// 오브젝트풀에 들어가는 발사체
    /// </summary>
    [RequireComponent(typeof(PooledObject))]
    public class TestProjectile : MonoBehaviour
    {
        #region Variables
        //참조
        private PooledObject pooledObject;

        [SerializeField] private float timeoutDelay = 3f; //발사 후 3초 릴리즈
        #endregion

        #region Unity Event Method
        private void Awake()
        {
            //참조
            pooledObject = GetComponent<PooledObject>();
        }
        #endregion

        #region Custom Method
        //비활성
        public void Deactivate()
        {
            StartCoroutine(DeactivateRoutine(timeoutDelay));
        }

        IEnumerator DeactivateRoutine(float delay)
        {
            yield return new WaitForSeconds(delay);

            //발사체 초기화
            Rigidbody rb = GetComponent<Rigidbody>();
            if(rb)
            {
                rb.linearVelocity = Vector3.zero;
                rb.angularVelocity = Vector3.zero;
            }

            //풀로 되돌리기
            pooledObject.Release();
            gameObject.SetActive(false);
        }
        #endregion
    }
}