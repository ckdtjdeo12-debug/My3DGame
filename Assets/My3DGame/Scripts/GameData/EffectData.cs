using UnityEngine;
using System.Collections.Generic;
using System.IO;

namespace My3DGame.GameData
{
    /// <summary>
    /// 이펙트 클립(데이터)들(리스트)을 관리하는 클래스
    /// BaseData를 상속받는다 (데이터 추가하기, 제거하기, 복사하기 기능 구현)
    /// 데이터 리스트 파일(xml, json)에 저장하기, 불러오기 
    /// </summary>
    public class EffectData : BaseData
    {
        #region Variables
        public List<EffectClip> clips;      //이펙트 클립(데이터) 리스트

        //파일 저장하기 로드하기
        private const string dataPath = "Data/effectData";      //Resource폴더의 하위 경로(Resources.Load<T>(path))

        //private const string xmlFileName = "effectData.xml";
        private const string jsonFileName = "effectData.json";
        #endregion

        #region Constructor
        public EffectData() { }
        #endregion

        #region Custom Method
        //데이터 리스트 파일(xml, json)에 저장하기
        public void SaveData()
        {
            //xml
            /*var xmlFullPath = Application.dataPath + dataDirectory + xmlFileName;
            Debug.Log($"xmlFullPath:{xmlFullPath}");

            using (XmlTextWriter xmlWriter = new XmlTextWriter(xmlFullPath, System.Text.Encoding.Unicode))
            {
                var xs = new XmlSerializer(typeof(List<EffectClip>));
                int length = GetDataCount();
                for (int i = 0; i < length; i++)
                {
                    clips[i].id = i;
                    clips[i].name = this.names[i];
                }
                xs.Serialize(xmlWriter, clips);
            }*/

            //json
            var jsonFullPath = Application.dataPath + dataDirectory + jsonFileName;
            Debug.Log($"jsonFullPath:{jsonFullPath}");

            int length = GetDataCount();
            for (int i = 0; i < length; i++)
            {
                clips[i].id = i;
                clips[i].name = this.names[i];
            }
            EffectClipData clipData = new EffectClipData();
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
                this.AddData("NewEffect");
                return;
            }

            //xml
            /*using (XmlTextReader reader = new XmlTextReader(new StringReader(asset.text)))
            {
                var xs = new XmlSerializer(typeof(List<EffectClip>));
                clips = (List<EffectClip>)xs.Deserialize(reader);

                //이름 목록 셋팅
                this.names = new List<string>();
                for (int i = 0; i < clips.Count; i++)
                {
                    this.names.Add(clips[i].name);
                }
            }*/

            //json
            EffectClipData clipData = JsonUtility.FromJson<EffectClipData>(asset.text);
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
                clips = new List<EffectClip>() { new EffectClip() }; //리스트 객체 생성 후 멤버 추가
            }
            else
            {
                this.names.Add(newName);        //새로운 이름을 이름 목록에 추가
                clips.Add(new EffectClip());    //이펙트 클립 생성하여 클립 리스트에 추가
            }   

            return GetDataCount();
        }

        //매개변수로 인덱스를 받아서 지정된 데이터를 데이터 리스트에서 제거하기
        public override void RemoveData(int index)
        {
            this.names.Remove(this.names[index]);
            if(this.names.Count == 0)
            {
                this.names = null;
            }
            this.clips.Remove(this.clips[index]);
            if(this.clips.Count == 0)
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
        private EffectClip GetCopy(int index)
        {
            //index 체크
            if (index < 0 || index >= this.clips.Count)
            {
                return null;
            }

            EffectClip originClip = this.clips[index];
            EffectClip newClip = new EffectClip();
            newClip.name = originClip.name;
            newClip.effectType = originClip.effectType;
            newClip.effectPath = originClip.effectPath;
            newClip.effectFileName = originClip.effectFileName;

            return newClip;
        }

        //매개변수로 지정된 클립을 가져온다
        public EffectClip GetClip(int index)
        {
            //index 체크
            if (index < 0 || index >= this.clips.Count)
            {
                return null;
            }

            clips[index].PreLoad();         //이펙트 클립의 프리팹 읽어오기
            return this.clips[index];
        }

        //이펙트 데이터 초기화하기
        public void ClearData()
        {
            //클립의 프리팹 로딩 해제
            foreach (var clip in this.clips)
            {
                clip.ReleaseEffect();
            }
            //리스트 초기화
            this.clips = null;
            this.names = null;
        }
        #endregion
    }
}