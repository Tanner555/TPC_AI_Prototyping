using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RTSCoreFramework;
using BaseFramework;

namespace RTSPrototype
{
    public class RTSIGBPIDataHandler : IGBPI_DataHandler
    {
        #region ConditionDictionary
        Dictionary<string, IGBPI_Condition> _appendedConditionDictionary;

        public override Dictionary<string, IGBPI_Condition> IGBPI_Conditions
        {
            get
            {
                if (_appendedConditionDictionary == null || _appendedConditionDictionary.Count <= 0)
                {
                    _appendedConditionDictionary = base.IGBPI_Conditions.AddRange(new Dictionary<string, IGBPI_Condition>
                    {

                    });
                }
                return _appendedConditionDictionary;
            }
        }
        #endregion

        #region ActionDictionary
        Dictionary<string, IGBPI_Action> _appendedActionDictionary;

        public override Dictionary<string, IGBPI_Action> IGBPI_Actions
        {
            get
            {
                if (_appendedActionDictionary == null || _appendedActionDictionary.Count <= 0)
                {
                    _appendedActionDictionary = base.IGBPI_Actions.AddRange(new Dictionary<string, IGBPI_Action>
                    {
                        { "Self: Area of Effect", new IGBPI_Action((_ally) =>
                        { _ally.allyEventHandler.CallOnTrySpecialAbility(typeof(AreaOfEffectConfigTPC)); },
                        (_ally) => _ally.CanUseAbility(typeof(AreaOfEffectConfigTPC)),
                        ActionFilters.Abilities)},
                        { "Self: Heal", new IGBPI_Action((_ally) => 
                        { _ally.allyEventHandler.CallOnTrySpecialAbility(typeof(SelfHealConfigTPC)); },
                        (_ally) => _ally.CanUseAbility(typeof(SelfHealConfigTPC)),
                        ActionFilters.Abilities)}
                    });
                }
                return _appendedActionDictionary;
            }
        }
        #endregion
    }
}