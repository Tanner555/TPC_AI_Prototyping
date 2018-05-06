using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

namespace RTSCoreFramework
{
    public class AllyVisuals : MonoBehaviour
    {
        #region PropsAndFields
        [Header("Ally Highlighting")]
        public Color selAllyColor;
        public Color selEnemyColor;
        public Light SelectionLight;
        [Header("Ally Waypoint Navigation")]
        public Material waypointRendererMaterial;
        private float waypointStartWidth = 0.05f;
        private float waypointEndWidth = 0.05f;
        private Color waypointStartColor = Color.yellow;
        private Color waypointEndColor = Color.yellow;
        private LineRenderer waypointRenderer;
        [Header("Ally Damage Effects")]
        public GameObject BloodParticles;

        RTSGameMaster gamemaster
        {
            get { return RTSGameMaster.thisInstance; }
        }

        AllyMember thisAlly
        {
            get { return GetComponent<AllyMember>(); }
        }

        AllyEventHandler myEventHandler
        {
            get { return GetComponent<AllyEventHandler>(); }
        }

        RTSUiMaster uiMaster { get { return RTSUiMaster.thisInstance; } }

        bool friend
        {
            get { return thisAlly.bIsInGeneralCommanderParty; }
        }

        //NavMesh used for Waypoint Rendering
        NavMeshAgent myNavMesh
        {
            get
            {
                if (_myNavMesh == null)
                    _myNavMesh = GetComponent<NavMeshAgent>();

                return _myNavMesh;
            }
        }
        NavMeshAgent _myNavMesh = null;

        bool cameraIsMoving = false;

        bool bHasSwitched = false;

        float waypointUpdateRate = 0.5f;
        #endregion

        #region UnityMessages
        private void OnDisable()
        {
            myEventHandler.OnHoverOver -= OnCursEnter;
            myEventHandler.OnHoverLeave -= OnCursExit;
            myEventHandler.EventAllyDied -= HandleDeath;
            myEventHandler.EventCommandMove -= SetupWaypointRenderer;
            myEventHandler.EventTogglebIsFreeMoving -= CheckToDisableWaypointRenderer;
            myEventHandler.EventFinishedMoving -= DisableWaypointRenderer;
            myEventHandler.EventPartySwitching -= OnPartySwitch;
            myEventHandler.EventCommandAttackEnemy -= OnCmdAttackEnemy;
            myEventHandler.EventCommandAttackEnemy -= DisableWaypointRenderer;
            myEventHandler.OnAllyTakeDamage -= SpawnBloodParticles;
            gamemaster.GameOverEvent -= HandleGameOver;
            gamemaster.EventHoldingRightMouseDown -= HandleCameraMovement;
            uiMaster.EventAnyUIToggle -= HandleUIEnable;
        }
        // Use this for initialization
        void Start()
        {
            SelectionLight.enabled = false;
            myEventHandler.OnHoverOver += OnCursEnter;
            myEventHandler.OnHoverLeave += OnCursExit;
            myEventHandler.EventAllyDied += HandleDeath;
            myEventHandler.EventCommandMove += SetupWaypointRenderer;
            myEventHandler.EventTogglebIsFreeMoving += CheckToDisableWaypointRenderer;
            myEventHandler.EventFinishedMoving += DisableWaypointRenderer;
            myEventHandler.EventPartySwitching += OnPartySwitch;
            myEventHandler.EventCommandAttackEnemy += OnCmdAttackEnemy;
            myEventHandler.EventCommandAttackEnemy += DisableWaypointRenderer;
            myEventHandler.OnAllyTakeDamage += SpawnBloodParticles;
            gamemaster.GameOverEvent += HandleGameOver;
            gamemaster.EventHoldingRightMouseDown += HandleCameraMovement;
            uiMaster.EventAnyUIToggle += HandleUIEnable;
        }
        #endregion

