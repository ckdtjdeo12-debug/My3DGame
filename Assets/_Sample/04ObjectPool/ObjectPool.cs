using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MySample
{
    /// <summary>
    /// 오브젝트 풀을 관리하는 클래스
    /// </summary>
    public class ObjectPool : MonoBehaviour
    {
        #region Variables
        private Stack<PooledObject> stack;

        //풀링되는 오브젝트 프리팹
        public PooledObject objectToPool;

        //풀링 갯수
        [SerializeField] private int initPoolSize = 10; //풀 초기 사이즈
        //private int maxPoolSize = 100;                  //풀 초대 사이즈
        #endregion

        #region Property
        public int InitPoolSize => initPoolSize;
        #endregion

        #region Unity Event Method
        private void Start()
        {
            //풀 초기화
            SetupPool();
        }
        #endregion

        #region Custom Method
        //풀 초기화
        private void SetupPool()
        {
            //프리팹 null 체크
            if (objectToPool == null)
            {
                return;
            }

            stack = new Stack<PooledObject>();

            PooledObject instance = null;
            for (int i = 0; i < initPoolSize; i++)
            {
                instance = Instantiate(objectToPool);
                instance.Pool = this;
                instance.gameObject.SetActive(false);
                stack.Push(instance);
            }
        }

        //풀에서 오브젝트 꺼내기
        public PooledObject GetPooledObject()
        {
            //프리팹 null 체크
            if (objectToPool == null)
            {
                return null;
            }

            //stack 잔고 체크
            if(stack.Count == 0)
            {
                PooledObject newInstance = Instantiate(objectToPool);
                newInstance.Pool = this;
                return newInstance;
            }

            PooledObject nextInstance = stack.Pop();
            nextInstance.gameObject.SetActive(true);
            return nextInstance;
        }

        //사용 후 풀에 다시 오브젝트 넣기
        public void ReturnToPool(PooledObject pooledObject)
        {
            //maxPoolSize 체크해서 킬

            pooledObject.gameObject.SetActive(false);
            stack.Push(pooledObject);
        }
        #endregion



    }
}