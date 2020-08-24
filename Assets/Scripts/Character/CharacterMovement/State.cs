using Character.CharacterMovement.States;
using Player;

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

        public bool CanAim { get; protected set; } = true;
        public bool HideWeapon { get; protected set; } = false;

        protected readonly StateMachine machine;

        public Types Type { get; protected set; }


        protected PlayerMovement Movement => machine.movement;

        protected State(StateMachine machine)
        {
            this.machine = machine;
        }

        #region Logics

        public virtual void HandleInput(){}

        public virtual void Enter()
        {
            machine.movement.WeaponController.CanAim = CanAim;
            machine.movement.WeaponController.HidingWeapon = HideWeapon;
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