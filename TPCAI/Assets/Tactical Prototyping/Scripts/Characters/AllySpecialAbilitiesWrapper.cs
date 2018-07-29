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
            //if (allymember.bIsCurrentPlayer == false) return;

            //if(_key == 1)
            //{
            //    var _area = GetComponent<RTSAreaEffectAbility>();
            //    if(_area != null && _area.CanStartAbility())
            //    {
            //        _area.StartAbility();
            //    }
            //}
            //if(_key == 2)
            //{
            //    var _heal = GetComponent<RTSSelfHealAbility>();
            //    if(_heal != null && _heal.CanStartAbility())
            //    {
            //        _heal.StartAbility();
            //    }
            //}
        }
    }
}