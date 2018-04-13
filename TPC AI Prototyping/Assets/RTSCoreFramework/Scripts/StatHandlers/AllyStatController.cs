using System.Collections;
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
            set
            {
                myCharacterStats.Health = value;
                CallOnHealthChanged();
            }
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
        //Other Character Stats
        public ECharacterType Stat_CharacterType
        {
            get { return myCharacterStats.CharacterType; }
        }
        public string Stat_CharacterName
        {
            get { return myCharacterStats.CharacterType.ToString(); }
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
            UpdateUnequippedWeaponType();
        }

        private void OnDelayStart()
        {
            //Equip whatever the ally is holding
            eventHandler.CallOnEquipTypeChanged(myCharacterStats.EquippedWeapon);
            CallOnHealthChanged();
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

        private WeaponStats GetWeaponStatsFromWeaponType(EWeaponType _weaponType)
        {
            return allWeaponStats[_weaponType];
        }
        #endregion

        #region Handlers
        /// <summary>
        /// It's not really a Handler right now. When the game starts,
        /// this method is called to update the unequipped weapon type
        /// on the allyEventHandler.
        /// </summary>
        void UpdateUnequippedWeaponType()
        {
            //Explicitly Set Weapon Because Unequipped weapon type
            //on allyEventHandler doesn't become set till OnDelayStart
            EWeaponType _weapon = myCharacterStats.EquippedWeapon == EEquipType.Primary ?
                myCharacterStats.SecondaryWeapon : myCharacterStats.PrimaryWeapon;
            eventHandler.UpdateUnequippedWeaponStats(
                GetWeaponStatsFromWeaponType(
                    _weapon));
        }

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

        #region Helpers
        void CallOnHealthChanged()
        {
            eventHandler.CallOnHealthChanged(myCharacterStats.Health, myCharacterStats.MaxHealth);
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