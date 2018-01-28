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
        //Wrappers
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
        //Third Person Controller
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

        //ORK Components
        public ORKFramework.Behaviours.CombatantComponent RPGCombatantComponent
        {
            get
            {
                if (_rpgCombatant == null)
                    _rpgCombatant = GetComponent<ORKFramework.Behaviours.CombatantComponent>();

                return _rpgCombatant;
            }
        }
        private ORKFramework.Behaviours.CombatantComponent _rpgCombatant = null;

        public ORKFramework.Combatant RPGCombatant
        {
            get { return RPGCombatantComponent != null ? 
                    RPGCombatantComponent.combatant : null; }
        }
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

        #region Fields
        //Ork Fields
        //ORKFramework.EquipmentPart[] previousEquipment;

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

        #region TestingEquipment
        //void TestingRPGEquipment()
        //{
        //    //if (RPGCombatant != null)
        //    //{
        //    //    Debug.Log(RPGCombatant.Equipment.IsEquipped(ORKFramework.EquipSet.Weapon, 50, 1));

        //    //    //Debug.Log(RPGCombatant.Equipment.Settings.ToString());
        //    //    //RPGCombatant.Equipment.IsEquipped()
        //    //    Debug.Log("Count" + RPGCombatant.Equipment.GetAvailableParts().Count);
        //    //    for (int i = 0; i < RPGCombatant.Equipment.GetAvailableParts().Count; i++)
        //    //    {
        //    //        if(RPGCombatant.Equipment.IsEquipped(ORKFramework.EquipSet.Weapon, i, 1))
        //    //        {
        //    //            Debug.Log(i + " is equiped");
        //    //            var _equipments = RPGCombatant.Inventory.GetEquipmentByPart(i, true, true);
        //    //            Debug.Log(_equipments.Count);
        //    //        }

        //    //    }

        //    //    //RPGCombatant.InventoryChanged += UpdateRPGEquipmentForTPC;
        //    //    RPGCombatant.Equipment.Changed += UpdateRPGEquipmentForTPC;
        //    //    //InvokeRepeating("UpdateRPGEquipmentForTPC", 0.5f, 0.5f);
        //    //}
        //}

        //void UpdateRPGEquipmentForTPC(ORKFramework.Combatant _combatant)
        //{
        //    Debug.Log("Hello");
        //    if (_combatant != null)
        //    {
        //        Debug.Log("Updating RPG Equipment");
        //        //Debug.Log("Weapons " + _combatant.Equipment.GetAvailableParts)
        //        //ORKFramework.ORK.Game.ActiveGroup.Inventory.Weapons.
        //        foreach (var _currPart in ORKFramework.ORK.EquipmentParts.data)
        //        {
        //            if(_currPart.GetName() == "Left Arm" || _currPart.GetName() == "Right Arm")
        //            {
        //                Debug.Log(_currPart.GetName());
        //            }
        //            //if (previousEquipment != null && previousEquipment.Length > 0)
        //            //{
        //            //    foreach (var _prevPart in previousEquipment)
        //            //    {
        //            //        if (_currPart.GetData() != _prevPart.GetData())
        //            //        {
        //            //            Debug.Log("Equiping: " + _currPart.GetData().ToString());
        //            //        }
        //            //    }
        //            //}
        //        }
        //        //previousEquipment = ORKFramework.ORK.EquipmentParts.data;
        //    }
        //}
        #endregion

    }
}