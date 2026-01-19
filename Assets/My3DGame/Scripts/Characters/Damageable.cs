using UnityEngine;
using UnityEngine.Events;

namespace My3DGame
{
    /// <summary>
    /// 데미지를 관리하는 클래스
    /// Health 연산
    /// </summary>
    public class Damageable : MonoBehaviour
    {
        #region Variables
        [Header("Health")]
        [SerializeField] protected HealthConfigSO _healthConfigSO;
        [SerializeField] protected HealthSO _currentHealthSO;

        //무적 타이머
        [SerializeField] protected float invulnerabiltyTime = 0.5f;
        protected float m_timeSinceLastHit = 0f;

        //이벤트 함수
        public event UnityAction<float> OnDamage;
        public event UnityAction OnDie;
        #endregion

        #region Property
        public bool IsInvulnerable { get; private set; }    //무적 체크
        public bool IsDeath {  get; private set; }          //죽음 체크
        public HealthSO CurrentHeathSO => _currentHealthSO;
        #endregion

        #region Unity Event Method
        private void Awake()
        {
            //CurrentHealth 체크 및 설정
            if(_currentHealthSO == null)
            {
                _currentHealthSO = ScriptableObject.CreateInstance<HealthSO>();
                _currentHealthSO.SetMaxHealth(_healthConfigSO.InitialHealth);
                _currentHealthSO.SetCurrentHealth(_healthConfigSO.InitialHealth);
            }
            else
            {
                _currentHealthSO.SetCurrentHealth(_currentHealthSO.MaxHealth);
            }
        }

        private void Update()
        {
            //죽음 체크
            if (IsDeath)
                return;

            //무적 타이머
            if(IsInvulnerable)
            {
                m_timeSinceLastHit += Time.deltaTime;
                if (m_timeSinceLastHit >= invulnerabiltyTime)
                {
                    IsInvulnerable = false;

                    //타이머 초기화
                    m_timeSinceLastHit = 0f;
                }
            }
        }
        #endregion

        #region Custom Method
        public void TakeDamage(float damage)
        {
            Debug.Log($"TakeDamage: {damage}");

            //무적 체크
            if (IsInvulnerable)
                return;

            IsInvulnerable = true;

            //체력 계산
            _currentHealthSO.InflictDamage(damage);
            Debug.Log($"CurrentHealth: {_currentHealthSO.CurrentHealth}");

            if (OnDamage != null)
                OnDamage.Invoke(damage);

            if(_currentHealthSO.CurrentHealth <= 0 && IsDeath == false)
            {
                Die();
            }
        }

        private void Die()
        {
            IsDeath = true;
            Debug.Log($"Die:");

            if (OnDie != null) 
                OnDie.Invoke();

            //죽음 처리
            //Destroy(gameObject);
        }

        //원샷 원킬
        public void Kill()
        {
            TakeDamage(_currentHealthSO.CurrentHealth);
        }

        //되살리기
        public void Revive()
        {
            _currentHealthSO.SetCurrentHealth(_currentHealthSO.MaxHealth);
            IsDeath = false;
        }

        //체력 회복
        public void Cure(float healthAdd)
        {
            //죽음 체크
            if (IsDeath)
                return;

            _currentHealthSO.RestoreHealth(healthAdd);
        }
        #endregion
    }
}