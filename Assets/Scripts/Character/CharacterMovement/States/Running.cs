using UnityEngine;

namespace Player.States
{
    public class Running : State
    {
        public Running(StateMachine machine) : base(machine)
        {
            IsGroundedState = true;
            IsMovingState = true;
            Type = Types.Running;
        }

        public override void HandleInput()
        {
            base.HandleInput();
            
            if (Movement.shiftInput==false)
            {
                OnStopRunning();
                return;
            }

            if (Movement.spaceInput)
            {
                OnTryToJump();
                return;
            }

            if (Movement.input == Vector3.zero)
            {
                OnStopMoving();
                return;
            }
            
            Movement.SetSimpleLocalVelocity(Movement.input * Movement.Stats.RunSpeed);
        }

        public override void Enter()
        {
            
        }

        public override void Leave()
        {
            
        }

        public override void OnLooseGround()
        {
            base.OnLooseGround();
            machine.ChangeState(new Falling(machine));
        }

        public override void OnStopMoving()
        {
            base.OnStopMoving();
            machine.ChangeState(new Standing(machine));
        }

        public override void OnStopRunning()
        {
            base.OnStopRunning();
            machine.ChangeState(new Walking(machine));
        }

        public override void OnTryToJump()
        {
            base.OnTryToJump();
            machine.ChangeState(new Jumping(machine));
        }
    }
}