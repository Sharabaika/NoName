using System.Diagnostics;
using UnityEngine;
using UnityEngine.XR;
using Debug = UnityEngine.Debug;

namespace Player
{
    public abstract class State
    {
        // TODO serialize
        
        public enum Types
        {
            Standing,
            Walking,
            Running,
            Jumping,
            Falling
        }

        public readonly float speed = 10f;

        protected readonly StateMachine machine;

        public virtual void HandleInput()
        {
        }

        public Types Type { get; protected set; }
        
        public bool IsMovingState{ get; protected set; }
        
        public bool IsGroundedState{ get; protected set; }
        
        protected PlayerMovement Movement => machine.movement;
        
        protected State(StateMachine machine)
        {
            this.machine = machine;
        }

        // TODO call from StateMachine class
        #region Logics

        public abstract void Enter();
        
        public abstract void Leave();


        public virtual void OnTryToJump()
        {
        }
        
        public virtual void OnLooseGround()
        {
        }

        public virtual void OnLanding()
        {
        }

        public virtual void OnStartRunning()
        {
        }

        public virtual void OnStopRunning()
        {
        }

        public virtual void OnStartMoving()
        {
        }

        public virtual void OnStopMoving()
        {
        }

        #endregion
    }
}