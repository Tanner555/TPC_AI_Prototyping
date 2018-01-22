using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RTSCoreFramework;

namespace RTSPrototype
{
    public class AllyMemberWrapper : AllyMember
    {
        #region Fields
        [Header("Camera Follow Transforms")]
        [SerializeField]
        private Transform chestTransform;
        [SerializeField]
        private Transform headTransform;
        #endregion

        #region Properties
        public Transform ChestTransform { get { return chestTransform; } }
        public Transform HeadTransform { get { return headTransform; } }
        public AllyAIControllerWrapper aiControllerWrapper
        {
            get
            {
                if (_aiControllerWrapper == null)
                    _aiControllerWrapper = GetComponent<AllyAIControllerWrapper>();

                return _aiControllerWrapper;
            }
        }
        private AllyAIControllerWrapper _aiControllerWrapper = null;
        public AllyMemberWrapper enemyTargetWrapper
        {
            get { return aiControllerWrapper.currentTargettedEnemyWrapper; }
        }
        #endregion

        #region UnityMessages
        protected override void OnEnable()
        {
            base.OnEnable();
        }

        protected override void OnDisable()
        {
            base.OnDisable();
        }

        protected override void Start()
        {
            base.Start();
        }
        #endregion

    }
}