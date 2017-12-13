using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Opsive.ThirdPersonController;
using Opsive.ThirdPersonController.Input;

namespace RTSPrototype
{
    public class RTSCameraHandler : CameraHandler
    {
        #region RTSFieldsAndProps
        RTSGameMaster gamemaster
        {
            get { return RTSGameMaster.thisInstance; }
        }

        private bool moveCamera = false;
        #endregion

        #region RTSHandlers
        void ToggleMoveCamera(bool enable)
        {
            moveCamera = enable;
        }
        #endregion

        protected override void InitializeCharacter(GameObject character)
        {
            if (character == null)
            {
                if (m_Character != null)
                {
                    EventHandler.UnregisterEvent<bool>(m_Character, "OnAllowGameplayInput", AllowGameplayInput);
                    EventHandler.UnregisterEvent<Item>(m_Character, "OnInventoryDualWieldItemChange", OnDualWieldItemChange);
                    m_Character = null;
                    m_PlayerInput = null;
                }
                return;
            }

            m_Character = character;
            m_PlayerInput = RTSPlayerInput.thisInstance;
            EventHandler.RegisterEvent<bool>(m_Character, "OnAllowGameplayInput", AllowGameplayInput);
            EventHandler.RegisterEvent<Item>(m_Character, "OnInventoryDualWieldItemChange", OnDualWieldItemChange);
        }

        #region UnityMessages
        protected void OnDisable()
        {
            gamemaster.EventEnableCameraMovement -= ToggleMoveCamera;
        }

        protected void Start()
        {
            gamemaster.EventEnableCameraMovement += ToggleMoveCamera;
        }

        protected override void Update()
        {
            if (moveCamera)
            {
                base.Update();
            }
        }
        #endregion



    }
}