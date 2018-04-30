using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BaseFramework
{
    public class GameMaster : MonoBehaviour
    {
        #region Properties
        //Components
        public GameInstance gameInstance
        {
            get { return GameInstance.thisInstance; }
        }

        public GameMode gamemode
        {
            get { return GameMode.thisInstance; }
        }

        protected UiMaster uiMaster
        {
            get { return UiMaster.thisInstance; }
        }

        public static GameMaster thisInstance
        {
            get; protected set;
        }

        //Access Properties
        public virtual bool bIsGamePaused
        {
            get { return _bIsGamePaused; }
            set { _bIsGamePaused = value; }
        }
        private bool _bIsGamePaused = false;

        public virtual bool isGameOver
        {
            get; protected set;
        }
        public virtual bool isMenuOn
        {
            get; protected set;
        }
        #endregion

        #region Fields

        #endregion

        #region UnityMessages
        protected virtual void OnEnable()
        {
            if (thisInstance != null)
                Debug.LogWarning("More than one instance of GameMaster in scene.");
            else
                thisInstance = this;
        }

        protected virtual void Start()
        {
            SubToEvents();
        }

        // Update is called once per frame
        protected virtual void Update()
        {

        }

        protected virtual void OnDisable()
        {
            UnsubFromEvents();
        }
        #endregion

        #region Events and Delegates
        public delegate void GameManagerEventHandler();
        public delegate void OneBoolArgsHandler(bool enable);
        public delegate void TwoBoolArgsHandler(bool enable, bool isPositive);
        //Levels, Scenarios, Progression
        public event GameManagerEventHandler RestartLevelEvent;
        public event GameManagerEventHandler GoToMenuSceneEvent;
        public event GameManagerEventHandler GoToNextLevelEvent;
        public event GameManagerEventHandler GoToNextScenarioEvent;
        public event GameManagerEventHandler GameOverEvent;
        public event GameManagerEventHandler EventAllObjectivesCompleted;
        //Input
        public event GameManagerEventHandler OnLeftClickNoSend;
        public event GameManagerEventHandler OnRightClickNoSend;
        public event TwoBoolArgsHandler EventEnableCameraZoom;
        //Camera and Pause
        public event OneBoolArgsHandler OnToggleIsGamePaused;
        public event OneBoolArgsHandler EventHoldingLeftMouseDown;
        public event OneBoolArgsHandler EventHoldingRightMouseDown;
        #endregion

        #region EventCalls
        public void CallEventRestartLevel()
        {
            CallOnToggleIsGamePaused(false);
            if (RestartLevelEvent != null) RestartLevelEvent();
            Invoke("WaitToRestartLevel", 0.2f);
        }

        private void WaitToRestartLevel()
        {
            gameInstance.RestartCurrentLevel();
        }

        public void CallEventGoToMenuScene()
        {
            CallOnToggleIsGamePaused(false);
            if (GoToMenuSceneEvent != null) GoToMenuSceneEvent();
            Invoke("WaitToGoToMenuScene", 0.2f);
        }

        private void WaitToGoToMenuScene()
        {
            gameInstance.GoToMainMenu();
        }

        public void CallEventGoToNextLevel()
        {
            CallOnToggleIsGamePaused(false);
            if (GoToNextLevelEvent != null) GoToNextLevelEvent();
            Invoke("WaitToGoToNextLevel", 0.2f);
        }

        private void WaitToGoToNextLevel()
        {
            gameInstance.GoToNextLevel();
        }

        public void CallEventGoToNextScenario()
        {
            CallOnToggleIsGamePaused(false);
            if (GoToNextScenarioEvent != null) GoToNextScenarioEvent();
            Invoke("WaitToGoToNextScenario", 0.2f);
        }

        private void WaitToGoToNextScenario()
        {
            gameInstance.GoToNextScenario();
        }

        public virtual void CallEventGameOver()
        {
            if (GameOverEvent != null)
            {
                if (!isGameOver)
                {
                    isGameOver = true;
                    GameOverEvent();
                }
            }
        }

        public virtual void CallEventAllObjectivesCompleted()
        {
            if (EventAllObjectivesCompleted != null) EventAllObjectivesCompleted();
        }

        public void CallOnToggleIsGamePaused()
        {
            bIsGamePaused = !bIsGamePaused;
            if (OnToggleIsGamePaused != null)
            {
                OnToggleIsGamePaused(bIsGamePaused);
            }
            Time.timeScale = bIsGamePaused ? 0f : 1f;
        }

        public void CallOnToggleIsGamePaused(bool _enable)
        {
            bIsGamePaused = _enable;
            if (OnToggleIsGamePaused != null)
            {
                OnToggleIsGamePaused(bIsGamePaused);
            }
            //Time.timeScale = bIsGamePaused ? 0f : 1f;
            if (bIsGamePaused)
            {
                Invoke("ToggleTimeScale", 0.2f);
            }
            else
            {
                //Cannot Invoke Methods While Timescale is 0
                //Time.timeScale = bIsGamePaused ? 0f : 1f;
                Time.timeScale = 1f;
            }
        }

        //Temporary Fix, Needs to Change In Future
        protected virtual void ToggleTimeScale()
        {
            //Time.timeScale = bIsGamePaused ? 0f : 1f;
            Time.timeScale = 0f;
        }

        public void CallEventHoldingRightMouseDown(bool _holding)
        {
            if (EventHoldingRightMouseDown != null)
            {
                EventHoldingRightMouseDown(_holding);
            }
        }

        public void CallEventHoldingLeftMouseDown(bool _holding)
        {
            if (EventHoldingLeftMouseDown != null)
            {
                EventHoldingLeftMouseDown(_holding);
            }
        }

        public virtual void CallEventOnLeftClick()
        {
            if (OnLeftClickNoSend != null) OnLeftClickNoSend();
        }

        public virtual void CallEventOnRightClick()
        {
            if (OnRightClickNoSend != null) OnRightClickNoSend();
        }

        public void CallEventEnableCameraZoom(bool enable, bool isPositive)
        {
            if (EventEnableCameraZoom != null) EventEnableCameraZoom(enable, isPositive);
        }
        #endregion

        #region Initialization
        protected virtual void SubToEvents()
        {
            
        }

        protected virtual void UnsubFromEvents()
        {
            
        }
        #endregion
    }
}