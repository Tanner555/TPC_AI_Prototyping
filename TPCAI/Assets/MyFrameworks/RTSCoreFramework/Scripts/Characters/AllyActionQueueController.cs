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
        #endregion

        #region ActionQueueProperties
        public RTSActionItem CommandActionItem
        {
            get; protected set;
        }

        public RTSActionItem AIActionItem
        {
            get; protected set;
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
                CommandActionItem = _actionItem;
                bHasCommandActionItem = true;
            }
            else
            {
                AIActionItem = _actionItem;
                bHasCommandActionItem = false;
                CommandActionItem = null;
            }
        }

        protected virtual void OnRemoveCommandActionFromQueue()
        {
            bHasCommandActionItem = false;
            CommandActionItem = null;
        }

        protected virtual void OnRemoveAIActionFromQueue()
        {
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
            if (bHasAnyActions == false) return;

            RTSActionItem _actionToPerform = bCommandActionIsReady ?
                CommandActionItem : AIActionItem;

            if (_actionToPerform.preparationIsComplete(allyMember))
            {
                //Action Either Doesn't Require Action Bar To Be
                //Full OR Action Bar is Already Full and Ready
                if (_actionToPerform.requiresFullActionBar == false ||
                    bActionBarIsFull)
                {
                    _actionToPerform.actionToPerform(allyMember);
                }
            }
        }

        protected virtual void StartServices()
        {
            InvokeRepeating("SE_UpdateActionQueue", 1f, 0.2f);
        }

        protected virtual void StopServices()
        {
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
            myEventHandler.EventAllyDied += OnDeath;
        }

        protected virtual void UnsubFromEvents()
        {
            myEventHandler.OnActiveTimeBarIsFull -= OnActiveTimeBarIsFull;
            myEventHandler.OnActiveTimeBarDepletion -= OnActiveTimeBarDepletion;
            myEventHandler.OnAddActionItemToQueue -= OnAddActionItemToQueue;
            myEventHandler.OnRemoveCommandActionFromQueue -= OnRemoveCommandActionFromQueue;
            myEventHandler.OnRemoveAIActionFromQueue -= OnRemoveAIActionFromQueue;
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
        public Action<AllyMember> actionToPerform;
        /// <summary>
        /// Not Being Performed By AI
        /// </summary>
        public bool isCommandAction;
        public bool requiresFullActionBar;
        /// <summary>
        /// Use (_ally) => true If No Preparation is Required
        /// </summary>
        public Func<AllyMember, bool> preparationIsComplete;

        public RTSActionItem(
            Action<AllyMember> actionToPerform,
            bool isCommandAction,
            bool requiresFullActionBar,
            Func<AllyMember, bool> preparationIsComplete
            )
        {
            this.actionToPerform = actionToPerform;
            this.isCommandAction = isCommandAction;
            this.requiresFullActionBar = requiresFullActionBar;
            this.preparationIsComplete = preparationIsComplete;
        }

        private RTSActionItem()
        {

        }
    }
    #endregion
}