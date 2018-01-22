using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

namespace RTSCoreFramework
{
    public class AllyAIController : MonoBehaviour
    {
        #region Components
        protected NavMeshAgent myNavAgent;
        protected AllyEventHandler myEventHandler;
        protected AllyMember allyMember;
        #endregion

        #region Fields
        bool bIsShooting = false;
        #endregion

        #region Properties
        protected RTSGameMaster gamemaster
        {
            get { return RTSGameMaster.thisInstance; }
        }

        protected RTSGameMode gamemode
        {
            get { return RTSGameMode.thisInstance; }
        }

        public AllyMember currentTargettedEnemy { get; protected set; }
        public AllyMember previousTargettedEnemy { get; protected set; }

        protected virtual bool AllCompsAreValid
        {
            get
            {
                return myNavAgent && myEventHandler
                    && allyMember;
            }
        }
        #endregion

        #region UnityMessages
        protected virtual void OnEnable()
        {

        }

        // Use this for initialization
        protected virtual void Start()
        {
            SetInitialReferences();
            SubToEvents();
            StartServices();
        }

        protected virtual void Update()
        {

        }

        protected virtual void LateUpdate()
        {

        }

        protected virtual void OnDisable()
        {
            UnSubFromEvents();
            CancelServices();
        }

        protected virtual void OnDrawGizmos()
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
        protected virtual void HandleCommandAttackEnemy(AllyMember enemy)
        {
            previousTargettedEnemy = currentTargettedEnemy;
            currentTargettedEnemy = enemy;
            if(IsInvoking("UpdateBattleBehavior") == false)
            {
                StartBattleBehavior();
            }
            else if(IsInvoking("UpdateBattleBehavior") && previousTargettedEnemy != currentTargettedEnemy)
            {
                StopBattleBehavior();
                Invoke("StartBattleBehavior", 0.05f);
            }
        }

        protected virtual void HandleStopTargetting()
        {
            currentTargettedEnemy = null;
            StopBattleBehavior();
        }

        protected virtual void HandleOnMoveAlly(rtsHitType hitType, RaycastHit hit)
        {
            if(IsInvoking("UpdateBattleBehavior"))
                StopBattleBehavior();
        }

        protected virtual void OnEnableCameraMovement(bool _enable)
        {
            if (!allyMember.isCurrentPlayer) return;
            myEventHandler.CallOnTryAim(_enable);
        }

        protected virtual void TogglebIsShooting(bool _enable)
        {
            bIsShooting = _enable;
        }
        #endregion

        #region ShootingAndBattleBehavior
        void UpdateBattleBehavior()
        {
            //If has line of sight to enemy
            //and is within a given range,
            //start shooting behavior
            //else move towards target enemy
            //possibly find a spot close enough if needed
            if(bIsShooting == false)
                StartShootingBehavior();
        }

        void StartBattleBehavior()
        {
            InvokeRepeating("UpdateBattleBehavior", 0f, 0.2f);
        }

        void StopBattleBehavior()
        {
            CancelInvoke("UpdateBattleBehavior");
            StopShootingBehavior();
        }

        void StartShootingBehavior()
        {
            myEventHandler.CallEventToggleIsShooting(true);
        }

        void StopShootingBehavior()
        {
            myEventHandler.CallEventToggleIsShooting(false);
        }
        #endregion

        #region Initialization
        protected virtual void SubToEvents()
        {
            myEventHandler.EventCommandAttackEnemy += HandleCommandAttackEnemy;
            myEventHandler.EventStopTargettingEnemy += HandleStopTargetting;
            myEventHandler.EventToggleIsShooting += TogglebIsShooting;
            myEventHandler.EventCommandMove += HandleOnMoveAlly;
            gamemaster.EventEnableCameraMovement += OnEnableCameraMovement;
        }

        protected virtual void UnSubFromEvents()
        {
            myEventHandler.EventCommandAttackEnemy -= HandleCommandAttackEnemy;
            myEventHandler.EventStopTargettingEnemy -= HandleStopTargetting;
            myEventHandler.EventToggleIsShooting -= TogglebIsShooting;
            myEventHandler.EventCommandMove -= HandleOnMoveAlly;
            gamemaster.EventEnableCameraMovement -= OnEnableCameraMovement;
        }

        protected virtual void StartServices()
        {

        }

        protected virtual void CancelServices()
        {
            CancelInvoke();
        }

        protected virtual void SetInitialReferences()
        {
            myNavAgent = GetComponent<NavMeshAgent>();
            myEventHandler = GetComponent<AllyEventHandler>();
            allyMember = GetComponent<AllyMember>();

            if (!AllCompsAreValid)
            {
                Debug.LogError("Not all comps are valid!");
            }
        }
        #endregion

    }
}