using System;
using UnityEngine;

namespace Gameplay
{
    public class Spawner : MonoBehaviour
    {
        
        public event Action<Cube> OnCubeSpawned = delegate { };
        private Pool _pool;
        public void Init( Pool pool)
        {
            _pool = pool;
        }
 
        public void Spawn(Vector3 pos)
        {
            var cube = _pool.Get();
            cube.transform.position = pos;
            OnCubeSpawned(cube);
        }
    }
}