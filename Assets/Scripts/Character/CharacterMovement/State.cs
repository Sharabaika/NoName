using System.Diagnostics;
using Character.CharacterMovement.States;
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
            Falling,
            Sliding
        }
        
        protected readonly StateMachine machine;

        public virtual void HandleInput()
        {
        }

        public Types Type { get; protected set; }
        
        
        protected PlayerMovement Movement => machine.movement;
        
        protected State(StateMachine machine)
        {
            this.machine = machine;
        }

        #region Logics

        public virtual void Enter(){}
        
        public virtual void Leave(){}


        public virtual void OnTryToJump()
        {
        }
        
        public virtual void OnLooseGround()
        {
        }

        public virtual void OnStartSliding()
        {
            machine.ChangeState(new Sliding(machine));
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