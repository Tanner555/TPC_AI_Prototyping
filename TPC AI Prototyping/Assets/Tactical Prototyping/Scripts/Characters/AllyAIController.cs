using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using Opsive.ThirdPersonController;
using Opsive.ThirdPersonController.Wrappers.Abilities;
using RTSCoreFramework;

namespace RTSPrototype
{
    public class AllyAIController : AllyAIControllerCore
    {
        #region Components
        RigidbodyCharacterController myRigidbodyTPC;
        Inventory myInventory;
        ItemHandler itemHandler;
        RTSNavBridge myRTSNavBridge;

        #endregion

        #region Properties
        protected override bool AllCompsAreValid
        {
            get
            {
                return myRigidbodyTPC && myInventory && itemHandler
                    && myNavAgent && myRTSNavBridge && myEventHandler
                    && allyMember;
            }
        }
        #endregion

        #region UnityMessages
        protected override void Start()
        {
            base.Start();
            myEventHandler.EventNpcDie += OnDeath;
        }

        protected override void OnDisable()
        {
            base.OnDisable();
            myEventHandler.EventNpcDie -= OnDeath;
        }
        #endregion

        #region Handlers
        void OnDeath()
        {
            var _mHitbox = transform.GetComponentInChildren<MeleeWeaponHitbox>();
            if (_mHitbox != null)
            {
                SphereCollider _sphereCol;
                if ((_sphereCol = _mHitbox.GetComponent<SphereCollider>()) != null)
                {
                    _sphereCol.enabled = false;
                }
                _mHitbox.SetActive(false);
            }
            this.enabled = false;
        }

        #endregion

        protected override void SetInitialReferences()
        {
            myNavAgent = GetComponent<NavMeshAgent>();
            myEventHandler = GetComponent<AllyEventHandler>();
            allyMember = GetComponent<AllyMember>();
            myRigidbodyTPC = GetComponent<RigidbodyCharacterController>();
            myInventory = GetComponent<Inventory>();
            itemHandler = GetComponent<ItemHandler>();
            myRTSNavBridge = GetComponent<RTSNavBridge>();

            if (!AllCompsAreValid)
            {
                Debug.LogError("Not all comps are valid!");
            }
        }        
    }
}