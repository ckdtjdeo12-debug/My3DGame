using UnityEngine;
using System.Collections.Generic;

namespace My3DGame.GameData
{
    /// <summary>
    /// Data를 관리하는 클래스들의 기본(부모) 클래스
    /// 공통 속성: (데이터)이름 리스트
    /// 공통 기능: 데이터 리스트 갯수 가져오기, 데이터 이름 목록 가져오기
    /// 공통 기능: 데이터 추가하기, 제거하기, 복사하기
    /// </summary>
    public class BaseData : ScriptableObject
    {
        #region Variables
        public List<string> names;              //이름 리스트

        //데이터가 저장되어 있는 경로
        protected const string dataDirectory = "/My3DGame/ResourcesData/Resources/Data/";
        #endregion

        #region Constructor
        public BaseData() { }
        #endregion

        #region Custom Method
        //데이터 리스트 갯수 가져오기
        public int GetDataCount()
        {
            //names null 체크
            if (names == null)
                return 0;

            return names.Count;
        }

        //(툴에 있는) 데이터 이름 목록 가져오기
        public string[] GetNameList(bool showID, string filterWord = "")
        {
            int length = GetDataCount();

            string[] nameList = new string[length];

            for (int i = 0; i < length; i++)
            {
                //필터링: 특정 단어가 포함되면 표시한다 (<-> 표시하지 않는다)
                if(filterWord != "")
                {
                    if (names[i].ToLower().Contains(filterWord.ToLower()) == false)
                    {
                        continue;
                    }
                }

                //showID true이면 이름 앞에 인덱스를 표시한다
                if (showID)
                {
                    nameList[i] = i.ToString() + " : " + names[i];  
                }
                else
                {
                    nameList[i] = names[i];
                }
            }

            return nameList;
        }

        //매개변수로 새로운 데이터 이름을 받아서 데이터 리스트에 추가하기
        public virtual int AddData(string newName)
        {

            //추가한 후의 데이터 갯수 반환하기
            return GetDataCount();
        }

        //매개변수로 인덱스를 받아서 지정된 데이터를 데이터 리스트에서 제거하기
        public virtual void RemoveData(int index)
        {

        }

        //매개변수로 인덱스를 받아서 지정된 데이터를 복사해서 추가하기
        public virtual void CopyData(int index)
        {

        }
        #endregion
    }
}