using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Opsive.ThirdPersonController;

namespace RTSPrototype
{
    public class RTSNavBridge : NavMeshAgentBridge
    {
        //Targeting
        public Transform targetTransform { get; protected set; }
        public bool isTargeting { get; protected set; }
        protected Quaternion lookTargetRotation
        {
            get
            {
                return targetTransform != null ? Quaternion.LookRotation(targetTransform.transform.position - transform.position) : new Quaternion();
            }
        }

        public void LookAtTarget(Transform _target)
        {
            if(_target != null)
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

        protected override void FixedUpdate()
        {
            var velocity = Vector3.zero;
            //Change if Target is set and targetting is active
            Quaternion lookRotation;
            if (isTargeting && targetTransform != null)
                lookRotation = lookTargetRotation;
            else
                lookRotation = Quaternion.LookRotation(m_Transform.forward);

            if (m_NavMeshAgent.isOnOffMeshLink)
            {
                UpdateOffMeshLink(ref velocity, ref lookRotation);
            }
            else
            {
                // Only move if a path exists.
                if (m_NavMeshAgent.desiredVelocity.sqrMagnitude > 0.01f)
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
        }
    }
}