using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using Opsive.ThirdPersonController;
using Opsive.ThirdPersonController.Wrappers.Abilities;

namespace RTSPrototype
{
    public class AllyAIController : MonoBehaviour
    {
        #region Components
        RigidbodyCharacterController myRigidbodyTPC;
        Inventory myInventory;
        ItemHandler itemHandler;
        NavMeshAgent myNavAgent;
        RTSNavBridge myRTSNavBridge;
        AllyEventHandler myEventHandler;
        AllyMember allyMember;
        #endregion

        #region Fields
        //For testing, will be deleted in the future
        //GameObject currentPlayer = null;
        #endregion

        #region Properties
        RTSGameMaster gamemaster
        {
            get { return RTSGameMaster.thisInstance; }
        }

        RTSGameMode gamemode
        {
            get { return RTSGameMode.thisInstance; }
        }

        public AllyMember currentTargettedEnemy { get; protected set; }

        bool AllCompsAreValid
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
        // Use this for initialization
        void Start()
        {
            SetInitialReferences();
            myEventHandler.EventCommandAttackEnemy += HandleCommandAttackEnemy;
            myEventHandler.EventStopTargettingEnemy += HandleStopTargetting;
            myEventHandler.EventNpcDie += OnDeath;
            gamemaster.EventEnableCameraMovement += OnEnableCameraMovement;
        }

        private void OnDisable()
        {
            myEventHandler.EventCommandAttackEnemy -= HandleCommandAttackEnemy;
            myEventHandler.EventStopTargettingEnemy -= HandleStopTargetting;
            myEventHandler.EventNpcDie -= OnDeath;
            gamemaster.EventEnableCameraMovement -= OnEnableCameraMovement;
        }

        // Update is called once per frame
        void Update()
        {
            
        }
        #endregion

        #region Getters
        public bool isEnemyFor(Transform _transform, out AllyMember _ally)
        {
            _ally = null;
            if (_transform.root.GetComponent<AllyMember>())
                _ally = _transform.root.GetComponent<AllyMember>();

            return _ally != null && allyMember.IsEnemyFor(_ally);
        }

        public bool isSurfaceWalkable(RaycastHit hit)
        {
            return myNavAgent.CalculatePath(hit.point, myNavAgent.path) &&
            myNavAgent.path.status == NavMeshPathStatus.PathComplete;
        }
        #endregion

        #region Handlers
        void HandleCommandAttackEnemy(AllyMember enemy)
        {
            currentTargettedEnemy = enemy;
        }

        void HandleStopTargetting()
        {
            currentTargettedEnemy = null;
        }

        void OnEnableCameraMovement(bool _enable)
        {
            if (!allyMember.isCurrentPlayer) return;
            myEventHandler.CallOnTryAim(_enable);
        }

        void OnDeath()
        {
            var _mHitbox = transform.GetComponentInChildren<MeleeWeaponHitbox>();
            if (_mHitbox != null)
            {
                _mHitbox.SetActive(false);
            }
            this.enabled = false;
        }

        #endregion

        void SetInitialReferences()
        {
            myRigidbodyTPC = GetComponent<RigidbodyCharacterController>();
            myInventory = GetComponent<Inventory>();
            itemHandler = GetComponent<ItemHandler>();
            myNavAgent = GetComponent<NavMeshAgent>();
            myRTSNavBridge = GetComponent<RTSNavBridge>();
            myEventHandler = GetComponent<AllyEventHandler>();
            allyMember = GetComponent<AllyMember>();

            if (!AllCompsAreValid)
            {
                Debug.LogError("Not all comps are valid!");
            }
        }        
    }
}