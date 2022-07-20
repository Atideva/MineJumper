using UnityEngine;
using UnityEngine.Pool;

namespace Gameplay
{
    public class Pool : MonoBehaviour
    {
        private Cube _prefab;
        private ObjectPool<Cube> _pool;
        private void Awake() => _pool = new ObjectPool<Cube>(Create, OnTake, OnRelease);

        public void Init(Cube obj) => _prefab = obj;
        public Cube Get() => _pool.Get();

        private Cube Create()
        {
            var obj = Instantiate(_prefab);
            obj.SetPool(_pool);
            return obj;
        }

        private static void OnTake(Cube obj)
        {
            obj.gameObject.SetActive(true);
        }

        private static void OnRelease(Cube obj)
        {
            obj.gameObject.SetActive(false);
        }
    }
}