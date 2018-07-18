using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RTSCoreFramework
{
    public class RTSStatHandler : MonoBehaviour
    {
        #region Dictionaries
        //Used to Retrieve Information from A Character and Commander Enum
        protected Dictionary<ECharacterType, CharacterStats> CharacterStatDictionary = new Dictionary<ECharacterType, CharacterStats>();
        protected Dictionary<ECharacterType, CharacterTactics> CharacterTacticsDictionary = new Dictionary<ECharacterType, CharacterTactics>();
        protected Dictionary<RTSGameMode.ECommanders, PartyStats> PartyStatDictionary = new Dictionary<RTSGameMode.ECommanders, PartyStats>();
        protected Dictionary<EWeaponType, WeaponStats> weaponStatDictionary = new Dictionary<EWeaponType, WeaponStats>();
        #endregion

        #region Fields
        [Header("Data Containing Character Stats")]
        [SerializeField]
        protected CharacterStatsData characterStatsData;
        [Header("Data Containing Character Tactics")]
        [SerializeField]
        protected CharacterTacticsData characterTacticsData;
        [Header("Data Containing Party Stats")]
        [SerializeField]
        protected PartyStatsData partyStatsData;
        [Header("Data Containing Weapon Stats")]
        [SerializeField]
        protected WeaponStatsData weaponStatsData;

        //Used For Initialization
        bool bInitializedDictionaries = false;
        #endregion

        #region Properties
        public static RTSStatHandler thisInstance { get; protected set; }

        RTSGameMode gamemode
        {
            get { return RTSGameMode.thisInstance; }
        }

        public Dictionary<EWeaponType, WeaponStats> WeaponStatDictionary
        {
            get
            {
                CheckForDictionaryInit();
                return weaponStatDictionary;
            }
        }

        /// <summary>
        /// Used Only For Debugging, Need to implement actual saving in the future
        /// </summary>
        public CharacterTacticsData DebugGET_CharacterTacticsData
        {
            get
            {
                CheckForDictionaryInit();
                return characterTacticsData;
            }
        }
        // I'll probably use public methods to access and update stats
        //public Dictionary<RTSCharacterType, CharacterStats> GetCharacterStats
        //{
        //    get { return CharacterStatDictionary; }
        //}
        //public Dictionary<RTSGameMode.ECommanders, PartyStats> GetPartyStatDictionary
        //{
        //    get { return PartyStatDictionary; }
        //}
        #endregion

        #region Getters
        public CharacterStats RetrieveCurrentPlayerStats()
        {
            CheckForDictionaryInit();
            var _cPlayer = gamemode.CurrentPlayer;
            return RetrieveCharacterStats(_cPlayer, _cPlayer.CharacterType);
        }

        public CharacterTactics RetrieveCurrentPlayerTactics()
        {
            CheckForDictionaryInit();
            var _cPlayer = gamemode.CurrentPlayer;
            return RetrieveCharacterTactics(_cPlayer, _cPlayer.CharacterType);
        }

        /// <summary>
        ///  Used to retrieve an Anonymous Character's Stats, that may update
        ///  from a specific character instance if the instance is the player's
        ///  general partymembers.
        /// </summary>
        /// <param name="_ally"></param>
        /// <param name="_cType"></param>
        /// <returns></returns>
        public CharacterStats RetrieveCharacterStats(AllyMember _ally, ECharacterType _cType)
        {
            CheckForDictionaryInit();
            return RetrieveAnonymousCharacterStats(_cType);
        }
        /// <summary>
        ///  Used to retrieve an Anonymous Character's Stats, that will not update
        ///  from a specific character instance.
        /// </summary>
        /// <param name="_cType"></param>
        /// <returns></returns>
        public CharacterStats RetrieveAnonymousCharacterStats(ECharacterType _cType)
        {
            CheckForDictionaryInit();
            if (CharacterStatDictionary.ContainsKey(_cType))
            {
                return CharacterStatDictionary[_cType];
            }
            Debug.Log("Character Type: " + _cType.ToString() + " could not be found");
            return new CharacterStats
            {
                CharacterType = ECharacterType.NoCharacterType,
                Health = 0
            };
        }

        public CharacterTactics RetrieveCharacterTactics(AllyMember _ally, ECharacterType _cType)
        {
            CheckForDictionaryInit();
            return RetrieveAnonymousCharacterTactics(_cType);
        }

        /// <summary>
        ///  Used to retrieve an Anonymous Character's Tactics, that will not update
        ///  from a specific character instance.
        /// </summary>
        /// <param name="_cType"></param>
        /// <returns></returns>
        public CharacterTactics RetrieveAnonymousCharacterTactics(ECharacterType _cType)
        {
            CheckForDictionaryInit();
            if (CharacterTacticsDictionary.ContainsKey(_cType))
            {
                return CharacterTacticsDictionary[_cType];
            }
            Debug.Log("Character Tactics For Type: " + _cType.ToString() + " could not be found");
            return new CharacterTactics
            {
                CharacterType = ECharacterType.NoCharacterType
            };
        }

        public PartyStats RetrievePartyStats(PartyManager _party, RTSGameMode.ECommanders _commander)
        {
            CheckForDictionaryInit();
            if (PartyStatDictionary.ContainsKey(_commander))
            {
                return PartyStatDictionary[_commander];
            }
            Debug.Log("Commander: " + _commander.ToString() + " could not be found");
            return new PartyStats
            {
                Commander = RTSGameMode.ECommanders.Commander_06,
                healthPotionAmount = 0
            };
        }
        #endregion

        #region UnityMessages
        private void OnEnable()
        {
            if (thisInstance != null)
                Debug.LogWarning("More than one instance of RTSStatHandler in scene.");
            else
                thisInstance = this;

        }

        // Use this for initialization
        void Start()
        {
            
        }

        // Update is called once per frame
        void Update()
        {

        }
        #endregion

        #region Initialization
        /// <summary>
        /// Used to check for initialization of dictionaries
        /// </summary>
        void CheckForDictionaryInit()
        {
            if (bInitializedDictionaries == false)
            {
                InitializeDictionaryValues();
                bInitializedDictionaries = true;
            }
        }

        void InitializeDictionaryValues()
        {
            //Transfer Values From Serialized List To A Dictionary
            //Character Data
            if (characterStatsData == null)
            {
                Debug.LogError("No CharacterStats Data on StatHandler");
                return;
            }
            foreach (var _stat in characterStatsData.CharacterStatList)
            {
                CharacterStatDictionary.Add(_stat.CharacterType, _stat);
            }
            //Tactics Data
            if(CharacterTacticsDictionary == null)
            {
                Debug.LogError("No Tactics Data on StatHandler");
                return;
            }
            foreach (var _stat in characterTacticsData.CharacterTacticsList)
            {
                CharacterTacticsDictionary.Add(_stat.CharacterType, _stat);
            }
            //Party Data
            if (partyStatsData == null)
            {
                Debug.LogError("No PartyStats Data on StatHandler");
                return;
            }
            foreach (var _stat in partyStatsData.PartyStatList)
            {
                PartyStatDictionary.Add(_stat.Commander, _stat);
            }
            //Weapon Data
            if (weaponStatsData == null)
            {
                Debug.LogError("No WeaponStats Data on StatHandler");
                return;
            }
            foreach (var _stat in weaponStatsData.WeaponStatList)
            {
                weaponStatDictionary.Add(_stat.WeaponType, _stat);
            }
        }
        #endregion
    }
}