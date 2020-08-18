using System;
using System.Linq;
using Character.CharacterMovement.States;
using Cinemachine.Utility;
using UnityEngine;
using UnityEngine.XR;
using Weapons;

namespace Player
{
    public class PlayerMovement : MonoBehaviour
    {
        [SerializeField] private float maxSlope = 45f;
        [SerializeField] public CharacterMovementStats Stats;
        [SerializeField] private LayerMask whatIsFloor;
        [SerializeField] private float slopeForce = 100f;
        [SerializeField, Range(0,2)] private float slidingForceMultiplier=0.25f;


        private StateMachine _stateMachine;
        private Transform _cameraTransform;
        private WeaponController _weaponController;
        public State CurrentState =>  _stateMachine?.State;

        public new Transform Transform { get; private set; }
        public CharacterController Controller { get; private set; }

        public bool IsGrounded => IsOnSurface && GroundIsWalkable;
        public bool IsOnSurface { get; private set; }
        public bool GroundIsWalkable{ get; private set; }

        public float xInput =>Input.GetAxis("Horizontal");
        public float zInput =>Input.GetAxis("Vertical");
        public bool spaceInput =>Input.GetKey(KeyCode.Space);
        public bool shiftInput =>Input.GetKey(KeyCode.LeftShift);
        public Vector3 input => new Vector3(xInput, 0f, zInput).normalized;

        private RaycastHit _groundHit;


        private void Awake()
        {
            // _rigidBody = GetComponent<Rigidbody>();
            _weaponController = GetComponent<WeaponController>();
            Transform = GetComponent<Transform>();
            Controller = GetComponent<CharacterController>();
            
            _stateMachine = new StateMachine(this);
            _cameraTransform = Camera.main.transform;
        }

        private void Update()
        {
            // Ground check
            IsOnSurface = CheckGround(out _groundHit);
            if(IsOnSurface) GroundIsWalkable = IsWalkable(_groundHit);

            // Rotation
            Vector3 cameraViewDir = _cameraTransform.forward;
            Transform.rotation = Quaternion.LookRotation(
                new Vector3(cameraViewDir.x, 0f, cameraViewDir.z),
                Vector3.up);
            
            _weaponController.RotateWeapon(_cameraTransform.rotation);


            if (IsGrounded)
            {
                _stateMachine.State.OnLanding();
            }
            else
            {
                if (IsOnSurface)
                {
                    _stateMachine.State.OnStartSliding();
                }
                else
                {
                    _stateMachine.State.OnLooseGround();
                }
            }

            _stateMachine.State.HandleInput();
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