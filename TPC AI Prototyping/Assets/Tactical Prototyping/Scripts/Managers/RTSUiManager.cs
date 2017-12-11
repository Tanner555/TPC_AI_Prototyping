using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace RTSPrototype
{
    public class RTSUiManager : MonoBehaviour
    {
        #region Components

        #endregion

        #region Properties
        public static RTSUiManager thisInstance
        {
            get; protected set;
        }

        //public IGBPI_DataHandler dataHandler { get { return IGBPI_DataHandler.thisInstance; } }
        //public RTSSaveManager saveManager { get { return RTSSaveManager.thisInstance; } }

        //UGBPI
        public bool isBehaviorUIOn { get { return IGBPIUi != null && IGBPIUi.activeSelf; } }
        public List<IGBPI_UI_Panel> UI_Panel_Members { get; protected set; }
        public IGBPI_UI_Panel UIPanelSelection
        {
            get; protected set;
        }
        public IGBPI_UI_Panel PreviousPanelSelection
        {
            get; protected set;
        }

        public bool IGBPICompsAreValid
        {
            get
            {
                return UI_Panel_Prefab &&
ButtonChoicePrefab && behaviorContentTransform &&
choiceMenuTransform && choiceIndicatorText &&
choiceNavigateLeft && choiceNavigateRight &&
conditionButton && actionButton;
            }
        }
        #endregion

        #region Fields
        enum choiceEditModes
        {
            condition, action, none
        }
        choiceEditModes ChoiceEditMode = choiceEditModes.none;
        string choiceFilterNameNone = "Filters";
        bool hasStarted = false;
        //Used as a parameter for adding a dropdown instance
        bool usePanelCreationValues = false;
        IGBPIPanelValue panelCreationValues;
        #endregion

        #region UIGameObjects
        [Header("Main Ui GameObjects")]
        public GameObject PauseMenuUi;
        public GameObject InventoryUi;
        public GameObject IGBPIUi;
        [Header("Health Canvas")]
        public Text healthText;
        [Header("Ammo Canvas")]
        public GameObject AmmoPanel;
        public Text currentAmmoText;
        public Text carriedAmmoText;
        [Header("HurtCanvas")]
        public GameObject hurtCanvas;
        private float secsTillHideHurtCanvas = 2;
        [Header("IGBPI Objects")]
        public GameObject UI_Panel_Prefab;
        public GameObject ButtonChoicePrefab;
        public Transform behaviorContentTransform;
        public Transform choiceMenuTransform;
        public Text choiceIndicatorText;
        public Button choiceNavigateLeft;
        public Button choiceNavigateRight;
        public Button conditionButton;
        public Button actionButton;
        #endregion

        #region UnityMessages
        private void OnEnable()
        {
            if (thisInstance != null)
                Debug.LogWarning("More than one instance of UIManager in scene.");
            else
                thisInstance = this;

            //if (!IGBPICompsAreValid)
            //    Debug.LogError("Please drag components into their slots");

            //UI_Panel_Members = new List<IGBPI_UI_Panel>();
            //DisableIGBPIEditButtons();

            //if (hasStarted == true)
            //    SubToEvents();


        }

        private void Start()
        {
            //if (hasStarted == false)
            //{
            //    SubToEvents();
            //    hasStarted = true;
            //}
        }

        private void OnDisable()
        {
            //UnsubEvents();
        }
        #endregion

        #region UiMethodCalls

        #endregion

        #region ButtonCalls

        #endregion

        #region Handlers

        #endregion

        #region BehaviorUIMethods

        #endregion

        #region Initialization

        #endregion

    }
}