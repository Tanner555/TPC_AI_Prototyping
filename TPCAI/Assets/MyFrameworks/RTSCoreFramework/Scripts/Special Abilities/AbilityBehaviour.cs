using System.Collections;
using UnityEngine;

namespace RTSCoreFramework
{
    public abstract class AbilityBehaviour : MonoBehaviour
    {
        #region Properties
        protected virtual AudioSource audioSource
        {
            get
            {
                if (_audioSource == null)
                {
                    _audioSource = GetComponent<AudioSource>();
                    if (_audioSource == null)
                        _audioSource = gameObject.AddComponent<AudioSource>();

                }
                return _audioSource;
            }
        }
        private AudioSource _audioSource = null;
        #endregion

        #region Fields
        protected AbilityConfig config;

        protected const string ATTACK_TRIGGER = "Attack";
        protected const string DEFAULT_ATTACK_STATE = "DEFAULT ATTACK";
        protected const float PARTICLE_CLEAN_UP_DELAY = 20f;
        #endregion

        public abstract void Use(GameObject target = null);

        public virtual void SetConfig(AbilityConfig configToSet)
        {
            config = configToSet;
        }

        protected virtual void PlayParticleEffect()
        {
            var _particlePrefab = config.GetParticlePrefab();
            if (_particlePrefab == null) return;
            var _particleObject = Instantiate(
                _particlePrefab,
                transform.position,
                _particlePrefab.transform.rotation
            );
            _particleObject.transform.parent = transform; // set world space in prefab if required
            _particleObject.GetComponent<ParticleSystem>().Play();
            StartCoroutine(DestroyParticleWhenFinished(_particleObject));
        }

        protected virtual IEnumerator DestroyParticleWhenFinished(GameObject particlePrefab)
        {
            while (particlePrefab.GetComponent<ParticleSystem>().isPlaying)
            {
                yield return new WaitForSeconds(PARTICLE_CLEAN_UP_DELAY);
            }
            Destroy(particlePrefab);
            yield return new WaitForEndOfFrame();
        }

        protected virtual void PlayAbilityAnimation()
        {
            //Override To Add Ability Animations Functionality
            //var animatorOverrideController = GetComponent<RPGCharacter>().GetOverrideController();
            //var animator = GetComponent<Animator>();
            //animator.runtimeAnimatorController = animatorOverrideController;
            //animatorOverrideController[DEFAULT_ATTACK_STATE] = config.GetAbilityAnimation();
            //animator.SetTrigger(ATTACK_TRIGGER);
        }

        protected virtual void PlayAbilitySound()
        {
            var abilitySound = config.GetRandomAbilitySound();
            if (abilitySound == null) return;
            audioSource.PlayOneShot(abilitySound);
        }
    }
}