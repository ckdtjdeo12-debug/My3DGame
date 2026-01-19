using UnityEngine;

namespace My3DGame
{
    /// <summary>
    /// 체력의 초기값 설정 - 데이터 컨네이너
    /// </summary>
    [CreateAssetMenu(fileName = "HealthConfigSO", menuName = "EntityConfig/Health Config")]
    public class HealthConfigSO : DescriptionBaseSO
    {
        [SerializeField] protected float _initialHealth;

        public float InitialHealth => _initialHealth;
    }
}