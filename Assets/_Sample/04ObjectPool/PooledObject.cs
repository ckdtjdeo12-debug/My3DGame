using UnityEngine;

namespace MySample
{
    /// <summary>
    /// 오브젝트 풀에 풀링되는 오브젝트에 부착되는 클래스
    /// </summary>
    public class PooledObject : MonoBehaviour
    {
        private ObjectPool pool;    //자신이 들어갈 오브젝트 풀
        public ObjectPool Pool
        {
            get { return pool; }
            set { pool = value; }
        }

        //사용후 풀로 되돌아가기
        public void Release()
        {
            pool.ReturnToPool(this);
        }
    }
}