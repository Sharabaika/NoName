using System;
using System.Linq;
using UnityEngine;
using UnityEngine.XR;

namespace Player
{
    // TODO move control methods to separate CharacterMovement class
    // TODO mb 10g is to much
    public class PlayerMovement : MonoBehaviour
    {
        [SerializeField] private float maxSlope = 45f;
        [SerializeField] public CharacterMovementStats Stats;
        
        private CharacterController _controller;
        private StateMachine _stateMachine;
        private Rigidbody _rigidBody;
        private Transform _cameraTransform;
        public State CurrentState =>  _stateMachine?.State;

        public Vector3 Velocity
        {
            get => _rigidBody.velocity;
            set => _rigidBody.velocity = value;
        }

        public Vector3 LocalVelocity
        {
            get =>transform.InverseTransformDirection(_rigidBody.velocity);
            set =>_rigidBody.velocity = transform.TransformDirection(value);
        }

        /// <summary>
        /// ignores Y component
        /// </summary>
        public void SetSimpleLocalVelocity(Vector3 v)
        {
            var velocity = new Vector3(v.x,LocalVelocity.y,v.z);
            LocalVelocity = velocity;
        }

        public void AddForce(Vector3 force, ForceMode mode = ForceMode.Force) => _rigidBody.AddForce(force, mode);
        public void AddRelativeForce(Vector3 force, ForceMode mode = ForceMode.Force) => _rigidBody.AddRelativeForce(force, mode);
        public float xInput =>Input.GetAxis("Horizontal");
        public float zInput =>Input.GetAxis("Vertical");
        public bool spaceInput =>Input.GetKey(KeyCode.Space);
        public bool shiftInput =>Input.GetKey(KeyCode.LeftShift);
        public Vector3 input => new Vector3(xInput, 0f, zInput).normalized;
        
        private void Awake()
        {
            _controller = GetComponent<CharacterController>();
            _rigidBody = GetComponent<Rigidbody>();
            _stateMachine = new StateMachine(this);
            _cameraTransform = Camera.main.transform;
        }

        private void Update()
        {
            // Rotation
            Vector3 cameraViewDir = _cameraTransform.forward;
            _rigidBody.rotation = Quaternion.LookRotation(
                new Vector3(cameraViewDir.x, 0f, cameraViewDir.z),
                Vector3.up);
            
            _stateMachine.State.HandleInput();
        }

        private bool IsFloor(Vector3 normal)
        {
            return Mathf.Abs(Vector3.Angle(normal, Vector3.up)) < maxSlope;
        }

        private void OnCollisionStay(Collision other)
        {
            if (other.contacts.Any(collision => IsFloor(collision.normal)))
            {
                CurrentState.OnLanding();
            }
            else
            {
                CurrentState.OnLooseGround();
            }
        }

        private void OnCollisionEnter(Collision other)
        {
            if (other.contacts.Any(collision => IsFloor(collision.normal)))
            {
                CurrentState.OnLanding();
            }
        }

        private void OnCollisionExit(Collision other)
        {
            // BUG (other.contacts.Any(collision => IsFloor(collision.normal))) not working
            CurrentState.OnLooseGround();
        }
    }
}