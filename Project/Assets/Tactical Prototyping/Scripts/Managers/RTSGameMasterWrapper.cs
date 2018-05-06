using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BaseFramework;
using Chronos;

namespace RTSCoreFramework
{
    public class RTSGameMasterWrapper : RTSGameMaster
    {
        #region Fields
        string allyClocksName = "Allies";
        #endregion
        
        #region Properties
        GlobalClock allyClocks
        {
            get
            {
                if (_allyClocks == null)
                    _allyClocks = Chronos.Timekeeper.instance.Clock(allyClocksName);

                return _allyClocks;
            }
        }
        GlobalClock _allyClocks = null;
        #endregion

        #region EventCalls
        protected override void ToggleGamePauseTimeScale()
        {
            if (allyClocks != null)
            {
                allyClocks.localTimeScale = bIsGamePaused ?
                    0f : 1f;
            }
            else
            {
                Debug.Log("No Ally Global Clock Exists");
            }
        }

        protected override void TogglePauseControlModeTimeScale()
        {
            if (allyClocks != null)
            {
                allyClocks.localTimeScale = bIsGamePaused ?
                    0f : 1f;
            }
            else
            {
                Debug.Log("No Ally Global Clock Exists");
            }
        }
        #endregion
    }
}