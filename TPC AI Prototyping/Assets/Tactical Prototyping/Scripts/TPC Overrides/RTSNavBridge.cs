using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Opsive.ThirdPersonController;
using RTSCoreFramework;
using System;

namespace RTSPrototype
{
    public class RTSNavBridge : NavMeshAgentBridge
    {
        #region FieldsandProps
        AllyMember allyMember
        {
            get
            {
                if (_allyMember == null)
                    _allyMember = GetComponent<AllyMember>();

                return _allyMember;
            }
        }
        AllyMember _allyMember = null;

        AllyEventHandlerWrapper myEventHandler { get { return GetComponent<AllyEventHandlerWrapper>(); } }
        RTSGameMaster gamemaster { get { return RTSGameMaster.thisInstance; } }
        //Targeting
        public Transform targetTransform { get; protected set; }
        public bool isTargeting { get; protected set; }
        protected Quaternion lookTargetRotation
        {
            get
            {
                return targetTransform != null ? Quaternion.LookRotation(targetTransform.position - transform.position) : new Quaternion();
            }
        }
        protected Quaternion lookDestinationRotation
        {
            get
            {
                return (isMoving && !ReachedDestination()) ?
                    Quaternion.LookRotation(m_NavMeshAgent.destination - transform.position) :
                    Quaternion.LookRotation(m_Transform.forward);
            }
        }
        protected Quaternion mainCameraRotation
        {
            get
            {
                return Camera.main ? Camera.main.transform.rotation : new Quaternion(0, 0, 0, 0);
            }
        }

        protected bool canUpdateMovement
        {
            get { return isMoving == true; }
        }

        //NavMeshMovement
        bool isMoving { get { return myEventHandler.bIsNavMoving; } }

        //Camera is Moving
        private bool moveCamera = false;
        
        //See if need to rotate or continue moving
        bool bIsShooting = false;
        //LookRotation Local Variable
        Quaternion lookRotation;
        //Sprinting
        bool bIsSprinting = false;
        float speedMultiplier = 1f;
        float walkSpeed = 1f;
        float sprintSpeed = 1.5f;
        #endregion

        #region UnityMessages
        protected override void FixedUpdate()
        {
            UpdateMovementOrRotate();

        }

        private void Start()
        {
            OnToggleSprinting();
            SubToEvents();
        }

        private void OnDisable()
        {
            UnsubFromEvents();
        }
        #endregion

        #region Targetting
        public void LookAtTarget(Transform _target)
        {
            if (_target != null)
            {
                targetTransform = _target;
                isTargeting = true;
            }
            else
            {
                StopTargeting();
            }
        }

        public void StopTargeting()
        {
            targetTransform = null;
            isTargeting = false;
        }
        #endregion

        #region Handlers
        void OnPlayerCommandAttack(AllyMember _ally)
        {
            if (_ally != null)
                LookAtTarget(_ally.transform);
        }

        void OnAICommandAttack(AllyMember _ally)
        {
            if (_ally != null)
                LookAtTarget(_ally.transform);
        }

        void OnToggleSprinting()
        {
            bIsSprinting = myEventHandler.isSprinting;
            speedMultiplier = bIsSprinting ?
                sprintSpeed : walkSpeed;

        }

        void OnCommandStopTargetting()
        {
            StopTargeting();
        }

        void ToggleMoveCamera(bool _enable)
        {
            moveCamera = _enable;
        }

        void TogglebIsShooting(bool _enable)
        {
            bIsShooting = _enable;
        }
        #endregion

        #region NavMeshMovement
        public void MoveToDestination(Vector3 _destination)
        {
            m_NavMeshAgent.SetDestination(_destination);
        }

        void FinishMovingNavMesh()
        {
            myEventHandler.CallEventFinishedMoving();
        }

        bool ReachedDestination()
        {
            return m_NavMeshAgent != null && m_NavMeshAgent.enabled && m_NavMeshAgent.remainingDistance != Mathf.Infinity &&
                m_NavMeshAgent.remainingDistance <= 0.2f && !m_NavMeshAgent.pathPending && isMoving && m_NavMeshAgent.hasPath;
        }
        #endregion

