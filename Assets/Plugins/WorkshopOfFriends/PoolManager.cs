using System.Collections.Generic;
using UnityEngine;

namespace Wof
{
    public class PoolManager : MonoBehaviour
    {
        public static PoolManager Instance;

        private Dictionary<GameObject, Pool> pools = new Dictionary<GameObject, Pool>();

        private class Pool
        {
            public GameObject prefab;
            public Stack<GameObject> objects = new Stack<GameObject>();

            public Pool(GameObject prefab)
            {
                this.prefab = prefab;
            }
        }

        public class PoolMember : MonoBehaviour
        {
            public GameObject prefab;
        }

        private void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
                DontDestroyOnLoad(gameObject);
            }
            else
            {
                Destroy(gameObject);
            }
        }
        
        public T GetObject<T>(T prefab) where T : Component => GetObject<T>(prefab.gameObject);
        public T GetObject<T>(GameObject prefab) where T : Component
        {
            if (!pools.ContainsKey(prefab))
                pools[prefab] = new Pool(prefab);

            Pool pool = pools[prefab];
            GameObject obj;

            if (pool.objects.Count > 0)
            {
                obj = pool.objects.Pop();
                obj.SetActive(true);
            }
            else
            {
                obj = Instantiate(prefab);
                obj.AddComponent<PoolMember>().prefab = prefab;
            }

            T component = obj.GetComponent<T>();
            if (component == null)
                Debug.LogError($"Prefab {prefab.name} does not have component of type {typeof(T)}");

            return component;
        }

        public void ReturnObject(GameObject obj)
        {
            PoolMember member = obj.GetComponent<PoolMember>();
            if (member == null || member.prefab == null || !pools.ContainsKey(member.prefab))
            {
                Debug.LogError("Object not managed by pool or pool missing");
                Destroy(obj);
                return;
            }

            obj.SetActive(false);
            pools[member.prefab].objects.Push(obj);
        }
    }
}
