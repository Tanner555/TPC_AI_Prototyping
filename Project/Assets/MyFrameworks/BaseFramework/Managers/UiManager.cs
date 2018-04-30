using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BaseFramework
{
    public class UiManager : MonoBehaviour
    {
        #region Properties
        public static UiManager thisInstance
        {
            get; protected set;
        }
        #endregion

        #region UnityMessages
        void OnEnable()
        {
            if (thisInstance != null)
                Debug.LogWarning("More than one instance of UiManager in scene.");
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