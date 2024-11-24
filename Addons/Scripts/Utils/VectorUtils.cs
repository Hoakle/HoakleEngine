using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace HoakleEngine
{
    public static class VectorUtils
    {
        public static Vector3 With(this Vector3 vector, int z)
        {
            return new Vector3(vector.x, vector.y, z);
        }
    }
}
