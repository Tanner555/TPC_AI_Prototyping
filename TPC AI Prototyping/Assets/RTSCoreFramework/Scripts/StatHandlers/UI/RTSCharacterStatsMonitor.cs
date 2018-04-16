using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

namespace RTSCoreFramework
{
    public class RTSCharacterStatsMonitor : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler
    {
        #region Properties
        bool AllCompsAreValid
        {
            get {
                return UiCharacterPortrait && UiHealthSlider && UiAbilitySlider
                  && CurrentHealthText && MaxHealthText && CurrentAbilityText
                  && MaxAbilityText && CharacterNameText && UiCharacterPortraitPanel;
            }
        }

        //UiTarget Props
        AllyEventHandler uiTargetHandler
        {
            get { return uiTarget.allyEventHandler; }
        }

        public bool bUiTargetIsSet { get; protected set; }

        //Easy Access Image Props
        Image StatPanelImage
        {
            get
            {
                if (_StatPanelImage == null)
                    _StatPanelImage = GetComponent<Image>();

                return _StatPanelImage;
            }
        }
        Image _StatPanelImage = null;

        Image PortraitPanelImage
        {
            get
            {
                if (_PortraitPanelImage == null)
                    _PortraitPanelImage = UiCharacterPortraitPanel.GetComponent<Image>();

                return _PortraitPanelImage;
            }
        }
        Image _PortraitPanelImage = null;
        #endregion

        #region Fields
        //Ui Gameobjects
        [Header("Ui GameObjects")]
        [SerializeField]
        Image UiCharacterPortraitPanel;
        [SerializeField]
        Image UiCharacterPortrait;
        [SerializeField]
        GameObject UiHealthSlider;
        [SerializeField]
        GameObject UiAbilitySlider;
        [SerializeField]
        Text CurrentHealthText;
        [SerializeField]
        Text MaxHealthText;
        [SerializeField]
        Text CurrentAbilityText;
        [SerializeField]
        Text MaxAbilityText;
        [SerializeField]
        Text CharacterNameText;
        //Ui Target Info
        AllyMember uiTarget = null;
        //Colors
        [Header("Colors")]
        [SerializeField] Color HighlightColor;
        [SerializeField] Color SelectedColor;
        //Not Serialized Colors Used to Reference Start Color
        Color NormalStatPanelColor;
        Color NormalPortraitPanelColor;
        //hover info fields
        bool bIsHighlighted = false;
        #endregion

        #region UnityMessages
        private void OnEnable()
        {
            if (AllCompsAreValid)
            {
                NormalStatPanelColor = StatPanelImage.color;
                NormalPortraitPanelColor = PortraitPanelImage.color;
            }
        }

        public void OnPointerEnter(PointerEventData eventData)
        {
            if (uiTarget != null &&
                uiTargetHandler && !uiTarget.bIsCurrentPlayer)
            {
                uiTargetHandler.CallEventOnHoverOver();
            }
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            if (uiTarget != null && uiTargetHandler && 
                !uiTarget.bIsCurrentPlayer)
            {
                uiTargetHandler.CallEventOnHoverLeave();
            }
        }

        public void OnPointerClick(PointerEventData eventData)
        {
            bool _leftClick = eventData.button == 
                PointerEventData.InputButton.Left;
            if (_leftClick && uiTarget != null && uiTargetHandler && 
                !uiTarget.bIsCurrentPlayer &&
                uiTarget.partyManager != null)
            {
                uiTarget.partyManager.SetAllyInCommand(uiTarget);
                uiTargetHandler.CallEventOnHoverLeave();
            }
        }
        #endregion

