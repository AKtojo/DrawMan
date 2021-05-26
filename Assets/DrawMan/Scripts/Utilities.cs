using System;
using UnityEngine;

namespace DrawMan.Core
{
    public class Utilities
    {
        public static bool Int2Bool(int value)
        { return value != 0; }

        public static int Bool2Int(bool value)
        {
            return Convert.ToInt32(value);
        }

        public static bool MaskContainsLayer(LayerMask mask, int layer)
        {
            int content = 1 << layer;
            return (mask.value & content) == content;
        }

        public static float Inverp(float start, float end, float val)
        {
            return (val - start) / (end - start);
        }

        public static float Inverp(Vector3 start, Vector3 end, Vector3 val)
        {
            Vector3 AB = end - start;
            Vector3 AV = val - start;
            if (AB == Vector3.zero) return 0.0f;
            return Vector3.Dot(AV, AB) / Vector3.Dot(AB, AB);
        }
    }
}
