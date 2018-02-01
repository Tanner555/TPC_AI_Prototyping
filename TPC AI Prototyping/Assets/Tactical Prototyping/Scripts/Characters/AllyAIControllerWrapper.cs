using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using Opsive.ThirdPersonController;
using Opsive.ThirdPersonController.Wrappers.Abilities;
using RTSCoreFramework;

namespace RTSPrototype
{
    public class AllyAIControllerWrapper : AllyAIController
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

        public AllyMemberWrapper currentTargettedEnemyWrapper { get; protected set; }
        #endregion

        #region UnityMessages
        protected override void Start()
        {
            base.Start();

        }

        protected override void OnDisable()
        {
            base.OnDisable();
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

        #region Overrides
        protected override void HandleCommandAttackEnemy(AllyMember enemy)
        {
            base.HandleCommandAttackEnemy(enemy);
            currentTargettedEnemyWrapper = (AllyMemberWrapper)enemy;
        }

        protected override void HandleStopTargetting()
        {
            base.HandleStopTargetting();
            currentTargettedEnemyWrapper = null;
        }
        #endregion

        #region Initialization
        protected override void SubToEvents()
        {
            base.SubToEvents();
            myEventHandler.EventAllyDied += OnDeath;
        }

        protected override void UnSubFromEvents()
        {
            base.UnSubFromEvents();
            myEventHandler.EventAllyDied -= OnDeath;
        }

        protected override void SetInitialReferences()
        {
            myNavAgent = GetComponent<NavMeshAgent>();
            myEventHandler = GetComponent<AllyEventHandlerWrapper>();
            allyMember = GetComponent<AllyMemberWrapper>();
            myRigidbodyTPC = GetComponent<RigidbodyCharacterController>();
            myInventory = GetComponent<Inventory>();
            itemHandler = GetComponent<ItemHandler>();
            myRTSNavBridge = GetComponent<RTSNavBridge>();

            if (!AllCompsAreValid)
            {
                Debug.LogError("Not all comps are valid!");
            }
        }
        #endregion

    }
}