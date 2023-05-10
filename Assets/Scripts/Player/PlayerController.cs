using System;
using System.Collections;
using System.Collections.Generic;
using Player.States;
using Player.States.Controllers;
using Respawn;
using UnityEditor;
using UnityEngine;
using UnityEngine.InputSystem;

/**
 * Central hub for controlling player. Creates a 
 */
namespace Player
{
    public class PlayerController : MonoBehaviour
    {
        private Rigidbody body;
        private Collider coll; // used for determining ground
        private StateController stateController;
        private RespawnPointController currentController;
        private PlayerInput playerInput;
        private Coroutine dashCoroutine;
        private Coroutine respawnCoroutine;

        #region State Definitions
            private static IdleState idleState = new IdleState();
            private static MovingState movingState = new MovingState();
            private static TraversalState traversalState = new TraversalState();
            private static JumpingState jumpingState = new JumpingState();
            private static DashingState dashingState = new DashingState();
            private static InventoryState inventoryState = new InventoryState();
            private static TaskLogState taskLogState = new TaskLogState();
            private static DeathState deathState = new DeathState();
            private static DebugState debugState = new DebugState();
        #endregion

            private Vector2 move;
            private Vector2 look;

            [SerializeField] private float gravity;

        #region Ground Variables
        [Header("Ground Fields")]
            [SerializeField] private float sphereRadius;
            [SerializeField] private float groundOffset;
            [SerializeField] private LayerMask groundMask;
        #endregion

        #region  Rotation Variables
        [Header("Rotation Fields")]
            [SerializeField] private float rotationPower;
            [SerializeField] private float rotationSpeed;
        #endregion

        #region Game Object Variables
        [Header("Game Object Fields")]
            [SerializeField] private GameObject followTarget;
            [SerializeField] private GameObject model;
            [SerializeField] private GameObject corpse;
            [SerializeField] private GameObject inventoryWindow;
            [SerializeField] private GameObject taskWindow;
        #endregion

        #region Hover Variables
        [Header("Hover Fields")]
            [SerializeField] private float length;
            [SerializeField] private float traversalLength;
            [SerializeField] private float strength;
            [SerializeField] private float dampening;
            [SerializeField] private float rayOffset;
            private float lastHitDistance;
        #endregion

        #region Respawn Variables
            [SerializeField] private RespawnData respawnData;
            [SerializeField] private float respawnTime;
        #endregion

        #region Unity Functions
        void Awake()
        {
            body = GetComponent<Rigidbody>();
            coll = GetComponent<Collider>();
            playerInput = GetComponent<PlayerInput>();
            stateController = new StateController(idleState, body);
            Physics.gravity = new Vector3(0f, gravity, 0f);
        }

        void Update() {  } // for future use

        void FixedUpdate()
        {
            LookRotation();
            if (PlayerIsRespawning()) return;

            Hover();
            ManageStates();
            MovementRotation();
        }

        private void MovementRotation()
        {
            if(InventoryIsOpen() || TaskLogIsOpen()) return;

            // skip when dashing so player's dash is in the same direction as it is based off model.transform.forward
            if ( IsMoving() &&
                 !stateController.PlayerIsInState(typeof(DashingState)) )
            {
                Vector3 moveDir = transform.forward * move.y +
                                transform.right * move.x;
                moveDir.Normalize();

                Quaternion toRotation = Quaternion.LookRotation(moveDir, Vector3.up);
                model.transform.rotation = Quaternion.RotateTowards(model.transform.rotation, toRotation, rotationSpeed * Time.deltaTime);
            }
        }

