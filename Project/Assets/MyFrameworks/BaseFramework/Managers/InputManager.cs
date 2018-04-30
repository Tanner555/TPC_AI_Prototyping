using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BaseFramework
{
    public class InputManager : MonoBehaviour
    {
        #region Properties
        //Time Properties
        protected float CurrentGameTime
        {
            get { return Time.unscaledTime; }
        }

        //Mouse Setup - Scrolling
        protected bool bScrollAxisIsPositive
        {
            get { return scrollInputAxisValue >= 0.0f; }
        }

        protected UiMaster uiMaster { get { return UiMaster.thisInstance; } }

        protected UiManager uiManager
        {
            get { return UiManager.thisInstance; }
        }

        protected GameMode gamemode
        {
            get { return GameMode.thisInstance; }
        }

        protected GameMaster gamemaster
        {
            get { return GameMaster.thisInstance; }
        }

        public static InputManager thisInstance
        {
            get; protected set;
        }
        #endregion

        #region Fields
        //Handles Right Mouse Down Input
        [Header("Right Mouse Down Config")]
        public float RMHeldThreshold = 0.15f;
        protected bool isRMHeldDown = false;
        protected bool isRMHeldPastThreshold = false;
        protected float RMCurrentTimer = 5f;
        //Handles Left Mouse Down Input
        [Header("Left Mouse Down Config")]
        public float LMHeldThreshold = 0.15f;
        protected bool isLMHeldDown = false;
        protected bool isLMHeldPastThreshold = false;
        protected float LMCurrentTimer = 5f;
        //Handles Mouse ScrollWheel Input
        //Scroll Input
        protected string scrollInputName = "Mouse ScrollWheel";
        protected float scrollInputAxisValue = 0.0f;
        protected bool bScrollWasPreviouslyPositive = false;
        protected bool bScrollIsCurrentlyPositive = false;
        //Scroll Timer Handling
        protected bool isScrolling = false;
        //Used to Fix First Scroll Not Working Issue
        protected bool bBeganScrolling = false;
        //Stop Scroll Functionality
        [Header("Mouse ScrollWheel Config")]
        public float scrollStoppedThreshold = 0.15f;
        protected bool isNotScrollingPastThreshold = false;
        protected float noScrollCurrentTimer = 5f;
        //UI is enabled
        protected bool UiIsEnabled = false;
        #endregion

        #region UnityMessages
        protected virtual void OnEnable()
        {
            if (thisInstance != null)
                Debug.LogWarning("More than one instance of InputManager in scene.");
            else
                thisInstance = this;
        }

        protected virtual void Start()
        {
            
        }

        // Update is called once per frame
        protected virtual void Update()
        {

        }

        protected virtual void OnDisable()
        {
            
        }
        #endregion

        #region Handlers
        protected virtual void HandleUiActiveSelf(bool _state)
        {
            UiIsEnabled = _state;
            if (_state == true)
            {
                if (isRMHeldPastThreshold)
                {
                    isRMHeldPastThreshold = false;
                    gamemaster.CallEventEnableCameraMovement(false);
                }
                if (isLMHeldPastThreshold)
                {
                    isLMHeldPastThreshold = false;
                    //gamemaster.CallEventEnableSelectionBox(false);
                }
                isLMHeldDown = false;
                isRMHeldDown = false;
            }
        }

        protected virtual void HandleUiActiveSelf()
        {
            UiIsEnabled = uiMaster.isUiAlreadyInUse;
            if (uiMaster.isUiAlreadyInUse)
            {
                if (isRMHeldPastThreshold)
                {
                    isRMHeldPastThreshold = false;
                    gamemaster.CallEventEnableCameraMovement(false);
                }
                if (isLMHeldPastThreshold)
                {
                    isLMHeldPastThreshold = false;
                    //gamemaster.CallEventEnableSelectionBox(false);
                }
                isLMHeldDown = false;
                isRMHeldDown = false;
            }
        }
        #endregion

        #region InputCalls
        protected void CallMenuToggle() { uiMaster.CallEventMenuToggle(); }
        protected void CallToggleIsGamePaused() { gamemaster.CallOnToggleIsGamePaused(); }
        #endregion
    }
}