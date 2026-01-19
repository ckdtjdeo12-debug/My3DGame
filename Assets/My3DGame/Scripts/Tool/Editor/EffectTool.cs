using UnityEngine;
using UnityEditor;
using My3DGame.GameData;
using System.Text;
using UnityObject = UnityEngine.Object;

namespace My3DGame.Tool
{
    /// <summary>
    /// 이펙트 데이터를 관리하는 툴
    /// </summary>
    public class EffectTool : EditorWindow
    {
        #region Variables
        //윈도우 UI
        public int uiWidthLarge = 300;
        public int uiWidthMiddle = 200;

        private int selection = 0;      //현재 선택된 목록 인덱스
        private Vector2 SP1 = Vector2.zero;
        private Vector2 SP2 = Vector2.zero;

        //이펙트 데이터
        private static EffectData effectData;       //이펙트 데이터
        private GameObject effectSource = null;
        #endregion

        //Effect Tool 메뉴를 눌렀을때 실행 : 이펙트 툴 창을 연다
        [MenuItem("Tools/Effect Tool")]
        static void Init()
        {
            //이펙트 데이터 가져오기
            effectData = ScriptableObject.CreateInstance<EffectData>();
            effectData.LoadData();

            //윈도우 창 열기
            EffectTool window = GetWindow<EffectTool>(false, "Effect Tool");
            window.Show();
        }

        //윈도우 창 편집 
        private void OnGUI()
        {
            //데이터 체크
            if (effectData == null)
            {
                return;
            }

            //GUI 그리기
            EditorGUILayout.BeginVertical();
            {
                UnityObject source = effectSource;
                //데이터 추가,복사,제거 버튼 그리기
                EditorHelper.EditToolTopLayer(effectData, ref selection, ref source,
                    this.uiWidthMiddle);
                effectSource = (GameObject)source;

                EditorGUILayout.BeginHorizontal();
                {
                    //이름 목록 그리기
                    EditorHelper.EditorToolListLayer(effectData, ref selection, ref source,
                        this.uiWidthLarge, ref SP1);
                    effectSource = (GameObject)source;

                    //선택된 이펙트 데이터의 속성 설정 부분
                    EditorGUILayout.BeginVertical();
                    {
                        SP2 = EditorGUILayout.BeginScrollView(SP2);
                        {
                            //이펙트 데이터 체크
                            if (effectData.GetDataCount() > 0)
                            {
                                EditorGUILayout.BeginVertical();
                                {
                                    EditorGUILayout.Separator();
                                    //인덱스 그리기
                                    EditorGUILayout.LabelField("ID", selection.ToString(), GUILayout.Width(uiWidthLarge));
                                    //이름 입력 받기
                                    effectData.names[selection] = EditorGUILayout.TextField("이름", effectData.names[selection],
                                        GUILayout.Width(uiWidthLarge * 1.5f));
                                    //이펙트 타입 설정하기
                                    effectData.clips[selection].effectType = (EffectType)EditorGUILayout.EnumPopup("이펙트 타입",
                                        effectData.clips[selection].effectType, GUILayout.Width(uiWidthLarge));
                                                                        
                                    EditorGUILayout.Separator();
                                    //이펙트 프리팹 가져와서 경로와 이름 얻어오기 
                                    if(effectSource == null && effectData.clips[selection].effectFileName != string.Empty)
                                    {
                                        effectData.clips[selection].PreLoad();
                                        effectSource = ResourcesManager.Load<GameObject>(effectData.clips[selection].effectPath
                                            + effectData.clips[selection].effectFileName);
                                    }
                                    //오브젝트 입력 받기
                                    effectSource = (GameObject)EditorGUILayout.ObjectField("이펙트", this.effectSource,
                                        typeof(GameObject), false, GUILayout.Width(uiWidthLarge * 1.5f));
                                    //오브젝트로부터 저장 경로와 파일 이름 가져오기
                                    if (effectSource != null)
                                    {
                                        effectData.clips[selection].effectPath = EditorHelper.GetPath(effectSource);
                                        effectData.clips[selection].effectFileName = effectSource.name;
                                    }
                                    else
                                    {
                                        effectData.clips[selection].effectPath = string.Empty;
                                        effectData.clips[selection].effectFileName = string.Empty;
                                    }
                                }
                                EditorGUILayout.EndVertical();
                            }
                        }
                        EditorGUILayout.EndScrollView();
                    }
                    EditorGUILayout.EndVertical();
                }
                EditorGUILayout.EndHorizontal();
            }
            EditorGUILayout.EndVertical();

            EditorGUILayout.Separator();
            //하단
            EditorGUILayout.BeginHorizontal();
            {
                //다시 불러오기
                if(GUILayout.Button("RELOAD SETTINGS"))
                {
                    //이펙트 데이터 가져오기
                    effectData = ScriptableObject.CreateInstance<EffectData>();
                    effectData.LoadData();

                    //초기화
                    selection = 0;
                    effectSource = null;
                }

                //데이터 저장하기
                if (GUILayout.Button("SAVE"))
                {
                    effectData.SaveData();
                    //enum 리스트 만들기
                    CreateEnumStructure();
                    //새로 만든 enum, savedata 에셋 프로젝트에 강제 업데이트
                    AssetDatabase.Refresh(ImportAssetOptions.ForceUpdate);
                }
            }
            EditorGUILayout.EndHorizontal ();
        }

        //enum 파일 만들기
        public void CreateEnumStructure()
        {
            string enumName = "EffectList";
            StringBuilder builder = new StringBuilder();
            builder.AppendLine();
            for (int i = 0; i < effectData.names.Count; i++)
            {
                if (effectData.names[i] != string.Empty)
                {
                    builder.AppendLine("        " + effectData.names[i] + " = " + i + ",");
                }
            }
            EditorHelper.CreateEnumStructure(enumName, builder);
        }
    }
}