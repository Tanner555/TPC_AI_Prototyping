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

        }

        // Update is called once per frame
        protected virtual void Update()
        {

        }

        protected virtual void OnDisable()
        {

        }
        #endregion
    }
}