using UnityEngine;
using System.Collections.Generic;
using System.IO;

namespace My3DGame.GameData
{
    /// <summary>
    /// 사운드 클립(데이터)들(리스트)을 관리하는 클래스
    /// BaseData를 상속받는다 (데이터 추가하기, 제거하기, 복사하기 기능 구현)
    /// 데이터 리스트 파일(xml, json)에 저장하기, 불러오기 
    /// </summary>
    public class SoundData : BaseData
    {
        #region Variables
        public List<SoundClip> clips;      //사운드 클립(데이터) 리스트

        //파일 저장하기 로드하기
        private const string dataPath = "Data/soundData";      //Resource폴더의 하위 경로(Resources.Load<T>(path))
        
        private const string jsonFileName = "soundData.json";
        #endregion

        #region Constructor
        public SoundData() { }
        #endregion

        #region Custom Method
        //데이터 리스트 파일(xml, json)에 저장하기
        public void SaveData()
        {
            //json
            var jsonFullPath = Application.dataPath + dataDirectory + jsonFileName;
            Debug.Log($"jsonFullPath:{jsonFullPath}");

            int length = GetDataCount();
            for (int i = 0; i < length; i++)
            {
                clips[i].id = i;
                clips[i].name = this.names[i];
            }
            SoundClipData clipData = new SoundClipData();
            clipData.clips = clips;

            string jsonOutput = JsonUtility.ToJson(clipData, true);
            File.WriteAllText(jsonFullPath, jsonOutput);
        }

        //파일(xml, json)에서 데이터 리스트 불러오기
        public void LoadData()
        {
            //데이터 어셋 읽기
            TextAsset asset = ResourcesManager.Load<TextAsset>(dataPath);
            //파일체크
            if (asset == null || asset.text == null)
            {
                this.AddData("NewSound");
                return;
            }

            //json
            SoundClipData clipData = JsonUtility.FromJson<SoundClipData>(asset.text);
            clips = clipData.clips;

            //이름 목록 셋팅
            this.names = new List<string>();
            for (int i = 0; i < clips.Count; i++)
            {
                this.names.Add(clips[i].name);
            }
        }

        //매개변수로 새로운 데이터 이름을 받아서 데이터 리스트에 추가하기
        public override int AddData(string newName)
        {
            //this.names null 체크 (데이터가 아무것도 없을때)
            if (this.names == null)
            {
                this.names = new List<string>() { newName };         //리스트 객체 생성 후 멤버 추가
                clips = new List<SoundClip>() { new SoundClip() }; //리스트 객체 생성 후 멤버 추가
            }
            else
            {
                this.names.Add(newName);        //새로운 이름을 이름 목록에 추가
                clips.Add(new SoundClip());    //사운드 클립 생성하여 클립 리스트에 추가
            }

            return GetDataCount();
        }

        //매개변수로 인덱스를 받아서 지정된 데이터를 데이터 리스트에서 제거하기
        public override void RemoveData(int index)
        {
            this.names.Remove(this.names[index]);
            if (this.names.Count == 0)
            {
                this.names = null;
            }
            this.clips.Remove(this.clips[index]);
            if (this.clips.Count == 0)
            {
                this.clips = null;
            }
        }

        //매개변수로 인덱스를 받아서 지정된 데이터를 복사해서 추가하기
        public override void CopyData(int index)
        {
            this.names.Add(this.names[index]);      //기존 멤버의 이름를 가져와서 새로 추가

            //기존 멤버의 클립을 복사해 와서 추가한다
            this.clips.Add(GetCopy(index));
        }

        //매개변수로 지정된 클립의 복사본 만들어 반환하기
        private SoundClip GetCopy(int index)
        {
            //index 체크
            if (index < 0 || index >= this.clips.Count)
            {
                return null;
            }

            SoundClip originClip = this.clips[index];
            SoundClip newClip = new SoundClip();
            newClip.name = originClip.name;
            newClip.soundType = originClip.soundType;
            newClip.clipPath = originClip.clipPath;
            newClip.clipFileName = originClip.clipFileName;

            //오디오 소스 설정값 속성
            newClip.isLoop = originClip.isLoop;
            newClip.volume = originClip.volume;
            newClip.pitch = originClip.pitch;
            newClip.spatialBlend = originClip.spatialBlend;
            newClip.minDistance = originClip.minDistance;
            newClip.maxDistance = originClip.maxDistance;

            return newClip;
        }

        //매개변수로 지정된 클립을 가져온다
        public SoundClip GetClip(int index)
        {
            //index 체크
            if (index < 0 || index >= this.clips.Count)
            {
                return null;
            }

            clips[index].PreLoad();         //사운드 클립의 프리팹 읽어오기
            return this.clips[index];
        }

        //사운드 데이터 초기화하기
        public void ClearData()
        {
            //클립의 프리팹 로딩 해제
            foreach (var clip in this.clips)
            {
                clip.ReleaseClip();
            }
            //리스트 초기화
            this.clips = null;
            this.names = null;
        }
        #endregion
    }
}