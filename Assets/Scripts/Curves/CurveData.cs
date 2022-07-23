using UnityEngine;

namespace Curves
{
    [System.Serializable]
    public class CurveData
    {
        public Vector3 startPos;
        public AnimationCurve height;
        public AnimationCurve direction;
        public float maxHeight;
        public float maxWidth;
        public int totalSteps;
    }
}