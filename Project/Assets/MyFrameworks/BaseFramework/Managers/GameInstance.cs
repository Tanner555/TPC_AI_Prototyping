using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BaseFramework
{
    public class GameInstance : MonoBehaviour
    {
        #region Dictionaries
        //Used to Retrieve Information About a Level
        protected Dictionary<LevelIndex, LevelSettings> levelSettingsDictionary = new Dictionary<LevelIndex, LevelSettings>();
        protected Dictionary<ScenarioIndex, ScenarioSettings> currentScenarioSettingsDictionary = new Dictionary<ScenarioIndex, ScenarioSettings>();
        #endregion

        #region Properties
        public static GameInstance thisInstance
        {
            get; protected set;
        }

        public Dictionary<LevelIndex, LevelSettings> LevelSettingsDictionary
        {
            get { return levelSettingsDictionary; }
        }

        public Dictionary<ScenarioIndex, ScenarioSettings> CurrentScenarioSettingsDictionary
        {
            get { return currentScenarioSettingsDictionary; }
        }

        public LevelIndex CurrentLevel
        {
            get { return currentLevel; }
        }

        public ScenarioIndex CurrentScenario
        {
            get { return currentScenario; }
        }

        //Used To Protect Against Invalid Retrieval of CurrentScenarioSettings
        protected bool bCurrentScenarioIsValid
        {
            get { return CurrentScenario != ScenarioIndex.No_Scenario; }
        }
        //Quick Getter For Scenario Settings, Could Cause Issues
        protected ScenarioSettings CurrentScenarioSettings
        {
            get
            {
                if (bCurrentScenarioIsValid)
                    return currentScenarioSettingsDictionary[CurrentScenario];

                return new ScenarioSettings();
            }
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

        #region Fields
        [Header("Current Level Info")]
        [SerializeField]
        protected LevelIndex currentLevel = LevelIndex.Main_Menu;
        [SerializeField]
        protected ScenarioIndex currentScenario = ScenarioIndex.No_Scenario;
        [SerializeField]
        protected int FrameRateLimit = 60;
        [Header("Data Containing Level Settings")]
        [SerializeField]
        protected LevelSettingsData levelSettingsData;
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

        #region LevelManagement
        public virtual void LoadLevel(LevelIndex _level, ScenarioIndex _scenario)
        {
            
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