using System;
using Gameplay;
using UnityEngine;


// ReSharper disable Unity.InefficientPropertyAccess
namespace Spawn
{
    public class Spawner : MonoBehaviour
    {
        
        public event Action<Cube> OnCubeSpawned = delegate { };
        private SpawnPool _spawnPool;
        private Vector3 _baseCubeSize;

        public void Init( SpawnPool spawnPool, Vector3 baseCubeSize)
        {
            _baseCubeSize = baseCubeSize;
            _spawnPool = spawnPool;
        }
 
        public void Spawn(Vector3 pos)
        {
            var cube = _spawnPool.Get();
            cube.transform.position = pos;
            cube.SetSize(_baseCubeSize);
            
            OnCubeSpawned(cube);
        }
    }
}