using UnityEngine;
using System;
using My3DGame.Utillity;

namespace My3DGame
{
    /// <summary>
    /// 근접 전투 무기를 관리하는 클래스
    /// </summary>
    public class MeleeWeapon : MonoBehaviour
    {

        /// <summary>
        /// 무기의 공격 충돌 체크 포인트
        /// </summary>
        [Serializable]
        public class AttackPoint
        {
            public float radius;     //충돌 체크 반경
            public Vector3 offset;  //충돌 포인트 위치 조정값
            public Transform attackRoot; //충돌 포인트 위치 기준 오브젝트
        }

        #region Variabls
        [SerializeField] protected float attackDamage = 10f;        //공격 데미지

        [SerializeField] protected AttackPoint[] attackPoints = new AttackPoint[0]; //충돌 체크 포인트 배열

        public ParticleSystem hitParticlePrefab;                //hit 이펙트
        public TimeEffect[] effect;                             //공격 트레일 이펙트

        [SerializeField] protected LayerMask targetLayers;      //충돌 체크 레이어마스크

        protected GameObject m_Owner;
        protected Vector3[] m_PreviousPos = null;
        protected Vector3 m_Direction;                          //hit 방향

        protected bool m_IsThrowingHit = false;
        protected bool m_InAttack = false;

        const int PARTICLE_COUNT = 10;
        protected ParticleSystem[] m_ParticlesPool = new ParticleSystem[PARTICLE_COUNT];
        protected int m_CurrentParticle = 0;

        protected static RaycastHit[] s_RaycastHitCache = new RaycastHit[32];
        protected static Collider[] s_ColliderCache = new Collider[32];
        #endregion

        #region Unity Event Method
        private void Awake()
        {
            //hitParticle 풀 만들기
            if(hitParticlePrefab != null)
            {
                for (int i = 0; i < PARTICLE_COUNT; i++)
                {
                    m_ParticlesPool[i] = Instantiate(hitParticlePrefab);
                    m_ParticlesPool[i].Stop();
                }
            }
        }

        //
        private void FixedUpdate()
        {
            if(m_InAttack)
            {
                for (int i = 0; i < attackPoints.Length; i++)
                {
                    AttackPoint apt = attackPoints[i];

                    Vector3 worldPos = apt.attackRoot.position + apt.attackRoot.TransformVector(apt.offset);
                    Vector3 attackVector = worldPos - m_PreviousPos[i];

                    if(attackVector.magnitude < 0.001f)
                    {
                        attackVector = Vector3.forward * 0.001f;
                    }

                    Ray r = new Ray(worldPos, attackVector.normalized);
                    int contacts = Physics.SphereCastNonAlloc(r, apt.radius, s_RaycastHitCache,
                        attackVector.magnitude, ~0, QueryTriggerInteraction.Ignore);

                    for (int j = 0; j < contacts; j++)
                    {
                        Collider collider = s_RaycastHitCache[j].collider;

                        if (collider != null)
                        {
                            CheckDamage(collider, apt);
                        }
                    }

                }
            }
        }

        //충돌 체크 포인트 그리기
        private void OnDrawGizmosSelected()
        {
            for (int i = 0; i < attackPoints.Length; i++)
            {
                AttackPoint apt = attackPoints[i];
                if (apt.attackRoot != null)
                {
                    Vector3 worldPos = apt.attackRoot.TransformVector(apt.offset);
                    Gizmos.color = new Color(1f, 1f, 1f, 0.4f);
                    Gizmos.DrawSphere(apt.attackRoot.position + worldPos, apt.radius);
                }
            }
        }
        #endregion

        #region Custom Method
        //hit한 충돌체에 데미지 주기
        private void CheckDamage(Collider other, AttackPoint apt)
        {
            //Damageable 체크
            Damageable d = other.GetComponent<Damageable>();
            if (d == null)
                return;

            if (other.gameObject == m_Owner)
                return;

            if ((targetLayers.value & (1 << other.gameObject.layer)) == 0)
                return;

            //Debug.Log($"TakeDamage {attackDamage}");
            d.TakeDamage(attackDamage);

            //hit 이펙트
            if(hitParticlePrefab != null)
            {
                Vector3 worldPos = apt.attackRoot.position + apt.attackRoot.TransformVector(apt.offset);
                m_ParticlesPool[m_CurrentParticle].transform.position = worldPos;
                m_ParticlesPool[m_CurrentParticle].time = 0;
                m_ParticlesPool[m_CurrentParticle].Play();
                m_CurrentParticle = (m_CurrentParticle+1) % PARTICLE_COUNT;
            }
        }

        public void SetOwner(GameObject owner)
        {
            m_Owner = owner;
        }

        public void StartAttack(bool throwingAttack)
        {
            m_InAttack = true;

            m_IsThrowingHit = throwingAttack;

            m_PreviousPos = new Vector3[attackPoints.Length];
            for (int i = 0; i < attackPoints.Length; i++)
            {
                Vector3 worldPos = attackPoints[i].attackRoot.position +
                    attackPoints[i].attackRoot.TransformVector(attackPoints[i].offset);
                m_PreviousPos[i] = worldPos;
            }
        }

        public void EndAttack()
        {
            m_InAttack = false;
        }
        #endregion
    }
}