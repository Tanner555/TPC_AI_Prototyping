using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Opsive.ThirdPersonController;
using Opsive.ThirdPersonController.Wrappers.Abilities;
using RTSCoreFramework;

namespace RTSPrototype
{
    public class RTSItemAndControlHandler : MonoBehaviour
    {
        #region PropsAndFields
        RTSGameMaster gamemaster
        {
            get { return RTSGameMaster.thisInstance; }
        }

        RTSGameModeWrapper gamemode
        {
            get { return (RTSGameModeWrapper)RTSGameModeWrapper.thisInstance; }
        }

        bool isAiming = false;
        [Header("Gun Types")]
        public ItemType AssualtRifleType;
        public ItemType PistolType;
        public ItemType ShotgunType;
        public ItemType SniperRifleType;
        public ItemType FistType;

        public const string AssaultRifleName = "Assault Rifle";
        public string AssaultRifName { get { return AssaultRifleName; } }
        const string PistolName = "Pistol";
        const string ShotgunName = "Shotgun";
        const string SniperRifleName = "Sniper Rifle";
        #endregion

        #region Components
        AllyEventHandlerWrapper myEventHandler;
        ItemHandler itemHandler;
        Inventory myInventory; 
        RigidbodyCharacterController myController;
        RTSNavBridge myNavBidge;
        AllyMember allyMember;

        bool AllCompsAreValid
        {
            get { return myEventHandler && itemHandler && 
                    myInventory && myController && myNavBidge 
                    && allyMember; }
        }
        #endregion

        #region UnityMessages
        private void Awake()
        {
            InitialSetup();
            
        }

        private void Start()
        {
            
        }

        private void OnEnable()
        {
            SubToEvents();
        }

        private void OnDisable()
        {
            UnsubFromEvents();
        }
        #endregion

        #region Handlers
        void OnWeaponTypeChanged(EEquipType _eType, EWeaponType _weaponType, bool _equipped)
        {
            if (_equipped)
            {
                switch (_weaponType)
                {
                    case EWeaponType.Fist:
                        SetEquippedItem(FistType);
                        break;
                    case EWeaponType.Pistol:
                        SetEquippedItem(PistolType);
                        break;
                    case EWeaponType.AssaultRifle:
                        SetEquippedItem(AssualtRifleType);
                        break;
                    case EWeaponType.Shotgun:
                        SetEquippedItem(ShotgunType);
                        break;
                    case EWeaponType.SniperRifle:
                        SetEquippedItem(SniperRifleType);
                        break;
                    default:
                        break;
                }
            }
        }

        void OnSetAimHandler(bool _isAiming)
        {
            isAiming = _isAiming;
            myController.Aim = _isAiming;
        }

        void OnSwitchPrevItem()
        {
            myInventory.SwitchItem(true, true);
        }

        void OnSwitchNextItem()
        {
            myInventory.SwitchItem(true, false);
        }

        void OnTryFire()
        {
            if (!AllCompsAreValid) return;
            if (!itemHandler.TryUseItem(typeof(PrimaryItemType)))
            {
                Debug.Log("Couldn't fire primary weapon");
            }
        }

        void OnTryReload()
        {
            if (!AllCompsAreValid) return;
            if (!itemHandler.TryReload())
            {
                Debug.Log("Couldn't reload primary weapon");
            }
        }

        void OnTryCrouch()
        {
            //var _ability = FindAbility(typeof(HeightChange));
            var _ability = GetComponent<Opsive.ThirdPersonController.Abilities.HeightChange>();
            if (_ability != null)
            {
                if (!_ability.IsActive)
                {
                    if (!myController.TryStartAbility(_ability))
                    {
                        Debug.Log("Ability HeightChange Failed");
                    }
                }
                else
                {
                    myController.TryStopAbility(_ability);
                }
            }
        }
        #endregion

        #region Finders
        //Make sure to use wrapper ability namespace, 
        //otherwise the method won't find the ability
        Opsive.ThirdPersonController.Abilities.Ability FindAbility(System.Type _type)
        {
            if (AllCompsAreValid)
            {
                foreach (var _ability in myController.Abilities)
                {
                    if (_type.Equals(_ability.GetType()))
                    {
                        return _ability;
                    }
                }
            }
            return null;
        }
        #endregion

        #region RPGInventoryToTPC
        void SetEquippedItem(ItemType _type)
        {
            var _gun = myInventory.GetCurrentItem(typeof(PrimaryItemType));
            if (_gun != null && _gun.ItemType != _type)
            {
                myInventory.EquipItem((PrimaryItemType)_type);
            }
            else if (_gun == null)
            {
                Debug.Log("Not Setting Equipped Weapon " + _type.ToString());
            }
        }
        #endregion

        #region Initialization
        void InitialSetup()
        {
            myEventHandler = GetComponent<AllyEventHandlerWrapper>();
            itemHandler = GetComponent<ItemHandler>();
            myInventory = GetComponent<Inventory>();
            myController = GetComponent<RigidbodyCharacterController>();
            myNavBidge = GetComponent<RTSNavBridge>();
            allyMember = GetComponent<AllyMember>();

            if (!AllCompsAreValid)
            {
                Debug.LogError("Not all Components can be found");
            }
        }

        void SubToEvents()
        {
            if (!AllCompsAreValid) return;
            myEventHandler.OnTryAim += OnSetAimHandler;
            myEventHandler.OnSwitchToPrevItem += OnSwitchPrevItem;
            myEventHandler.OnSwitchToNextItem += OnSwitchNextItem;
            myEventHandler.OnTryFire += OnTryFire;
            myEventHandler.OnTryReload += OnTryReload;
            myEventHandler.OnTryCrouch += OnTryCrouch;
            myEventHandler.OnWeaponChanged += OnWeaponTypeChanged;
        }

        void UnsubFromEvents()
        {
            myEventHandler.OnTryAim -= OnSetAimHandler;
            myEventHandler.OnSwitchToPrevItem -= OnSwitchPrevItem;
            myEventHandler.OnSwitchToNextItem -= OnSwitchNextItem;
            myEventHandler.OnTryFire -= OnTryFire;
            myEventHandler.OnTryReload -= OnTryReload;
            myEventHandler.OnTryCrouch -= OnTryCrouch;
            myEventHandler.OnWeaponChanged -= OnWeaponTypeChanged;
        }
        #endregion
    }
}