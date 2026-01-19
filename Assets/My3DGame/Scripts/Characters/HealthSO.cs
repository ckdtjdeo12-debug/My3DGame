using UnityEngine;
using UnityEngine.Rendering;

namespace My3DGame
{
    /// <summary>
    /// 캐릭터의 체력을 관리하는 스크립터블 오브젝트
    /// 플레이어나 특정 캐릭터의 체력 설정 가능
    /// 기본적으로 HealthConfigSO에서 체력 속성값을 가져온다
    /// </summary>
    [CreateAssetMenu(fileName = "HealthSO", menuName = "EntityConfig/Health")]
    public class HealthSO : ScriptableObject
    {
        #region Variables
        [SerializeField] protected float _maxHealth;
        [SerializeField] protected float _currentHealth;
        #endregion

        #region Property
        public float MaxHealth => _maxHealth;
        public float CurrentHealth => _currentHealth;
        public float HealthRatio => _currentHealth / _maxHealth;
        #endregion

        #region Custom Method
        public void SetMaxHealth(float newValue)
        {
            _maxHealth = newValue;
        }

        public void SetCurrentHealth(float newValue)
        {
            _currentHealth = newValue;
        }

        public void InflictDamage(float damageValue)
        {
            _currentHealth -= damageValue;
            if (_currentHealth < 0f)
                _currentHealth = 0f;
        }

        public void RestoreHealth(float healthValue)
        {
            _currentHealth += healthValue;
            if(_currentHealth > _maxHealth)
                _currentHealth = _maxHealth;
        }
        #endregion
    }
}