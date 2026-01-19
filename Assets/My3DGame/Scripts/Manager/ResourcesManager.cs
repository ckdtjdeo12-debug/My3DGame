using Unity.VisualScripting;
using UnityEngine;
using UnityObject = UnityEngine.Object;

namespace My3DGame
{
    /// <summary>
    /// 리소스를 관리하는 클래스
    /// 리소스 로드, 리소스로드한 오브젝트를 게임오브젝트로 Instantiate
    /// </summary>
    public class ResourcesManager : MonoBehaviour
    {
        /// <summary>
        /// 매개 변수로 받은 경로에 있는 에쎗을 UnityObject로 가져오기
        /// </summary>
        public static T Load<T>(string path) where T : UnityObject
        {
            return Resources.Load<T>(path);
        }

        /// <summary>
        /// 매개 변수로 받은 경로에 있는 에쎗을 UnityObject로 가져와서 하이라키창에 올리기(Instantiate)
        /// </summary>
        public static GameObject LoadAndInstantiate(string path)
        {
            UnityObject source = Load<UnityObject>(path);
            //source 체크
            if (source == null)
            {
                return null;
            }

            return GameObject.Instantiate(source) as GameObject;
        }

    }
}