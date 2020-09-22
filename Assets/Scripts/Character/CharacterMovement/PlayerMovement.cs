using System;
using System.Linq;
using Character.CharacterMovement;
using Character.CharacterMovement.States;
using UnityEngine;
using UnityEngine.XR;
using Weapons;

namespace Player
{
    [RequireComponent(typeof(CharacterController),typeof(WeaponController))]
    public class PlayerMovement : MonoBehaviour
    {
        [SerializeField] private float maxSlope = 45f;
        [SerializeField] public CharacterMovementStats Stats;
        [SerializeField] private LayerMask whatIsFloor;
        [SerializeField] private float slopeForce = 100f;
        [SerializeField, Range(0,2)] private float slidingForceMultiplier=0.25f;


        public StateMachine Machine { get; private set; }
        public State CurrentState =>  Machine?.State;

        public WeaponController WeaponController { get; private set; }
        public new Transform Transform { get; private set; }
        public CharacterController Controller { get; private set; }

        public bool IsGrounded => IsOnSurface && GroundIsWalkable;
        public bool IsOnSurface { get; private set; }
        public bool GroundIsWalkable{ get; private set; }

        public PlayerInput Input { get; private set; }
        
        private RaycastHit _groundHit;

        private void Awake()
        {
            Input = GetComponent<PlayerInput>();
            WeaponController = GetComponent<WeaponController>();
            Transform = GetComponent<Transform>();
            Controller = GetComponent<CharacterController>();
        }

        private void Start()
        {
            Machine = new StateMachine(this);
        }

        private void Update()
        {
            // Ground check
            IsOnSurface = CheckGround(out _groundHit);
            if(IsOnSurface) GroundIsWalkable = IsWalkable(_groundHit);


            if (IsGrounded)
            {
                Machine.State.OnLanding();
            }
            else
            {
                if (IsOnSurface)
                {
                    Machine.State.OnStartSliding();
                }
                else
                {
                    Machine.State.OnLooseGround();
                }
            }

            Machine.State.HandleInput();
        }

        // TODO add more rays and increase collider radius
        private bool CheckGround(out RaycastHit hit)
        {
            Ray ray = new Ray(transform.position,Vector3.down);
            return Physics.Raycast(ray, out hit, 1.5f, whatIsFloor);
        }

        public bool IsWalkable(RaycastHit hit)
        {
            return Mathf.Abs(Vector3.Angle(hit.normal, Vector3.up)) <= Controller.slopeLimit;
        }


        public Vector3 SlopeForce()
        {
            if(IsOnSurface == false) return Vector3.zero;
            // return Vector3.down * slopeForce;

            if (GroundIsWalkable)
            {
                // Stick to surface 
                return Vector3.down * slopeForce;
            }
            else
            {
                return Vector3.ProjectOnPlane(Physics.gravity, _groundHit.normal) * slidingForceMultiplier;
            }
        }
    }
}