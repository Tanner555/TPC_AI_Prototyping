using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RTSCoreFramework
{
    [CreateAssetMenu(menuName = ("RTSPrototype/SpecialAbilties/Self Heal"))]
    public class SelfHealConfig : AbilityConfig
	{
		[Header("Self Heal Specific")]
		[SerializeField] float extraHealth = 50f;

        public override AbilityBehaviour AddBehaviourComponent(GameObject objectToAttachTo)
        {
            return objectToAttachTo.AddComponent<SelfHealBehaviour>();
        }

		public float GetExtraHealth()
		{
			return extraHealth;
		}
	}
}