        #region HookingUiTarget-Initialization
        public void HookAllyCharacter(AllyMember _targetToSet)
        {
            if(AllCompsAreValid == false)
            {
                Debug.LogError(@"Cannot Hook Character Because
                Not All Components Are Set In The Inspector");
                return;
            }
            if (_targetToSet != null)
            {
                SetupUITargetHandlers(uiTarget, _targetToSet);
                uiTarget = _targetToSet;
                if (uiTarget.bIsCurrentPlayer)
                {
                    SetToSelectedColor();
                }
            }
        }

        protected virtual void SetupUITargetHandlers(AllyMember _previousTarget, AllyMember _currentTarget)
        {
            if (_previousTarget != null && _previousTarget.bAllyIsUiTarget)
            {
                UnsubscribeFromUiTargetHandlers(_previousTarget);
            }
            if (_currentTarget != null && !_currentTarget.bAllyIsUiTarget)
            {
                SubscribeToUiTargetHandlers(_currentTarget);
                TransferCharacterStatsToText(_currentTarget);
            }
        }

        protected virtual void TransferCharacterStatsToText(AllyMember _ally)
        {
            CharacterNameText.text = _ally.CharacterName;
        }
        #endregion

        #region HookingUiTarget-Subscribe/Desubscribe
        protected virtual void SubscribeToUiTargetHandlers(AllyMember _target)
        {
            if (_target == null) return;
            var _handler = _target.allyEventHandler;
            SetAllyIsUiTarget(_target, true);
            //Sub to Current UiTarget Handlers
            _handler.OnHealthChanged += UiTargetHandle_OnHealthChanged;
            _handler.EventAllyDied += UiTargetHandle_OnAllyDeath;
            _handler.EventSetAsCommander += UiTargetHandle_SetAsCommander;
            _handler.EventSwitchingFromCom += UiTargetHandle_SwitchFromCommander;
            _handler.OnHoverOver += UiTargetHandle_OnHoverOver;
            _handler.OnHoverLeave += UiTargetHandle_OnHoverLeave;
        }

        protected virtual void UnsubscribeFromUiTargetHandlers(AllyMember _target)
        {
            if (_target == null) return;
            var _handler = _target.allyEventHandler;
            SetAllyIsUiTarget(_target, false);
            //Unsub From Previous UiTarget Handlers
            _handler.OnHealthChanged -= UiTargetHandle_OnHealthChanged;
            _handler.EventAllyDied -= UiTargetHandle_OnAllyDeath;
            _handler.EventSetAsCommander -= UiTargetHandle_SetAsCommander;
            _handler.EventSwitchingFromCom -= UiTargetHandle_SwitchFromCommander;
            _handler.OnHoverOver -= UiTargetHandle_OnHoverOver;
            _handler.OnHoverLeave -= UiTargetHandle_OnHoverLeave;
        }

        void SetAllyIsUiTarget(AllyMember _target, bool _isTarget)
        {
            if (_target == null) return;
            var _handler = _target.allyEventHandler;
            _handler.SetAllyIsUiTarget(_isTarget);
            bUiTargetIsSet = _isTarget;
        }
        #endregion

        #region UITargetHandlers
        protected virtual void UiTargetHandle_OnHealthChanged(int _current, int _max)
        {
            if (AllCompsAreValid == false) return;
            CurrentHealthText.text = _current.ToString();
            MaxHealthText.text = _max.ToString();
            Slider _slider = UiHealthSlider.GetComponent<Slider>();
            if (_slider != null)
            {
                _slider = UiHealthSlider.GetComponent<Slider>();
                _slider.maxValue = _max;
                _slider.value = _current;
            }
        }

        protected virtual void UiTargetHandle_OnAllyDeath()
        {
            if(uiTarget != null && uiTarget.bAllyIsUiTarget)
            {
                UnsubscribeFromUiTargetHandlers(uiTarget);
            }
        }

        protected virtual void UiTargetHandle_SetAsCommander()
        {
            SetToSelectedColor();
        }

        protected virtual void UiTargetHandle_SwitchFromCommander()
        {
            SetToNormalColor();
        }

        protected virtual void UiTargetHandle_OnHoverOver()
        {
            if(bIsHighlighted == false && uiTarget != null &&
                !uiTarget.bIsCurrentPlayer)
            {
                SetToHighlightColor();
            }
        }

        protected virtual void UiTargetHandle_OnHoverLeave()
        {
            if (bIsHighlighted && uiTarget != null &&
                !uiTarget.bIsCurrentPlayer)
            {
                SetToNormalColor();
            }
        }
        #endregion

        #region Helpers
        void SetToHighlightColor()
        {
            if(AllCompsAreValid && PortraitPanelImage != null)
            {
                bIsHighlighted = true;
                PortraitPanelImage.color = HighlightColor;
            }
        }

        void SetToSelectedColor()
        {
            if (AllCompsAreValid && PortraitPanelImage != null)
            {
                bIsHighlighted = false;
                PortraitPanelImage.color = SelectedColor;
            }
        }

        void SetToNormalColor()
        {
            if (AllCompsAreValid && PortraitPanelImage != null)
            {
                bIsHighlighted = false;
                PortraitPanelImage.color = NormalPortraitPanelColor;
            }
        }
        #endregion
    }
}