using UnityEditor.Rendering;
using UnityEngine;

namespace Player.States
{
    public class Standing : State
    {
        public Standing(StateMachine machine) : base(machine)
        {
            IsGroundedState = true;
            IsMovingState = false;
            Type = Types.Standing;
        }

        public override void HandleInput()
        {
            base.HandleInput();
            
            if (Movement.input.magnitude>0f)
            {
                OnStartMoving();
                return;
            }
            

            if (Movement.shiftInput)
            {
                OnStartRunning();
                return;
            }

            if (Movement.spaceInput)
            {
                OnTryToJump();
                return;
            }
            
            machine.movement.LocalVelocity = new Vector3(0f,machine.movement.LocalVelocity.y,0f);
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
        
        public override void OnStartMoving()
        {
            base.OnStartMoving();
            machine.ChangeState(new Walking(machine));
        }

        public override void OnStartRunning()
        {
            base.OnStartRunning();
            machine.ChangeState(new Running(machine));
        }

        public override void OnTryToJump()
        {
            base.OnTryToJump();
            machine.ChangeState(new Jumping(machine));
        }
    }
}