        private void LookRotation()
        {
            if(InventoryIsOpen() || TaskLogIsOpen()) return;
            
            followTarget.transform.rotation *= Quaternion.AngleAxis(look.x * rotationPower, Vector3.up);
            followTarget.transform.rotation *= Quaternion.AngleAxis(look.y * rotationPower, Vector3.right);

            Vector3 angles = followTarget.transform.localEulerAngles;
            angles.z = 0;

            float angle = followTarget.transform.localEulerAngles.x;
            if (angle > 180 && angle < 340)
            { // clamps the angle past 340 degrees to prevent overextension
                angles.x = 340;
            }
            else if (angle < 180 && angle > 60)
            { // clamps the angle below 60 degrees to prevent overextension
                angles.x = 60;
            }

            followTarget.transform.localEulerAngles = angles;

            if (IsMoving())
            {
                Quaternion y_rot = Quaternion.Euler(0,
                                                    followTarget.transform.rotation.eulerAngles.y,
                                                    0
                                                    );

                body.rotation = y_rot;
            }
        }

        private void OnTriggerEnter(Collider other) 
        {
            if(ObjectCollidedHasRespawnTag(other.tag))
            {
                currentController = other.GetComponentInParent<RespawnPointController>();
                respawnData = currentController.Data;
            }
        }
        private void OnTriggerExit(Collider other) 
        {
            if(ObjectCollidedHasRespawnTag(other.tag))
            {
                if(currentController.IsOpen())
                {
                    currentController.CloseRespawnWindow();
                }
                currentController = null;
            }    
        }
        #endregion

        #region State Functions
        /**
         * Manages the state controller. First, checks the state stack to 
         * see if any ExtraState 
         */
        void ManageStates()
        {
            if (PlayerIsDashing()  || InventoryIsOpen() || TaskLogIsOpen() || PlayerIsInDebugState()) return;
            stateController.CheckStateStack();
            switch (stateController.CurrentState)
            {
                case IdleState:
                    stateController.RunCurrentState();
                    break;
                case MovingState:
                    stateController.RunCurrentState(move);
                    break;
                case TraversalState:
                    stateController.RunCurrentState(move);
                    break;
                case DashingState:
                    dashCoroutine = StartCoroutine(Dash());
                    break;
                case JumpingState:
                    stateController.RunCurrentState();
                    break;
                case InventoryState:
                    stateController.RunCurrentState(inventoryWindow);
                    break;
                case TaskLogState:
                    stateController.RunCurrentState(taskWindow);
                    break;
                case DeathState:
                    respawnCoroutine = StartCoroutine(Respawn());
                    break;
                case DebugState:
                    stateController.RunCurrentState();
                    break;
            }
        }
        private IEnumerator Dash ()
        {
            dashingState.canDash = false;
            body.useGravity = false; // gravity disabled so dash can be used in platforming context

            stateController.RunCurrentState(model);
            yield return new WaitForSeconds(dashingState.dashTime);

            dashCoroutine = null;
            stateController.CheckStateStack();
            body.useGravity = true;

            yield return new WaitForSeconds(dashingState.dashCooldown);
            dashingState.canDash = true;
        }
        private IEnumerator Respawn()
        {
            model.SetActive(false);
            body.useGravity = false;
            stateController.RunCurrentState(corpse);
            yield return new WaitForSecondsRealtime(respawnTime);

            body.MovePosition(respawnData.points);
            model.SetActive(true);
            body.useGravity = true;
            stateController.SetState(idleState);

            respawnCoroutine = null;
        }
        #endregion
        

        #region Hover Functions
        /**
         * Function makes player hover above the ground at specified distance.
         * Oscillates for a bit before stabilizing at the specified length. The
         * length switches depending on if the player is in vehicle mode or not.
         * This code was provided by Nick of Bit Galaxis 
         */
        private void Hover()
        {
            if (PlayerIsDashing() ) return; // dashing turns off gravity which affects hover height

            RaycastHit hit;
            Vector3 ray_pos = new Vector3(transform.position.x,
                                          transform.position.y + rayOffset, // offset to account for player's hovering
                                          transform.position.z);
            float hoverLength = (stateController.PlayerIsInState(typeof(TraversalState))) ? traversalLength : length;
            if (Physics.Raycast(ray_pos, transform.TransformDirection(-Vector3.up), out hit, hoverLength, groundMask))
            {
                float force_amt = HooksLawDampen(hit.distance, hoverLength);

                body.AddForceAtPosition(transform.up * force_amt, transform.position);
            }
            else
            {
                lastHitDistance = hoverLength * 1.1f;
            }
        }
        private float HooksLawDampen (float hitDistance, float hoverLength)
        {
            float forceAmt = strength * (hoverLength - hitDistance) + (dampening * (lastHitDistance - hitDistance));
            forceAmt = Mathf.Max(0f, forceAmt);
            lastHitDistance = hitDistance;

            return forceAmt;
        }
        #endregion

