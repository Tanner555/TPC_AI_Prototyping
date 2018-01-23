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
        #region Common Properties
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

        //RTSPrototype.AllyMemberWrapper allyMember
        //{
        //    get
        //    {
        //        if (_allyMember == null)
        //            _allyMember = GetComponent<RTSPrototype.AllyMemberWrapper>();

        //        return _allyMember;
        //    }
        //}
        //RTSPrototype.AllyMemberWrapper _allyMember = null;

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
        #endregion
        /// <summary>
        /// RTSPrototype-OpsiveTPC-Health: Use isCurrentPlayer Property
        /// To Make Sure only the Current Player Receives the UI updates.
        /// </summary>
        //void DamageLocal_SetHealthAmount_SetShieldAmount()
        //{
        //    if (allyMember.isCurrentPlayer)
        //        ExecuteEvent("MyEvent", gameObject);
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

    }
}