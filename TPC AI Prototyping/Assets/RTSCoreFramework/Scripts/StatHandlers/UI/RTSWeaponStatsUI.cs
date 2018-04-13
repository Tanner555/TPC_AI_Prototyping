using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

namespace RTSCoreFramework
{
    public class RTSWeaponStatsUI : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
    {
        #region UIGameObjects
        [Header("Primary Weapon UI")]
        [SerializeField]
        private GameObject PrimaryWeaponGameObject;
        [SerializeField]
        private Image PrimaryWeaponUIImage;
        [SerializeField]
        private Text PrimaryLoadedText;
        [SerializeField]
        private Text PrimaryUnloadedText;
        [Header("Secondary Weapon UI")]
        [SerializeField]
        private GameObject SecondaryWeaponGameObject;
        [SerializeField]
        private Image SecondaryWeaponUIImage;
        [SerializeField]
        private Text SecondaryLoadedText;
        [SerializeField]
        private Text SecondaryUnloadedText;
        #endregion

        #region Fields
        Color currentColor;
        Color hoverColor;

        AllyMember currentUiTarget = null;
        bool bHasRegisteredTarget = false;
        #endregion

        #region Properties
        RTSUiMaster uiMaster
        {
            get
            {
                if (_uiMaster == null)
                {
                    if ((_uiMaster = RTSUiMaster.thisInstance) == null)
                    {
                        _uiMaster = GetComponentInParent<RTSUiMaster>();
                    }
                }
                return _uiMaster;
            }
        }
        RTSUiMaster _uiMaster = null;

        RTSGameMaster gameMaster
        {
            get
            {
                if (_gameMaster == null)
                {
                    if ((_gameMaster = RTSGameMaster.thisInstance) == null)
                    {
                        _gameMaster = GameObject.FindObjectOfType<RTSGameMaster>();
                    }
                }
                return _gameMaster;
            }
        }
        RTSGameMaster _gameMaster = null;
        #endregion

        #region UnityMessages
        private void OnEnable()
        {
            SubToEvents();
        }

        private void OnDisable()
        {
            UnsubFromEvents();
        }

        public void OnPointerEnter(PointerEventData eventData)
        {
            
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            
        }
        #endregion

        #region Handlers
        void OnRegisterUiTarget(AllyMember _target, AllyEventHandler _handler, PartyManager _party)
        {
            currentUiTarget = _target;
            _handler.OnAmmoChanged += OnWeaponAmmoChanged;
            bHasRegisteredTarget = true;
        }

        void OnDeregisterUiTarget(AllyMember _target, AllyEventHandler _handler, PartyManager _party)
        {
            if(_target == currentUiTarget && bHasRegisteredTarget)
            {
                _handler.OnAmmoChanged -= OnWeaponAmmoChanged;
                bHasRegisteredTarget = false;
            }
        }

        #endregion

        #region UiTargetHandlers
        void OnWeaponAmmoChanged(int _loaded, int _max)
        {
            PrimaryLoadedText.text = _loaded.ToString();
            PrimaryUnloadedText.text = _max.ToString();
        }
        #endregion

        #region Initialization
        void SubToEvents()
        {
            gameMaster.OnRegisterUiTarget += OnRegisterUiTarget;
            gameMaster.OnDeregisterUiTarget += OnDeregisterUiTarget;
        }

        void UnsubFromEvents()
        {
            //Temporary Hides Error When Exiting Playmode
            if (gameMaster == null) return;
            gameMaster.OnRegisterUiTarget -= OnRegisterUiTarget;
            gameMaster.OnDeregisterUiTarget -= OnDeregisterUiTarget;
        }
        #endregion

    }
}