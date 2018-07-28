using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RTSCoreFramework;
using Opsive.ThirdPersonController.Abilities;

namespace RTSPrototype
{
    public class AllySpecialAbilitiesWrapper : AllySpecialAbilities
    {
        protected override void OnKeyPress(int _key)
        {
            base.OnKeyPress(_key);
            if (allymember.bIsCurrentPlayer == false) return;

            if(_key == 1)
            {
                var _area = GetComponent<RTSAreaEffectAbility>();
                if(_area != null)
                {
                    Debug.Log("Casting Area of Effect");
                    _area.StartAbility();
                    Invoke("StopAreaEffectAbility", 1f);
                }
            }
            if(_key == 2)
            {
                var _heal = GetComponent<RTSSelfHealAbility>();
                if(_heal != null)
                {
                    Debug.Log("Casting Heal");
                    _heal.StartAbility();
                    Invoke("StopHealAbility", 1f);
                }
            }
        }

        void StopAreaEffectAbility()
        {
            var _areaeffect = GetComponent<RTSAreaEffectAbility>();
            if (_areaeffect != null)
            {
                Debug.Log("Stop Area Effect");
                _areaeffect.StopAbility();
            }
        }

        void StopHealAbility()
        {
            var _heal = GetComponent<RTSSelfHealAbility>();
            if (_heal != null)
            {
                Debug.Log("Stop Healing");
                _heal.StopAbility();
            }
        }

    }
}