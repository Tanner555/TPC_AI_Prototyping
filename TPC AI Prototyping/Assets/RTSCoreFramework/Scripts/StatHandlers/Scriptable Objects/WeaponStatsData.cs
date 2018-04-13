using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RTSCoreFramework
{
    public enum EWeaponType
    {
        Fist, Pistol, AssaultRifle, Shotgun, SniperRifle,
        //Used For Null Error Checking
        NoWeaponType = -1
    }

    public enum EEquipType
    {
        Primary, Secondary
    }

    [System.Serializable]
    public struct WeaponStats
    {
        [Tooltip("Used to Identify a Weapon")]
        public EWeaponType WeaponType;

        public Sprite WeaponIcon;
        //Weapon Stats
        public int DamageRate;
        public int Accuracy;
    }

    [CreateAssetMenu(menuName = "RTSPrototype/WeaponStatsData")]
    public class WeaponStatsData : ScriptableObject
    {
        [Header("Weapon Stats")]
        [SerializeField]
        public List<WeaponStats> WeaponStatList;
    }
}
