using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BaseFramework;

namespace RTSCoreFramework
{
    public class RTSSaveManager : BaseSingleton<RTSSaveManager>
    {
        #region Properties
        protected IGBPI_DataHandler dataHandler { get { return IGBPI_DataHandler.thisInstance; } }
        protected RTSStatHandler statHandler { get { return RTSStatHandler.thisInstance; } }
        protected virtual string tacticsXMLPath
        {
            get
            {
                return $"{Application.dataPath}/StreamingAssets/XML/tactics_data.xml";
            }
        }
        #endregion

        #region UnityMessages
        protected virtual void OnEnable()
        {

        }
        #endregion

        #region PublicAccessors
        public virtual List<CharacterTactics> LoadCharacterTacticsList()
        {
            List<CharacterTactics> _tacticsList = new List<CharacterTactics>();
            foreach (var _checkCharacter in LoadXMLTactics())
            {
                if(_checkCharacter.CharacterType != ECharacterType.NoCharacterType)
                {
                    _tacticsList.Add(new CharacterTactics
                    {
                        CharacterName = _checkCharacter.CharacterName,
                        CharacterType = _checkCharacter.CharacterType,
                        Tactics = ValidateIGBPIValues(_checkCharacter.Tactics)
                    });
                }
            }
            return _tacticsList;
        }

        public virtual List<IGBPIPanelValue> Load_IGBPI_PanelValues(ECharacterType _cType)
        {
            CharacterTactics _tactics;
            if (!isIGBPISavingPermitted(_cType, out _tactics)) return null;
            return ValidateIGBPIValues(_tactics.Tactics);
        }

        public virtual void Save_IGBPI_PanelValues(ECharacterType _cType, List<IGBPI_UI_Panel> _panels)
        {
            CharacterTactics _tactics;
            if (!isIGBPISavingPermitted(_cType, out _tactics)) return;
            List<IGBPIPanelValue> _saveValues = new List<IGBPIPanelValue>();
            foreach (var _panel in _panels)
            {
                _saveValues.Add(new IGBPIPanelValue(
                    _panel.orderText.text,
                    _panel.conditionText.text,
                    _panel.actionText.text
                ));
            }
            Save_IGBPI_Values(_cType, _saveValues);
        }
        #endregion

        #region SaveHelpers
        protected virtual List<IGBPIPanelValue> ValidateIGBPIValues(List<IGBPIPanelValue> _values)
        {
            List<IGBPIPanelValue> _validValues = new List<IGBPIPanelValue>();
            bool _changedSaveFile = false;
            foreach (var _data in _values)
            {
                bool _hasCondition = dataHandler.IGBPI_Conditions.ContainsKey(_data.condition);
                bool _hasAction = dataHandler.IGBPI_Actions.ContainsKey(_data.action);
                int _order = -1;
                bool _hasOrder = int.TryParse(_data.order, out _order) && _order != -1;
                if (_hasCondition && _hasAction && _hasOrder)
                    _validValues.Add(_data);
                else
                    _changedSaveFile = true;
            }
            if (_changedSaveFile) Debug.Log("Loaded Save File Contents will change on next save.");
            return _validValues;
        }

        protected virtual void Save_IGBPI_Values(ECharacterType _cType, List<IGBPIPanelValue> _values)
        {
            CharacterTactics _tactics;
            if (!isIGBPISavingPermitted(_cType, out _tactics)) return;

            List<CharacterTactics> _allCharacterTactics = LoadCharacterTacticsList();
            int _indexOf = -1;
            CharacterTactics _characterToChange = new CharacterTactics
            {
                CharacterName = "",
                CharacterType = ECharacterType.NoCharacterType,
                Tactics = new List<IGBPIPanelValue>()
            };
            foreach (var _checkCharacter in _allCharacterTactics)
            {
                if (_cType != ECharacterType.NoCharacterType &&
                    _checkCharacter.CharacterType == _cType)
                {
                    _indexOf = _allCharacterTactics.IndexOf(_checkCharacter);
                    _characterToChange.CharacterName = _checkCharacter.CharacterName;
                    _characterToChange.CharacterType = _checkCharacter.CharacterType;
                    _characterToChange.Tactics = ValidateIGBPIValues(_values);
                }
            }

            if (_characterToChange.CharacterType != ECharacterType.NoCharacterType &&
                _indexOf != -1)
            {
                _allCharacterTactics[_indexOf] = _characterToChange;
                SaveXMLTactics(_allCharacterTactics);
            }
        }

        protected virtual IEnumerator YieldSave_IGBPI_Values(ECharacterType _cType, List<IGBPIPanelValue> _values)
        {
            Save_IGBPI_Values(_cType, _values);
            yield return new WaitForSecondsRealtime(0.5f);
            Debug.Log("Finished Saving");
        }

        protected virtual bool isIGBPISavingPermitted(ECharacterType _cType, out CharacterTactics _tactics)
        {
            _tactics = GetTacticsFromCharacter(_cType);
            if (_tactics.CharacterType == ECharacterType.NoCharacterType)
            {
                Debug.LogError("No IGBPI Data Object on Save Manager For Character Type " + _tactics.CharacterType.ToString());
                return false;
            }
            if (dataHandler == null)
            {
                Debug.LogError("No Data Handler could be found.");
                return false;
            }
            return true;
        }

        protected virtual CharacterTactics GetTacticsFromCharacter(ECharacterType _cType)
        {
            return statHandler.RetrieveAnonymousCharacterTactics(_cType);
        }

        protected virtual List<IGBPIPanelValue> GetPanelValuesFromCharacter(ECharacterType _cType)
        {
            return GetTacticsFromCharacter(_cType).Tactics;
        }
        #endregion
        
        #region XMLHelpers
        protected virtual void SaveXMLTactics(List<CharacterTactics> _cTacticsList)
        {
            MyXmlManager.SaveXML<List<CharacterTactics>>(_cTacticsList, tacticsXMLPath);
        }

        protected virtual List<CharacterTactics> LoadXMLTactics()
        {
            var _tacticsList = MyXmlManager.LoadXML<List<CharacterTactics>>(tacticsXMLPath);
            if(_tacticsList == null)
            {
                return new List<CharacterTactics>();
            }
            return _tacticsList;
        }
        #endregion
    }
}