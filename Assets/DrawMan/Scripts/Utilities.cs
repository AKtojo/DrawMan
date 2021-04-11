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
    }
}
