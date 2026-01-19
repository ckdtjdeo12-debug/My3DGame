using UnityEngine;
using UnityEditor;
using My3DGame.GameData;
using System.Text;
using System.IO;
using UnityObject = UnityEngine.Object;

namespace My3DGame.Tool
{
    /// <summary>
    /// 데이터 툴의 공통 기능 구현
    /// 데이터 추가하기, 복사하기, 제거하기 그리기
    /// 데이터 이름 목록 그리기
    /// 유니티 소스 경로 가져오기, 데이터 이름 목록가져와서 enum 리스트 만들기
    /// </summary>
    public class EditorHelper
    {
        /// <summary>
        /// 유니티 오브젝트 소스 경로 가져오기 - Resources 하위 경로
        /// </summary>
        public static string GetPath(UnityObject source)
        {
            string path = string.Empty;
            //소스의 풀소스 가져오기 : "Assets/My3DGame/ResourcesData/Resources/EffectData/Effect_Cube.prefab"
            path = AssetDatabase.GetAssetPath(source);
            string[] path_node = path.Split('/');
            bool findResources = false;
            for (int i = 0; i < path_node.Length - 1; i++)
            {
                if(findResources == false)
                {
                    if(path_node[i] == "Resources")
                    {
                        findResources = true;
                        path = string.Empty;
                    }
                }
                else
                {
                    path += path_node[i] + "/";
                }
            }

            return path;
        }

        public static void CreateEnumStructure(string enumName, StringBuilder data)
        {            
            string templateFilePath = "Assets/My3DGame/Editor/EnumTemplate.txt";
            string enumFilePath = "Assets/My3DGame/Scripts/GameData/";

            string entittyTemplate = File.ReadAllText(templateFilePath);

            entittyTemplate = entittyTemplate.Replace("$DATA$", data.ToString());
            entittyTemplate = entittyTemplate.Replace("$ENUM$", enumName);

            //파일저장
            //파일 경로 확인
            if(Directory.Exists(enumFilePath) == false)
            {
                Directory.CreateDirectory(enumFilePath);
            }

            string filePath = enumFilePath + enumName + ".cs";
            if (File.Exists(filePath))
            {
                File.Delete(filePath);  //기존 파일 삭제
            }
            File.WriteAllText(filePath, entittyTemplate);
        }

        // 데이터 추가하기, 복사하기, 제거하기 그리기
        public static void EditToolTopLayer(BaseData data, ref int selection, 
            ref UnityObject source, int uiWidth)
        {
            EditorGUILayout.BeginHorizontal();
            {
                //추가하기 버튼 그리기
                if(GUILayout.Button("ADD", GUILayout.Width(uiWidth)))
                {
                    data.AddData("NewData");
                    selection = data.GetDataCount() - 1;
                    source = null;
                }
                //복사하기 버튼 그리기
                if (GUILayout.Button("COPY", GUILayout.Width(uiWidth)))
                {
                    data.CopyData(selection);
                    selection = data.GetDataCount() - 1;
                    source = null;
                }
                //제거하기 버튼 그리기 - 한개 있을때는 제거 안됨
                if (data.GetDataCount() > 1)
                {
                    if (GUILayout.Button("REMOVE", GUILayout.Width(uiWidth)))
                    {
                        source = null;
                        data.RemoveData(selection);
                    }
                }

                //selection 범위 체크
                if(selection >= data.GetDataCount())
                {
                    selection = data.GetDataCount() - 1;
                }
            }
            EditorGUILayout.EndHorizontal();
        }

        /// <summary>
        /// 데이터 이름 목록 그리기
        /// </summary>
        public static void EditorToolListLayer(BaseData data, ref int selection,
            ref UnityObject source, int uiWidth, ref Vector2 scrollPostion)
        {
            EditorGUILayout.BeginVertical(GUILayout.Width(uiWidth));
            {
                EditorGUILayout.Separator();        //공백 한줄 주기
                EditorGUILayout.BeginVertical("box");
                {
                    scrollPostion = EditorGUILayout.BeginScrollView(scrollPostion);
                    {
                        if (data.GetDataCount() > 0)
                        {
                            int lastSelection = selection;
                            selection = GUILayout.SelectionGrid(selection, 
                                data.GetNameList(true), 1);
                            if (lastSelection != selection)
                            {
                                source = null;
                            }
                        }
                    }
                    EditorGUILayout.EndScrollView();
                }
                EditorGUILayout.EndVertical();
            }
            EditorGUILayout.EndVertical();
        }
    }
}