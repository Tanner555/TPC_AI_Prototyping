using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RTSCoreFramework;

namespace RTSPrototype
{
    /// <summary>
    /// Used to document what code changes I make to certain classes
    /// and functions inside Opsive's third person controller
    /// </summary>
    /// 
    public class OpsiveTPCRewriteImplementation : MonoBehaviour
    {
        #region Common Properties
        //Transform rootAllyTransform
        //{
        //    get
        //    {
        //        if (_rootAllyTransform == null)
        //        {
        //            _rootAllyTransform = transform.root;
        //        }
        //        return _rootAllyTransform;
        //    }
        //}
        //Transform _rootAllyTransform = null;
        #endregion

        #region Used Code
        /// <summary>
        /// RTSPrototype-OpsiveTPC-ShootableWeapon: Inside HitscanFire() method, comment out code
        /// and insert this function. Replaces default hitscan fire with autotargeting ally target.
        /// </summary>
        //void OnRTSHitscanFire()
        //{
        //    var fireDirection = FireDirection();
        //    var _force = fireDirection * m_HitscanImpactForce;
        //    rootAllyTransform.SendMessage("CallOnTryHitscanFire", _force, SendMessageOptions.RequireReceiver);
        //}

        /// <summary>
        /// RTSPrototype-OpsiveTPC-MeleeWeapon: Inside Attack() method, comment out all of the code.
        /// This method references the Health script that my game doesn't use. 
        /// </summary>
        //protected virtual void Attack(Transform hitTransform, Vector3 hitPoint, Vector3 hitNormal)
        //{
        //    Comment out the method code
        //}

        /// <summary>
        /// RTSPrototype-OpsiveTPC-MeleeWeapon: Inside TryUse() method, inside the second if statement
        /// right before where the method returns true, add this message call to the rootally
        /// </summary>
        //public override bool TryUse()
        //{
        //    if (!m_InUse && m_LastAttackTime + m_AttackDelay < Time.time)
        //    {
        //        .....
        //        .....
        //        rootAllyTransform.SendMessage("CallOnTryMeleeAttack", SendMessageOptions.RequireReceiver);
        //        return true;
        //    }
        //}

        /// <summary>
        /// RTSPrototype-OpsiveTPC-CameraHandler: Inside Update() method, comment out ln of code
        /// at the end, where m_StepZoom is being set.
        /// This allows me to use a custom stepzoom solution in my RTSCameraController inherited class.
        /// </summary>
        //void Update()
        //{
        //    m_StepZoom = m_CameraController.ActiveState.StepZoomSensitivity > 0 ?
        //    m_PlayerInput.GetAxisRaw(Constants.StepZoomName) : 0;
        //}
        #endregion

    }
}