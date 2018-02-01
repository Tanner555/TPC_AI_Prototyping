using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Opsive.ThirdPersonController.Wrappers;
using RTSCoreFramework;

namespace RTSPrototype
{
    public class AllyEventHandlerWrapper : AllyEventHandler
    {
        #region UnityMessages
        protected override void Awake()
        {
            base.Awake();
        }

        protected override void OnDisable()
        {
            base.OnDisable();
        }
        #endregion

        #region Overrides
        public override void CallEventNpcDie()
        {
            base.CallEventNpcDie();
            EventHandler.ExecuteEvent(this.gameObject, "OnDeath");
        }
        #endregion
    }
}