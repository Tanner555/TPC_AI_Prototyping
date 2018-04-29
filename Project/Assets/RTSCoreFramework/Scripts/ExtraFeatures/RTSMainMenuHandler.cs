using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace RTSCoreFramework
{
    /// <summary>
    /// Temporary Class For Handling the Main Menu
    /// Will Likely Replace in the Future with a Global
    /// Game Manager Class
    /// </summary>
    public class RTSMainMenuHandler : MonoBehaviour
    {
        RTSGameInstance gameInstance
        {
            get { return RTSGameInstance.thisInstance; }
        }

        [SerializeField]
        public LevelIndex loadLevel;
        [SerializeField]
        public ScenarioIndex scenario;

        public void Btn_PlayGame()
        {
            if(gameInstance != null)
            {
                gameInstance.LoadLevel(loadLevel, scenario);
            }
        }

        public void Btn_QuitGame()
        {
            Application.Quit();
        }

        #region Testing
        //private void Start()
        //{
        //    Time.timeScale = 0f;
        //    StartCoroutine(CallTest1(0.4f));
        //    var _chart = GameObject.FindObjectOfType<Fungus.Flowchart>();
        //    if (_chart != null)
        //    {
        //        _chart.SendFungusMessage("Start");
        //    }

        //}

        //IEnumerator CallTest1(float _delay)
        //{
        //    yield return new WaitForSecondsRealtime(_delay);
        //    Invoke("Test1", 0.0f);
        //}

        //void Test1()
        //{
        //    Debug.Log("Start From Test1");
        //}
        #endregion

    }
}