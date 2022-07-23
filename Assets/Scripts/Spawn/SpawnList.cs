using System.Collections.Generic;
using Gameplay;
using UnityEngine;

namespace Spawn
{
    public class SpawnList : MonoBehaviour
    {
        [Header("DEBUG")]
        [SerializeField] List<Cube> cubes = new();

        public void Init(Spawner spawner)
        {
            spawner.OnCubeSpawned += OnCubeSpawn;
        }

        void OnCubeSpawn(Cube cube)
        {
            cubes.Add(cube);
        }

        public Cube GetLastCube()
        {
            return cubes.Count != 0 ? cubes[^1] : null;
        }

        public Cube GetCube(int id) => cubes.Find(c => c.ID == id);

        public void DestroyLastCube()
        {
            cubes[0].ReturnToPool();
            cubes.RemoveAt(0);
        }
    }
}