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
        protected bool bIsShooting = false;
        protected bool bIsMoving = false;
        protected float defaultFireRepeatRate = 0.25f;
        //Used for finding closest ally
        [Header("AI Finder Properties")]
        public float sightRange = 40f;
        public LayerMask allyLayers;
        public LayerMask sightLayers;

        protected Collider[] colliders;
        protected List<Transform> uniqueTransforms = new List<Transform>();
        protected List<AllyMember> scanEnemyList = new List<AllyMember>();
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

        //AllyMember Transforms
        Transform headTransform { get { return allyMember.HeadTransform; } }
        Transform chestTransform { get { return allyMember.ChestTransform; } }

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
        public virtual bool isEnemyFor(Transform _transform, out AllyMember _ally)
        {
            _ally = null;
            if (_transform.root.GetComponent<AllyMember>())
                _ally = _transform.root.GetComponent<AllyMember>();

            return _ally != null && allyMember.IsEnemyFor(_ally);
        }

        public virtual bool isSurfaceWalkable(RaycastHit hit)
        {
            return myNavAgent.CalculatePath(hit.point, myNavAgent.path) &&
            myNavAgent.path.status == NavMeshPathStatus.PathComplete;
        }

        public virtual float GetFiringRate()
        {
            return defaultFireRepeatRate;
        }
        #endregion

        #region Handlers
        protected virtual void HandlePlayerCommandAttackEnemy(AllyMember enemy)
        {
            CommandAttackEnemy(enemy);
        }

        protected virtual void HandleAICommandAttackEnemy(AllyMember enemy)
        {
            CommandAttackEnemy(enemy);
        }

        protected virtual void HandleStopTargetting()
        {
            currentTargettedEnemy = null;
            StopBattleBehavior();
            if (bIsMoving)
                myEventHandler.CallEventAIMove(this.transform.position);
        }

        protected virtual void HandleOnPlayerMoveAlly(rtsHitType hitType, RaycastHit hit)
        {
            bIsMoving = true;
            if(IsInvoking("UpdateBattleBehavior"))
                StopBattleBehavior();
        }

        protected virtual void HandleOnAIMoveAlly(Vector3 _point)
        {
            bIsMoving = true;
        }

        protected virtual void HandleOnAIStopMoving()
        {
            bIsMoving = false;
        }

        protected virtual void OnEnableCameraMovement(bool _enable)
        {
            
        }

        protected virtual void TogglebIsShooting(bool _enable)
        {
            bIsShooting = _enable;
        }
        #endregion

        #region AITacticsHelpers
        protected virtual AllyMember FindClosestEnemy()
        {
            AllyMember _closestEnemy = null;
            if (headTransform == null)
            {
                Debug.LogError("No head assigned on AIController, cannot run look service");
                return _closestEnemy;
            }
            colliders = Physics.OverlapSphere(transform.position, sightRange, allyLayers);
            AllyMember _enemy = null;
            scanEnemyList.Clear();
            uniqueTransforms.Clear();
            foreach (Collider col in colliders)
            {
                if (uniqueTransforms.Contains(col.transform.root)) continue;
                uniqueTransforms.Add(col.transform.root);
                if (isEnemyFor(col.transform, out _enemy))
                {
                    RaycastHit hit;
                    if (hasLOSWithinRange(_enemy, out hit))
                    {
                        if (hit.transform.root == _enemy.transform.root)
                            scanEnemyList.Add(_enemy);
                    }
                }
            }

            if (scanEnemyList.Count > 0)
                _closestEnemy = DetermineClosestAllyFromList(scanEnemyList);

            return _closestEnemy;
        }
        
        bool hasLOSWithinRange(AllyMember _enemy, out RaycastHit _hit)
        {
            bool _bHit = Physics.Linecast(chestTransform.position,
                        _enemy.ChestTransform.position, out _hit);
            return _bHit && _hit.transform != null &&
                _hit.transform.root.tag == gamemode.AllyTag &&
                _hit.transform.root == _enemy.transform;
        }

        AllyMember DetermineClosestAllyFromList(List<AllyMember> _allies)
        {
            AllyMember _closestAlly = null;
            float _closestDistance = Mathf.Infinity;
            foreach (var _ally in _allies)
            {
                float _newDistance = Vector3.Distance(_ally.transform.position,
                    transform.position);
                if (_newDistance < _closestDistance)
                {
                    _closestDistance = _newDistance;
                    _closestAlly = _ally;
                }
            }
            return _closestAlly;
        }
        #endregion

        #region ShootingAndBattleBehavior
        protected virtual void CommandAttackEnemy(AllyMember enemy)
        {
            previousTargettedEnemy = currentTargettedEnemy;
            currentTargettedEnemy = enemy;
            if (IsInvoking("UpdateBattleBehavior") == false)
            {
                StartBattleBehavior();
            }
            else if (IsInvoking("UpdateBattleBehavior") && previousTargettedEnemy != currentTargettedEnemy)
            {
                StopBattleBehavior();
                Invoke("StartBattleBehavior", 0.05f);
            }
        }

        protected virtual void UpdateBattleBehavior()
        {
            if(currentTargettedEnemy == null || currentTargettedEnemy.IsAlive == false)
            {
                myEventHandler.CallEventStopTargettingEnemy();
                return;
            }
            RaycastHit _hit;
            if(hasLOSWithinRange(currentTargettedEnemy, out _hit))
            {
                if (bIsShooting == false)
                    StartShootingBehavior();
            }
            else
            {
                if (bIsShooting == true)
                    StopShootingBehavior();

                if(bIsMoving == false)
                {
                    myEventHandler.CallEventAIMove(currentTargettedEnemy.transform.position);
                }
            }
            
        }

        protected virtual void StartBattleBehavior()
        {
            InvokeRepeating("UpdateBattleBehavior", 0f, 0.2f);
        }

        protected virtual void StopBattleBehavior()
        {
            CancelInvoke("UpdateBattleBehavior");
            StopShootingBehavior();
        }

        protected virtual void StartShootingBehavior()
        {
            myEventHandler.CallEventToggleIsShooting(true);
            InvokeRepeating("MakeFireRequest", 0.0f, GetFiringRate());
        }

        protected virtual void StopShootingBehavior()
        {
            myEventHandler.CallEventToggleIsShooting(false);
            CancelInvoke("MakeFireRequest");
        }

        protected virtual void MakeFireRequest()
        {
            myEventHandler.CallOnTryFire();
        }
        #endregion

        #region Initialization
        protected virtual void SubToEvents()
        {
            myEventHandler.EventPlayerCommandAttackEnemy += HandlePlayerCommandAttackEnemy;
            myEventHandler.EventAICommandAttackEnemy += HandleAICommandAttackEnemy;
            myEventHandler.EventStopTargettingEnemy += HandleStopTargetting;
            myEventHandler.EventToggleIsShooting += TogglebIsShooting;
            myEventHandler.EventPlayerCommandMove += HandleOnPlayerMoveAlly;
            myEventHandler.EventAIMove += HandleOnAIMoveAlly;
            myEventHandler.EventFinishedMoving += HandleOnAIStopMoving;
            gamemaster.EventEnableCameraMovement += OnEnableCameraMovement;
        }

        protected virtual void UnSubFromEvents()
        {
            myEventHandler.EventPlayerCommandAttackEnemy -= HandlePlayerCommandAttackEnemy;
            myEventHandler.EventAICommandAttackEnemy -= HandleAICommandAttackEnemy;
            myEventHandler.EventStopTargettingEnemy -= HandleStopTargetting;
            myEventHandler.EventToggleIsShooting -= TogglebIsShooting;
            myEventHandler.EventPlayerCommandMove -= HandleOnPlayerMoveAlly;
            myEventHandler.EventAIMove -= HandleOnAIMoveAlly;
            myEventHandler.EventFinishedMoving -= HandleOnAIStopMoving;
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