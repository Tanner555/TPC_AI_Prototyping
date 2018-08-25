﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using TMPro;

namespace RTSCoreFramework
{
    /// <summary>
    /// Used To Simplify The UiTarget Registering Process.
    /// Inherit To Add Specific Functionality.
    /// </summary>
    public class RTSUITargetRegister : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler
    {
        #region Fields
        //UiTargetInfo
        protected AllyMember currentUiTarget = null;
        protected bool bHasRegisteredTarget = false;
        #endregion

        #region Properties
        protected RTSUiMaster uiMaster
        {
            get { return RTSUiMaster.thisInstance; }
        }

        protected RTSGameMaster gameMaster
        {
            get { return RTSGameMaster.thisInstance; }
        }

        protected AllyEventHandler uiTargetHandler
        {
            get { return currentUiTarget.allyEventHandler; }
        }
        #endregion

        #region UnityMessages
        protected virtual void OnEnable()
        {
            SubToEvents();
        }

        protected virtual void OnDisable()
        {
            UnsubFromEvents();
        }

        public virtual void OnPointerEnter(PointerEventData eventData)
        {

        }

        public virtual void OnPointerExit(PointerEventData eventData)
        {

        }

        public virtual void OnPointerClick(PointerEventData eventData)
        {

        }
        #endregion

        #region GameMasterHandlers/RegisterUiTarget
        protected virtual void OnRegisterUiTarget(AllyMember _target, AllyEventHandler _handler, PartyManager _party)
        {
            currentUiTarget = _target;
            bHasRegisteredTarget = true;
        }

        protected virtual void OnCheckToDeregisterUiTarget(AllyMember _target, AllyEventHandler _handler, PartyManager _party)
        {
            if (_target == currentUiTarget && bHasRegisteredTarget)
            {
                OnDeregisterUiTarget(_target, _handler, _party);
            }
        }

        protected virtual void OnDeregisterUiTarget(AllyMember _target, AllyEventHandler _handler, PartyManager _party)
        {
            bHasRegisteredTarget = false;
        }

        #endregion

        #region Initialization
        protected virtual void SubToEvents()
        {
            gameMaster.OnRegisterUiTarget += OnRegisterUiTarget;
            gameMaster.OnDeregisterUiTarget += OnDeregisterUiTarget;
        }

        protected virtual void UnsubFromEvents()
        {
            ///Temporary Hides Error When Exiting Playmode
            if (gameMaster == null) return;
            gameMaster.OnRegisterUiTarget -= OnRegisterUiTarget;
            gameMaster.OnDeregisterUiTarget -= OnDeregisterUiTarget;
        }
        #endregion
    }
}