using UnityEngine;
using My3DGame.GameData;

namespace My3DGame
{
    public class EffectManager : MonoBehaviour
    {
        #region Variables
        private Transform effectRoot = null;    //생성하는 이펙트 게임오브젝트의 부모 오브젝트

        [Header ("Listening on")]
        [SerializeField] protected EffectDataChannelSO _EffectOneShot = default;
        #endregion

        #region Unity Event Method
        private void Start()
        {
            //effectRoot 생성
            if (effectRoot == null)
            {
                effectRoot = new GameObject("Effect Root").transform;
                effectRoot.SetParent(this.transform);
            }

            //이벤트 체널 이벤트 함수에 등록
            _EffectOneShot.OnEffectOnShotRaised += EffectOneShot;

            //테스트
            //EffectOneShot((int)EffectList.EffectCube, new Vector3(-120, 1, 70));
            //EffectOneShot((int)EffectList.EffectSphere, new Vector3(-122, 1, 70));
        }
        #endregion

        #region Custom Method
        //이펙트 데이터 있는 이펙트를 불러와서 이펙트 생성하기
        public GameObject EffectOneShot(EffectList effectList, Vector3 position)
        {
            EffectClip clip = DataManager.GetEffectData().GetClip((int)effectList);
            GameObject effectInstance = clip.Instantiate(position);
            effectInstance.SetActive(true);
            return effectInstance;
        }
        #endregion

    }
}