using System;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using UnityEngine;

namespace Player
{
    
    // Unused
    // а ведь идея была хорошая
    [RequireComponent(typeof(Rigidbody))]
    public class PlayerRigidBodyMovement : MonoBehaviour
    {
        // TODO serialize states
        [SerializeField] private float maxSlope = 45f;
        [SerializeField] private Vector3 gravity = Vector3.down * 10f;
        [SerializeField] private float jumpForce = 20f;
        [SerializeField] private float stopSpeed = 0.3f;

        private State CurrentState => _stateMachine.State;
        
        private StateMachine _stateMachine;
        private Camera _camera;
        private Transform _cameraTransform;

        private Rigidbody _rigidbody;
        private Transform _transform;

        private Vector3 _localVelocity;
        private float _localSpeed;

        private float _xInput;
        private float _zInput;

        private void OnEnable()
        {
            _rigidbody = GetComponent<Rigidbody>();
            _transform = GetComponent<Transform>();
            _camera = Camera.main;
            _cameraTransform = _camera.transform;

            // _stateMachine = new StateMachine(this);
            // _stateMachine.Initialize();
            _rigidbody.useGravity = false;
        }

        private void Update()
        {
            // Updating physics variables
            _localVelocity = _transform.InverseTransformDirection(_rigidbody.velocity);
            _localSpeed = _localVelocity.magnitude;
            
            // Rotation
            Vector3 cameraViewDir = _cameraTransform.forward;
            _rigidbody.rotation = Quaternion.LookRotation(
                new Vector3(cameraViewDir.x, 0f, cameraViewDir.z),
                Vector3.up);
            
            
            // Gravity
            _rigidbody.AddForce(gravity);
            
            // TODO split into methods
            // TODO fix diagonals
            
            // Input
            _xInput = Input.GetAxisRaw("Horizontal");
            _zInput = Input.GetAxisRaw("Vertical");

            ManageStates();

            // RIP
            // if (CurrentState.IsMovingState)
            // {
            //     // Counter movement
            //     var counterMov = GetCounterMovement(_xInput, _zInput);
            //     _rigidbody.AddRelativeForce(counterMov, ForceMode.Force);
            //
            //     // Limiting speed
            //     var xMult = _localVelocity.x * _xInput < 0f
            //         ? 1f
            //         : -(_localVelocity.x - CurrentState.maxSpeed) * (_localVelocity.x + CurrentState.maxSpeed) /
            //           CurrentState.maxSpeed / CurrentState.maxSpeed;
            //     var zMult = _localVelocity.z * _zInput < 0f
            //         ? 1f
            //         : -(_localVelocity.z - CurrentState.maxSpeed) * (_localVelocity.z + CurrentState.maxSpeed) /
            //           CurrentState.maxSpeed / CurrentState.maxSpeed;
            //
            //     // Movement
            //     var inputDirection = Vector3.ClampMagnitude(new Vector3(_xInput * xMult, 0, _zInput * zMult), 1f);
            //     _rigidbody.AddRelativeForce(inputDirection * CurrentState.acceleration);
            // }
        }

        private void ManageStates()
        {
            bool anyMovInput = (Mathf.Abs(_xInput) > float.Epsilon || Mathf.Abs(_zInput) > float.Epsilon);

            // TODO rework 
            if (Input.GetKeyDown(KeyCode.Space))
            {
                CurrentState.OnTryToJump();
            }
            if (anyMovInput && Input.GetKey(KeyCode.LeftShift))
            {
                CurrentState.OnStartRunning();
            }
            if (Input.GetKeyUp(KeyCode.LeftShift))
            {
                CurrentState.OnStopRunning();
            }
            if (CurrentState.IsMovingState && !anyMovInput && Mathf.Abs(_localSpeed) < stopSpeed)
            {
                StopMoving();
            }
            if (CurrentState.IsMovingState == false && anyMovInput)
            {
                CurrentState.OnStartMoving();
            }
        }

        public void Jump()
        {
            _rigidbody.AddForce(Vector3.up *jumpForce,ForceMode.Impulse);
        }

        public void StopMoving()
        {
            var vel = _rigidbody.velocity;
            vel *= -1f;
            vel.y = 0f;
            _rigidbody.AddForce(vel,ForceMode.VelocityChange);
            CurrentState.OnStopMoving();
        }
        
        private Vector3 GetCounterMovement(float xInput, float zInput)
        {
            // var counterX = Mathf.Abs(xInput) < Double.Epsilon
            //     ? _localVelocity.x * (-CurrentState.counterMovement)
            //     : 0f;
            // var counterZ = Mathf.Abs(zInput) < Double.Epsilon
            //     ? _localVelocity.z * (-CurrentState.counterMovement)
            //     : 0f;
            //
            // var counterMov = new Vector3(counterX, 0f, counterZ);
            //
            // return counterMov;
            return  Vector3.zero;
        }


        private bool IsFloor(Vector3 normal)
        {
            return Mathf.Abs(Vector3.Angle(normal, Vector3.up)) < maxSlope;
        }

        private void OnCollisionEnter(Collision other)
        {
            if (CurrentState.IsGroundedState == false && other.contacts.Any(collision => IsFloor(collision.normal)))
            {
                CurrentState.OnLanding();
            }
        }

        private void OnCollisionExit(Collision other)
        {
            // TODO add event for losingGround 
            CurrentState.OnLooseGround();
        }
    } 
}
