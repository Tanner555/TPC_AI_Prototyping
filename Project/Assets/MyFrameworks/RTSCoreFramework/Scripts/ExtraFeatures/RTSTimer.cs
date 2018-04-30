using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RTSCoreFramework.Utilities
{
    public class RTSTimer
    {
        float timerRate = 0.0f;
        float currentTimer = 0.0f;

        public RTSTimer(float rate)
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

    public class RTSInvoker
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
}