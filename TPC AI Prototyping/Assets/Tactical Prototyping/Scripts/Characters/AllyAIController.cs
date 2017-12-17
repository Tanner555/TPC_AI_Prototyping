using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using Opsive.ThirdPersonController;
using Opsive.ThirdPersonController.Wrappers.Abilities;
//using Opsive.ThirdPersonController.Wrappers;

namespace RTSPrototype
{
    public class AllyAIController : MonoBehaviour
    {
        #region Components
        RigidbodyCharacterController myRigidbodyTPC;
        Inventory myInventory;
        ItemHandler itemHandler;
        NavMeshAgent myNavAgent;
        RTSNavBridge myRTSNavBridge;
        AllyEventHandler myEventHandler;
        AllyMember allyMember;
        #endregion

        #region Fields
        //For testing, will be deleted in the future
        //GameObject currentPlayer = null;
        #endregion

        #region Properties
        bool AllCompsAreValid
        {
            get
            {
                return myRigidbodyTPC && myInventory && itemHandler
                    && myNavAgent && myRTSNavBridge && myEventHandler
                    && allyMember;
            }
        }
        #endregion
      
        #region UnityMessages
        // Use this for initialization
        void Start()
        {
            SetInitialReferences();
            if (AllCompsAreValid)
            {
                //currentPlayer = GameObject.FindGameObjectWithTag("Player");
                //if (currentPlayer == null)
                //{
                //    Debug.Log("Couldn't find local player");
                //}
                //else
                //{
                //    myRTSNavBridge.LookAtTarget(currentPlayer.transform);
                //}
            }
            myEventHandler.EventCommandMove += OnCommandMove;
            myEventHandler.EventNpcDie += OnDeath;
        }

        private void OnDisable()
        {
            myEventHandler.EventCommandMove -= OnCommandMove;
            myEventHandler.EventNpcDie -= OnDeath;
        }

        // Update is called once per frame
        void Update()
        {
            
        }
        #endregion

        #region Getters
        public bool isEnemyFor(Transform _transform, out AllyMember _ally)
        {
            _ally = null;
            if (_transform.root.GetComponent<AllyMember>())
                _ally = _transform.root.GetComponent<AllyMember>();

            return _ally != null && allyMember.IsEnemyFor(_ally);
        }

        public bool isSurfaceWalkable(RaycastHit hit)
        {
            return myNavAgent.CalculatePath(hit.point, myNavAgent.path) &&
            myNavAgent.path.status == NavMeshPathStatus.PathComplete;
        }
        #endregion

        #region Finders
        Opsive.ThirdPersonController.Abilities.Ability FindAbility(System.Type _type)
        {
            if (AllCompsAreValid)
            {
                foreach (var _ability in myRigidbodyTPC.Abilities)
                {
                    if (_type.Equals(_ability.GetType()))
                    {
                        return _ability;
                    }
                }
            }
            return null;
        }
        #endregion

        #region Handlers
        void OnCommandMove(rtsHitType hitType, RaycastHit hit)
        {
            if (AllCompsAreValid)
            {
                myNavAgent.SetDestination(hit.point);
                myNavAgent.isStopped = false;
            }
        }

        void OnDeath()
        {
            var _mHitbox = transform.GetComponentInChildren<MeleeWeaponHitbox>();
            if (_mHitbox != null)
            {
                _mHitbox.SetActive(false);
            }
            this.enabled = false;
        }
        #endregion

        void SetInitialReferences()
        {
            myRigidbodyTPC = GetComponent<RigidbodyCharacterController>();
            myInventory = GetComponent<Inventory>();
            itemHandler = GetComponent<ItemHandler>();
            myNavAgent = GetComponent<NavMeshAgent>();
            myRTSNavBridge = GetComponent<RTSNavBridge>();
            myEventHandler = GetComponent<AllyEventHandler>();
            allyMember = GetComponent<AllyMember>();

            if (!AllCompsAreValid)
            {
                Debug.LogError("Not all comps are valid!");
            }
        }

        #region Commented Code
        //if (Input.GetKeyDown(KeyCode.C))
        //{
        //    Debug.Log("Finding Ability...");
        //    var _ability = FindAbility(typeof(HeightChange));
        //    if (_ability != null)
        //    {
        //        Debug.Log(_ability.GetType().Name);
        //        Debug.Log(_ability.IsActive);
        //        if(!_ability.IsActive)
        //        {
        //            if (!myRigidbodyTPC.TryStartAbility(_ability))
        //            {
        //                Debug.Log("Ability Failed");
        //            }
        //        }
        //        else
        //        {
        //            myRigidbodyTPC.TryStopAbility(_ability);
        //        }

        //    }
        //}

        //if (Input.GetKey(KeyCode.Mouse0))
        //{
        //    Debug.Log("Try using item");
        //    if (!itemHandler.TryUseItem(typeof(PrimaryItemType)))
        //    {
        //        Debug.Log("Couldn't use primary weapon");
        //    }
        //}
        #endregion
        
    }
}