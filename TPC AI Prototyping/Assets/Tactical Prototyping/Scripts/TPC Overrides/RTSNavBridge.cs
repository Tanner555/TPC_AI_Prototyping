using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Opsive.ThirdPersonController;
using RTSCoreFramework;

namespace RTSPrototype
{
    public class RTSNavBridge : NavMeshAgentBridge
    {
        #region FieldsandProps
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
        bool canUpdateMovement
        {
            get { return (!isMoving && !isTargeting) == false; }
        }
        //Camera is Moving
        private bool moveCamera = false;
        //NavMeshMovement
        bool isMoving = false;
        //LookRotation Local Variable
        Quaternion lookRotation;
        //Velocity Rotation
        Quaternion myVelRotation;
        #endregion

        #region UnityMessages
        protected override void FixedUpdate()
        {
            //Testing New Move Code
            //ReplaceFixedMove();
            //return;
            MoveCharacterMain();
        }

        private void Start()
        {
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
        void OnCommandAttack(AllyMember _ally)
        {
            if (_ally != null)
                LookAtTarget(_ally.transform);
        }

        void OnCommandStopTargetting()
        {
            StopTargeting();
        }

        void ToggleMoveCamera(bool enable)
        {
            moveCamera = enable;
        }
        #endregion

        #region NavMeshMovement
        public void MoveToDestination(Vector3 _destination)
        {
            m_NavMeshAgent.SetDestination(_destination);
            isMoving = true;
        }

        void FinishMovingNavMesh()
        {
            isMoving = false;
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
            //Change localRotation if targetting is active
            if (isTargeting && targetTransform != null)
            {
                lookRotation = lookTargetRotation;
            }
            else
            {
                //Still targetting enemy but enemy transform is null
                if (isTargeting)
                    myEventHandler.CallEventStopTargettingEnemy();

                lookRotation = Quaternion.LookRotation(m_Transform.forward);
            }

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
                        // Smoothly come to a stop at the destination.
                        if (m_NavMeshAgent.remainingDistance < 1f)
                        {
                            velocity *= m_ArriveRampDownCurve.Evaluate(1 - m_NavMeshAgent.remainingDistance);
                        }
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

        #region ReplaceMoveTesting
        void ReplaceFixedMoveTesting()
        {
            var velocity = Vector3.zero;
            //Change localRotation if targetting is active
            if (isTargeting && targetTransform != null)
            {
                lookRotation = lookTargetRotation;
            }
            else
            {
                //Still targetting enemy but enemy transform is null
                if (isTargeting)
                    myEventHandler.CallEventStopTargettingEnemy();

                //lookRotation = Quaternion.LookRotation(m_Transform.forward);
                lookRotation = lookDestinationRotation;
            }

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
                    myVelRotation = lookRotation;
                    if (m_NavMeshAgent.updateRotation)
                    {
                        myVelRotation = Quaternion.LookRotation(m_NavMeshAgent.desiredVelocity);
                        //lookRotation = Quaternion.LookRotation(m_NavMeshAgent.desiredVelocity);
                    }
                    else
                    {
                        //lookRotation = Quaternion.LookRotation(m_Transform.forward);
                        //myVelRotation = lookDestinationRotation;
                    }
                    // The normalized velocity should be relative to the look direction.
                    velocity = (Quaternion.Inverse(lookRotation) * m_NavMeshAgent.desiredVelocity);
                    // Only normalize if the magnitude is greater than 1. This will allow the character to walk.
                    if (velocity.sqrMagnitude > 1)
                    {
                        //velocity.Normalize();
                        // Smoothly come to a stop at the destination.
                        if (m_NavMeshAgent.remainingDistance < 1f)
                        {
                            velocity *= m_ArriveRampDownCurve.Evaluate(1 - m_NavMeshAgent.remainingDistance);
                        }
                    }
                    else
                    {
                        if(true/*velocity.sqrMagnitude < 0.3*/)
                        {
                            velocity *= 1.2f;
                        }
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

        #region Initialization
        void SubToEvents()
        {
            myEventHandler.EventCommandAttackEnemy += OnCommandAttack;
            myEventHandler.EventStopTargettingEnemy += OnCommandStopTargetting;
            gamemaster.EventEnableCameraMovement += ToggleMoveCamera;
        }

        void UnsubFromEvents()
        {
            myEventHandler.EventCommandAttackEnemy -= OnCommandAttack;
            myEventHandler.EventStopTargettingEnemy -= OnCommandStopTargetting;
            gamemaster.EventEnableCameraMovement -= ToggleMoveCamera;
        }
        #endregion
    }
}