﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RTSCoreFramework
{
    public class AllyStatController : MonoBehaviour
    {
        #region Fields
        [Header("Will be used to identify the character")]
        public ECharacterType characterType;

        private CharacterStats myCharacterStats;
        private Dictionary<EWeaponType, WeaponStats> allWeaponStats = new Dictionary<EWeaponType, WeaponStats>();
        #endregion

        #region SetupProperties
        AllyMember allyMember
        {
            get
            {
                if (__allyMember == null)
                    __allyMember = GetComponent<AllyMember>();

                return __allyMember;
            }
        }
        AllyMember __allyMember = null;

        AllyEventHandler eventHandler
        {
            get
            {
                if (__eventHandler == null)
                    __eventHandler = GetComponent<AllyEventHandler>();

                return __eventHandler;
            }
        }
        AllyEventHandler __eventHandler = null;

        RTSStatHandler statHandler
        {
            get
            {
                //For Faster Access when using OnEnable method
                if(RTSStatHandler.thisInstance != null) 
                    return RTSStatHandler.thisInstance;

                return GameObject.FindObjectOfType<RTSStatHandler>();
            }
        }
        #endregion

        #region AccessProperties
        //Health
        public int Stat_Health
        {
            get { return myCharacterStats.Health; }
            set { myCharacterStats.Health = value; }
        }
        public int Stat_MaxHealth
        {
            get { return myCharacterStats.MaxHealth; }
        }
        //Weapons
        public EEquipType Stat_EquipType
        {
            get { return myCharacterStats.EquippedWeapon; }
        }
        public EWeaponType Stat_PrimaryWeapon
        {
            get { return myCharacterStats.PrimaryWeapon; }
        }
        public EWeaponType Stat_SecondaryWeapon
        {
            get { return myCharacterStats.SecondaryWeapon; }
        }
        #endregion

        #region UnityMessages
        // Use this for initialization
        void OnEnable()
        {
            InitializeCharacterStats();
            SubToEvents();
        }

        private void Start()
        {
            Invoke("OnDelayStart", 0.5f);
            RetrieveAllWeaponStats();
        }

        private void OnDelayStart()
        {
            //Equip whatever the ally is holding
            eventHandler.CallOnEquipTypeChanged(myCharacterStats.EquippedWeapon);
        }

        private void OnDisable()
        {
            UnsubFromEvents();
        }
        #endregion

        #region Getters
        public int CalculateDamageRate()
        {
            var _weapon = GetWeaponStats();
            return _weapon.DamageRate;
        }

        private WeaponStats GetWeaponStats()
        {
            switch (myCharacterStats.EquippedWeapon)
            {
                case EEquipType.Primary:
                    return allWeaponStats[myCharacterStats.PrimaryWeapon];
                case EEquipType.Secondary:
                    return allWeaponStats[myCharacterStats.SecondaryWeapon];
                default:
                    return new WeaponStats();
            }
        }
        #endregion

        #region Handlers
        void HandleEquipTypeChanged(EEquipType _eType)
        {
            myCharacterStats.EquippedWeapon = _eType;
            var _weapon = myCharacterStats.EquippedWeapon == EEquipType.Primary ?
                myCharacterStats.PrimaryWeapon : myCharacterStats.SecondaryWeapon;
            eventHandler.CallOnWeaponChanged(myCharacterStats.EquippedWeapon, _weapon, true);
            
        }
        void HandleWeaponChanged(EEquipType _eType, EWeaponType _weaponType, bool _equipped)
        {
            switch (_eType)
            {
                case EEquipType.Primary:
                    if (_weaponType != myCharacterStats.PrimaryWeapon)
                        myCharacterStats.PrimaryWeapon = _weaponType;
                    break;
                case EEquipType.Secondary:
                    if (_weaponType != myCharacterStats.SecondaryWeapon)
                        myCharacterStats.SecondaryWeapon = _weaponType;
                    break;
                default:
                    break;
            }
        }
        #endregion

        #region Initialization
        void SubToEvents()
        {
            eventHandler.OnEquipTypeChanged += HandleEquipTypeChanged;
            eventHandler.OnWeaponChanged += HandleWeaponChanged;
        }
        void UnsubFromEvents()
        {
            eventHandler.OnEquipTypeChanged -= HandleEquipTypeChanged;
            eventHandler.OnWeaponChanged -= HandleWeaponChanged;
        }
        void InitializeCharacterStats()
        {
            myCharacterStats = statHandler.RetrieveCharacterStats(allyMember, characterType);
        }
        void RetrieveAllWeaponStats()
        {
            allWeaponStats = statHandler.WeaponStatDictionary;
        }
        #endregion


    }
}