using UnityEngine;

namespace My3DGame.Utillity
{
    /// <summary>
    /// 지정된 오브젝트에 부착되어 계속 따라간다
    /// </summary>
    public class FixedUpdateFollow : MonoBehaviour
    {
        #region Variables
        public Transform toFollow;  //지정된 오브젝트
        #endregion

        private void FixedUpdate()
        {
            transform.position = toFollow.position;
            transform.rotation = toFollow.rotation;
        }
    }
}