        #region Unity Input Functions
        void OnMove(InputValue val)
        {
            if(PlayerIsInDebugState()) return;
            move = val.Get<Vector2>();

            if( stateController.PlayerIsInState( typeof(TraversalState)) ) return;
            if( !IsMoving() )
            {
                stateController.SetState(idleState);
                return;
            }
            if( stateController.PlayerIsInState( typeof(MovingState)) ) return;
            stateController.SetState(movingState);
        }
        void OnLook(InputValue val)
        {
            if(PlayerIsInDebugState()) return;
            look = val.Get<Vector2>();
        }
        void OnFire()
        {
            if(PlayerIsInDebugState()) return;
           // stateController.SetState(deathState); // TODO remove later, for testing death mechanic 
        }
        void OnInventory()
        {
            if(PlayerIsInDebugState()) return;
            if (InventoryIsOpen())
            {
                stateController.CheckStateStack();
            }
            else
            {
                stateController.SetState(inventoryState);
            }
        }
        void OnDash()
        {
            if(PlayerIsInDebugState()) return;
            if( !dashingState.canDash ) return;
            stateController.SetState(dashingState);
        }
        void OnJump()
        {
            if(PlayerIsInDebugState()) return;
            if( !IsGrounded() ) return;
            stateController.SetState(jumpingState);
        }
        void OnTraversal()
        {
            if(PlayerIsInDebugState()) return;
            if( stateController.PlayerIsInState(typeof(TraversalState)) )
            {
                PlayerState state = ( !IsMoving() ) ? idleState : movingState;
                stateController.SetState(state);
            }
            else
            {
                stateController.SetState(traversalState);
            }
        }
        void OnActivate()
        {
            if(PlayerIsInDebugState()) return;
            if(PlayerIsWithinRespawnPoint())
            {
                if(currentController.IsOpen())
                {
                    currentController.CloseRespawnWindow();
                    return;
                }
                currentController.OpenRespawnWindow();
            }
        }
        void OnTaskLog()
        {
            if(PlayerIsInDebugState()) return;
            if(TaskLogIsOpen())
            {
                stateController.CheckStateStack();
            }
            else
            {
                stateController.SetState(taskLogState);
            }
        }
        void OnToggleDebug()
        {
            if(PlayerIsInDebugState())
            {
                stateController.SetState(idleState);
                stateController.CheckStateStack();
            }
            else
            {
                stateController.SetState(debugState);
            }
        }

        private bool PlayerIsInDebugState()
        {
            return stateController.PlayerIsInState(typeof(DebugState));
        }

        private bool PlayerIsWithinRespawnPoint()
        {
            return currentController != null;
        }
        #endregion

        #region Boolean / Check Functions
        bool IsGrounded()
        { 
            Vector3 ground_vector = new Vector3(coll.bounds.center.x, 
                                                coll.bounds.center.y - groundOffset, // offset to account for player's hover
                                                coll.bounds.center.z
                                                );

            return Physics.CheckSphere(ground_vector, sphereRadius, groundMask);
        }
        bool IsMoving()
        {
            return (Mathf.Abs(move.magnitude) != 0);
        }
        private bool PlayerIsDashing()
        {
            return !(dashCoroutine == null);
        }
        private bool PlayerIsRespawning()
        {
            return respawnCoroutine != null;
        }
        private bool ObjectCollidedHasRespawnTag(string tag)
        {
            return tag == "Respawn";
        }
        private bool InventoryIsOpen()
        {
            return inventoryWindow.activeInHierarchy;
        }
        private bool TaskLogIsOpen()
        {
            return taskWindow.activeInHierarchy;
        }
        #endregion

    }
}