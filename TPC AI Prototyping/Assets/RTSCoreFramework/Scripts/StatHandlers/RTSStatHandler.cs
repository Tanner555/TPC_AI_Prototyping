using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RTSCoreFramework
{
    #region Enums
    public enum RTSCharacterType
    {
        BlueSillyPants, BrownSillyPants, EvilAssaultVillian1,

        //Only Used When Character Type Could Not Be Found
        NoCharacterType
    }
    #endregion

    #region Structs
    [System.Serializable]
    public struct CharacterStats
    {
        [Tooltip("Used to Identify a Character")]
        public RTSCharacterType CharacterType;

        //Health Stats
        public int MaxHealth;
        public int Health;
    }
    [System.Serializable]
    public struct PartyStats
    {
        [Tooltip("Used to Identify the Commander")]
        public RTSGameMode.ECommanders Commander;

        public int healthPotionAmount;
    }
    #endregion

    #region ScriptableObjects
    [CreateAssetMenu(menuName = "RTSPrototype/RTSStatsData")]
    public class RTSStatsData : ScriptableObject
    {
        [Header("Character and Party Stats")]
        [SerializeField]
        public List<CharacterStats> CharacterStatList;
        [SerializeField]
        public List<PartyStats> PartyStatList;
    }
    #endregion

    public class RTSStatHandler : MonoBehaviour
    {
        #region Dictionaries
        //Used to Retrieve Information from A Character and Commander Enum
        protected Dictionary<RTSCharacterType, CharacterStats> CharacterStatDictionary = new Dictionary<RTSCharacterType, CharacterStats>();
        protected Dictionary<RTSGameMode.ECommanders, PartyStats> PartyStatDictionary = new Dictionary<RTSGameMode.ECommanders, PartyStats>();
        #endregion

        #region Fields
        [Header("Data Containing Character and Party Stats")]
        [SerializeField]
        protected RTSStatsData rtsStatsData;
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

        #region UnityMessages
        private void OnEnable()
        {
            if (thisInstance != null)
                Debug.LogWarning("More than one instance of RTSStatHandler in scene.");
            else
                thisInstance = this;

            //Transfer Values From Serialized List To A Dictionary
            if (rtsStatsData == null) return;
            foreach (var _stat in rtsStatsData.CharacterStatList)
            {
                CharacterStatDictionary.Add(_stat.CharacterType, _stat);
            }
            foreach (var _stat in rtsStatsData.PartyStatList)
            {
                PartyStatDictionary.Add(_stat.Commander, _stat);
            }
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

        #region CharacterStatRequests
        public CharacterStats RetrieveCharacterStats(AllyMember _ally, RTSCharacterType _cType)
        {
            if (CharacterStatDictionary.ContainsKey(_cType))
            {
                return CharacterStatDictionary[_cType];
            }
            Debug.Log("Character Type: " + _cType.ToString() + " could not be found");
            return new CharacterStats
            {
                CharacterType = RTSCharacterType.NoCharacterType,
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