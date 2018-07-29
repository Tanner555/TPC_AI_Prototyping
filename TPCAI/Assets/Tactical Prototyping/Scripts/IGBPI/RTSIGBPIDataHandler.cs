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

                    });
                }
                return _appendedActionDictionary;
            }
        }
        #endregion
    }
}