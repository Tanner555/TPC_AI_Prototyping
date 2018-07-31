﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BaseFramework;

namespace RTSCoreFramework
{
    public class RTSSaveManager : MonoBehaviour
    {
        public static RTSSaveManager thisInstance { get; protected set; }
        protected IGBPI_DataHandler dataHandler { get { return IGBPI_DataHandler.thisInstance; } }
        protected RTSStatHandler statHandler { get { return RTSStatHandler.thisInstance; } }
        protected virtual string tacticsXMLPath
        {
            get
            {
                return $"{Application.dataPath}/StreamingAssets/XML/tactics_data.xml";
            }
        }

        protected virtual void OnEnable()
        {
            if (thisInstance != null)
                Debug.LogError("More than one save manager in scene");
            else
                thisInstance = this;

        }

        public virtual List<IGBPIPanelValue> Load_IGBPI_PanelValues(ECharacterType _cType)
        {
            CharacterTactics _tactics;
            if (!isIGBPISavingPermitted(_cType, out _tactics)) return null;
            return ValidateIGBPIValues(_tactics.Tactics);
        }

        public virtual List<IGBPIPanelValue> ValidateIGBPIValues(List<IGBPIPanelValue> _values)
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

        public virtual void Save_IGBPI_Values(ECharacterType _cType, List<IGBPIPanelValue> _values)
        {
            CharacterTactics _tactics;
            if (!isIGBPISavingPermitted(_cType, out _tactics)) return;
            //TODO: RTSPrototype Find Another Way to Save IGBPI Values
            //Serializable Objects cannot be used in builds
            var _Debug_CharacterTacticsObject = statHandler.DebugGET_CharacterTacticsData;
            int _index = _Debug_CharacterTacticsObject.CharacterTacticsList.IndexOf(_tactics);
            //Testing Save For Now, Change Location in Future
            SaveXMLTactics(_Debug_CharacterTacticsObject.CharacterTacticsList);
            _Debug_CharacterTacticsObject.CharacterTacticsList[_index].Tactics.Clear();
            _Debug_CharacterTacticsObject.CharacterTacticsList[_index].Tactics.AddRange(ValidateIGBPIValues(_values));         
#if UNITY_EDITOR
            UnityEditor.EditorUtility.SetDirty(_Debug_CharacterTacticsObject);
            UnityEditor.AssetDatabase.SaveAssets();
#endif
        }

        public virtual IEnumerator YieldSave_IGBPI_Values(ECharacterType _cType, List<IGBPIPanelValue> _values)
        {
            Save_IGBPI_Values(_cType, _values);
            yield return new WaitForSecondsRealtime(0.5f);
            Debug.Log("Finished Saving");
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

        protected virtual void SaveXMLTactics(List<CharacterTactics> _cTacticsList)
        {
            MyXmlManager.SaveXML<List<CharacterTactics>>(_cTacticsList, tacticsXMLPath);
        }

        protected virtual List<CharacterTactics> LoadXMLTactics()
        {
            return MyXmlManager.LoadXML<List<CharacterTactics>>(tacticsXMLPath);
        }
    }
}