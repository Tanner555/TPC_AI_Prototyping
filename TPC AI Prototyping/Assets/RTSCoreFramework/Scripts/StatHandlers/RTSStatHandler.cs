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
        protected Dictionary<RTSGameMode.ECommanders, PartyStats> PartyStatDictionary = new Dictionary<RTSGameMode.ECommanders, PartyStats>();
        protected Dictionary<EWeaponType, WeaponStats> weaponStatDictionary = new Dictionary<EWeaponType, WeaponStats>();
        #endregion

        #region Fields
        [Header("Data Containing Character Stats")]
        [SerializeField]
        protected CharacterStatsData characterStatsData;
        [Header("Data Containing Party Stats")]
        [SerializeField]
        protected PartyStatsData partyStatsData;
        [Header("Data Containing Weapon Stats")]
        [SerializeField]
        protected WeaponStatsData weaponStatsData;
        #endregion

        #region Properties
        public static RTSStatHandler thisInstance { get; protected set; }
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
        public Dictionary<EWeaponType, WeaponStats> WeaponStatDictionary
        {
            get { return weaponStatDictionary; }
        }
        #endregion

        #region UnityMessages
        private void OnEnable()
        {
            if (thisInstance != null)
                Debug.LogWarning("More than one instance of RTSStatHandler in scene.");
            else
                thisInstance = this;

            InitializeDictionaryValues();
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

        #region CharacterStatRequests
        public CharacterStats RetrieveCharacterStats(AllyMember _ally, ECharacterType _cType)
        {
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

        public PartyStats RetrievePartyStats(PartyManager _party, RTSGameMode.ECommanders _commander)
        {
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

    }
}