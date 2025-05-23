using System.Collections.Generic;
using Curves;
using UnityEngine;

namespace Spawn
{
    public class SpawnLocator : MonoBehaviour
    {
        [Header("Setup")]
        [Header("DEBUG")]
        [SerializeField] private int _step;
        [Header("DEBUG CURVES")]
        [SerializeField] private bool useHeightCurve = true;
        [SerializeField] private bool useDirectionCurve = true;
        [SerializeField] private CurveData data;
        [SerializeField] private List<float> heightValues = new List<float>();
        [SerializeField] private List<float> directionValues = new List<float>();

        int _cubesPerCurve;
        Vector3 _curveStartDir;
        Vector3 _curveEndDir;
        float _padding;
        public bool IsEndOfCurve => _step + 1 >= _cubesPerCurve;


        public void Init(float spawnPadding, Vector3 spawnDirection, CurveData startCurve)
        {
            _padding = spawnPadding;
            _curveEndDir = spawnDirection;
            SetCurve(startCurve);
        }

        public void NextStep()
        {
            _step++;
        }

        public void SetCurve(CurveData locatorData)
        {
            Debug.Log("Next curve");
            // curves are glued, so "last cube" in previous curve = "first cube" in new curve
            data = locatorData;
            _step = 0;

            _curveStartDir = _curveEndDir; //set NEW curve startDir to LAST curve endDir
            _curveEndDir = GetEndDir();
            _cubesPerCurve = locatorData.totalSteps;

            heightValues = GetRelativeValues(locatorData.height);
            directionValues = GetRelativeValues(locatorData.direction);
        }

        Vector3 GetEndDir()
        {
            var p1 = 0.99f;
            var p2 = 1f;

            var v0 = data.direction.Evaluate(0f);
            var v1 = data.direction.Evaluate(p1);
            var v2 = data.direction.Evaluate(p2);

            var offset1 = (v1 - v0) * data.maxWidth;
            var offset2 = (v2 - v0) * data.maxWidth;

            var pos1 = data.startPos + _curveStartDir * data.totalSteps * _padding * p1;
            var pos2 = data.startPos + _curveStartDir * data.totalSteps * _padding * p2;

            var cross = Vector3.Cross(_curveStartDir, Vector3.up).normalized;
            pos1 += cross * offset1;
            pos2 += cross * offset2;

            var result = pos2 - pos1;
            result.Normalize();
            return result;
        }

        public Vector3 Position
        {
            get
            {
                var heightValue = heightValues[_step];
                var offsetY = heightValue * data.maxHeight;

                var v0 = directionValues[0];
                var v1 = directionValues[_step];
                var directionValue = v1 - v0;
                var offsetXZ = directionValue * data.maxWidth;

                var crossXZ = Vector3.Cross(_curveStartDir, Vector3.up).normalized;

                var posByPadding = data.startPos + _curveStartDir * (_padding * _step);
                var pos = posByPadding + crossXZ * offsetXZ;

                var y = useHeightCurve ? offsetY : 0;
                var x = useDirectionCurve ? pos.x : 0;
                var z = useDirectionCurve ? pos.z : 0;
                var resultPos = new Vector3(x, y, z);

                return resultPos;
            }
        }

        List<float> GetRelativeValues(AnimationCurve curve)
        {
            var curveValues = new List<float> {0};

            var maxValue = 0f;
            var v0 = curve.Evaluate(0);

            for (var i = 1; i < _cubesPerCurve; i++)
            {
                var p = (float) i / _cubesPerCurve;
                var v = curve.Evaluate(p);
                var relativeValue = v - v0;

                curveValues.Add(relativeValue);

                var abs = Mathf.Abs(relativeValue);
                if (abs > maxValue)
                {
                    maxValue = abs;
                }
            }

            for (var i = 0; i < curveValues.Count; i++)
            {
                curveValues[i] /= maxValue;
            }

            return curveValues;
        }
    }
}