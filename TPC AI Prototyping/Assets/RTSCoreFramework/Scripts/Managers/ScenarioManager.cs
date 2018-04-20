using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RTSCoreFramework
{
    public class ScenarioManager : MonoBehaviour
    {
        #region Properties
        RTSGameInstance gameInstance
        {
            get { return RTSGameInstance.thisInstance; }
        }

        ScenarioIndex levelScenario
        {
            get { return gameInstance.CurrentScenario; }
        }
        #endregion

        #region Fields
        [Header("Independent Game Objects")]
        [SerializeField]
        GameObject RTSManagersObject;
        [SerializeField]
        GameObject RTSCoreCanvas;
        [SerializeField]
        GameObject RTSMainCamera;
        [Header("Scenario Dependent Game Objects")]
        [SerializeField]
        GameObject Scenario_1_Spawners;
        [SerializeField]
        GameObject Scenario_2_Spawners;
        [SerializeField]
        GameObject Scenario_3_Spawners;
        [SerializeField]
        GameObject Scenario_4_Spawners;
        [SerializeField]
        GameObject Scenario_5_Spawners;
        [SerializeField]
        GameObject Scenario_6_Spawners;
        #endregion

        #region UnityMessages
        private void Awake()
        {
            if (RTSMainCamera.activeSelf)
            {
                RTSMainCamera.SetActive(false);
            }
            SetupScenario();
        }
        #endregion

        #region ScenarioSetup
        void SetupScenario()
        {
            switch (levelScenario)
            {
                case ScenarioIndex.Scenario_1:
                    ActivateAllObjects(Scenario_1_Spawners);
                    break;
                case ScenarioIndex.Scenario_2:
                    ActivateAllObjects(Scenario_2_Spawners);
                    break;
                case ScenarioIndex.Scenario_3:
                    ActivateAllObjects(Scenario_3_Spawners);
                    break;
                case ScenarioIndex.Scenario_4:
                    ActivateAllObjects(Scenario_4_Spawners);
                    break;
                case ScenarioIndex.Scenario_5:
                    ActivateAllObjects(Scenario_5_Spawners);
                    break;
                case ScenarioIndex.Scenario_6:
                    ActivateAllObjects(Scenario_6_Spawners);
                    break;
                case ScenarioIndex.No_Scenario:
                    ActivateAllObjects(null);
                    break;
                default:
                    break;
            }
        }

        void ActivateAllObjects(GameObject _scenarioSpawners)
        {
            RTSManagersObject.SetActive(true);
            RTSCoreCanvas.SetActive(true);
            RTSMainCamera.SetActive(true);
            if (_scenarioSpawners != null)
            {
                _scenarioSpawners.SetActive(true);
            }
            else
            {
                Debug.LogError(@"No Spawners Have Been Assigned For 
                Scenario " + levelScenario.ToString());
            }
        }
        #endregion

    }
}