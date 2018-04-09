using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RTSCoreFramework
{
    public class AllyStatController : MonoBehaviour
    {
        #region Fields
        [Header("Will be used to identify the character")]
        public ECharacterType characterType;

        private CharacterStats myCharacterStats;
        #endregion

        #region SetupProperties
        AllyMember allyMember
        {
            get
            {
                if (__allyMember == null)
                    __allyMember = GetComponent<AllyMember>();

                return __allyMember;
            }
        }
        AllyMember __allyMember = null;

        RTSStatHandler statHandler
        {
            get
            {
                //For Faster Access when using OnEnable method
                if(RTSStatHandler.thisInstance != null) 
                    return RTSStatHandler.thisInstance;

                return GameObject.FindObjectOfType<RTSStatHandler>();
            }
        }
        #endregion

        #region AccessProperties
        public int Stat_Health
        {
            get { return myCharacterStats.Health; }
            set { myCharacterStats.Health = value; }
        }
        public int Stat_MaxHealth
        {
            get { return myCharacterStats.MaxHealth; }
        }
        #endregion

        #region UnityMessages
        // Use this for initialization
        void OnEnable()
        {
            InitializeCharacterStats();
        }

        // Update is called once per frame
        void Update()
        {

        }
        #endregion

        void InitializeCharacterStats()
        {
            myCharacterStats = statHandler.RetrieveCharacterStats(allyMember, characterType);
        }
    }
}