using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Opsive.ThirdPersonController.Abilities;
using RTSCoreFramework;

namespace RTSPrototype
{
    public abstract class AbilityBehaviourTPC : AbilityBehaviour
    {
        public override abstract void Use(GameObject target = null);

        protected override void PlayAbilityAnimation()
        {
            var _tpcAbility = GetTPCAbility();
            if(_tpcAbility != null && _tpcAbility.CanStartAbility())
            {
                _tpcAbility.StartAbility();
            }
        }

        public virtual Opsive.ThirdPersonController.Abilities.Ability GetTPCAbility()
        {
            //Override To Get the Actual TPC Ability
            return null;
        }
    }
}