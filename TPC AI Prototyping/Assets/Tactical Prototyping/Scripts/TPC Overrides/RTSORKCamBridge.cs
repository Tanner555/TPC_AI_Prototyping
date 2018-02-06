using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Opsive.ThirdPersonController.ThirdParty.ORK;

namespace RTSPrototype
{
    public class RTSORKCamBridge : ORKCameraControllerBridge
    {
        RTSCamBehaviorController myCamBehaviorController
        {
            get
            {
                if (_myCamBehaviorController == null)
                    _myCamBehaviorController = GetComponent<RTSCamBehaviorController>();

                return _myCamBehaviorController;
            }
        }
        private RTSCamBehaviorController _myCamBehaviorController = null;

        protected override void OnEnable()
        {
            //if (myCamBehaviorController.CanEnableCameraComps)
            //{
            //    base.OnEnable();
            //} 
        }

        protected override void OnDisable()
        {
            //base.OnDisable();
        }
    }
}