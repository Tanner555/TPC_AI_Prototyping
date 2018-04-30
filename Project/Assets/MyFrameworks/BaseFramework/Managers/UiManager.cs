using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BaseFramework
{
    public class UiManager : MonoBehaviour
    {
        #region Properties
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

        #region UnityMessages
        protected virtual void OnEnable()
        {
            if (thisInstance != null)
                Debug.LogWarning("More than one instance of UiManager in scene.");
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