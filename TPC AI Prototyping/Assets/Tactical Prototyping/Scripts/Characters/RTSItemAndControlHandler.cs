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
        #endregion

        #region Components
        AllyEventHandlerWrapper myEventHandler;
        ItemHandler itemHandler;
        Inventory myInventory; 
        RigidbodyCharacterController myController;
        RTSNavBridge myNavBidge;

        bool AllCompsAreValid
        {
            get { return myEventHandler && itemHandler && 
                    myInventory && myController && myNavBidge; }
        }
        #endregion

        #region UnityMessages
        private void Awake()
        {
            InitialSetup();
            
        }

        private void Start()
        {
            SubToEvents();
        }

        private void OnDisable()
        {
            UnsubFromEvents();
        }
        #endregion

        #region Handlers
        void OnPlayerCommandMove(rtsHitType hitType, RaycastHit hit)
        {
            if (AllCompsAreValid) myNavBidge.MoveToDestination(hit.point);
        }

        void OnAICommandMove(Vector3 _point)
        {
            if (AllCompsAreValid) myNavBidge.MoveToDestination(_point);
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

        #region Initialization
        void InitialSetup()
        {
            myEventHandler = GetComponent<AllyEventHandlerWrapper>();
            itemHandler = GetComponent<ItemHandler>();
            myInventory = GetComponent<Inventory>();
            myController = GetComponent<RigidbodyCharacterController>();
            myNavBidge = GetComponent<RTSNavBridge>();

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
            myEventHandler.EventPlayerCommandMove += OnPlayerCommandMove;
            myEventHandler.EventAIMove += OnAICommandMove;
        }

        void UnsubFromEvents()
        {
            myEventHandler.OnTryAim -= OnSetAimHandler;
            myEventHandler.OnSwitchToPrevItem -= OnSwitchPrevItem;
            myEventHandler.OnSwitchToNextItem -= OnSwitchNextItem;
            myEventHandler.OnTryFire -= OnTryFire;
            myEventHandler.OnTryReload -= OnTryReload;
            myEventHandler.OnTryCrouch -= OnTryCrouch;
            myEventHandler.EventPlayerCommandMove -= OnPlayerCommandMove;
            myEventHandler.EventAIMove -= OnAICommandMove;
        }
        #endregion
    }
}