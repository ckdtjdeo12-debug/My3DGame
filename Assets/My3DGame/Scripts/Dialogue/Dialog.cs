using System;

namespace My3DGame
{
    /// <summary>
    /// 대화 속성을 관리하는 직렬화된 클래스
    /// </summary>
    [Serializable]
    public class Dialog
    {
        public int number;
        public int character;
        public string name;
        public string sentence;
        public int next;
    }
}