using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BaseFramework
{
    public class UiMaster : MonoBehaviour
    {
        #region Properties
        public static UiMaster thisInstance
        {
            get; protected set;
        }
        #endregion

        #region UnityMessages
        void OnEnable()
        {
            if (thisInstance != null)
                Debug.LogWarning("More than one instance of UiMaster in scene.");
            else
                thisInstance = this;
        }

        // Update is called once per frame
        void Update()
        {

        }
        #endregion
    }
}