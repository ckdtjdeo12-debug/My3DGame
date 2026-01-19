using UnityEngine;

namespace My3DGame.Utillity
{
    public class PersistantSingleton<T> : Singleton<T> where T : Singleton<T>
    {
        protected override void Awake()
        {
            base.Awake();

            DontDestroyOnLoad(this.gameObject);
        }
    }
}
