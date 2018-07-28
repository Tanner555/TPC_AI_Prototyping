﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RTSCoreFramework;
using Opsive.ThirdPersonController.Abilities;

namespace RTSPrototype
{
    public class RTSAreaEffectAbility : Ability
    {
        RTSGameMode gamemode
        {
            get { return RTSGameMode.thisInstance; }
        }

        public override string GetDestinationState(int layer)
        {
            if (layer != m_AnimatorMonitor.BaseLayerIndex && layer != m_AnimatorMonitor.UpperLayerIndex &&
                !m_AnimatorMonitor.ItemUsesAbilityLayer(this, layer))
            {
                return string.Empty;
            }

            return "AreaEffect.Movement";
        }
    }
}