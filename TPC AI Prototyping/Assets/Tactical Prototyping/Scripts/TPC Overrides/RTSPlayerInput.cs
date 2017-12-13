using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Opsive.ThirdPersonController.Input;
using Opsive.ThirdPersonController;

namespace RTSPrototype
{
    public class RTSPlayerInput : UnityInput
    {
        #region RTSFieldsAndProps
        public static RTSPlayerInput thisInstance { get; protected set; }

        #endregion

        #region UnityMessages
        private void OnEnable()
        {
            if (thisInstance != null)
                Debug.Log("More than one instance of rtsplayer input exists");
            else
                thisInstance = this;

        }
        #endregion


    }
}