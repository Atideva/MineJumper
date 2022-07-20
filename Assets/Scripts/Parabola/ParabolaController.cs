using System;
using System.Collections.Generic;
using UnityEngine;

namespace Parabola
{
    public class ParabolaController : MonoBehaviour
    {
        public float speed = 1;
        public GameObject parabolaRoot;
        public bool autostart = true;
        public bool animate = true;

        //next parabola event

        private float _animationTime = float.MaxValue;

        //gizmo
        private ParabolaFly _gizmo;
        //draw
        private ParabolaFly _parabolaFly;

        private void OnDrawGizmos()
        {
            _gizmo ??= new ParabolaFly(parabolaRoot.transform);

            _gizmo.RefreshTransforms(1f);
            if ((_gizmo.Points.Length - 1) % 2 != 0)
                return;

            const int accuracy = 50;
            var prevPos = _gizmo.Points[0].position;
            for (var c = 1; c <= accuracy; c++)
            {
                var currTime = c * _gizmo.GetDuration() / accuracy;
                var currPos = _gizmo.GetPositionAtTime(currTime);
                var mag = (currPos - prevPos).magnitude * 2;
                Gizmos.color = new Color(mag, 0, 0, 1);
                Gizmos.DrawLine(prevPos, currPos);
                Gizmos.DrawSphere(currPos, 0.01f);
                prevPos = currPos;
            }
        }


        private void Start()
        {
            _parabolaFly = new ParabolaFly(parabolaRoot.transform);

            if (autostart)
            {
                _parabolaFly.RefreshTransforms(speed);
                FollowParabola();
            }
        }

        private void Update()
        {
            switch (animate)
            {
                case true when _parabolaFly != null && _animationTime < _parabolaFly.GetDuration():
                    _parabolaFly.GetParabolaIndexAtTime(_animationTime, out _);
                    _animationTime += Time.deltaTime;
                    _parabolaFly.GetParabolaIndexAtTime(_animationTime, out _);

                    transform.position = _parabolaFly.GetPositionAtTime(_animationTime);
                    break;
                case true when _parabolaFly != null && _animationTime > _parabolaFly.GetDuration():
                    _animationTime = float.MaxValue;
                    animate = false;
                    break;
            }
        }

        private void FollowParabola()
        {
            _parabolaFly.RefreshTransforms(speed);
            _animationTime = 0f;
            transform.position = _parabolaFly.Points[0].position;
            animate = true;
        }

        public Vector3 GetHighestPoint(int parabolaIndex)
        {
            return _parabolaFly.GetHighPoint(parabolaIndex);
        }

        public Transform[] GetPoints()
        {
            return _parabolaFly.Points;
        }

        public Vector3 GetPositionAtTime(float time)
        {
            return _parabolaFly.GetPositionAtTime(time);
        }

        public float GetDuration()
        {
            return _parabolaFly.GetDuration();
        }

        public void StopFollow()
        {
            _animationTime = float.MaxValue;
        }

        /// <summary>
        /// Returns children transforms, sorted by name.
        /// </summary>
        public static float DistanceToLine(Ray ray, Vector3 point)
        {
            //see:http://answers.unity3d.com/questions/62644/distance-between-a-ray-and-a-point.html
            return Vector3.Cross(ray.direction, point - ray.origin).magnitude;
        }

        public static Vector3 ClosestPointInLine(Ray ray, Vector3 point)
        {
            return ray.origin + ray.direction * Vector3.Dot(ray.direction, point - ray.origin);
        }

        private class ParabolaFly
        {
            public readonly Transform[] Points;
            private readonly Parabola3D[] _parabolas;
            private readonly float[] _partDuration;
            private float _completeDuration;

            public ParabolaFly(Component parabolaRoot)
            {
                var components = new List<Component>(parabolaRoot.GetComponentsInChildren(typeof(Transform)));
                var transforms = components.ConvertAll(c => (Transform) c);

                transforms.Remove(parabolaRoot.transform);
                transforms.Sort((a, b) => string.Compare(a.name, b.name, StringComparison.Ordinal));

                Points = transforms.ToArray();

                //check if odd
                if ((Points.Length - 1) % 2 != 0)
                {
                    Debug.LogError("ParabolaRoot needs odd number of points");
                    return;
                }

                //if larger is needed
                _parabolas = new Parabola3D[(Points.Length - 1) / 2];
                _partDuration = new float[_parabolas.Length];
            }

