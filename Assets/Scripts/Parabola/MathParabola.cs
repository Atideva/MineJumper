using UnityEngine;

namespace Parabola
{
    public static class MathParabola
    {
        public static Vector3 Parabola(Vector3 start, Vector3 end, float height, float t)
        {
            t = Mathf.Clamp(t, 0, 1);

            var yOffset = Mathf.Abs(start.y - end.y);
            height += yOffset * 0.2f;
            
            var pos = Vector3.Lerp(start, end, t);
            var y = -4 * height * t * t + 4 * height * t;
            var y1 = Mathf.Lerp(start.y, end.y, t);
            
            return new Vector3(pos.x, y + y1, pos.z);
        }

        public static Vector2 Parabola(Vector2 start, Vector2 end, float height, float t)
        {
            float Func(float x) => -4 * height * x * x + 4 * height * x;
            var mid = Vector2.Lerp(start, end, t);
            return new Vector2(mid.x, Func(t) + Mathf.Lerp(start.y, end.y, t));
        }
    }
}