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
            //Listen to TPC Events
            EventHandler.RegisterEvent(this.gameObject, "OnDeath", CallEventNpcDie);
   
        }

        protected override void OnDisable()
        {
            base.OnDisable();
            //Unsub from TPC Events
            EventHandler.UnregisterEvent(this.gameObject, "OnDeath", CallEventNpcDie);
        }
        #endregion
    }
}