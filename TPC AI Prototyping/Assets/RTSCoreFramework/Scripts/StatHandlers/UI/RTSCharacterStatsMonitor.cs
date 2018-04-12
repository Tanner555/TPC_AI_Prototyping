using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace RTSCoreFramework
{
    public class RTSCharacterStatsMonitor : MonoBehaviour
    {
        #region Properties
        bool AllCompsAreValid
        {
            get {
                return UiCharacterPortrait && UiHealthSlider && UiAbilitySlider
                  && CurrentHealthText && MaxHealthText && CurrentAbilityText
                  && MaxAbilityText && CharacterNameText;
            }
        }
        #endregion

        #region Fields
        //Ui Gameobjects
        [Header("Ui GameObjects")]
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
        #endregion

        #region UnityMessages
        // Use this for initialization
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {

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
            }
        }
        #endregion

        #region HookingUiTarget-Subscribe/Desubscribe
        protected virtual void SubscribeToUiTargetHandlers(AllyMember _target)
        {
            if (_target == null) return;
            var _handler = _target.allyEventHandler;
            _handler.SetAllyIsUiTarget(true);
            //Sub to Current UiTarget Handlers
            _handler.OnHealthChanged += UiTargetHandle_OnHealthChanged;
            _handler.EventAllyDied += UiTargetHandle_OnAllyDeath;
        }

        protected virtual void UnsubscribeFromUiTargetHandlers(AllyMember _target)
        {
            if (_target == null) return;
            var _handler = _target.allyEventHandler;
            _handler.SetAllyIsUiTarget(false);
            //Unsub From Previous UiTarget Handlers
            _handler.OnHealthChanged -= UiTargetHandle_OnHealthChanged;
            _handler.EventAllyDied -= UiTargetHandle_OnAllyDeath;
        }
        #endregion
        
        #region UITargetHandlers
        protected virtual void UiTargetHandle_OnHealthChanged(int _current, int _max)
        {
            if (AllCompsAreValid == false) return;
            CurrentHealthText.text = _current.ToString();
            MaxHealthText.text = _max.ToString();
        }

        protected virtual void UiTargetHandle_OnAllyDeath()
        {
            if(uiTarget != null && uiTarget.bAllyIsUiTarget)
            {
                UnsubscribeFromUiTargetHandlers(uiTarget);
            }
        }
        #endregion

    }
}