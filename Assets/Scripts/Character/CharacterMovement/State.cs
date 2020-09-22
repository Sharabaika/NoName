using Character.CharacterMovement.States;
using Player;
using UnityEngine;
using Weapons;

namespace Character.CharacterMovement
{
    public abstract class State
    {
        public enum Types
        {
            Standing,
            Walking,
            Running,
            Jumping,
            Falling,
            Sliding
        }

        public WeaponPositioningRestrictions Restrictions { get; protected set; } = WeaponPositioningRestrictions.None;
        public bool CanReload { get; protected set; } = true;
        public Types Type { get; protected set; }

        protected PlayerInput Input => Movement.Input;

        protected readonly StateMachine machine;
        protected PlayerMovement Movement => machine.Movement;

        protected State(StateMachine machine)
        {
            this.machine = machine;
        }

        #region Logics

        public virtual void HandleInput(){}

        public virtual void Enter()
        {
            
        }

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