using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BaseFramework
{
    public class GameMaster : MonoBehaviour
    {
        #region Properties
        public static GameMaster thisInstance
        {
            get; protected set;
        }
        #endregion

        #region UnityMessages
        void OnEnable()
        {
            if (thisInstance != null)
                Debug.LogWarning("More than one instance of GameMaster in scene.");
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