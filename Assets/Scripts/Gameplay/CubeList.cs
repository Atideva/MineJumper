using System.Collections.Generic;
using UnityEngine;

namespace Gameplay
{
    public class CubeList : MonoBehaviour
    {
        [Header("Debug")]
        [SerializeField]
        private List<Cube> cubes = new List<Cube>();

        private void Awake()
        {
            cubes = new List<Cube>();
        }

        public Cube GetLastCube()
        {
            if (cubes.Count == 0)
            {
                Debug.LogWarning("Cubes count = 0");
                return null;
            }
            return cubes[cubes.Count - 1];
        }

        public void Add(Cube cube) => cubes.Add(cube);

        public Cube GetCube(int id) => cubes.Find(c => c.ID == id);

        public void Dequeue()
        {
            cubes[0].ReturnToPool();
            cubes.RemoveAt(0);
        }
    }
}