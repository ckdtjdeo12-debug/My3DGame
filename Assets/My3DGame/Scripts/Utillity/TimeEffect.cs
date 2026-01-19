using UnityEngine;
using System.Collections;

namespace My3DGame.Utillity
{
    /// <summary>
    /// 공격 이펙트를 관리하는 클래스
    /// </summary>
    public class TimeEffect : MonoBehaviour
    {
        #region Variables
        //참조
        protected Animation m_Animation;
        public Light staffLight;
        #endregion

        #region Unity Event Method
        private void Awake()
        {
            //참조
            m_Animation = GetComponent<Animation>();
            //이펙트 비활성화
            gameObject.SetActive(false);
        }
        #endregion

        #region Custom Method
        //이펙트 활성화
        public void Activate()
        {
            gameObject.SetActive(true);
            staffLight.enabled = true;

            if(m_Animation)
                m_Animation.Play();

            //이펙트 비활성화
            StartCoroutine(DisableAtEndofAnimation());
        }

        //이펙트 비활성화
        IEnumerator DisableAtEndofAnimation()
        {
            yield return new WaitForSeconds(m_Animation.clip.length);

            gameObject.SetActive(false);
            staffLight.enabled = false;
        }
        #endregion
    }
}