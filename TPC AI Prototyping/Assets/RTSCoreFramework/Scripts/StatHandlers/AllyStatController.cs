using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RTSCoreFramework
{
    public class AllyStatController : MonoBehaviour
    {
        #region Fields
        [Header("Will be used to identify the character")]
        public RTSCharacterType characterType;

        private CharacterStats myCharacterStats;
        #endregion

        #region Properties
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
            get { return RTSStatHandler.thisInstance; }
        }
        #endregion

        #region UnityMessages
        // Use this for initialization
        void Start()
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