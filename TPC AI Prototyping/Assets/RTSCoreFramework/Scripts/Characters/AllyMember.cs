﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RTSCoreFramework
{
    public class AllyMember : MonoBehaviour
    {
        #region Fields
        //Inspector Set Variables
        [Header("Faction and General Settings")]
        public RTSGameMode.EFactions AllyFaction;
        public RTSGameMode.ECommanders GeneralCommander;
        [Header("Debug Menu")]
        public bool Debug_InfiniteHealth = false;
        public bool Debug_DoNotShoot = false;

        [Header("Camera Follow Transforms")]
        [SerializeField]
        protected Transform chestTransform;
        [SerializeField]
        protected Transform headTransform;

        //Gun Properties, Can Delete in the Future
        protected float lowAmmoThreshold = 14.0f;
        protected float firerate = 0.3f;

        //Other private variables
        protected bool hasStarted = false;
        protected bool ExecutingShootingBehavior;
        protected bool wantsFreedomToMove;
        protected float freeMoveThreshold;
        protected float DefaultShootDelay;
        #endregion

        #region Properties
        public RTSGameMode gamemode { get { return RTSGameMode.thisInstance; } }
        public AllyMember DamageInstigator { get; protected set; }
        //Faction Properties
        public PartyManager partyManager { get; protected set; }
        public int FactionPlayerCount { get { return gamemode.GetAllyFactionPlayerCount((AllyMember)this); } }
        public int GeneralPlayerCount { get { return gamemode.GetAllyGeneralPlayerCount((AllyMember)this); } }
        //Camera Follow Transforms
        public Transform ChestTransform { get { return chestTransform; } }
        public Transform HeadTransform { get { return headTransform; } }

        public virtual AllyMember enemyTarget
        {
            get { return aiController.currentTargettedEnemy; }
        }

        public int PartyKills
        {
            get { return partyManager.PartyKills; }
            set { partyManager.PartyKills = value; }
        }
        public int PartyPoints
        {
            get { return partyManager.PartyPoints; }
            set { partyManager.PartyPoints = value; }
        }
        public int PartyDeaths
        {
            get { return partyManager.PartyDeaths; }
            set { partyManager.PartyDeaths = value; }
        }

        //Health Properties
        public virtual float AllyHealth
        {
            get { return 0f; }
        }

        public virtual float AllyMaxHealth
        {
            get { return 100f; }
        }

        public virtual float AllyShield
        {
            get { return 0f; }
        }

        public virtual float AllyMaxShield
        {
            get { return 0f; }
        }

        public virtual bool IsAlive
        {
            get { return AllyHealth > 0f; }
        }

        //Ammo Properties
        public virtual int CurrentEquipedAmmo
        {
            get { return 0; }
        }

        #endregion

        #region PlayerComponents
        public AllyEventHandler allyEventHandler { get; protected set; }
        public AllyAIController aiController { get; protected set; }
        #endregion

        #region BooleanProperties
        protected virtual bool AllComponentsAreValid
        {
            get { return allyEventHandler && aiController; }
        }

        //public bool IsAlive
        //{
        //    get { return npcHealth.npcHealth > 0; }
        //}

        public bool isCurrentPlayer { get { return partyManager ? partyManager.AllyIsCurrentPlayer(this) : false; } }
        public bool pManIsGeneralCommander { get { return partyManager.isCurrentPlayerCommander; } }
        //public bool IsCarryingWeapon { get { return AllComponentsAreValid && pWeaponHandler.CurrentWeapon; } }
        #endregion

        #region UnityMessages
        protected virtual void OnEnable()
        {
            SetInitialReferences();
            if (hasStarted == true)
            {
                SubToEvents();
            }

        }

        protected virtual void OnDisable()
        {
            UnSubFromEvents();
        }

        // Use this for initialization
        protected virtual void Start()
        {
            if (gamemode == null)
                Debug.LogError("No gamemode on ai player!");

            if (hasStarted == false)
            {
                SubToEvents();
                hasStarted = true;
            }
        }
        #endregion

        #region Handlers
        void SetDamageInstigator(AllyMember _instigator)
        {
            if (_instigator != null && _instigator != DamageInstigator)
            {
                DamageInstigator = _instigator;
            }
        }

        public void AllyOnDeath()
        {
            //if gamemode, find allies and exclude this ally
            if (gamemode != null && partyManager != null)
            {
                AllyMember _firstAlly = partyManager.FindPartyMembers(true, this);
                if (_firstAlly != null)
                {
                    partyManager.SetAllyInCommand(_firstAlly);
                }
                else
                {
                    partyManager.CallEventNoPartyMembers(partyManager, this, true);
                }
                //Add to death count
                PartyDeaths += 1;

                gamemode.ProcessAllyDeath(this);
                Invoke("DestroyAlly", 0.1f);
            }
            else
            {
                Debug.LogError(@"Could not kill allymember because 
                there is no partymember or gamemode");

            }
        }

        private void DestroyAlly() { Destroy(this); }
        #endregion

        #region Getters
        public bool IsEnemyFor(AllyMember player)
        {
            return player.AllyFaction != AllyFaction;
        }

        #endregion

        #region Initialization
        protected virtual void SetInitialReferences()
        {
            allyEventHandler = GetComponent<AllyEventHandler>();
            aiController = GetComponent<AllyAIController>();
            TryFindingPartyManager();

            if (partyManager == null)
                Debug.LogError("No partymanager on allymember!");
            if (allyEventHandler == null)
                Debug.LogError("No npcmaster on allymember!");
            if (aiController == null)
                Debug.LogError("No aiController on allymember!");

            if (AllyFaction == RTSGameMode.EFactions.Faction_Default)
            {
                AllyFaction = RTSGameMode.EFactions.Faction_Allies;
            }

        }

        protected void SubToEvents()
        {
            allyEventHandler.EventNpcDie += AllyOnDeath;
        }

        protected void UnSubFromEvents()
        {
            allyEventHandler.EventNpcDie -= AllyOnDeath;
        }

        public bool TryFindingPartyManager()
        {
            foreach (var pManager in GameObject.FindObjectsOfType<PartyManager>())
            {
                if (pManager.GeneralCommander == GeneralCommander)
                    partyManager = pManager;
            }
            return partyManager != null;
        }
        #endregion

        #region Commented Code
        //Health
        //public float AllyHealth { get { return npcHealth.npcHealth; } }
        //public float AllyMaxHealth { get { return npcHealth.npcMaxHealth; } }
        //public float healthAsPercentage { get { return AllyHealth / AllyMaxHealth; } }
        //shortcut properties for partymanager gamemode properties
        #endregion
    }
}