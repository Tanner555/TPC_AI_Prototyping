using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RTSCoreFramework;
/// <summary>
/// Use to document what code changes I make to certain classes
/// and functions inside Opsive's third person controller
/// </summary>
/// 
namespace RTSPrototype
{
    public class OpsiveTPCRewriteImplementation : MonoBehaviour
    {
        #region Used Code
        //RTSPrototype.AllyMemberWrapper rootAlly
        //{
        //    get
        //    {
        //        if (_rootAlly == null)
        //            _rootAlly = transform.root.GetComponent<RTSPrototype.AllyMemberWrapper>();

        //        return _rootAlly;
        //    }
        //}
        //RTSPrototype.AllyMemberWrapper _rootAlly = null;

        //RTSCoreFramework.RTSGameMode gamemode
        //{
        //    get { return RTSCoreFramework.RTSGameMode.thisInstance; }
        //}

        //bool validShot
        //{
        //    get
        //    {
        //        return rootAlly != null &&
        //          rootAlly.ChestTransform &&
        //          rootAlly.enemyTargetWrapper != null;
        //    }
        //}
        /// <summary>
        /// RTSPrototype-OpsiveTPC-ShootableWeapon: Inside HitscanFire() method, comment out code
        /// and insert this function. Replaces default hitscan fire with autotargeting ally target.
        /// </summary>
        //void OnRTSHitscanFire()
        //{
        //    var fireDirection = FireDirection();
        //    RaycastHit m_RaycastHit;
        //    if (validShot && Physics.Linecast(rootAlly.ChestTransform.position, rootAlly.enemyTargetWrapper.ChestTransform.position, out m_RaycastHit))
        //    {
        //        // Make sure we hit an ally before subtracting health
        //        Transform _enemyRoot = m_RaycastHit.transform.root;
        //        bool _isEnemy = _enemyRoot.tag == gamemode.AllyTag;
        //        // If the Health component exists it will apply a force to the rigidbody in addition to deducting the health. Otherwise just apply the force to the rigidbody. 
        //        Health hitHealth;
        //        if (_isEnemy && (hitHealth = m_RaycastHit.transform.GetComponentInParent<Health>()) != null)
        //        {
        //            hitHealth.Damage(m_HitscanDamageAmount, m_RaycastHit.point, fireDirection * m_HitscanImpactForce, m_Character, m_RaycastHit.transform.gameObject);
        //            _enemyRoot.SendMessage("SetDamageInstigator", rootAlly, SendMessageOptions.DontRequireReceiver);
        //        }
        //        else if (m_HitscanImpactForce > 0 && m_RaycastHit.rigidbody != null && !m_RaycastHit.rigidbody.isKinematic)
        //        {
        //            m_RaycastHit.rigidbody.AddForceAtPosition(fireDirection * m_HitscanImpactForce, m_RaycastHit.point);
        //        }
        //        AddHitscanEffects(m_RaycastHit.transform, m_RaycastHit.point, m_RaycastHit.normal);
        //    }
        //    else if (Physics.Raycast(m_FirePoint.position, fireDirection, out m_RaycastHit, m_HitscanFireRange, m_HitscanImpactLayers.value, QueryTriggerInteraction.Ignore))
        //    {
        //        if (m_HitscanImpactForce > 0 && m_RaycastHit.rigidbody != null && !m_RaycastHit.rigidbody.isKinematic)
        //        {
        //            m_RaycastHit.rigidbody.AddForceAtPosition(fireDirection * m_HitscanImpactForce, m_RaycastHit.point);
        //        }
        //        else if (m_Tracer)
        //        {
        //            AddHitscanTracer(m_FirePoint.position + fireDirection * 1000);
        //        }
        //        AddHitscanEffects(m_RaycastHit.transform, m_RaycastHit.point, m_RaycastHit.normal);
        //    }
        //    else if (m_Tracer)
        //    {
        //        AddHitscanTracer(m_FirePoint.position + fireDirection * 1000);
        //    }
        //}
        #endregion

        #region Unused Code
        /// <summary>
        /// RTSPrototype-OpsiveTPC-RigidbodyCharacterController: Similar to Move Method, but rotates dummy
        /// instead of character.
        /// </summary>
        /// <param name="horizontalMovement">-1 to 1 value specifying the amount of horizontal movement.</param>
        /// <param name="forwardMovement">-1 to 1 value specifying the amount of forward movement.</param>
        /// <param name="lookRotation">The direction the Dummy should look or move relative to.</param>
        /// <param name="dummyTransform">The Transform which will be rotated instead of the character</param>
        public void MoveWithIndependentRotation(float horizontalMovement, float forwardMovement, Quaternion lookRotation, ref Transform dummyTransform)
        {
            if (dummyTransform == null)
            {
                Debug.Log("No dummy transform on character");
                return;
            }
            //Note Insert after UpdateRotation() method
            // Rotate Dummy in the correct direction.
            //UpdateDummyRotation(lookRotation, ref dummyTransform);
            //if (allyMember && allyMember.isCurrentPlayer)
            //{
            //    Debug.Log(" MTranRot: " + m_Transform.rotation +
            //        " DummyTranRot: " + dummyTransform.rotation +
            //        " NormTrans: " + transform.rotation);
            //}
        }

