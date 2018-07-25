using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RTSCoreFramework
{
    public class AllyTacticsController : MonoBehaviour
    {
        #region Properties
        protected AllyEventHandler myEventHandler
        {
            get
            {
                if (_myEventHandler == null)
                    _myEventHandler = GetComponent<AllyEventHandler>();

                return _myEventHandler;
            }
        }
        private AllyEventHandler _myEventHandler = null;
        protected AllyMember allyMember
        {
            get
            {
                if (_allyMember == null)
                    _allyMember = GetComponent<AllyMember>();

                return _allyMember;
            }
        }
        private AllyMember _allyMember = null;
        protected AllyAIController aiController
        {
            get
            {
                if (_aiController == null)
                    _aiController = GetComponent<AllyAIController>();

                return _aiController;
            }
        }
        private AllyAIController _aiController = null;
        protected RTSStatHandler statHandler
        {
            get
            {
                //For Faster Access when using OnEnable method
                if (RTSStatHandler.thisInstance != null)
                    return RTSStatHandler.thisInstance;

                return GameObject.FindObjectOfType<RTSStatHandler>();
            }
        }
        protected RTSGameMaster gameMaster { get { return RTSGameMaster.thisInstance; } }
        protected RTSGameMode gamemode { get { return RTSGameMode.thisInstance; } }
        protected RTSUiMaster uiMaster { get { return RTSUiMaster.thisInstance; } }
        protected RTSUiManager uiManager { get { return RTSUiManager.thisInstance; } }
        protected RTSSaveManager saveManager { get { return RTSSaveManager.thisInstance; } }
        protected IGBPI_DataHandler dataHandler { get { return IGBPI_DataHandler.thisInstance; } }
        protected PartyManager myPartyManager { get { return allyMember ? allyMember.partyManager : null; } }
        protected AllyMember allyInCommand
        {
            get
            {
                return myPartyManager != null ? myPartyManager.AllyInCommand : null;
            }
        }

        protected bool AllyComponentsAreReady
        {
            get
            {
                return allyMember && myEventHandler && aiController &&
                  gamemode && gameMaster && uiManager && saveManager;
            }
        }
        #endregion

        #region Fields
        protected bool hasStarted = false;
        protected bool bEnableTactics = true;
        protected bool bPreviouslyEnabledTactics = false;
        protected List<AllyTacticsItem> evalTactics = new List<AllyTacticsItem>();
        public List<AllyTacticsItem> AllyTacticsList;
        public int executionsPerSec = 5;
        #endregion

        #region UnityMessages
        protected virtual void OnEnable()
        {
            if (hasStarted == true)
            {
                SetInitialReferences();
                SubToEvents();
                LoadAndExecuteAllyTactics();
            }
        }

        protected virtual void OnDisable()
        {
            UnsubFromEvents();
            UnLoadAndCancelTactics();
        }

        // Use this for initialization
        protected virtual void Start()
        {
            if (hasStarted == false)
            {
                SetInitialReferences();
                SubToEvents();
                LoadAndExecuteAllyTactics();
                hasStarted = true;
            }
        }

        // Update is called once per frame
        protected virtual void Update()
        {

        }
        #endregion

        #region Handlers
        protected virtual void OnSaveTactics()
        {
            if (bEnableTactics)
            {
                LoadAndExecuteAllyTactics();
            }
        }

        protected virtual void HandleAllyDeath()
        {
            UnsubFromEvents();
            UnLoadAndCancelTactics();
            Destroy(this);
        }

        protected virtual void HandleToggleTactics(bool _enable)
        {
            bPreviouslyEnabledTactics = bEnableTactics;
            bEnableTactics = _enable;
            if (bEnableTactics)
            {
                LoadAndExecuteAllyTactics();
            }
            else
            {
                UnLoadAndCancelTactics();
            }
        }

        #endregion

        #region TacticsMethods
        protected virtual void LoadAndExecuteAllyTactics()
        {
            UnLoadAndCancelTactics();
            var _tactics = statHandler.RetrieveCharacterTactics(
                    allyMember, allyMember.CharacterType);
            foreach (var _data in _tactics.Tactics)
            {
                bool _hasCondition = dataHandler.IGBPI_Conditions.ContainsKey(_data.condition);
                bool _hasAction = dataHandler.IGBPI_Actions.ContainsKey(_data.action);
                int _order = -1;
                bool _hasOrder = int.TryParse(_data.order, out _order) && _order != -1;
                if (_hasCondition && _hasAction && _hasOrder)
                {
                    AllyTacticsList.Add(new AllyTacticsItem(_order,
                        dataHandler.IGBPI_Conditions[_data.condition],
                        dataHandler.IGBPI_Actions[_data.action]));
                }
            }

            if (AllyTacticsList.Count > 0)
                InvokeRepeating("ExecuteAllyTacticsList", 0.05f, 1f / executionsPerSec);
        }

        protected virtual void UnLoadAndCancelTactics()
        {
            if (IsInvoking("ExecuteAllyTacticsList"))
            {
                CancelInvoke("ExecuteAllyTacticsList");
            }
            AllyTacticsList.Clear();
        }

        protected virtual void ExecuteAllyTacticsList()
        {
            if (!AllyComponentsAreReady)
            {
                Debug.LogError("Not All Components are Available, cannot execute Tactics.");
                UnLoadAndCancelTactics();
                UnsubFromEvents();
                Destroy(this);
            }

            // Pause Ally Tactics If Ally Is Paused
            // Due to the Game Pausing Or Control Pause Mode
            // Is Active
            if (myEventHandler.bAllyIsPaused) return;

            //Temporary Fix for PartyManager Delaying Initial AllyInCommand Methods
            if (allyInCommand == null) return;

            evalTactics.Clear();
            foreach (var _tactic in AllyTacticsList)
            {
                if (_tactic.condition.action(allyMember))
                    evalTactics.Add(_tactic);
            }
            if (evalTactics.Count > 0)
            {
                var _currentExecution = EvaluateTacticalConditionOrders(evalTactics);
                if (_currentExecution != null &&
                    _currentExecution.action.action != null)
                {
                    _currentExecution.action.action(allyMember);
                }
            }
        }

        protected virtual AllyTacticsItem EvaluateTacticalConditionOrders(List<AllyTacticsItem> _tactics)
        {
            int _order = int.MaxValue;
            AllyTacticsItem _exeTactic = null;
            foreach (var _tactic in _tactics)
            {
                if (_tactic.order < _order)
                {
                    _order = _tactic.order;
                    _exeTactic = _tactic;
                }
            }
            return _exeTactic;
        }

        #endregion

        #region Initialization
        protected virtual void SetInitialReferences()
        {

        }

        protected virtual void SubToEvents()
        {
            uiMaster.EventOnSaveIGBPIComplete += OnSaveTactics;
            myEventHandler.EventToggleAllyTactics += HandleToggleTactics;
            myEventHandler.EventAllyDied += HandleAllyDeath;
        }

        protected virtual void UnsubFromEvents()
        {
            uiMaster.EventOnSaveIGBPIComplete -= OnSaveTactics;
            myEventHandler.EventToggleAllyTactics -= HandleToggleTactics;
            myEventHandler.EventAllyDied -= HandleAllyDeath;
        }
        #endregion

        #region Structs
        [System.Serializable]
        public class AllyTacticsItem
        {
            public int order;
            public IGBPI_DataHandler.IGBPI_Condition condition;
            public IGBPI_DataHandler.IGBPI_Action action;

            public AllyTacticsItem(int order,
                IGBPI_DataHandler.IGBPI_Condition condition,
                IGBPI_DataHandler.IGBPI_Action action)
            {
                this.order = order;
                this.condition = condition;
                this.action = action;
            }
        }
        #endregion

    }
}