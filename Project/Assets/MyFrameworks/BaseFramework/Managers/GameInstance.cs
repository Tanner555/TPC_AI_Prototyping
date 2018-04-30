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

        protected virtual int RefreshRate
        {
            get
            {
                if (_refreshRate == -1)
                    _refreshRate = Screen.currentResolution.refreshRate;

                return _refreshRate;
            }
        }
        private int _refreshRate = -1;
        #endregion

        #region UnityMessages
        protected virtual void OnEnable()
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

        protected virtual void Update()
        {
            UpdateFrameRateLimit();
        }
        #endregion

        #region Updaters
        void UpdateFrameRateLimit()
        {
            if (QualitySettings.vSyncCount != 0)
            {
                //Frame Limit Doesn't Work If VSync Is Set Above 0
                QualitySettings.vSyncCount = 0;
            }
            if (Application.targetFrameRate != RefreshRate)
            {
                Application.targetFrameRate = RefreshRate;
            }
        }
        #endregion

    }
}