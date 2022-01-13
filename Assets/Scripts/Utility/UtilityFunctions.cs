using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

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

        public static void WaitDoAction(MonoBehaviour caller,
            Action action, float waitTime)
        {
            caller.StartCoroutine(Co_WaitDoAction(action, waitTime));
        }

        private static IEnumerator Co_WaitDoAction(Action action, float waitTime)
        {
            yield return new WaitForSeconds(waitTime);
            action();
        }
    }
}
