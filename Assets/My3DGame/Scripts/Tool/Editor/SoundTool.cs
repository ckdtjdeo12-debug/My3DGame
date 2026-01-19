using UnityEngine;
using UnityEditor;
using My3DGame.GameData;
using System.Text;
using UnityObject = UnityEngine.Object;

namespace My3DGame.Tool
{
    /// <summary>
    /// 사운드 데이터를 관리하는 툴
    /// </summary>
    public class SoundTool : EditorWindow
    {
        #region Variables
        //윈도우 UI
        public int uiWidthLarge = 300;
        public int uiWidthMiddle = 200;

        private int selection = 0;      //현재 선택된 목록 인덱스
        private Vector2 SP1 = Vector2.zero;
        private Vector2 SP2 = Vector2.zero;

        //사운드 데이터
        private static SoundData soundData;       //사운드 데이터
        private AudioClip soundSource = null;       //오디오 클립
        #endregion

        //Sound Tool 메뉴를 눌렀을때 실행 : 이펙트 툴 창을 연다
        [MenuItem("Tools/Sound Tool")]
        static void Init()
        {
            //이펙트 데이터 가져오기
            soundData = ScriptableObject.CreateInstance<SoundData>();
            soundData.LoadData();

            //윈도우 창 열기
            SoundTool window = GetWindow<SoundTool>(false, "Sound Tool");
            window.Show();
        }

        //윈도우 창 편집 
        private void OnGUI()
        {
            //데이터 체크
            if (soundData == null)
            {
                return;
            }

            //GUI 그리기
            EditorGUILayout.BeginVertical();
            {
                UnityObject source = soundSource;
                //데이터 추가,복사,제거 버튼 그리기
                EditorHelper.EditToolTopLayer(soundData, ref selection, ref source,
                    this.uiWidthMiddle);
                soundSource = (AudioClip)source;

                EditorGUILayout.BeginHorizontal();
                {
                    //이름 목록 그리기
                    EditorHelper.EditorToolListLayer(soundData, ref selection, ref source,
                        this.uiWidthLarge, ref SP1);
                    soundSource = (AudioClip)source;

                    //선택된 사운드 데이터의 속성 설정 부분
                    EditorGUILayout.BeginVertical();
                    {
                        SP2 = EditorGUILayout.BeginScrollView(SP2);
                        {
                            //이펙트 데이터 체크
                            if (soundData.GetDataCount() > 0)
                            {
                                EditorGUILayout.BeginVertical();
                                {
                                    EditorGUILayout.Separator();
                                    //인덱스 그리기
                                    EditorGUILayout.LabelField("ID", selection.ToString(), GUILayout.Width(uiWidthLarge));
                                    //이름 입력 받기
                                    soundData.names[selection] = EditorGUILayout.TextField("이름", soundData.names[selection],
                                        GUILayout.Width(uiWidthLarge * 1.5f));
                                    //이펙트 타입 설정하기
                                    soundData.clips[selection].soundType = (SoundType)EditorGUILayout.EnumPopup("사운드 타입",
                                        soundData.clips[selection].soundType, GUILayout.Width(uiWidthLarge));

                                    EditorGUILayout.Separator();
                                    //이펙트 프리팹 가져와서 경로와 이름 얻어오기 
                                    if (soundSource == null && soundData.clips[selection].clipFileName != string.Empty)
                                    {
                                        soundData.clips[selection].PreLoad();
                                        soundSource = ResourcesManager.Load<AudioClip>(soundData.clips[selection].clipPath
                                            + soundData.clips[selection].clipFileName);
                                    }
                                    //오브젝트 입력 받기
                                    soundSource = (AudioClip)EditorGUILayout.ObjectField("오디오 클립", this.soundSource,
                                        typeof(AudioClip), false, GUILayout.Width(uiWidthLarge * 1.5f));
                                    //오브젝트로부터 저장 경로와 파일 이름 가져오기
                                    if (soundSource != null)
                                    {
                                        soundData.clips[selection].clipPath = EditorHelper.GetPath(soundSource);
                                        soundData.clips[selection].clipFileName = soundSource.name;
                                    }
                                    else
                                    {
                                        soundData.clips[selection].clipPath = string.Empty;
                                        soundData.clips[selection].clipFileName = string.Empty;
                                    }

                                    EditorGUILayout.Separator();
                                    //오디오 소스 설정값 받아오기
                                    soundData.clips[selection].isLoop = EditorGUILayout.Toggle("Loop Clip",
                                        soundData.clips[selection].isLoop, GUILayout.Width(uiWidthLarge));

                                    soundData.clips[selection].volume = EditorGUILayout.Slider("Volume",
                                        soundData.clips[selection].volume, 0f, 1f, GUILayout.Width(uiWidthLarge));

                                    soundData.clips[selection].pitch = EditorGUILayout.Slider("Pitch",
                                        soundData.clips[selection].pitch, 0.1f, 2f, GUILayout.Width(uiWidthLarge));

                                    soundData.clips[selection].spatialBlend = EditorGUILayout.Slider("Spatial Blend",
                                        soundData.clips[selection].spatialBlend, 0f, 1f, GUILayout.Width(uiWidthLarge));

                                    soundData.clips[selection].minDistance = EditorGUILayout.FloatField("Min Distance",
                                        soundData.clips[selection].minDistance, GUILayout.Width(uiWidthLarge));

                                    soundData.clips[selection].maxDistance = EditorGUILayout.FloatField("Max Distance",
                                        soundData.clips[selection].maxDistance, GUILayout.Width(uiWidthLarge));
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
                if (GUILayout.Button("RELOAD SETTINGS"))
                {
                    //이펙트 데이터 가져오기
                    soundData = ScriptableObject.CreateInstance<SoundData>();
                    soundData.LoadData();

                    //초기화
                    selection = 0;
                    soundSource = null;
                }

                //데이터 저장하기
                if (GUILayout.Button("SAVE"))
                {
                    soundData.SaveData();
                    //enum 리스트 만들기
                    CreateEnumStructure();
                    //새로 만든 enum, savedata 에셋 프로젝트에 강제 업데이트
                    AssetDatabase.Refresh(ImportAssetOptions.ForceUpdate);
                }
            }
            EditorGUILayout.EndHorizontal();
        }

        //enum 파일 만들기
        public void CreateEnumStructure()
        {
            string enumName = "SoundList";
            StringBuilder builder = new StringBuilder();
            builder.AppendLine();
            for (int i = 0; i < soundData.names.Count; i++)
            {
                if (soundData.names[i] != string.Empty)
                {
                    builder.AppendLine("        " + soundData.names[i] + " = " + i + ",");
                }
            }
            EditorHelper.CreateEnumStructure(enumName, builder);
        }
    }
}
