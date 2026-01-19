using UnityEngine;
using My3DGame.GameData;

namespace My3DGame
{
    /// <summary>
    /// 이펙트 플레이 하는 이벤트 채널 
    /// </summary>
    [CreateAssetMenu(fileName = "EffectDataChannelSO", menuName = "Events/Effect Data Channel")]
    public class EffectDataChannelSO : DescriptionBaseSO
    {
        //델리게이트 변수 선언
        public EffectOneShotAction OnEffectOnShotRaised;

        //델리게이트를 브로드캐스팅 함수 구현
        public GameObject RaiseEvent(EffectList effectList, Vector3 position)
        {
            GameObject effectGo = null;

            if(OnEffectOnShotRaised != null)
            {
                effectGo = OnEffectOnShotRaised.Invoke(effectList, position);
            }

            return effectGo;
        }
    }

    //델리게이트 정의
    public delegate GameObject EffectOneShotAction(EffectList effectList, Vector3 position);
}
