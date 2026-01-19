using System;
using System.Collections.Generic;
using UnityEngine;

namespace My3DGame.GameData
{
    /// <summary>
    /// 이펙트 클립(데이터) 리스트를 관리하는 클래스, json파일 저장,불러오기 용도
    /// </summary>
    [SerializeField]
    public class EffectClipData
    {
        public List<EffectClip> clips;
    }

    /// <summary>
    /// 이펙트를 관리하는 클래스
    /// 이펙트 데이터 속성 정의
    /// 이펙트 프리팹 인스턴스 기능, 프리팹 사전 로딩 기능, 프리팹 로딩 해제 기능
    /// </summary>
    [Serializable]
    public class EffectClip
    {
        #region Variables
        public int id;                          //이펙트 데이터 리스트의 인덱스
        public string name;                     //이펙트 데이터 이름
        public EffectType effectType;           //이펙트 타입

        public string effectPath;               //이펙트 파일(소스) 저장 경로(Resources 폴더 하위 경로)
        public string effectFileName;           //이펙트 파일(소스) 이름

        private GameObject effectPrefab = null; //이펙트 프리팹
        #endregion

        #region Contructor
        public EffectClip() { }
        #endregion

        #region Custom Method
        //프리팹 사전 로딩 기능
        public void PreLoad()
        {
            var effectFullPath = effectPath + effectFileName;
            if(effectFullPath != string.Empty && effectPrefab == null)
            {
                effectPrefab = ResourcesManager.Load<GameObject>(effectFullPath);
            }
        }

        //프리팹 로딩 해제 기능
        public void ReleaseEffect()
        {
            if (effectPrefab != null)
            {
                effectPrefab = null;
            }
        }

        //이펙트 프리팹 인스턴스 기능, 매개변수로 생성위치 받아 인스턴스 생성
        public GameObject Instantiate(Vector3 position)
        {
            //effectPrefab null 체크
            if (effectPrefab == null)
            {
                PreLoad();
            }

            if (effectPrefab != null)
            {
                GameObject effectGo = GameObject.Instantiate(effectPrefab, position,
                    Quaternion.identity);
                return effectGo;
            }

            return null;
        }
        #endregion
    }
}