        #region MainMovementMethod
        void MoveCharacterMain()
        {
            var velocity = Vector3.zero;
            lookRotation = Quaternion.LookRotation(m_Transform.forward);

            if (m_NavMeshAgent.isOnOffMeshLink)
            {
                UpdateOffMeshLink(ref velocity, ref lookRotation);
            }
            else
            {
                // Only move if a path exists.
                // Update only when needed by targeting or move command
                if (canUpdateMovement && m_NavMeshAgent.desiredVelocity.sqrMagnitude > 0.01f)
                {
                    if (m_NavMeshAgent.updateRotation)
                    {
                        lookRotation = Quaternion.LookRotation(m_NavMeshAgent.desiredVelocity);
                    }
                    else
                    {
                        lookRotation = Quaternion.LookRotation(m_Transform.forward);
                    }
                    // The normalized velocity should be relative to the look direction.
                    velocity = Quaternion.Inverse(lookRotation) * m_NavMeshAgent.desiredVelocity;
                    // Only normalize if the magnitude is greater than 1. This will allow the character to walk.
                    if (velocity.sqrMagnitude > 1)
                    {
                        velocity.Normalize();
                    }
                    // Smoothly come to a stop at the destination.
                    if (m_NavMeshAgent.remainingDistance < 1f)
                    {
                        velocity *= m_ArriveRampDownCurve.Evaluate(1 - m_NavMeshAgent.remainingDistance);
                    }
                    else
                    {
                        //Change Velocity to Speed Multiplier
                        velocity *= speedMultiplier;
                    }
                }
            }

            // Don't let the NavMeshAgent move the character - the controller can move it.
            m_NavMeshAgent.updatePosition = false;
            m_NavMeshAgent.velocity = Vector3.zero;
            m_Controller.Move(velocity.x, velocity.z, lookRotation);
            m_NavMeshAgent.nextPosition = m_Transform.position;

            //Check for end of destination if moving
            if (isMoving && ReachedDestination()) FinishMovingNavMesh();
        }
        #endregion

        #region MoveOrRotateMethod
        void UpdateMovementOrRotate()
        {
            //Change localRotation if targetting is active
            if (bIsShooting && isTargeting && targetTransform != null)
            {
                //Stand Still and Rotate towards Target
                m_NavMeshAgent.updatePosition = false;
                m_NavMeshAgent.velocity = Vector3.zero; 
                Vector3 velocity = Vector3.zero;
                lookRotation = lookTargetRotation;
                m_Controller.Move(velocity.x, velocity.z, lookRotation);
            }
            else
            {
                //Still targetting enemy but enemy transform is null
                if (targetTransform == null && isTargeting)
                    myEventHandler.CallEventStopTargettingEnemy();

                MoveCharacterMain();
            }   
        }
        #endregion

        #region TestingNewRotation
        #region TestFields
        ////Velocity Rotation
        //Quaternion myVelRotation;
        //[SerializeField]
        //private Transform myRotateTransform;
        //float myHorizontalMovement = 0.0f;
        //float myForwardMovement = 0.0f;
        //Vector3 previousMMovement;
        //Quaternion mLookRotation;
        #endregion

        #region MoveFreeLookTesting
        //private void moveCharacterFreeLook()
        //{
        //    myHorizontalMovement = RTSPlayerInput.thisInstance.GetAxisRaw(Constants.HorizontalInputName);
        //    myForwardMovement = RTSPlayerInput.thisInstance.GetAxisRaw(Constants.ForwardInputName);
        //    MoveCameraTesting();
        //    //var direction = Vector3.zero;
        //    //direction.x = myHorizontalMovement;
        //    //direction.z = myForwardMovement;
        //    //direction.y = 0;
        //    //if(direction.sqrMagnitude > 0.01f)
        //    //{
        //    //    if(isMoving == true)
        //    //    {
        //    //        FinishMovingNavMesh();
        //    //    }

        //    //}

        //    //Quaternion _look = mainCameraRotation;

        //    //m_Controller.LookInMoveDirection = true;
        //    m_NavMeshAgent.updatePosition = false;
        //    m_NavMeshAgent.velocity = Vector3.zero;
        //    //m_Controller.MoveWithIndependentRotation(myHorizontalMovement, myForwardMovement, mLookRotation, ref myRotateTransform);
        //}

        //void MoveCameraTesting()
        //{
        //    mLookRotation = mainCameraRotation;
        //    //var mousePosition = (Vector3)RTSPlayerInput.thisInstance.GetMousePosition();
        //    //if ((mousePosition - previousMMovement).sqrMagnitude > 0.1f /*&& !m_Controller.IndependentLook()*/)
        //    //{
        //    //    var ray = Camera.main.ScreenPointToRay(mousePosition);
        //    //    RaycastHit hit;
        //    //    if (Physics.Raycast(ray, out hit, Mathf.Infinity, LayerManager.Mask.IgnoreInvisibleLayersPlayer, QueryTriggerInteraction.Ignore))
        //    //    {
        //    //        //var hitPosition = hit.point;
        //    //        //hitPosition.y = m_Transform.position.y;
        //    //        //mLookRotation = Quaternion.LookRotation(hitPosition - transform.position);

