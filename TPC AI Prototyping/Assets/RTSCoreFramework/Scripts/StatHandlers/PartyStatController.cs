using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RTSCoreFramework
{
    public class PartyStatController : MonoBehaviour
    {
        #region Properties
        PartyManager partyManager
        {
            get
            {
                if (__partyManager == null)
                    __partyManager = GetComponent<PartyManager>();

                return __partyManager;
            }
        }
        PartyManager __partyManager = null;
        #endregion

        #region UnityMessages
        // Use this for initialization
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {

        }
        #endregion

    }
}