using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RTSCoreFramework
{
    public enum LevelIndex
    {
        Main_Menu = 0,
        Level_1 = 1,
        Level_2 = 2,
        Level_3 = 3,
        Level_4 = 4,
        Level_5 = 5,
        //Used For Error Checking
        No_Level = -1
    }

    public enum ScenarioIndex
    {
        Scenario_1 = 0,
        Scenario_2 = 1,
        Scenario_3 = 2,
        Scenario_4 = 3,
        Scenario_5 = 4,
        Scenario_6 = 5,
        //Used For Error Checking
        No_Scenario = -1
    }

    [System.Serializable]
    public struct LevelSettings
    {
        public string LevelName;
        public LevelIndex Level;
        public Sprite LevelImage;
        public Object Scene;
        //May add a Scenario Amount in the future
        //Not Needed Right Now
        //public string ScenarioName;
        //public ScenarioIndex Scenario;
    }

    [CreateAssetMenu(menuName = "RTSPrototype/LevelSettingsData")]
    public class LevelSettingsData : ScriptableObject
    {
        [Header("Level Settings")]
        [SerializeField]
        public List<LevelSettings> LevelSettingsList = new List<LevelSettings>();
    }
}