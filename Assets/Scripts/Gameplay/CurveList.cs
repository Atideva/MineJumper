using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Gameplay
{
    public class CurveList : MonoBehaviour
    {
        [SerializeField] private AnimationCurve curvesChance;
        [Range(0.1f, 30f)]
        [SerializeField] private float chanceFactor = 4f;
        [SerializeField] private List<AnimationCurve> curves;
        public List<AnimationCurve> Curves => curves;


        public AnimationCurve GetRandomCurve()
        {
            var r = Random.Range(0, 100) * 0.01f;
            var sum = 0f;

            for (var i = 0; i < curves.Count; i++)
            {
                var chance = GetChance(i);
                sum += chance;
                if (r <= sum)
                {
                    return curves[i];
                }
            }

            Debug.LogError("Null curve returned");
            return null;
        }

        public float GetChance(int curveId)
        {
            var factor = 1 / chanceFactor;

            var point = (float) curveId / (curves.Count - 1);
            var value = curvesChance.Evaluate(point);

            var factorValue = value + factor;
            var factorTotal = TotalChance + factor * curves.Count;

            return factorValue / factorTotal;
        }

        private float TotalChance => curves
            .Select((t, i) => i / (float) (curves.Count - 1))
            .Sum(curvesChance.Evaluate);
    }
}