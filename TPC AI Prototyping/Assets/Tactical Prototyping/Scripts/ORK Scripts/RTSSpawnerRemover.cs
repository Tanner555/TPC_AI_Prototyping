using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ORKFramework;
using ORKFramework.Behaviours;

namespace RTSPrototype {
    public class RTSSpawnerRemover : MonoBehaviour {

        // Use this for initialization
        void Start() {
            Invoke("DeleteSpawner", 1f);
        }

        void DeleteSpawner()
        {
            if (GetComponent<CombatantSpawner>())
            {
                var _cSpawner = GetComponent<CombatantSpawner>();
                Destroy(_cSpawner, 0.1f);
            }
        }
    }
}