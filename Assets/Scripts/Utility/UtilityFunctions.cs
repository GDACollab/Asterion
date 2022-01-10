using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Utility
{
    public static class UtilityFunctions
    {
        public static float Rescale(float oldMin, float oldMax,
            float newMin, float newMax, float oldValue)
        {
            float oldRange = oldMax - oldMin;
            float newRange = newMax - newMin;
            float newValue = ((oldValue - oldMin) * newRange / oldRange) + newMin;

            return newValue;
        }
    }
}
