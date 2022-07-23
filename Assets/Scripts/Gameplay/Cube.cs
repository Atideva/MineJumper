using UnityEngine;
using UnityEngine.Pool;

namespace Gameplay
{
    public class Cube : MonoBehaviour
    {
        [Header("Setup")]
        [SerializeField] private Transform model3D;
        [SerializeField] float baseModel3DSize = 1f;

        [Header("DEBUG")]
        [SerializeField] private int id;


        public int ID => id;
        private ObjectPool<Cube> _pool;

        public float GetTopY(Vector3 cubeSize)
        {
            return cubeSize.y  * baseModel3DSize;
        }

          float GetModelOffset(Vector3 cubeSize)
        {
            return (cubeSize.y / 2) * baseModel3DSize;
        }

        public void SetSize(Vector3 cubeSize)
        {
            if (model3D == null)
            {
                model3D = GetComponentInChildren<Transform>();
                Debug.LogWarning("You forgot to setup model3D for cube");
            }

            if (model3D == null)
            {
                Debug.LogError("There's no model3D for cube");
                return;
            }
            
            model3D.localScale = cubeSize;
            var offsetY = GetModelOffset(cubeSize);
            model3D.localPosition = new Vector3(0, offsetY, 0);
        }

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