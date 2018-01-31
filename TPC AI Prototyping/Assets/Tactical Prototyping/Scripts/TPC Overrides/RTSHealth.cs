using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Opsive.ThirdPersonController;
using RTSCoreFramework;

namespace RTSPrototype
{
    public class RTSHealth : CharacterHealth
    {
        #region Properties
        AllyMemberWrapper allyMember
        {
            get
            {
                if (_allyMember == null)
                    _allyMember = GetComponent<AllyMemberWrapper>();

                return _allyMember;
            }
        }
        AllyMemberWrapper _allyMember = null;
        protected override float m_CurrentHealth
        {
            get
            {
                return allyMember.AllyHealth;
            }

            set
            {
                allyMember.AllyHealth = value;
            }
        }

        protected override float m_MaxHealth
        {
            get
            {
                return allyMember.AllyMaxHealth;
            }

            set
            {
                allyMember.AllyMaxHealth = value;
            }
        }
        #endregion

        protected override void Awake()
        {
            m_GameObject = gameObject;
            m_Transform = transform;
            m_Rigidbody = GetComponent<Rigidbody>();

            SharedManager.Register(this);

            //m_CurrentHealth = m_MaxHealth;
            //m_CurrentShield = m_MaxShield;
            if (m_DamageMultipliers != null && m_DamageMultipliers.Length > 0)
            {
                m_DamageMultiplierMap = new Dictionary<GameObject, DamageMultiplier>();
                for (int i = 0; i < m_DamageMultipliers.Length; ++i)
                {
                    m_DamageMultiplierMap.Add(m_DamageMultipliers[i].GameObject, m_DamageMultipliers[i]);
                }
            }

            // Register for OnRespawn so the health and sheild can be reset.
            //EventHandler.RegisterEvent(m_GameObject, "OnRespawn", OnRespawn);

            m_AudioSource = GetComponent<AudioSource>();
        }
    }
}