        //    //        var direction = mousePosition - Camera.main.WorldToScreenPoint(m_Transform.position + m_Controller.CapsuleCollider.center);
        //    //        // Convert the XY direction to an XYZ direction with Y equal to 0.
        //    //        direction.z = direction.y;
        //    //        direction.y = 0;
        //    //        mLookRotation = Quaternion.LookRotation(direction);
        //    //    }
        //    //    else
        //    //    {
        //    //        var direction = mousePosition - Camera.main.WorldToScreenPoint(m_Transform.position + m_Controller.CapsuleCollider.center);
        //    //        // Convert the XY direction to an XYZ direction with Y equal to 0.
        //    //        direction.z = direction.y;
        //    //        direction.y = 0;
        //    //        mLookRotation = Quaternion.LookRotation(direction);
        //    //    }
        //    //}
        //    //else
        //    //{
        //    //    var direction = Vector3.zero;
        //    //    direction.x = RTSPlayerInput.thisInstance.GetAxisRaw(Constants.YawInputName);
        //    //    direction.z = RTSPlayerInput.thisInstance.GetAxisRaw(Constants.PitchInputName);
        //    //    if (direction.sqrMagnitude > 0.1f)
        //    //    {
        //    //        mLookRotation = Quaternion.LookRotation(direction);
        //    //    }
        //    //    else
        //    //    {
        //    //        mLookRotation = m_Transform.rotation;
        //    //    }
        //    //}
        //    //previousMMovement = mousePosition;
        //}
        #endregion

        #region ReplaceMoveTesting
        //void ReplaceFixedMoveTesting()
        //{
        //    if (myRotateTransform == null)
        //    {
        //        Debug.Log("No Dummy Transform on RTSNavBridge");
        //        return;
        //    }

        //    var velocity = Vector3.zero;
        //    //Change localRotation if targetting is active
        //    if (isTargeting && targetTransform != null)
        //    {
        //        lookRotation = lookTargetRotation;
        //    }
        //    else
        //    {
        //        //Still targetting enemy but enemy transform is null
        //        if (isTargeting)
        //            myEventHandler.CallEventStopTargettingEnemy();

        //        //lookRotation = Quaternion.LookRotation(m_Transform.forward);
        //        lookRotation = Quaternion.LookRotation(myRotateTransform.forward);
        //    }

        //    if (m_NavMeshAgent.isOnOffMeshLink)
        //    {
        //        Debug.Log("On off mesh link");
        //        UpdateOffMeshLink(ref velocity, ref lookRotation);
        //    }
        //    else
        //    {
        //        // Only move if a path exists.
        //        // Update only when needed by targeting or move command
        //        if (canUpdateMovement && m_NavMeshAgent.desiredVelocity.sqrMagnitude > 0.01f)
        //        {
        //            if (m_NavMeshAgent.updateRotation)
        //            {
        //                lookRotation = Quaternion.LookRotation(m_NavMeshAgent.desiredVelocity);
        //            }
        //            else
        //            {
        //                //lookRotation = Quaternion.LookRotation(m_Transform.forward);
        //                lookRotation = Quaternion.LookRotation(myRotateTransform.forward);
        //            }
        //            // The normalized velocity should be relative to the look direction.
        //            velocity = Quaternion.Inverse(lookRotation) * m_NavMeshAgent.desiredVelocity;
        //            // Only normalize if the magnitude is greater than 1. This will allow the character to walk.
        //            if (velocity.sqrMagnitude > 1)
        //            {
        //                velocity.Normalize();
        //                // Smoothly come to a stop at the destination.
        //                if (m_NavMeshAgent.remainingDistance < 1f)
        //                {
        //                    velocity *= m_ArriveRampDownCurve.Evaluate(1 - m_NavMeshAgent.remainingDistance);
        //                }
        //            }
        //        }
        //    }

        //    // Don't let the NavMeshAgent move the character - the controller can move it.
        //    m_NavMeshAgent.updatePosition = false;
        //    m_NavMeshAgent.velocity = Vector3.zero;
        //    //Quaternion _look = isTargeting == false ? lookRotation : lookTargetRotation;
        //    m_Controller.MoveWithIndependentRotation(velocity.x, velocity.z, lookRotation, ref myRotateTransform);
        //    m_NavMeshAgent.nextPosition = m_Transform.position;

        //    //Check for end of destination if moving
        //    if (isMoving && ReachedDestination()) FinishMovingNavMesh();
        //}
        #endregion

        #endregion       

        #region Initialization
        void SubToEvents()
        {
            myEventHandler.EventPlayerCommandAttackEnemy += OnPlayerCommandAttack;
            myEventHandler.EventAICommandAttackEnemy += OnAICommandAttack;
            myEventHandler.EventStopTargettingEnemy += OnCommandStopTargetting;
            myEventHandler.EventToggleIsShooting += TogglebIsShooting;
            myEventHandler.EventToggleIsSprinting += OnToggleSprinting;
            gamemaster.EventEnableCameraMovement += ToggleMoveCamera;
        }

        void UnsubFromEvents()
        {
            myEventHandler.EventPlayerCommandAttackEnemy -= OnPlayerCommandAttack;
            myEventHandler.EventAICommandAttackEnemy -= OnAICommandAttack;
            myEventHandler.EventStopTargettingEnemy -= OnCommandStopTargetting;
            myEventHandler.EventToggleIsShooting -= TogglebIsShooting;
            myEventHandler.EventToggleIsSprinting -= OnToggleSprinting;
            gamemaster.EventEnableCameraMovement -= ToggleMoveCamera;
        }
        #endregion
    }
}