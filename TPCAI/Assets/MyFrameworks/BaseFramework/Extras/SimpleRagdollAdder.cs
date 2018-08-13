using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BaseFramework
{
    /// <summary>
    /// Simply Attach To A Temporary Gameobject, Assign
    /// The Character, And Press The Button To Create a Ragdoll
    /// </summary>
    public class SimpleRagdollAdder : MonoBehaviour
    {
        [Header("Character Ragdoll Will Be Added To")]
        public GameObject myCharacter = null;
    }
}