            public Vector3 GetPositionAtTime(float time)
            {
                GetParabolaIndexAtTime(time, out var parabolaIndex, out var timeInParabola);

                var percent = timeInParabola / _partDuration[parabolaIndex];
                return _parabolas[parabolaIndex].GetPositionAtLength(percent * _parabolas[parabolaIndex].Length);
            }

            public void GetParabolaIndexAtTime(float time, out int parabolaIndex)
            {
                GetParabolaIndexAtTime(time, out parabolaIndex, out _);
            }

            public void GetParabolaIndexAtTime(float time, out int parabolaIndex, out float timeInParabola)
            {
                //f(x) = axÂ² + bx + c
                timeInParabola = time;
                parabolaIndex = 0;

                //determine parabola
                while (parabolaIndex < _parabolas.Length - 1 && _partDuration[parabolaIndex] < timeInParabola)
                {
                    timeInParabola -= _partDuration[parabolaIndex];
                    parabolaIndex++;
                }
            }

            public float GetDuration()
            {
                return _completeDuration;
            }

            public Vector3 GetHighPoint(int parabolaIndex)
            {
                return _parabolas[parabolaIndex].GetHighPoint();
            }

            /// <summary>
            /// Returns children transforms, sorted by name.
            /// </summary>
            public void RefreshTransforms(float speed)
            {
                if (speed <= 0f)
                    speed = 1f;

                if (Points != null)
                {
                    _completeDuration = 0;

                    //create parabolas
                    for (var i = 0; i < _parabolas.Length; i++)
                    {
                        _parabolas[i] ??= new Parabola3D();

                        _parabolas[i].Set(Points[i * 2].position, Points[i * 2 + 1].position,
                            Points[i * 2 + 2].position);
                        _partDuration[i] = _parabolas[i].Length / speed;
                        _completeDuration += _partDuration[i];
                    }
                }
            }
        }

        private class Parabola3D
        {
            public float Length { get; private set; }

            private Vector3 _a;
            private Vector3 _b;
            private Vector3 _c;

            private Parabola2D _parabola2D;
            private Vector3 _h;
            private bool _tooClose;

            public Parabola3D()
            {
            }

            public Parabola3D(Vector3 a, Vector3 b, Vector3 c)
            {
                Set(a, b, c);
            }

            public void Set(Vector3 a, Vector3 b, Vector3 c)
            {
                _a = a;
                _b = b;
                _c = c;
                RefreshCurve();
            }

            public Vector3 GetHighPoint()
            {
                var d = (_c.y - _a.y) / _parabola2D.Length;
                var e = _a.y - _c.y;

                var parabola = new Parabola2D(_parabola2D.A, _parabola2D.B + d, _parabola2D.C + e, _parabola2D.Length);

                var highPoint = new Vector3
                {
                    y = parabola.E.y,
                    x = _a.x + (_c.x - _a.x) * (parabola.E.x / parabola.Length),
                    z = _a.z + (_c.z - _a.z) * (parabola.E.x / parabola.Length)
                };

                return highPoint;
            }

            public Vector3 GetPositionAtLength(float length)
            {
                //f(x) = axÂ² + bx + c
                var percent = length / Length;

                var x = percent * (_c - _a).magnitude;
                if (_tooClose)
                    x = percent * 2f;

                var pos = _a * (1f - percent) + _c * percent + _h.normalized * _parabola2D.F(x);
                if (_tooClose)
                    pos.Set(_a.x, pos.y, _a.z);

                return pos;
            }

            private void RefreshCurve()
            {
                if (Vector2.Distance(new Vector2(_a.x, _a.z), new Vector2(_b.x, _b.z)) < 0.1f &&
                    Vector2.Distance(new Vector2(_b.x, _b.z), new Vector2(_c.x, _c.z)) < 0.1f)
                    _tooClose = true;
                else
                    _tooClose = false;

                Length = Vector3.Distance(_a, _b) + Vector3.Distance(_b, _c);

                if (!_tooClose)
                {
                    RefreshCurveNormal();
                }
                else
                {
                    RefreshCurveClose();
                }
            }


