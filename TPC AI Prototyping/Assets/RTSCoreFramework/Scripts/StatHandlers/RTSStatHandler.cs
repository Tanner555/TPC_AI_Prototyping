using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RTSCoreFramework
{
    #region Enums
    public enum RTSCharacterType
    {
        BlueSillyPants, BrownSillyPants, EvilAssaultVillian1
    }
    #endregion

    #region Structs
    public struct CharacterStats
    {
        public int Health;
    }

    public struct PartyStats
    {

    }
    #endregion

    public class RTSStatHandler : MonoBehaviour
    {
        #region Dictionaries
        protected Dictionary<RTSCharacterType, CharacterStats> CharacterStatDictionary = new Dictionary<RTSCharacterType, CharacterStats>();
        protected Dictionary<RTSGameMode.ECommanders, PartyStats> PartyStatDictionary = new Dictionary<RTSGameMode.ECommanders, PartyStats>();
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
        public void RequestCharacterStatEntry(AllyMember _ally, RTSCharacterType _cType)
        {
            bool _isFriend = _ally.bIsInGeneralCommanderParty;
            if (_isFriend) RequestAllyStatEntry(_ally, _cType);
            else RequestEnemyStatEntry(_ally, _cType);
        }

        private void RequestAllyStatEntry(AllyMember _ally, RTSCharacterType _cType)
        {
            // Check if Ally is already inside the dictionary
            if (CharacterStatDictionary.ContainsKey(_cType)) return;
            CharacterStatDictionary.Add(_cType, new CharacterStats
            {
                Health = 100
            });
        }

        private void RequestEnemyStatEntry(AllyMember _ally, RTSCharacterType _cType)
        {

        }
        #endregion

    }
}