﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RTSCoreFramework
{
    public class AllyEventHandler : MonoBehaviour
    {
        #region FieldsAndProps
        public bool isSprinting { get; protected set; }
        public bool bIsTacticsEnabled { get; protected set; }
        //Is moving through nav mesh agent, regardless of
        //whether it's ai or a command
        public bool bIsNavMoving { get { return _bIsNavMoving; } }
        private bool _bIsNavMoving = false;
        public bool bIsCommandMoving
        {
            get { return _bIsCommandMoving; }
            set { _bIsCommandMoving = value; UpdatebIsNavMoving(); }
        }
        private bool _bIsCommandMoving = false;
        public bool bIsAIMoving
        {
            get { return _bIsAIMoving; }
            set { _bIsAIMoving = value; UpdatebIsNavMoving(); }
        }
        private bool _bIsAIMoving = false;
        public bool bIsFreeMoving { get; protected set; }
        public bool bIsCommandShooting { get; protected set; }
        public bool bIsAiShooting { get; protected set; }
        public bool bIsAimingToShoot { get; protected set; }
        public bool bCanEnableAITactics
        {
            get { return (bIsCommandMoving == true || bIsFreeMoving == true) == false && bIsCommandShooting == false; }
        }
        
        #endregion

        #region DelegatesAndEvents
        public delegate void GeneralEventHandler();
        public delegate void GeneralOneBoolHandler(bool _enable);
        public event GeneralEventHandler EventNpcDie;
        public event GeneralEventHandler EventNpcLowHealth;
        public event GeneralEventHandler EventNpcHealthRecovered;
        public event GeneralEventHandler EventNpcWalkAnim;
        public event GeneralEventHandler EventNpcStruckAnim;
        public event GeneralEventHandler EventNpcAttackAnim;
        public event GeneralEventHandler EventNpcRecoveredAnim;
        public event GeneralEventHandler EventNpcIdleAnim;
        public event GeneralEventHandler EventInventoryChanged;
        public event GeneralEventHandler EventHandsEmpty;
        public event GeneralEventHandler EventAmmoChanged;
        public event GeneralEventHandler EventSwitchingFromCom;
        public event GeneralEventHandler EventPartySwitching;
        public event GeneralEventHandler EventSetAsCommander;
        public event GeneralEventHandler EventKilledEnemy;
        public event GeneralEventHandler EventStopTargettingEnemy;
        public event GeneralEventHandler EventFinishedMoving;
        public event GeneralEventHandler EventToggleIsSprinting;
        public event GeneralOneBoolHandler EventToggleAllyTactics;
        public event GeneralOneBoolHandler EventTogglebIsFreeMoving;
        public event GeneralOneBoolHandler EventToggleIsShooting;
        //Opsive TPC Events
        public event GeneralEventHandler OnSwitchToPrevItem;
        public event GeneralEventHandler OnSwitchToNextItem;
        public event GeneralEventHandler OnTryFire;
        public event GeneralEventHandler OnTryReload;
        public event GeneralEventHandler OnTryCrouch;
        public event GeneralOneBoolHandler OnTryAim;

        //public delegate void AmmoChangeEventHandler(Gun_Master gun, string ammoType, int currentAmmo, int carriedAmmo);
        //public event AmmoChangeEventHandler EventAmmoChanged;

        public delegate void HealthEventHandler(float health);
        public event HealthEventHandler EventNpcDeductHealth;
        public event HealthEventHandler EventNpcIncreaseHealth;

        public delegate void AmmoPickupEventHandler(string ammoName, int quantity);
        public event AmmoPickupEventHandler EventPickedUpAmmo;

        public delegate void NPCRelationsChangeEventHandler();
        public event NPCRelationsChangeEventHandler EventNPCRelationsChange;

        public delegate void RtsHitTypeAndRayCastHitHandler(rtsHitType hitType, RaycastHit hit);
        public event RtsHitTypeAndRayCastHitHandler OnHoverOver;
        public event RtsHitTypeAndRayCastHitHandler OnHoverLeave;
        public event RtsHitTypeAndRayCastHitHandler EventPlayerCommandMove;

        public delegate void GeneralVector3Handler(Vector3 _point);
        public event GeneralVector3Handler EventAIMove;

        public delegate void AllyHandler(AllyMember ally);
        public event AllyHandler EventPlayerCommandAttackEnemy;
        public event AllyHandler EventAICommandAttackEnemy;

        //public delegate void NavSpeedHandler(AllyMoveSpeed _navSpeed);
        //public event NavSpeedHandler EventSetNavSpeed;
        #endregion

        #region UnityMessages
        protected virtual void Awake()
        {
            isSprinting = true;
            bIsTacticsEnabled = false;
            bIsAimingToShoot = false;
            bIsFreeMoving = false;
            bIsCommandShooting = false;
            bIsAiShooting = false;
        }

        protected virtual void OnDisable()
        {

        }
        #endregion

        #region EventCalls
        public void CallEventNpcDie()
        {
            if (EventNpcDie != null)
            {
                EventNpcDie();
                this.enabled = false;
            }
        }

        public void CallEventNpcLowHealth()
        {
            if (EventNpcLowHealth != null)
            {
                EventNpcLowHealth();
            }
        }

        public void CallEventNpcHealthRecovered()
        {
            if (EventNpcHealthRecovered != null)
            {
                EventNpcHealthRecovered();
            }
        }

        public void CallEventNpcWalkAnim()
        {
            if (EventNpcWalkAnim != null)
            {
                EventNpcWalkAnim();
            }
        }

        public void CallEventNpcStruckAnim()
        {
            if (EventNpcStruckAnim != null)
            {
                EventNpcStruckAnim();
            }
        }

        public void CallEventNpcAttackAnim()
        {
            if (EventNpcAttackAnim != null)
            {
                EventNpcAttackAnim();
            }
        }

        public void CallEventNpcRecoveredAnim()
        {
            if (EventNpcRecoveredAnim != null)
            {
                EventNpcRecoveredAnim();
            }
        }

        public void CallEventNpcIdleAnim()
        {
            if (EventNpcIdleAnim != null)
            {
                EventNpcIdleAnim();
            }
        }

        public void CallEventToggleIsShooting(bool _enable)
        {
            bIsAimingToShoot = _enable;
            if (EventToggleIsShooting != null)
            {
                EventToggleIsShooting(_enable);
            }
        }

        public void CallEventToggleIsSprinting()
        {
            isSprinting = !isSprinting;
            if (EventToggleIsSprinting != null) EventToggleIsSprinting();
        }

        public void CallEventFinishedMoving()
        {
            bIsCommandMoving = false;
            bIsAIMoving = false;
            if (bCanEnableAITactics)
            {
                CallEventToggleAllyTactics(true);
            }          
            if (EventFinishedMoving != null) EventFinishedMoving();
        }

        public void CallOnSwitchToPrevItem()
        {
            if (OnSwitchToPrevItem != null) OnSwitchToPrevItem();
        }

        public void CallOnSwitchToNextItem()
        {
            if (OnSwitchToNextItem != null) OnSwitchToNextItem();
        }

        public void CallOnTryFire()
        {
            if (OnTryFire != null) OnTryFire();
        }

        public void CallOnTryReload()
        {
            if (OnTryReload != null) OnTryReload();
        }

        public void CallOnTryCrouch()
        {
            if (OnTryCrouch != null) OnTryCrouch();
        }

        public void CallOnTryAim(bool _enable)
        {
            if (OnTryAim != null) OnTryAim(_enable);
        }

        public void CallEventNpcDeductHealth(float health)
        {
            if (EventNpcDeductHealth != null)
            {
                EventNpcDeductHealth(health);
            }
        }

        public void CallEventNpcIncreaseHealth(float health)
        {
            if (EventNpcIncreaseHealth != null)
            {
                EventNpcIncreaseHealth(health);
            }
        }

        public void CallEventNPCRelationsChange()
        {
            if (EventNPCRelationsChange != null)
            {
                EventNPCRelationsChange();
            }
        }

        public void CallEventPickedUpAmmo(string ammoName, int quantity)
        {
            if (EventPickedUpAmmo != null)
            {
                EventPickedUpAmmo(ammoName, quantity);
            }
        }

        public void CallEventInventoryChanged()
        {
            if (EventInventoryChanged != null)
            {
                EventInventoryChanged();
            }
        }

        public void CallEventHandsEmpty()
        {
            if (EventHandsEmpty != null)
            {
                EventHandsEmpty();
            }
        }

        public void CallEventAmmoChanged()
        {
            if (EventAmmoChanged != null)
            {
                EventAmmoChanged();
            }
        }

        public void CallEventSwitchingFromCom()
        {
            if (EventSwitchingFromCom != null) EventSwitchingFromCom();
        }

        public void CallEventPartySwitching()
        {
            if (EventPartySwitching != null) EventPartySwitching();
        }

        public void CallEventSetAsCommander()
        {
            if (EventSetAsCommander != null) EventSetAsCommander();
        }

        public void CallEventKilledEnemy()
        {
            if (EventKilledEnemy != null) EventKilledEnemy();
        }

        public void CallEventOnHoverOver(rtsHitType hitType, RaycastHit hit)
        {
            if (OnHoverOver != null)
            {
                OnHoverOver(hitType, hit);
            }
        }

        public void CallEventOnHoverLeave(rtsHitType hitType, RaycastHit hit)
        {
            if (OnHoverLeave != null)
            {
                OnHoverLeave(hitType, hit);
            }
        }

        public void CallEventCommandMove(rtsHitType hitType, RaycastHit hit)
        {
            bIsAimingToShoot = bIsCommandShooting = bIsAiShooting = false;
            bIsCommandMoving = true;
            bIsAIMoving = false;
            if (EventPlayerCommandMove != null)
            {
                EventPlayerCommandMove(hitType, hit);
            }
        }

        public void CallEventAIMove(Vector3 _point)
        {
            bIsAimingToShoot = false;
            bIsCommandShooting = false;
            bIsAIMoving = true;
            bIsCommandMoving = false;
            if (EventAIMove != null) EventAIMove(_point);
        }

        public void CallEventCommandAttackEnemy(AllyMember ally)
        {
            bIsAIMoving = bIsCommandMoving = false;
            bIsCommandShooting = true;
            bIsAiShooting = false;
            if (EventPlayerCommandAttackEnemy != null)
            {
                EventPlayerCommandAttackEnemy(ally);
            }
        }

        public void CallEventAICommandAttackEnemy(AllyMember ally)
        {
            bIsAIMoving = bIsCommandMoving = false;
            bIsAiShooting = true;
            bIsCommandShooting = false;
            if (EventAICommandAttackEnemy != null)
            {
                EventAICommandAttackEnemy(ally);
            }
        }

        public void CallEventStopTargettingEnemy()
        {
            bIsCommandShooting = bIsAiShooting = bIsAimingToShoot = false;
            if (EventStopTargettingEnemy != null) EventStopTargettingEnemy();
        }

        public void CallEventTogglebIsFreeMoving(bool _enable)
        {
            bIsFreeMoving = _enable;
            bool _enableTactics = _enable && bCanEnableAITactics;
            CallEventToggleAllyTactics(_enableTactics);
            if (EventTogglebIsFreeMoving != null) EventTogglebIsFreeMoving(_enable);
        }

        //Want event handler to control event, makes code more centralized
        private void CallEventToggleAllyTactics(bool _enable)
        {
            bIsTacticsEnabled = _enable;
            if (EventToggleAllyTactics != null) EventToggleAllyTactics(_enable);
        }
        #endregion

        #region Helpers
        void UpdatebIsNavMoving()
        {
            _bIsNavMoving = (bIsCommandMoving || bIsAIMoving);
        }
        #endregion

    }
}