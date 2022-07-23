using Gameplay;
using UnityEngine;

namespace Spawn
{
    public class SpawnID : MonoBehaviour
    {
        [Header("DEBUG")]
        [SerializeField] private int cubeId;

        public void Init(Spawner spawner)
        {
            cubeId = 0;
            spawner.OnCubeSpawned += OnCubeSpawn;
        }

        void OnCubeSpawn(Cube cube)
        {
            cube.SetID(cubeId++);
        }

 
    }
}
