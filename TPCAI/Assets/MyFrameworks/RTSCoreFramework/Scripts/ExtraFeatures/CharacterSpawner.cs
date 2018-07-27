using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RTSCoreFramework
{
    public class CharacterSpawner : MonoBehaviour
    {
        #region Fields
        [Header("Spawner Fields")]
        [Tooltip("Used to Identify a Character")]
        public ECharacterType CharacterType;
        [Header("Gizmos Fields")]
        [Tooltip("Used to Determine Gizmos Color")]
        public bool isFriendlyAlly = false;
        public Color FriendlyColor = Color.green;
        public Color EnemyColor = Color.red;
        public float GizmosRadius = 1.5f;
        #endregion

        #region Properties
        RTSStatHandler statHandler
        {
            get
            {
                //For Faster Access when using OnEnable method
                if (RTSStatHandler.thisInstance != null)
                    return RTSStatHandler.thisInstance;

                return GameObject.FindObjectOfType<RTSStatHandler>();
            }
        }

        Color gizmosColor
        {
            get
            {
                return isFriendlyAlly ? FriendlyColor : EnemyColor;
            }
        }
        #endregion

        #region UnityMessages
        private void OnEnable()
        {
            CharacterStats _stats;
            if (statHandler != null && (_stats =
                statHandler.RetrieveAnonymousCharacterStats(CharacterType))
                .CharacterType != ECharacterType.NoCharacterType &&
                _stats.CharacterPrefab != null)
            {
                SpawnCharacterPrefab(_stats.CharacterPrefab, _stats.CharacterType.ToString());
            }

            Destroy(gameObject, 0.5f);
        }

        private void OnDrawGizmos()
        {
            Gizmos.color = gizmosColor;
            Gizmos.DrawWireSphere(transform.position, GizmosRadius);
        }
        #endregion

        #region Helpers
        void SpawnCharacterPrefab(GameObject _character, string _name)
        {
            GameObject _spawnedC = Instantiate(_character, transform.position, transform.rotation) as GameObject;
            if (isFriendlyAlly)
            {
                _spawnedC.name = _name;
            }
        }
        #endregion
    }
}