        //Replaces mIsRotating field for dummy rotation method
        private bool bDummyIsRotating = false;
        private RaycastHit rDummyRayHit;
        /// <summary>
        /// RTSPrototype-OpsiveTPC-RigidbodyCharacterController: Similar to UpdateRotation Method, but rotates dummy
        /// instead of character.
        /// </summary>
        /// <param name="lookRotation">The direction the Dummy should look or move relative to.</param>
        /// <param name="dummyTransform">The Transform which will be rotated instead of the character</param>
        private void UpdateDummyRotation(Quaternion lookRotation, ref Transform dummyTransform)
        {
            //for (int i = 0; i < m_Abilities.Length; ++i)
            //{
            //    if (m_Abilities[i].IsActive && !m_Abilities[i].UpdateRotation())
            //    {
            //        return;
            //    }
            //}

            //var eulerRotation = dummyTransform.eulerAngles;

            //if ((m_MovementType == MovementType.Adventure || m_MovementType == MovementType.FourLegged) && !Aiming)
            //{
            //    // Face in the direction that the character is moving if not in combat mode.
            //    if (m_InputVector != Vector3.zero)
            //    {
            //        if (m_MovementType == MovementType.Adventure)
            //        {
            //            eulerRotation.y = Quaternion.LookRotation(lookRotation * m_InputVector.normalized).eulerAngles.y;
            //        }
            //        else
            //        {
            //            // Do not rotate when the character should move directly backwards.
            //            if (Mathf.Abs(m_InputVector.x) > 0.01f || m_InputVector.z > -0.01f)
            //            {
            //                eulerRotation.y = Quaternion.LookRotation(((Mathf.Abs(m_InputVector.x) < 0.01f && m_InputVector.z > 0.01) ? lookRotation : dummyTransform.rotation) * m_InputVector.normalized).eulerAngles.y;
            //                // Allow the character to take tight turns when turning and moving forward.
            //                if (Mathf.Abs(m_InputVector.x) > 0.01f && m_InputVector.z > 0.01)
            //                {
            //                    eulerRotation.y += 90 * Mathf.Sign(m_InputVector.x);
            //                }
            //            }
            //        }
            //    }
            //}
            //else
            //{
            //    // Do not rotate if the delta angle between the character and the look rotation is less than a threshold. This wil allow the character's upper body to
            //    // look without having to rotate the entire character.
            //    var deltaAngle = Mathf.Abs(Mathf.DeltaAngle(eulerRotation.y, lookRotation.eulerAngles.y));
            //    if (deltaAngle < m_TorsoLookThreshold && !bDummyIsRotating)
            //    {
            //        return;
            //    }

            //    bDummyIsRotating = deltaAngle > 0.1f;
            //    eulerRotation.y = lookRotation.eulerAngles.y;
            //}

            //eulerRotation.y = Mathf.LerpAngle(dummyTransform.eulerAngles.y, eulerRotation.y, (Aiming ? m_AimRotationSpeed : m_RotationSpeed) * Time.deltaTime);

            //var rotation = Quaternion.Euler(eulerRotation);
            //if (m_AlignToGround && m_Grounded)
            //{
            //    // The normal is determined by the height of the front and the back of the character.
            //    var frontPoint = dummyTransform.position;
            //    if (Physics.Raycast(dummyTransform.position + (dummyTransform.up * 0.1f) + dummyTransform.forward * m_AlignToGroundDepthOffset * Mathf.Sign(m_InputVector.z), -dummyTransform.up, out rDummyRayHit, float.MaxValue, LayerManager.Mask.Ground, QueryTriggerInteraction.Ignore))
            //    {
            //        frontPoint = rDummyRayHit.point;
            //    }
            //    var backPoint = frontPoint;
            //    if (Physics.Raycast(dummyTransform.position + (dummyTransform.up * 0.1f) + dummyTransform.forward * m_AlignToGroundDepthOffset * -Mathf.Sign(m_InputVector.z), -dummyTransform.up, out rDummyRayHit, float.MaxValue, LayerManager.Mask.Ground, QueryTriggerInteraction.Ignore))
            //    {
            //        backPoint = rDummyRayHit.point;
            //    }
            //    var direction = (frontPoint - backPoint);
            //    var normal = Vector3.Cross(direction, Vector3.Cross(Vector3.up, direction)).normalized;
            //    // Do not rotate unless the slope is less than the slope limit.
            //    if (Mathf.Acos(normal.y) * Mathf.Rad2Deg < m_SlopeLimit)
            //    {
            //        // Rotate the character to always stay flat on the ground.
            //        var proj = (rotation * Vector3.forward) - (Vector3.Dot((rotation * Vector3.forward), normal)) * normal;
            //        var targetEuler = Quaternion.LookRotation(proj, normal).eulerAngles;
            //        targetEuler.z = dummyTransform.eulerAngles.z;
            //        rotation = Quaternion.Slerp(rotation, Quaternion.Euler(targetEuler), m_AlignToGroundRotationSpeed * Time.deltaTime);
            //    }
            //}

            //// Apply the rotation.
            //dummyTransform.rotation = rotation;
        }
        #endregion

    }
}