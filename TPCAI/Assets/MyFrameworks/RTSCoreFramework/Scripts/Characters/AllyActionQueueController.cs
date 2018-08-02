using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RTSCoreFramework
{
    public class AllyActionQueueController : MonoBehaviour
    {
        #region Fields
        protected bool bHasCommandActionItem = false;
        protected bool bHasAIActionItem = false;
        protected bool bActionBarIsFull = false;
        protected bool bIsUsingServices = false;
        protected bool bIsInvokingWait = false;
        protected bool bIsInvokingMultExecutions = false;
        #endregion

        #region ActionQueueProperties
        public RTSActionItem CommandActionItem
        {
            get; protected set;
        }

        protected RTSActionItem PreviousCommandActionItem
        {
            get; set;
        }

        public RTSActionItem AIActionItem
        {
            get; protected set;
        }

        protected RTSActionItem PreviousAIActionItem
        {
            get; set;
        }

        protected bool bCommandActionIsReady
        {
            get { return (bHasCommandActionItem && CommandActionItem != null); }
        }

        protected bool bAIActionIsReady
        {
            get { return (bHasAIActionItem && AIActionItem != null); }
        }

        protected bool bHasAnyActions
        {
            get
            {
                return bCommandActionIsReady || bAIActionIsReady;
            }
        }
        #endregion

        #region ComponentProperties
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
                return RTSStatHandler.thisInstance;
            }
        }

        protected AllyMember allyInCommand
        {
            get
            {
                return myPartyManager != null ? myPartyManager.AllyInCommand : null;
            }
        }
        protected RTSGameMaster gameMaster { get { return RTSGameMaster.thisInstance; } }
        protected RTSGameMode gamemode { get { return RTSGameMode.thisInstance; } }
        protected RTSUiMaster uiMaster { get { return RTSUiMaster.thisInstance; } }
        protected RTSUiManager uiManager { get { return RTSUiManager.thisInstance; } }
        protected RTSSaveManager saveManager { get { return RTSSaveManager.thisInstance; } }
        protected IGBPI_DataHandler dataHandler { get { return IGBPI_DataHandler.thisInstance; } }
        protected PartyManager myPartyManager { get { return allyMember ? allyMember.partyManager : null; } }
        #endregion

        #region UnityMessages
        protected virtual void OnEnable()
        {
            SetInitialReferences();
            SubToEvents();
            StartServices();
        }

        protected virtual void OnDisable()
        {
            StopServices();
            UnsubFromEvents();
        }

        // Use this for initialization
        protected virtual void Start()
        {

        }
        #endregion

        #region Handlers
        protected virtual void OnActiveTimeBarIsFull()
        {
            bActionBarIsFull = true;
        }

        protected virtual void OnActiveTimeBarDepletion()
        {
            bActionBarIsFull = false;
        }

        protected virtual void OnAddActionItemToQueue(RTSActionItem _actionItem)
        {
            if (_actionItem == null) return;

            if (_actionItem.isCommandAction)
            {
                //Stops Previous Action Item If It Exists 
                //and Isn't The Current Item
                if (PreviousCommandActionItem != null &&
                    PreviousCommandActionItem != _actionItem)
                {
                    PreviousCommandActionItem.stopPerformingTask(allyMember);
                }
                PreviousCommandActionItem = CommandActionItem;

                //Sets Necessary Toggles and Handlers
                CommandActionItem = _actionItem;
                bHasCommandActionItem = true;

                AIActionItem = null;
                bHasAIActionItem = false;
            }
            else
            {
                //Stops Previous Action Item If It Exists 
                //and Isn't The Current Item
                if (PreviousAIActionItem != null &&
                    PreviousAIActionItem != _actionItem)
                {
                    PreviousAIActionItem.stopPerformingTask(allyMember);
                }
                PreviousAIActionItem = AIActionItem;

                //Sets Necessary Toggles and Handlers
                AIActionItem = _actionItem;
                bHasAIActionItem = true;

                bHasCommandActionItem = false;
                CommandActionItem = null;
            }

            //Only Toggle When Necessary
            if (_actionItem.requiresFullActionBar ||
                _actionItem.requiresActiveBarRegeneration)
            {
                myEventHandler.CallOnToggleActiveTimeRegeneration(true);
            }
            else
            {
                myEventHandler.CallOnToggleActiveTimeRegeneration(false);
            }

            //Start Invoking If Action Requires Full Bar
            //Or Multiple Executions Are Required
            if (_actionItem.requiresFullActionBar)
            {
                if (bIsInvokingWait == false)
                    InvokeWaitingForActionBarToFill(true);


            }
            else
            {
                if(bIsInvokingWait)
                    InvokeWaitingForActionBarToFill(false);

                _actionItem.actionToPerform(allyMember);
            }

            if (_actionItem.executeMultipleTimes)
            {
                if (bIsInvokingMultExecutions == false)
                    InvokeUpdateMultipleExecuteAction(true);
            }
            else
            {
                if (bIsInvokingMultExecutions)
                    InvokeUpdateMultipleExecuteAction(false);
            }
        }

        protected virtual void OnToggleTactics(bool _enable)
        {
            //Only If Turning Tactics Off
            if (_enable) return;

            if (bHasCommandActionItem)
                myEventHandler.CallOnRemoveCommandActionFromQueue();
            if (bHasAIActionItem)
                myEventHandler.CallOnRemoveAIActionFromQueue();
            if (bIsInvokingWait)
                InvokeWaitingForActionBarToFill(false);
            if (bIsInvokingMultExecutions)
                InvokeUpdateMultipleExecuteAction(false);
            if (allyMember.bActiveTimeBarIsRegenerating)
                myEventHandler.CallOnToggleActiveTimeRegeneration(false);
        }

        protected virtual void OnRemoveCommandActionFromQueue()
        {
            InvokeWaitingForActionBarToFill(false);
            InvokeUpdateMultipleExecuteAction(false);
            //Stops Action Item If It Exists 
            if (CommandActionItem != null)
            {
                CommandActionItem.stopPerformingTask(allyMember);
            }
            bHasCommandActionItem = false;
            CommandActionItem = null;
        }

        protected virtual void OnRemoveAIActionFromQueue()
        {
            //Stops Action Item If It Exists 
            if (AIActionItem != null)
            {
                AIActionItem.stopPerformingTask(allyMember);
            }
            bHasAIActionItem = false;
            AIActionItem = null;
        }

        protected virtual void OnDeath()
        {
            StopServices();
            Destroy(this);
        }
        #endregion

        #region Services
        protected virtual void SE_UpdateActionQueue()
        {
            //if (bHasAnyActions == false) return;

            //RTSActionItem _actionToPerform = bCommandActionIsReady ?
            //    CommandActionItem : AIActionItem;

            //if (_actionToPerform.preparationIsComplete(allyMember))
            //{
            //    //Action Either Doesn't Require Action Bar To Be
            //    //Full OR Action Bar is Already Full and Ready
            //    if (_actionToPerform.requiresFullActionBar == false ||
            //        bActionBarIsFull)
            //    {
            //        _actionToPerform.actionToPerform(allyMember);
            //    }
            //}
        }

        protected virtual void SE_WaitForActionBarToFill()
        {

        }

        protected virtual void SE_UpdateMultipleExecuteAction()
        {
            if (bCommandActionIsReady)
            {
                CommandActionItem.actionToPerform(allyMember);
            }
            else if (bAIActionIsReady)
            {
                AIActionItem.actionToPerform(allyMember);
            }
        }

        protected virtual void InvokeWaitingForActionBarToFill(bool _invoke)
        {
            bIsInvokingWait = _invoke;
            if (_invoke)
            {
                if (IsInvoking("SE_WaitForActionBarToFill") == false)
                {
                    InvokeRepeating("SE_WaitForActionBarToFill", 1f, 0.2f);
                }
            }
            else
            {
                if (IsInvoking("SE_WaitForActionBarToFill"))
                {
                    CancelInvoke("SE_WaitForActionBarToFill");
                }
            }
        }

        protected virtual void InvokeUpdateMultipleExecuteAction(bool _invoke)
        {
            bIsInvokingMultExecutions = _invoke;
            if (_invoke)
            {
                if (IsInvoking("SE_UpdateMultipleExecuteAction") == false)
                {
                    InvokeRepeating("SE_UpdateMultipleExecuteAction", 1f, 0.2f);
                }
            }
            else
            {
                if (IsInvoking("SE_UpdateMultipleExecuteAction"))
                {
                    CancelInvoke("SE_UpdateMultipleExecuteAction");
                }
            }
        }

        protected virtual void StartServices()
        {
            bIsUsingServices = true;
        }

        protected virtual void StopServices()
        {
            bIsUsingServices = false;
            CancelInvoke();
        }
        #endregion

        #region Initialization
        protected virtual void SetInitialReferences()
        {

        }

        protected virtual void SubToEvents()
        {
            myEventHandler.OnActiveTimeBarIsFull += OnActiveTimeBarIsFull;
            myEventHandler.OnActiveTimeBarDepletion += OnActiveTimeBarDepletion;
            myEventHandler.OnAddActionItemToQueue += OnAddActionItemToQueue;
            myEventHandler.OnRemoveCommandActionFromQueue += OnRemoveCommandActionFromQueue;
            myEventHandler.OnRemoveAIActionFromQueue += OnRemoveAIActionFromQueue;
            myEventHandler.EventToggleAllyTactics += OnToggleTactics;
            myEventHandler.EventAllyDied += OnDeath;
        }

        protected virtual void UnsubFromEvents()
        {
            myEventHandler.OnActiveTimeBarIsFull -= OnActiveTimeBarIsFull;
            myEventHandler.OnActiveTimeBarDepletion -= OnActiveTimeBarDepletion;
            myEventHandler.OnAddActionItemToQueue -= OnAddActionItemToQueue;
            myEventHandler.OnRemoveCommandActionFromQueue -= OnRemoveCommandActionFromQueue;
            myEventHandler.OnRemoveAIActionFromQueue -= OnRemoveAIActionFromQueue;
            myEventHandler.EventToggleAllyTactics -= OnToggleTactics;
            myEventHandler.EventAllyDied -= OnDeath;
        }
        #endregion
    }

    #region ActionItemClass
    /// <summary>
    /// Used For Queueing Actions Inside AllyActionQueueController
    /// </summary>
    public class RTSActionItem
    {
        //These Variables Are From The IGBPI_Action Struct
        public Action<AllyMember> actionToPerform;
        /// <summary>
        /// Use (_ally) => true If No Additional Condition is Needed
        /// </summary>
        public Func<AllyMember, bool> canPerformAction;
        public ActionFilters actionFilter;
        /// <summary>
        /// Not Being Performed By AI
        /// </summary>
        public bool isCommandAction;
        /// <summary>
        /// Full Action Bar is Needed To Perform Action
        /// </summary>
        public bool requiresFullActionBar;
        /// <summary>
        /// Action requires Action Bar Regeneration, But 
        /// Doesn't Necessarily Need a Full Bar to Perform
        /// </summary>
        public bool requiresActiveBarRegeneration;
        /// <summary>
        /// Set to False if Action Should Only Be Executed Once
        /// </summary>
        public bool executeMultipleTimes;
        /// <summary>
        /// Use (_ally) => true If No Preparation is Required
        /// </summary>
        public Func<AllyMember, bool> preparationIsComplete;
        /// <summary>
        /// Essential To Telling When the Given Task Is Completed.
        /// Will Execute Once Even If Task Is Already Finished
        /// </summary>
        public Func<AllyMember, bool> taskIsFinished;
        /// <summary>
        /// Optional Action That Will Stop The Execution Of A Task.
        /// Use (_ally) => {} if Stopping Isn't Needed
        /// </summary>
        public Action<AllyMember> stopPerformingTask;

        public RTSActionItem(
            Action<AllyMember> actionToPerform,
            Func<AllyMember, bool> canPerformAction,
            ActionFilters actionFilter,
            bool isCommandAction,
            bool requiresFullActionBar,
            bool requiresActiveBarRegeneration,
            bool executeMultipleTimes,
            Func<AllyMember, bool> preparationIsComplete,
            Func<AllyMember, bool> taskIsFinished,
            Action<AllyMember> stopPerformingTask
            )
        {
            this.actionToPerform = actionToPerform;
            this.canPerformAction = canPerformAction;
            this.actionFilter = actionFilter;
            this.isCommandAction = isCommandAction;
            this.requiresActiveBarRegeneration = requiresActiveBarRegeneration;
            this.requiresFullActionBar = requiresFullActionBar;
            this.executeMultipleTimes = executeMultipleTimes;
            this.preparationIsComplete = preparationIsComplete;
            this.taskIsFinished = taskIsFinished;
            this.stopPerformingTask = stopPerformingTask;
        }

        private RTSActionItem()
        {

        }
    }
    #endregion
}