using UnityEngine;

namespace Gameplay
{
    [System.Serializable]
    public class SpawnerLocatorData
    {
        public Vector3 startPos;
        public AnimationCurve height;
        public AnimationCurve direction;
        public float maxHeight;
        public float maxWidth;
        public int totalSteps;
    }
}