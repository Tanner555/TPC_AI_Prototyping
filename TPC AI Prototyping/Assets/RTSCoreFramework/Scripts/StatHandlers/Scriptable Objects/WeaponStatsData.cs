using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RTSCoreFramework
{
    public enum EWeaponType
    {
        Fist, Pistol, AssaultRifle, Shotgun, SniperRifle
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
