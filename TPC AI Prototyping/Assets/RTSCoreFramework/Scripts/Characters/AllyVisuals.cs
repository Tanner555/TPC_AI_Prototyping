using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RTSCoreFramework
{
    public class AllyVisuals : MonoBehaviour
    {

        [Header("Ally Highlighting")]
        public Color selAllyColor;
        public Color selEnemyColor;
        public Light SelectionLight;

        RTSGameMaster gamemaster
        {
            get { return RTSGameMaster.thisInstance; }
        }

        AllyMember thisAlly
        {
            get { return GetComponent<AllyMember>(); }
        }

        AllyEventHandler npcMaster
        {
            get { return GetComponent<AllyEventHandler>(); }
        }

        RTSUiMaster uiMaster { get { return RTSUiMaster.thisInstance; } }

        bool friend
        {
            get { return thisAlly.bIsInGeneralCommanderParty; }
        }

        bool cameraIsMoving = false;

        private void OnDisable()
        {
            npcMaster.OnHoverOver -= OnCursEnter;
            npcMaster.OnHoverLeave -= OnCursExit;
            npcMaster.EventNpcDie -= HandleDeath;
            gamemaster.GameOverEvent -= HandleGameOver;
            gamemaster.EventEnableCameraMovement -= HandleCameraMovement;
            uiMaster.EventAnyUIToggle -= HandleUIEnable;
        }
        // Use this for initialization
        void Start()
        {
            SelectionLight.enabled = false;
            npcMaster.OnHoverOver += OnCursEnter;
            npcMaster.OnHoverLeave += OnCursExit;
            npcMaster.EventNpcDie += HandleDeath;
            gamemaster.GameOverEvent += HandleGameOver;
            gamemaster.EventEnableCameraMovement += HandleCameraMovement;
            uiMaster.EventAnyUIToggle += HandleUIEnable;
        }

        #region CursorHoverandExit
        void OnCursEnter(rtsHitType hitType, RaycastHit hit)
        {
            if (cameraIsMoving) return;
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

        void OnCursExit(rtsHitType hitType, RaycastHit hit)
        {
            if (cameraIsMoving) return;
            SelectionLight.enabled = false;
        }

        #endregion

        void HandleDeath()
        {
            if (SelectionLight != null)
            {
                SelectionLight.enabled = false;
                Destroy(this);
            }
        }

        void HandleGameOver()
        {
            if (SelectionLight != null)
            {
                SelectionLight.enabled = false;
                Destroy(this);
            }
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
    }
}