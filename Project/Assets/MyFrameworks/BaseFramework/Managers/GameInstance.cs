using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BaseFramework
{
    public class GameInstance : MonoBehaviour
    {
        #region Properties
        public static GameInstance thisInstance
        {
            get; protected set;
        }
        #endregion

        #region UnityMessages
        private void OnEnable()
        {
            if (thisInstance == null)
            {
                thisInstance = this;
                GameObject.DontDestroyOnLoad(this);
            }
            else
            {
                Debug.Log("More than one game instance in scene, destroying instance");
                DestroyImmediate(this.gameObject);
            }
        }
        #endregion

    }
}