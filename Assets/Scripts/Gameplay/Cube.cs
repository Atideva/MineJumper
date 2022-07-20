using UnityEngine;
using UnityEngine.Pool;

namespace Gameplay
{
    public class Cube : MonoBehaviour
    {
        [SerializeField] private int id;
        public int ID => id;
        private ObjectPool<Cube> _pool;

        public void SetID(int value) => id = value;
        public void SetPool(ObjectPool<Cube> pool) => _pool = pool;

        public void ReturnToPool()
        {
            if (_pool != null)
            {
                _pool.Release(this);
            }
            else
            {
                Debug.LogError("Pool has not been set!");
                Destroy(gameObject);
            }
        }
    }
}