            private void RefreshCurveNormal()
            {
                //                        .  E   .
                //                   .       |       point[1]
                //             .             |h         |       .
                //         .                 |       ___v1------point[2]
                //      .            ______--vl------    
                // point[0]---------
                //

                //lower v1
                var rl = new Ray(_a, _c - _a);
                var v1 = ClosestPointInLine(rl, _b);

                //get A=(x1,y1) B=(x2,y2) C=(x3,y3)
                Vector2 a2d, b2d, c2d;

                a2d.x = 0f;
                a2d.y = 0f;
                b2d.x = Vector3.Distance(_a, v1);
                b2d.y = Vector3.Distance(_b, v1);
                c2d.x = Vector3.Distance(_a, _c);
                c2d.y = 0f;

                _parabola2D = new Parabola2D(a2d, b2d, c2d);

                //lower v
                //var p = parabola.E.x / parabola.Length;
                //Vector3 vl = points[0].position * (1f - p) + points[2].position * p;

                //h
                _h = (_b - v1) / Vector3.Distance(v1, _b) * _parabola2D.E.y;
            }

            private void RefreshCurveClose()
            {
                //distance to x0 - x2 line = |(x1-x0)x(x1-x2)|/|x2-x0|
                var fac01 = (_a.y <= _b.y) ? 1f : -1f;
                var fac02 = (_a.y <= _c.y) ? 1f : -1f;

                Vector2 a2d, b2d, c2d;

                //get A=(x1,y1) B=(x2,y2) C=(x3,y3)
                a2d.x = 0f;
                a2d.y = 0f;

                //b = sqrt(cÂ²-aÂ²)
                b2d.x = 1f;
                b2d.y = Vector3.Distance((_a + _c) / 2f, _b) * fac01;

                c2d.x = 2f;
                c2d.y = Vector3.Distance(_a, _c) * fac02;

                _parabola2D = new Parabola2D(a2d, b2d, c2d);
                _h = Vector3.up;
            }
        }

        private class Parabola2D
        {
            public float A { get; }
            public float B { get; }
            public float C { get; }

            public Vector2 E { get; private set; }
            public float Length { get; private set; }

            public Parabola2D(float a, float b, float c, float length)
            {
                A = a;
                B = b;
                C = c;

                SetMetadata();
                Length = length;
            }

            public Parabola2D(Vector2 a, Vector2 b, Vector2 c)
            {
                //f(x) = axÂ² + bx + c
                //a = (x1(y2 - y3) + x2(y3 - y1) + x3(y1 - y2)) / ((x1 - x2)(x1 - x3)(x3 - x2))
                //b = (x1Â²(y2 - y3) + x2Â²(y3 - y1) + x3Â²(y1 - y2))/ ((x1 - x2)(x1 - x3)(x2 - x3))
                //c = (x1Â²(x2y3 - x3y2) + x1(x3Â²y2 - x2Â²y3) + x2x3y1(x2 - x3))/ ((x1 - x2)(x1 - x3)(x2 - x3))
                var divisor = ((a.x - b.x) * (a.x - c.x) * (c.x - b.x));
                if (divisor == 0f)
                {
                    a.x += 0.00001f;
                    b.x += 0.00002f;
                    c.x += 0.00003f;
                    divisor = ((a.x - b.x) * (a.x - c.x) * (c.x - b.x));
                }

                A = (a.x * (b.y - c.y) + b.x * (c.y - a.y) + c.x * (a.y - b.y)) / divisor;
                B = (a.x * a.x * (b.y - c.y) + b.x * b.x * (c.y - a.y) + c.x * c.x * (a.y - b.y)) / divisor;
                C = (a.x * a.x * (b.x * c.y - c.x * b.y) + a.x * (c.x * c.x * b.y - b.x * b.x * c.y) +
                     b.x * c.x * a.y * (b.x - c.x)) / divisor;

                B *= -1f; //hack

                SetMetadata();
                Length = Vector2.Distance(a, c);
            }

            public float F(float x)
            {
                return A * x * x + B * x + C;
            }

            private void SetMetadata()
            {
                //derive
                //a*xÂ²+b*x+c = 0
                //2ax+b=0
                //x = -b/2a
                var x = -B / (2 * A);
                E = new Vector2(x, F(x));
            }
        }
    }
}