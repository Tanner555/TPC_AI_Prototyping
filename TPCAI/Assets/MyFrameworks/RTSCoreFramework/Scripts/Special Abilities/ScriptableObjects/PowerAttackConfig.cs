﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RTSCoreFramework
{
    [CreateAssetMenu(menuName = ("RTSPrototype/SpecialAbilties/Power Attack"))]
    public class PowerAttackConfig : AbilityConfig
    {
        [Header("Power Attack Specific")]
        [SerializeField] float extraDamage = 10f;

        public override AbilityBehaviour AddBehaviourComponent(GameObject objectToAttachTo)
        {
            return objectToAttachTo.AddComponent<PowerAttackBehaviour>();
        }
        public float GetExtraDamage()
        {
            return extraDamage;
        }
    }
}