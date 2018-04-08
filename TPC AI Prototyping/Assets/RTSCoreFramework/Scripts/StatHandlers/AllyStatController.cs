using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RTSCoreFramework
{
    public class AllyStatController : MonoBehaviour
    {
        #region Fields
        [Header("Will be used to identify the character")]
        public RTSCharacterType CharacterType;
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
            InitializeCharacter();
        }

        // Update is called once per frame
        void Update()
        {

        }

        void InitializeCharacter()
        {
            //TODO: RTSPrototype StatController Consider Changing Initialize Method For Enemies
            statHandler.RequestCharacterStatEntry(allyMember, CharacterType);
        }
        #endregion

    }
}