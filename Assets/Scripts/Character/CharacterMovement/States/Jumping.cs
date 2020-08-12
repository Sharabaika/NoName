using UnityEngine;
using UnityEngine.Experimental.XR;

namespace Player.States
{
    // TODO do i rly need this???
    public class Jumping: State
    {
        public Jumping(StateMachine machine) : base(machine)
        {
            IsGroundedState = true;
            IsMovingState = true;
            Type = Types.Jumping;
        }

        public override void HandleInput()
        {
            base.HandleInput();
        }

        public override void Enter()
        {
            // TODO mb jump in direction of (normal+Up)/2 ?
            Movement.LocalVelocity += Vector3.up * Movement.Stats.JumpSpeed;
            machine.ChangeState(new Falling(machine));
        }

        public override void Leave()
        {
        }
    }
}