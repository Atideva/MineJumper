using UnityEngine;

namespace Curves
{
    public class Curve : MonoBehaviour
    {
        [SerializeField] private CurveList heightList;
        [SerializeField] private CurveList directionList;

        public AnimationCurve GetHeightCurve() => heightList.GetRandomCurve();
        public AnimationCurve GetDirectionCurve() => directionList.GetRandomCurve();
    }
}