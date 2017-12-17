using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Opsive.ThirdPersonController.UI;

namespace RTSPrototype
{
    public class RTSCrosshairsMonitor : CrosshairsMonitor
    {
        RTSGameMaster gameMaster
        {
            get { return RTSGameMaster.thisInstance; }
        }

        protected override void Start()
        {
            base.Start();
            gameMaster.EventEnableCameraMovement += DisableCrosshairsHandler;
        }

        protected override void OnDestroy()
        {
            base.OnDestroy();
            gameMaster.EventEnableCameraMovement -= DisableCrosshairsHandler;
        }

        void DisableCrosshairsHandler(bool enableCamera)
        {
            DisableCrosshairs(!enableCamera);
        }

        protected override void AttachCharacter(GameObject character)
        {
            base.AttachCharacter(character);
            DisableCrosshairs(true);
        }
    }
}