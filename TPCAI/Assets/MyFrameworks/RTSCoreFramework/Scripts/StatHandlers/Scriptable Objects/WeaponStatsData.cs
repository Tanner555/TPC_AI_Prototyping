using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RTSCoreFramework
{
    public enum EWeaponType
    {
        Fist = 0, Knife = 1, Pistol = 2,
        AssaultRifle = 3, Shotgun = 4, SniperRifle = 5,
        //Used For Null Error Checking
        NoWeaponType = -1
    }

    public enum EWeaponUsage
    {
        Melee = 0, Shootable = 1
    }

    public enum EEquipType
    {
        Primary = 0, Secondary = 1
    }

    [System.Serializable]
    public struct WeaponStats
    {
        public string name;

        [Header("Weapon Type and Usage")]
        [Tooltip("Used to Identify a Weapon")]
        public EWeaponType WeaponType;
        public EWeaponUsage WeaponUsage;
        [Header("HUD")]
        public Sprite WeaponIcon;
        [Header("Weapon Stats")]
        public int DamageRate;
        public int Accuracy;
        public int AttackRate;
        [Header("Melee Weapon Stats")]
        //Used For Determine Distance Required to use Melee Weapon
        public int MeleeAttackDistance;
    }

    [CreateAssetMenu(menuName = "RTSPrototype/WeaponStatsData")]
    public class WeaponStatsData : ScriptableObject
    {
        [Header("Weapon Stats")]
        [SerializeField]
        public List<WeaponStats> WeaponStatList;
    }
}
