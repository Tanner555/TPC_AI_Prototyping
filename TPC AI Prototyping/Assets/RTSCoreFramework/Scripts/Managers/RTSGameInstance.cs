using System.Collections;
using System.Collections.Generic;
using System.Linq;
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

        public void GoToNextLevel()
        {
            LevelSettings _level;
            if(GetNextLevelIsSuccessful(out _level))
            {
                LoadLevel(_level.Level, ScenarioIndex.Scenario_1);
            }
            else
            {
                Debug.LogError("Couldn't Load Next Level Because Level Settings doesn't exist");
            }
        }

        public void GoToNextScenario()
        {
            //TODO:RTSPrototype Implement GoToNextScenario Functionality
            Debug.Log("Going to next scenario");
        }
        #endregion

        #region Getters/Checks
        public bool IsLoadingNextPermitted(out bool _nextScenario, out bool _nextLevel)
        {
            _nextScenario = IsLoadingNextScenarioPermitted();
            _nextLevel = IsLoadingNextLevelPermitted();
            return _nextScenario || _nextLevel;
        }

        private bool IsLoadingNextScenarioPermitted()
        {
            return false;
        }

        private bool IsLoadingNextLevelPermitted()
        {
            LevelSettings _level;
            return GetNextLevelIsSuccessful(out _level);
        }

        private bool GetNextLevelIsSuccessful(out LevelSettings _level)
        {
            var _keysDic = levelSettingsDictionary.Keys;
            var _keysList = _keysDic.ToList();
            int _keyIndex = _keysList.IndexOf(CurrentLevel);
            LevelIndex _nextLevel = GetLevelIndexFromNumber(_keyIndex + 1);
            if (_keyIndex + 1 > 0 && _keyIndex + 1 <= _keysList.Count && 
                _nextLevel != LevelIndex.No_Level &&
                levelSettingsDictionary.ContainsKey(_nextLevel))
            {
                _level = levelSettingsDictionary[_nextLevel];
                return true;
            }
            _level = levelSettingsDictionary[CurrentLevel];
            return false;
        }

        private LevelIndex GetLevelIndexFromNumber(int _index)
        {
            switch (_index)
            {
                case 0:
                    return LevelIndex.Main_Menu;
                case 1:
                    return LevelIndex.Level_1;
                case 2:
                    return LevelIndex.Level_2;
                case 3:
                    return LevelIndex.Level_3;
                case 4:
                    return LevelIndex.Level_4;
                case 5:
                    return LevelIndex.Level_5;
                default:
                    return LevelIndex.No_Level;
            }
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