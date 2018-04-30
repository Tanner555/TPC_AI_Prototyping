using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BaseFramework
{
    public static class BaseUtilities
    {
        #region Timer
        public class TimerUtility
        {
            float timerRate = 0.0f;
            float currentTimer = 0.0f;

            public TimerUtility(float rate)
            {
                timerRate = rate;
            }

            public void StartTimer()
            {
                currentTimer = Time.time + timerRate;
            }

            public bool IsTimerFinished()
            {
                return Time.time > currentTimer;
            }
        }
        #endregion

        #region Invoker
        public class InvokerUtility
        {
            private static MonoBehaviour invokeCaller = null;

            public static void InvokeInRealTime(ref MonoBehaviour caller, string methodName, float time)
            {
                invokeCaller = caller;
                caller.StartCoroutine(InvokeFromCoroutine(methodName, time));
            }

            static IEnumerator InvokeFromCoroutine(string methodName, float time)
            {
                yield return new WaitForSecondsRealtime(time);
                invokeCaller.Invoke(methodName, 0.0f);
            }
        }
        #endregion

    }
}