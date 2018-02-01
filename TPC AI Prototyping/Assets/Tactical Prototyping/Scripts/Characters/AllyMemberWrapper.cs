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
                return HPValue;
                //return myCharacterHealth.CurrentHealth;
            }
            set
            {
                HPValue = (int)value;
            }
        }

        public override float AllyMaxHealth
        {
            get
            {
                return MaxHPValue;
                //return myCharacterHealth.MaxHealth;
            }
            set
            {
                MaxHPValue = (int)value;
            }
        }

        public override bool IsAlive
        {
            get
            {
                return AllyHealth > HPStatus.minValue;
            }
        }

        //public override float AllyShield
        //{
        //    get
        //    {
        //        return myCharacterHealth.CurrentShield;
        //    }
        //}

        //public override float AllyMaxShield
        //{
        //    get
        //    {
        //        return myCharacterHealth.MaxShield;
        //    }
        //}
        #endregion

        #region ORK Status Properties
        //Ork Value Names
        string MaxHPName = "MaxHP";
        string HPName = "HP";
        string MaxMPName = "MaxMP";
        string MPName = "MP";
        string ATKName = "ATK";
        string DEFName = "DEF";
        string MATKName = "MATK";
        string MDEFName = "MDEF";
        string AGIName = "AGI";
        string DEXName = "DEX";
        string LUKName = "LUK";
        string EXPName = "EXP";

        //Ork Value Getters
        int HPValue
        {
            get { return RPGCombatant.Status[HPStatus.ID].GetValue(); }
            set { RPGCombatant.Status[HPStatus.ID].SetValue(value, false, false, false, true, false, false); }
        }
        int MaxHPValue
        {
            get { return RPGCombatant.Status[MaxHPStatus.ID].GetValue(); }
            set { RPGCombatant.Status[MaxHPStatus.ID].SetValue(value, false, false, false, true, false, false); }
        }
        int ATKValue
        {
            get { return RPGCombatant.Status[ATKStatus.ID].GetValue(); }
        }
        int DEFValue
        {
            get { return RPGCombatant.Status[DEFStatus.ID].GetValue(); }
        }
        //Status Values Properties
        ORKFramework.StatusValueSetting HPStatus
        {
            get
            {
                if (_HPStatus == null)
                    _HPStatus = GetRPGStatusFromName(HPName);

                return _HPStatus;
            }
        }
        ORKFramework.StatusValueSetting _HPStatus = null;
        ORKFramework.StatusValueSetting MaxHPStatus
        {
            get
            {
                if (_MaxHPStatus == null)
                    _MaxHPStatus = GetRPGStatusFromName(MaxHPName);

                return _MaxHPStatus;
            }
        }
        ORKFramework.StatusValueSetting _MaxHPStatus = null;
        ORKFramework.StatusValueSetting ATKStatus
        {
            get
            {
                if (_ATKStatus == null)
                    _ATKStatus = GetRPGStatusFromName(ATKName);

                return _ATKStatus;
            }
        }
        ORKFramework.StatusValueSetting _ATKStatus = null;
        ORKFramework.StatusValueSetting DEFStatus
        {
            get
            {
                if (_DEFStatus == null)
                    _DEFStatus = GetRPGStatusFromName(DEFName);

                return _DEFStatus;
            }
        }
        ORKFramework.StatusValueSetting _DEFStatus = null;

        ORKFramework.StatusValueSetting GetRPGStatusFromName(string _value)
        {
            foreach (var _status in ORKFramework.ORK.StatusValues.data)
            {
                if (_status.GetName() == _value)
                    return _status;
            }
            return null;
        }
        #endregion

        #region ORK Equipment Properties
        ORKFramework.EquipPartSlot HelmetEquipSlot { get { return RPGCombatant.Equipment[HelmetID]; } }
        ORKFramework.EquipPartSlot RightHandEquipSlot { get { return RPGCombatant.Equipment[RightHandID]; } }
        ORKFramework.EquipPartSlot LeftHandEquipSlot { get { return RPGCombatant.Equipment[LeftHandID]; } }
        ORKFramework.EquipPartSlot ArmorEquipSlot { get { return RPGCombatant.Equipment[ArmorID]; } }
        ORKFramework.EquipPartSlot AccessoryEquipSlot { get { return RPGCombatant.Equipment[AccessoryID]; } }

        int HelmetID { get { return 0; } }
        int RightHandID { get { return 1; } }
        int LeftHandID { get { return 2; } }
        int ArmorID { get { return 3; } }
        int AccessoryID { get { return 4; } }

        string HelmetName { get { return HelmetEquipSlot.Equipped ? HelmetEquipSlot.Equipment.GetName() : ""; } }
        string RightHandName { get { return RightHandEquipSlot.Equipped ? RightHandEquipSlot.Equipment.GetName() : ""; } }
        string LeftHandName { get { return LeftHandEquipSlot.Equipped ? LeftHandEquipSlot.Equipment.GetName() : ""; } }
        string ArmorName { get { return ArmorEquipSlot.Equipped ? ArmorEquipSlot.Equipment.GetName() : ""; } }
        string AccessoryName { get { return AccessoryEquipSlot.Equipped ? AccessoryEquipSlot.Equipment.GetName() : ""; } }

        List<string> EquipmentNames
        {
            get
            {
                return new List<string>(){ HelmetName, RightHandName,
                LeftHandName, ArmorName, AccessoryName };
            }
        }

        #endregion

        #region ORKInventoryAndGameProperties
        ORKFramework.Item GetItemFromName(string _name)
        {
            foreach (var _item in ORKFramework.ORK.Items.data)
            {
                if (_item.GetName() == _name)
                    return _item;
            }
            return null;
        }

        ORKFramework.Weapon GetWeaponFromName(string _name)
        {
            foreach (var _weapon in ORKFramework.ORK.Weapons.data)
            {
                if (_weapon.GetName() == _name)
                    return _weapon;
            }
            return null;
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
            InvokeRepeating("EquipTesting", 1, 0.5f);
            
        }

        #endregion

        #region Handlers
        public override void AllyTakeDamage(float amount, Vector3 position, Vector3 force, AllyMember _instigator, GameObject hitGameObject)
        {
            if (IsAlive == false) return;
            if(AllyHealth > HPStatus.minValue)
            {
                AllyHealth = Mathf.Max(HPStatus.minValue, AllyHealth - amount);
            }
            // Apply a force to the hit rigidbody if the force is greater than 0.
            if (myRigidbody != null && !myRigidbody.isKinematic && force.sqrMagnitude > 0)
            {
                myRigidbody.AddForceAtPosition(force, position);
            }
            if (bIsCurrentPlayer)
                EventHandler.ExecuteEvent<float, Vector3, Vector3, GameObject>(gameObject, "OnHealthDamageDetails", amount, position, force, _instigator.gameObject);

            if (IsAlive == false)
            {
                EventHandler.ExecuteEvent(gameObject, "OnDeath");
                EventHandler.ExecuteEvent<Vector3, Vector3, GameObject>(gameObject, "OnDeathDetails", force, position, _instigator.gameObject);

            }
        }
        #endregion

        #region Getters
        public override int GetDamageRate()
        {
            return ATKValue;
        }
        #endregion

        #region Testing
        void EquipTesting()
        {
            //if (bIsCurrentPlayer == false) return;
            //Debug.Log("ATK " + ATKValue);
            //Debug.Log("DEF " + DEFValue);

            //var _inventory = ORKFramework.ORK.Game.ActiveGroup.Inventory;

            //var _w = GetWeaponFromName("Shotgun");
            //if (_w != null)
            //{
            //    Debug.Log("Equipping " + _w.GetName());
            //    var _shortcut = new ORKFramework.EquipShortcut(ORKFramework.EquipSet.Weapon, _w.ID, 1, 1);
            //    var _b = RPGCombatant.Equipment.Equip(RightHandID, _shortcut, RPGCombatant.Inventory, false, false);
            //    Debug.Log("Is Equipped " + RPGCombatant.Equipment.IsEquipped(ORKFramework.EquipSet.Weapon, _w.ID, 1));
            //    Debug.Log("Right Hand " + RightHandName);
            //    Debug.Log("Left Hand " + LeftHandName);
            //    RPGCombatant.Equipment.FireChanged();
            //}

            //var _potion = GetItemFromName("Potion");
            //if(_potion != null)
            //{
            //    Debug.Log("Adding Potion");
            //    var _pgain = new ORKFramework.ItemGain()
            //    {
            //        chance = 100,
            //        quantity = 1,
            //        level = 1,
            //        type = ORKFramework.ItemDropType.Item,
            //        id = _potion.ID
            //    };
            //    _inventory.Add(new ORKFramework.ItemGain[1] { _pgain }, true, true);
            //}

            //var _weapon = GetWeaponFromName("Pistol");
            //if(_weapon != null)
            //{
            //    var _gain = new ORKFramework.ItemGain()
            //    {
            //        chance = 100,
            //        quantity = 1,
            //        level = 1,
            //        type = ORKFramework.ItemDropType.Weapon,
            //        id = _weapon.ID
            //    };
            //    RPGCombatant.Inventory.Add(new ORKFramework.ItemGain[1] { _gain}, false, false);
            //}

            //Debug.Log("W "+ORKFramework.ORK.Weapons.data.Length);
            //Debug.Log("I "+ORKFramework.ORK.Items.data.Length);
            //Debug.Log("Right Equipped " + RightHandName);
            //Debug.Log("Left Equipped " + LeftHandName);
            //Debug.Log("Right IsEquipped " + RightHandEquipSlot.Equipped.ToString());
            //Debug.Log("Left IsEquipped " + LeftHandEquipSlot.Equipped.ToString());
            //var _rpgInventory = ORKFramework.ORK.Game.ActiveGroup.Inventory;
            //RPGCombatant.Inventory.Add(RightHandEquipSlot.Equipment, false, false, false);
            //Debug.Log(RPGCombatant.Inventory.Weapons.GetCount(0));
            //RPGCombatant.Inventory.Add(new ORKFramework.ItemGain[2], false, false);

            //foreach (var _item in _rpgInventory.GetContent(false,false,true,false,0,false))
            //{
            //    Debug.Log(_item);
            //}
            //if (RightHandEquipSlot.Equipped)
            //{
            //    var _myHandler = GetComponent<RTSItemAndControlHandler>();
            //    if (_previousName != RightHandName) Debug.Log(RightHandName);
            //    _previousName = RightHandName;
            //    if (_myHandler != null && _myHandler.CheckForInventoryMatch(RightHandName))
            //    {
            //        _myHandler.SetEquippedItemFromString(RightHandName);
            //    }
            //}
        }

        //void WaitForGunn()
        //{
        //    if (bIsCurrentPlayer == false) return;
        //    for (int i = 0; i < ORKFramework.ORK.Weapons.Count; i++)
        //    {
        //        var _slot = RPGCombatant.Equipment.GetFakeEquip(1, new ORKFramework.EquipShortcut(ORKFramework.EquipSet.Weapon, i, 1, 1));
        //        foreach (var _s in _slot)
        //        {
        //            if (_s != null && _s.Equipment != null && _s.Equipped == true)
        //            {
        //                var _myHandler = GetComponent<RTSItemAndControlHandler>();
        //                if (_myHandler != null && _myHandler.CheckForInventoryMatch(_s.Equipment.GetName()))
        //                {
        //                    _myHandler.SetEquippedItemFromString(_s.Equipment.GetName());
        //                }
        //            }
        //        }
        //    }

        //}
        #endregion

    }
}