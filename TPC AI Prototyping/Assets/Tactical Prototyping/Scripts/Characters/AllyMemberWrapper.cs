using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RTSCoreFramework;
using Opsive.ThirdPersonController;

namespace RTSPrototype
{
    public class AllyMemberWrapper : AllyMember
    {
        #region Components
        public AllyAIControllerWrapper aiControllerWrapper
        {
            get
            {
                if (_aiControllerWrapper == null)
                    _aiControllerWrapper = GetComponent<AllyAIControllerWrapper>();

                return _aiControllerWrapper;
            }
        }
        private AllyAIControllerWrapper _aiControllerWrapper = null;
        public CharacterHealth myCharacterHealth
        {
            get
            {
                if (_myCharacterHealth == null)
                    _myCharacterHealth = GetComponent<CharacterHealth>();

                return _myCharacterHealth;
            }
        }
        public CharacterHealth _myCharacterHealth = null;

        public ItemHandler itemHandler
        {
            get
            {
                if (_itemHandler == null)
                    _itemHandler = GetComponent<ItemHandler>();

                return _itemHandler;
            }
        }
        private ItemHandler _itemHandler = null;

        public Inventory myInventory
        {
            get
            {
                if (_myInventory == null)
                    _myInventory = GetComponent<Inventory>();

                return _myInventory;
            }
        }
        private Inventory _myInventory = null;

        #endregion

        #region Properties

        #region HealthProps
        public override float AllyHealth
        {
            get
            {
                return myCharacterHealth.CurrentHealth;
            }
        }

        public override float AllyMaxHealth
        {
            get
            {
                return myCharacterHealth.MaxHealth;
            }
        }

        public override float AllyShield
        {
            get
            {
                return myCharacterHealth.CurrentShield;
            }
        }

        public override float AllyMaxShield
        {
            get
            {
                return myCharacterHealth.MaxShield;
            }
        }
        #endregion

        public override int CurrentEquipedAmmo
        {
            get
            {
                return myInventory.GetCurrentItemCount(typeof(PrimaryItemType), false) +
                    myInventory.GetCurrentItemCount(typeof(PrimaryItemType), true);
            }
        }

        public AllyMemberWrapper enemyTargetWrapper
        {
            get { return aiControllerWrapper.currentTargettedEnemyWrapper; }
        }
        #endregion

        #region UnityMessages
        protected override void OnEnable()
        {
            base.OnEnable();
        }

        protected override void OnDisable()
        {
            base.OnDisable();
        }

        protected override void Start()
        {
            base.Start();
        }
        #endregion

    }
}