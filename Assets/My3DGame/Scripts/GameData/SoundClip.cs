using UnityEngine;
using System;
using System.Collections.Generic;

namespace My3DGame.GameData
{
    /// <summary>
    /// 사운드 클립(데이터) 리스트를 관리하는 클래스, json파일 저장,불러오기 용도
    /// </summary>
    [Serializable]
    public class SoundClipData
    {
        public List<SoundClip> clips;
    }

    /// <summary>
    /// 오디오 소스를 관리하는 클래스
    /// 오디오 소스 데이터 속성 정의
    /// 오디오 클립 가져오기 기능, 클립 사전 로딩 기능, 클립 로딩 해제 기능
    /// </summary>
    [Serializable]
    public class SoundClip
    {
        #region Variables
        public int id;                          //사운드 데이터 리스트의 인덱스
        public string name;                     //사운드 데이터 이름
        public SoundType soundType;           //사운드 타입

        public string clipPath;               //사운드 파일(소스) 저장 경로(Resources 폴더 하위 경로)
        public string clipFileName;           //사운드 파일(소스) 이름

        //오디오 소스 설정값 속성
        public bool isLoop;
        public float volume;
        public float pitch;
        public float spatialBlend;
        public float minDistance;
        public float maxDistance;

        private AudioClip audioClip = null;     //오디오 클립
        #endregion

        #region Contructor
        public SoundClip()
        {
            //멤버 초기화
            isLoop = false;
            volume = 1f;
            pitch = 1f;
            spatialBlend = 0f;
            minDistance = 1f;
            maxDistance = 500f;
        }
        #endregion

        #region Custom Method
        //프리팹 사전 로딩 기능
        public void PreLoad()
        {
            var clipFullPath = clipPath + clipFileName;
            if (clipFullPath != string.Empty && audioClip == null)
            {
                audioClip = ResourcesManager.Load<AudioClip>(clipFullPath);
            }
        }

        //프리팹 로딩 해제 기능
        public void ReleaseClip()
        {
            if (audioClip != null)
            {
                audioClip = null;
            }
        }

        //오디오 클립 가져오기
        public AudioClip GetAudioClip()
        {
            //audioClip null 체크
            if (audioClip == null)
            {
                PreLoad();
            }

            return audioClip;
        }
        #endregion
    }
}