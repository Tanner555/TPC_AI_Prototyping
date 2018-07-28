using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RTSCoreFramework
{
    public class SelfHealBehaviour : AbilityBehaviour
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

        void Start()
        {
            
        }

		public override void Use(GameObject target)
		{
            float _extraHealth = (config as SelfHealConfig).GetExtraHealth();
            PlayAbilitySound();
            allymember.AllyHeal((int)_extraHealth);
            PlayParticleEffect();
            PlayAbilityAnimation();
		}
    }
}