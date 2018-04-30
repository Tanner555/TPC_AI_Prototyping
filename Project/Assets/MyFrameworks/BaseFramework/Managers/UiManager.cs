using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BaseFramework
{
    public class UiManager : MonoBehaviour
    {
        #region Properties
        public GameMaster gamemaster { get { return GameMaster.thisInstance; } }
        public GameMode gamemode { get { return GameMode.thisInstance; } }
        public GameInstance gameInstance { get { return GameInstance.thisInstance; } }

        public virtual bool AllUiCompsAreValid
        {
            get
            {
                return false;
            }
        }

        public UiMaster uiMaster
        {
            get
            {
                if (UiManager.thisInstance != null)
                    return UiMaster.thisInstance;

                return GetComponent<UiMaster>();
            }
        }

        public static UiManager thisInstance
        {
            get; protected set;
        }
        #endregion

        #region Fields
        bool hasStarted = false;
        #endregion

        #region UnityMessages
        protected virtual void OnEnable()
        {
            if (thisInstance != null)
                Debug.LogWarning("More than one instance of UiManager in scene.");
            else
                thisInstance = this;

            SubToEvents();
        }

        protected virtual void Start()
        {
            if (hasStarted == false)
            {
                hasStarted = true;
            }
        }

        // Update is called once per frame
        protected virtual void Update()
        {

        }

        protected virtual void OnDisable()
        {
            UnsubEvents();
        }
        #endregion

        #region Initialization
        protected virtual void SubToEvents()
        {

        }

        protected virtual void UnsubEvents()
        {

        }
        #endregion
    }
}