        #region Handlers-CursorHoverandExit
        void OnCursEnter()
        {
            if (cameraIsMoving ||
                thisAlly.bIsCurrentPlayer) return;

            SelectionLight.enabled = true;
            if (friend)
            {
                SelectionLight.color = selAllyColor;
            }
            else
            {
                SelectionLight.color = selEnemyColor;
            }

        }

        void OnCursExit()
        {
            if (cameraIsMoving) return;
            SelectionLight.enabled = false;
        }

        #endregion

        #region Handlers
        void SpawnBloodParticles(int amount, Vector3 position, Vector3 force, AllyMember _instigator, GameObject hitGameObject)
        {
            if (BloodParticles == null) return;
            GameObject.Instantiate(BloodParticles, position, Quaternion.identity);
        }

        void SetupWaypointRenderer(Vector3 _point)
        {
            if (IsInvoking("UpdateWaypointRenderer"))
            {
                CancelInvoke("UpdateWaypointRenderer");
            }
            InvokeRepeating("UpdateWaypointRenderer", 0.1f, waypointUpdateRate);
        }

        void DisableWaypointRenderer()
        {
            if (IsInvoking("UpdateWaypointRenderer"))
                CancelInvoke("UpdateWaypointRenderer");

            if(waypointRenderer != null)
            {
                waypointRenderer.enabled = false;
            }
        }

        void DisableWaypointRenderer(AllyMember _ally)
        {
            if (waypointRenderer != null)
            {
                waypointRenderer.enabled = false;
            }
        }

        void CheckToDisableWaypointRenderer(bool _isFreeMoving)
        {
            //If Free Moving, Need to disable waypoint renderer
            if (_isFreeMoving)
            {
                DisableWaypointRenderer();
            }
        }

        void OnPartySwitch()
        {
            DisableWaypointRenderer();
            bHasSwitched = true;
            Invoke("SetbHasSwitchedToFalse", 0.2f);
        }

        void OnCmdAttackEnemy(AllyMember _ally)
        {
            DisableWaypointRenderer();
            bHasSwitched = true;
            Invoke("SetbHasSwitchedToFalse", 0.2f);
        }

        void SetbHasSwitchedToFalse()
        {
            bHasSwitched = false;
        }

        void HandleDeath()
        {
            DestroyOnDeath();
        }

        void HandleGameOver()
        {
            DestroyOnDeath();
        }

        void DestroyOnDeath()
        {
            if (SelectionLight != null)
            {
                SelectionLight.enabled = true;
                Destroy(SelectionLight);
            }
            if (waypointRenderer != null)
            {
                waypointRenderer.enabled = true;
                Destroy(waypointRenderer);
            }
            Destroy(this);
        }

        void HandleCameraMovement(bool _isMoving)
        {
            cameraIsMoving = _isMoving;
            SelectionLight.enabled = false;
        }

        void HandleUIEnable(bool _enabled)
        {
            if (_enabled && SelectionLight != null && SelectionLight.enabled)
            {
                SelectionLight.enabled = false;
            }
        }
        #endregion

        #region Helpers
        void UpdateWaypointRenderer()
        {
            if (bHasSwitched || myNavMesh == null ||
                myNavMesh.path == null ||
                myEventHandler.bIsAIMoving)
                return;

            if (waypointRenderer != null && waypointRenderer.enabled == false)
            {
                waypointRenderer.enabled = true;
            }
            else if (waypointRenderer == null)
            {
                waypointRenderer = this.gameObject.AddComponent<LineRenderer>();
                if (waypointRendererMaterial != null)
                    waypointRenderer.material = waypointRendererMaterial;

                waypointRenderer.startWidth = waypointStartWidth;
                waypointRenderer.endWidth = waypointEndWidth;
                waypointRenderer.startColor = waypointStartColor;
                waypointRenderer.endColor = waypointEndColor;
            }

            var path = myNavMesh.path;

            waypointRenderer.positionCount = path.corners.Length;

            for (int i = 0; i < path.corners.Length; i++)
            {
                waypointRenderer.SetPosition(i, path.corners[i]);
            }
        }
        #endregion

    }
}