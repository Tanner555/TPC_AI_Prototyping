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
        public bool bIsNavMoving { get { return bIsCommandMoving || bIsAIMoving; } }
        public bool bIsCommandMoving { get; protected set; }
        public bool bIsAIMoving { get; protected set; }
        public bool bIsFreeMoving { get; protected set; }
        public bool bIsCommandAttacking { get; protected set; }
        public bool bIsAiAttacking { get; protected set; }
        public bool bIsAimingToShoot { get; protected set; }
        public bool bCanEnableAITactics
        {
            get { return (bIsCommandMoving || 
                    bIsFreeMoving) == false && 
                    bIsCommandAttacking == false; }
        }

        //Ui Target Info
        public bool bAllyIsUiTarget { get; protected set; }
        //Character Weapon Stats
        public EEquipType MyEquippedType { get; protected set; }
        public EEquipType MyUnequippedType
        {
            get {
                return MyEquippedType == EEquipType.Primary ?
                  EEquipType.Secondary : EEquipType.Primary;
            }
        }
        public EWeaponType MyEquippedWeaponType { get; protected set; }
        public EWeaponType MyUnequippedWeaponType { get; protected set; }
        public int PrimaryLoadedAmmoAmount { get; protected set; }
        public int PrimaryUnloadedAmmoAmount { get; protected set; }
        public int SecondaryLoadedAmmoAmount { get; protected set; }
        public int SecondaryUnloadedAmmoAmount { get; protected set; }

        protected bool bHasStartedFromDelay = false;
        
        #endregion

        #region DelegatesAndEvents
        public delegate void GeneralEventHandler();
        public delegate void GeneralOneBoolHandler(bool _enable);
        public event GeneralEventHandler EventAllyDied;
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

        public delegate void RtsHitTypeAndRayCastHitHandler(rtsHitType hitType, RaycastHit hit);
        public event RtsHitTypeAndRayCastHitHandler OnHoverOver;
        public event RtsHitTypeAndRayCastHitHandler OnHoverLeave;

        public delegate void GeneralVector3Handler(Vector3 _point);
        public event GeneralVector3Handler EventCommandMove;

        public delegate void AllyHandler(AllyMember ally);
        public event AllyHandler EventCommandAttackEnemy;

        public delegate void EEquipTypeHandler(EEquipType _eType);
        public delegate void EWeaponTypeHandler(EEquipType _eType, EWeaponType _weaponType, bool _equipped);
        public event EEquipTypeHandler OnEquipTypeChanged;
        public event EWeaponTypeHandler OnWeaponChanged;

        public delegate void TwoIntArgsHandler(int _firstNum, int _secondNum);
        public event TwoIntArgsHandler OnAmmoChanged;
        public event TwoIntArgsHandler OnHealthChanged;

        #endregion

        #region UnityMessages
        protected virtual void Awake()
        {
            isSprinting = true;
            bIsTacticsEnabled = false;
            bIsAimingToShoot = false;
            bIsFreeMoving = false;
            bIsCommandAttacking = false;
            bIsAiAttacking = false;
        }

        protected virtual void Start()
        {
            Invoke("OnDelayStart", 0.5f);
            SubToEvents();
        }

        protected virtual void OnDelayStart()
        {
            bHasStartedFromDelay = true;
            SubToEventsLater();
        }

        protected virtual void OnDisable()
        {
            UnsubFromEvents();
        }
        #endregion

        #region Initialization
        protected virtual void SubToEvents()
        {

        }

        protected virtual void SubToEventsLater()
        {

        }

        protected virtual void UnsubFromEvents()
        {

        }
        #endregion

        #region EventCalls
        public virtual void CallEventAllyDied()
        {
            if (EventAllyDied != null)
            {
                EventAllyDied();
                this.enabled = false;
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

        public void CallEventSwitchingFromCom()
        {
            if (bIsFreeMoving) CallEventTogglebIsFreeMoving(false);
            if (EventSwitchingFromCom != null) EventSwitchingFromCom();
        }

        public void CallEventPartySwitching()
        {
            if (EventPartySwitching != null) EventPartySwitching();
        }

        public virtual void CallEventSetAsCommander()
        {
            CallEventFinishedMoving();
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
            bIsAimingToShoot = bIsCommandAttacking = bIsAiAttacking = false;
            bIsCommandMoving = true;
            bIsAIMoving = false;
            CallEventCommandMove(hit.point);
        }

        public void CallEventAIMove(Vector3 _point)
        {
            bIsAimingToShoot = false;
            bIsCommandAttacking = false;
            bIsAIMoving = true;
            bIsCommandMoving = false;
            CallEventCommandMove(_point);
        }

        private void CallEventCommandMove(Vector3 _point)
        {
            if (EventCommandMove != null) EventCommandMove(_point);
        }

        public void CallEventPlayerCommandAttackEnemy(AllyMember ally)
        {
            bIsAIMoving = bIsCommandMoving = false;
            bIsCommandAttacking = true;
            bIsAiAttacking = false;
            CallEventCommandAttackEnemy(ally);
        }

        public void CallEventAICommandAttackEnemy(AllyMember ally)
        {
            bIsAIMoving = bIsCommandMoving = false;
            bIsAiAttacking = true;
            bIsCommandAttacking = false;
            CallEventCommandAttackEnemy(ally);
        }

        private void CallEventCommandAttackEnemy(AllyMember ally)
        {
            if (EventCommandAttackEnemy != null)
            {
                EventCommandAttackEnemy(ally);
            }
        }

        public void CallEventStopTargettingEnemy()
        {
            bIsCommandAttacking = bIsAiAttacking = bIsAimingToShoot = false;
            if (EventStopTargettingEnemy != null) EventStopTargettingEnemy();
        }

        public void CallEventTogglebIsFreeMoving(bool _enable)
        {
            bIsFreeMoving = _enable;
            //If Free Moving Is Enabled, Diable Tactics
            bool _enableTactics = !_enable && bCanEnableAITactics;
            CallEventToggleAllyTactics(_enableTactics);
            if (EventTogglebIsFreeMoving != null) EventTogglebIsFreeMoving(_enable);
        }

        //Event handler controls bIsTacticsEnabled, makes code more centralized
        private void CallEventToggleAllyTactics(bool _enable)
        {
            bIsTacticsEnabled = _enable;
            if (EventToggleAllyTactics != null) EventToggleAllyTactics(_enable);
        }

        public void CallOnEquipTypeChanged(EEquipType _eType)
        {
            MyEquippedType = _eType;
            if (OnEquipTypeChanged != null) OnEquipTypeChanged(_eType);
        }

        public void CallToggleEquippedWeapon()
        {
            var _toggleType = MyEquippedType == EEquipType.Primary ?
                EEquipType.Secondary : EEquipType.Primary;
            CallOnEquipTypeChanged(_toggleType);
        }

        public void CallOnWeaponChanged(EEquipType _eType, EWeaponType _weaponType, bool _equipped)
        {
            MyUnequippedWeaponType = MyEquippedWeaponType;
            MyEquippedWeaponType = _weaponType;
            if (OnWeaponChanged != null)
            {
                OnWeaponChanged(_eType, _weaponType, _equipped);
            }
        }

        protected void CallOnAmmoChanged(int _loaded, int _unloaded)
        {
            if(MyEquippedType == EEquipType.Primary)
            {
                PrimaryLoadedAmmoAmount = _loaded;
                PrimaryUnloadedAmmoAmount = _unloaded;
            }
            else
            {
                SecondaryLoadedAmmoAmount = _loaded;
                SecondaryUnloadedAmmoAmount = _unloaded;
            }
            if (OnAmmoChanged != null) OnAmmoChanged(_loaded, _unloaded);
        }

        public void CallOnHealthChanged(int _current, int _max)
        {
            if (OnHealthChanged != null) OnHealthChanged(_current, _max);
        }
        #endregion

        #region PublicMethods
        /// <summary>
        /// Used Primarily to update unequipped ammo count, called
        /// from allyStatsController.
        /// </summary>
        /// <param name="_eType"></param>
        /// <param name="_loaded"></param>
        /// <param name="_unloaded"></param>
        public void UpdateWeaponAmmoCount(EEquipType _eType, int _loaded, int _unloaded)
        {
            if(_eType == EEquipType.Primary)
            {
                PrimaryLoadedAmmoAmount = _loaded;
                PrimaryUnloadedAmmoAmount = _unloaded;
            }
            else
            {
                SecondaryLoadedAmmoAmount = _loaded;
                SecondaryUnloadedAmmoAmount = _unloaded;
            }
            CallOnAmmoChanged(_loaded, _unloaded);
        }

        public void UpdateUnequippedWeaponType(EWeaponType _weaponType)
        {
            MyUnequippedWeaponType = _weaponType;
        }

        public void SetAllyIsUiTarget(bool _isTarget)
        {
            bAllyIsUiTarget = _isTarget;
        }
        #endregion

    }
}