﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BaseFramework
{
    public class UiMaster : MonoBehaviour
    {
        #region DelegatesAndEvents
        public delegate void GeneralEventHandler();
        public delegate void MenuToggleHandler(bool enable);
        public event MenuToggleHandler EventMenuToggle;
        public event MenuToggleHandler EventAnyUIToggle;
        #endregion

        #region EventCalls-General/Toggles
        public virtual void CallEventMenuToggle()
        {
            //If Ui Item isn't being used or Pause Menu is turned on
            if (isUiAlreadyInUse == false || isPauseMenuOn)
                WaitToCallEventMenuToggle();
        }

        protected virtual void WaitToCallEventMenuToggle()
        {
            CallEventAnyUIToggle(!isPauseMenuOn);
            if (EventMenuToggle != null) EventMenuToggle(!isPauseMenuOn);
        }

        protected virtual void CallEventAnyUIToggle(bool _enabled)
        {
            if (EventAnyUIToggle != null) EventAnyUIToggle(_enabled);
        }
        #endregion

        #region Properties
        //For Ui Conflict Checking
        public virtual bool isUiAlreadyInUse
        {
            get { return isPauseMenuOn; }
        }
        //Override Inside Wrapper Class
        public virtual bool isPauseMenuOn
        {
            //get { return uiManager.MenuUiPanel.activeSelf; }
            get { return false; }
        }

        public UiManager uiManager
        {
            get { return UiManager.thisInstance; }
        }

        protected GameMaster gamemaster
        {
            get { return GameMaster.thisInstance; }
        }

        public static UiMaster thisInstance
        {
            get; protected set;
        }
        #endregion

        #region UnityMessages
        protected virtual void OnEnable()
        {
            if (thisInstance != null)
                Debug.LogWarning("More than one instance of UiMaster in scene.");
            else
                thisInstance = this;
        }

        // Update is called once per frame
        protected virtual void Update()
        {

        }
        #endregion
    }
}