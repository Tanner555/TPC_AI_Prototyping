using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace RTSCoreFramework
{
    public class AllySpecialAbilities : MonoBehaviour
    {
        #region Properties
        RTSGameMaster gamemaster
        {
            get { return RTSGameMaster.thisInstance; }
        }

        AllyEventHandler eventhandler
        {
            get
            {
                if (_eventhandler == null)
                    _eventhandler = GetComponent<AllyEventHandler>();

                return _eventhandler;
            }
        }
        AllyEventHandler _eventhandler = null;
        AllyMember allymember
        {
            get
            {
                if (_allymember == null)
                    _allymember = GetComponent<AllyMember>();

                return _allymember;
            }
        }
        AllyMember _allymember = null;

        bool bIsDead
        {
            get
            {
                return allymember == null ||
                allymember.IsAlive == false;
            }
        }

        //Stamina
        int AllyStamina
        {
            get { return allymember.AllyStamina; }
        }
        int AllyMaxStamina
        {
            get { return allymember.AllyMaxStamina; }
        }
        float energyAsPercent { get { return AllyStamina / AllyMaxStamina; } }

        //AudioSource
        protected virtual AudioSource audioSource
        {
            get
            {
                if (_audioSource == null)
                {
                    _audioSource = GetComponent<AudioSource>();
                    if (_audioSource == null)
                        _audioSource = gameObject.AddComponent<AudioSource>();

                }
                return _audioSource;
            }
        }
        private AudioSource _audioSource = null;
        #endregion

        #region Fields
        [SerializeField] AbilityConfig[] abilities;
        //Once per second
        float addStaminaRepeatRate = 1f;
        int regenPointsPerSecond = 10;
        [SerializeField] AudioClip outOfEnergy;
        
        /// <summary>
        /// Allows me to store a behavior on this script
        /// instead of depending on the config for behavior reference
        /// </summary>
        Dictionary<AbilityConfig, AbilityBehaviour> AbilityDictionary = new Dictionary<AbilityConfig, AbilityBehaviour>();
        #endregion

        #region UnityMessages
        private void OnEnable()
        {
            InitializeAbilityDictionary();
            InvokeRepeating("SE_AddEnergyPoints", 1f, addStaminaRepeatRate);

            eventhandler.EventAllyDied += OnAllyDeath;
            gamemaster.OnNumberKeyPress += OnKeyPress;
        }

        private void OnDisable()
        {
            eventhandler.EventAllyDied -= OnAllyDeath;
            gamemaster.OnNumberKeyPress -= OnKeyPress;
        }
        #endregion

        #region AbilitiesAndEnergy
        public void AttemptSpecialAbility(int abilityIndex, GameObject target = null)
        {
            var energyCost = abilities[abilityIndex].GetEnergyCost();

            if (energyCost <= AllyStamina)
            {
                ConsumeEnergy(energyCost);
                AbilityDictionary[abilities[abilityIndex]].Use(target);
            }
            else
            {
                audioSource.PlayOneShot(outOfEnergy);
            }
        }

        public int GetNumberOfAbilities()
        {
            return abilities.Length;
        }

        public void ConsumeEnergy(float amount)
        {
            allymember.AllyDrainStamina((int)amount);
        }
        #endregion

        #region Services
        void SE_AddEnergyPoints()
        {
            allymember.AllyRegainStamina(regenPointsPerSecond);
        }
        #endregion

        #region Handlers
        void OnKeyPress(int _key)
        {
            if (bIsDead) return;

            if (_key == 0 ||
                _key > GetNumberOfAbilities() ||
                allymember.bIsCurrentPlayer == false) return;

            Debug.Log($"Attempting Special Ability Index: {_key}");
            //AttemptSpecialAbility(_key);
        }

        void OnAllyDeath()
        {
            CancelInvoke();
        }
        #endregion

        #region DictionaryBehavior
        public AbilityBehaviour AddAbilityBehaviorFromConfig(AbilityConfig _config, GameObject objectToattachTo)
        {
            AbilityBehaviour _behaviourComponent =
                _config.AddBehaviourComponent(objectToattachTo);
            _behaviourComponent.SetConfig(_config);
            return _behaviourComponent;
        }

        void InitializeAbilityDictionary()
        {
            for (int abilityIndex = 0; abilityIndex < abilities.Length; abilityIndex++)
            {
                AbilityDictionary.Add(
                    abilities[abilityIndex],
                    AddAbilityBehaviorFromConfig(
                        abilities[abilityIndex], this.gameObject
                    ));
            }
        }
        #endregion
    }
}