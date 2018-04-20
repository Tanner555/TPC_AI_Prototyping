using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace RTSCoreFramework
{
    public class RTSGameInstance : MonoBehaviour
    {
        #region Dictionaries
        //Used to Retrieve Information About a Level
        protected Dictionary<LevelIndex, LevelSettings> levelSettingsDictionary = new Dictionary<LevelIndex, LevelSettings>();
        #endregion

        #region Properties
        public static RTSGameInstance thisInstance { get; protected set; }

        public Dictionary<LevelIndex, LevelSettings> LevelSettingsDictionary
        {
            get { return levelSettingsDictionary; }
        }

        //Used For Quick References, May use public methods in the future
        public LevelIndex CurrentLevel = LevelIndex.No_Level;
        public ScenarioIndex CurrentScenario = ScenarioIndex.No_Scenario;
        #endregion

        #region Fields
        [Header("Data Containing Level Settings")]
        [SerializeField]
        protected LevelSettingsData levelSettingsData;
        #endregion

        #region UnityMessages
        // Use this for initialization
        void OnEnable()
        {
            if(thisInstance == null)
            {
                thisInstance = this;
                GameObject.DontDestroyOnLoad(this);
            }
            else
            {
                Debug.Log("More than one game instance in scene, destroying instance");
                DestroyImmediate(this.gameObject);
            }

            InitializeDictionaryValues();
        }

        // Update is called once per frame
        void Update()
        {

        }
        #endregion

        #region LevelManagement
        public void LoadLevel(LevelIndex _level, ScenarioIndex _scenario)
        {
            var _levelSettings = levelSettingsDictionary[_level];
            if(_levelSettings.Scene != null)
            {
                CurrentLevel = _level;
                CurrentScenario = _scenario;
                SceneManager.LoadScene(_levelSettings.Scene.name);
            }
            else
            {
                Debug.Log("There is no scene for Level " + _levelSettings.LevelName);
                return;
            }
        }

        public void RestartCurrentLevel()
        {
            LoadLevel(CurrentLevel, CurrentScenario);
        }

        public void GoToMainMenu()
        {
            LoadLevel(LevelIndex.Main_Menu, ScenarioIndex.No_Scenario);
        }
        #endregion

        #region Initialization
        void InitializeDictionaryValues()
        {
            //Transfer Values From Serialized List To A Dictionary
            //Level Settings Data
            if (levelSettingsData == null)
            {
                Debug.LogError("No levelSettingsData on GameInstance");
                return;
            }
            foreach (var _settings in levelSettingsData.LevelSettingsList)
            {
                levelSettingsDictionary.Add(_settings.Level, _settings);
            }
        }
        #endregion
    }
}