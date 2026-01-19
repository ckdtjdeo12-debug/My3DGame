using UnityEngine;

namespace MySample
{
    /// <summary>
    /// 풀링된 발사체 발사하는 터렛
    /// </summary>
    public class TestTurret : MonoBehaviour
    {
        #region Variables
        //public GameObject projectPrefab;    //발사체 프리팹
        public ObjectPool objectPool;

        //발사 설정
        public Transform mezzlePosition;        //파이어 포인트
        [SerializeField] private float muzzleVelocity = 700f;    //발사 속도
        [SerializeField] private float cooldownWindow = 0.1f;    //발사 간격
        private float nextTimeToShoot;
        #endregion

        #region Unity Event Method
        private void FixedUpdate()
        {
            if(Input.GetButton("Fire1") && Time.time > nextTimeToShoot && objectPool != null)
            {
                //발사
                GameObject bulletObject = objectPool.GetPooledObject().gameObject;

                if (bulletObject == null)
                    return;

                bulletObject.SetActive(true);

                bulletObject.transform.SetPositionAndRotation(mezzlePosition.position, mezzlePosition.rotation);
                bulletObject.GetComponent<Rigidbody>().AddForce(bulletObject.transform.forward * muzzleVelocity,
                    ForceMode.Acceleration);

                TestProjectile projectile = bulletObject.GetComponent<TestProjectile>();
                if (projectile != null)
                {
                    projectile.Deactivate();
                }

                nextTimeToShoot = Time.time + cooldownWindow;
            }
        }
        #endregion
    }
}