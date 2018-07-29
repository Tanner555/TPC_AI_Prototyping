﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RTSCoreFramework;

namespace RTSPrototype
{
    public class AreaOfEffectBehaviourTPC : AbilityBehaviourTPC
    {
        #region Properties
        RTSGameMaster gamemaster
        {
            get { return RTSGameMaster.thisInstance; }
        }

        AllyEventHandler eventhandler
        {
            get
            {
                if (_eventhandler == null)
                    _eventhandler = GetComponent<AllyEventHandler>();

                return _eventhandler;
            }
        }
        AllyEventHandler _eventhandler = null;
        AllyMember allymember
        {
            get
            {
                if (_allymember == null)
                    _allymember = GetComponent<AllyMember>();

                return _allymember;
            }
        }
        AllyMember _allymember = null;
        #endregion

        public override void Use(GameObject target = null)
        {
            PlayAbilitySound();
            DealRadialDamage();
            PlayParticleEffect();
            PlayAbilityAnimation();
        }

        private void DealRadialDamage()
        {
            // Static sphere cast for targets
            RaycastHit[] hits = Physics.SphereCastAll(
                transform.position,
                (config as AreaOfEffectConfigTPC).GetRadius(),
                Vector3.up,
                (config as AreaOfEffectConfigTPC).GetRadius()
            );

            foreach (RaycastHit hit in hits)
            {
                var damageable = hit.collider.gameObject.GetComponent<AllyMember>();
                //Cannot Hurt Self Or Allies in Same Party (Friends)
                if (damageable != null &&
                    damageable != allymember &&
                    damageable.bIsCurrentPlayer == false &&
                    damageable.IsEnemyFor(allymember))
                {
                    float damageToDeal = (config as AreaOfEffectConfigTPC).GetDamageToEachTarget();
                    damageable.AllyTakeDamage((int)damageToDeal, allymember);
                }
            }
        }

        public override Opsive.ThirdPersonController.Abilities.Ability GetTPCAbility()
        {
            return GetComponent<RTSAreaEffectAbility>();
        }
    }
}
