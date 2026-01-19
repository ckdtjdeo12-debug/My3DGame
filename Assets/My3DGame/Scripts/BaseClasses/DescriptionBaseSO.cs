using UnityEngine;

namespace My3DGame
{
    /// <summary>
    /// 모든 스크립터블 오브젝트의 부모 클래스
    /// 스크립터블 오브젝트 설명 텍스트 속성을 가진다
    /// </summary>
    public class DescriptionBaseSO : ScriptableObject
    {
        [TextArea] public string description;
    }
}