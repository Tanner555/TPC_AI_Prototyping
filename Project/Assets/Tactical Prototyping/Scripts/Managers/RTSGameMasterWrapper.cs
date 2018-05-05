using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BaseFramework;
using Chronos;

namespace RTSCoreFramework
{
    public class RTSGameMasterWrapper : RTSGameMaster
    {
        string allyClocks = "Allies";

        protected override void ToggleTimeScale()
        {
            var _allyClocks = Chronos.Timekeeper.instance.Clock(allyClocks);
            if(_allyClocks != null)
            {
                _allyClocks.localTimeScale = bIsGamePaused ?
                    0f : 1f;
            }
            else
            {
                Debug.Log("No Ally Global Clock Exists");
            }
        }
    }
}