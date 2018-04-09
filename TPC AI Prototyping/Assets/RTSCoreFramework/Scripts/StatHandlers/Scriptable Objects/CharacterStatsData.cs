using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RTSCoreFramework
{
    public enum ECharacterType
    {
        BlueSillyPants, BrownSillyPants, EvilAssaultVillian1,

        //Only Used When Character Type Could Not Be Found
        NoCharacterType
    }

    [System.Serializable]
    public struct CharacterStats
    {
        [Tooltip("Used to Identify a Character")]
        public ECharacterType CharacterType;

        [Header("Health Stats")]
        public int MaxHealth;
        public int Health;

        [Header("Weapon Equipped")]
        public EEquipType EquippedWeapon;

        [Header("Weapon Stats")]
        public EWeaponType PrimaryWeapon;
        public EWeaponType SecondaryWeapon;
    }

    [CreateAssetMenu(menuName = "RTSPrototype/CharacterStatsData")]
    public class CharacterStatsData : ScriptableObject
    {
        [Header("Character Stats")]
        [SerializeField]
        public List<CharacterStats> CharacterStatList;
    }
}
