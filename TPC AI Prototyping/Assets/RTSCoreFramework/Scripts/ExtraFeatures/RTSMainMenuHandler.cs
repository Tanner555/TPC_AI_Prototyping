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
        [SerializeField]
        public Object LoadScene;

        public void Btn_PlayGame()
        {
            if (LoadScene != null)
            {
                SceneManager.LoadScene(LoadScene.name);
            }
            else
            {
                Debug.Log("Please drag a scene into the main menu handler");
            }
        }

        public void Btn_QuitGame()
        {
            Application.Quit();
        }
    }
}