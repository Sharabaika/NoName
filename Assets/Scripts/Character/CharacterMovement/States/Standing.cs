using Character.CharacterMovement.States;
using UnityEditor.Rendering;
using UnityEngine;

namespace Player.States
{
    public class Standing : State
    {
        public Standing(StateMachine machine) : base(machine)
        {
            Type = Types.Standing;
        }

        public override void HandleInput()
        {
            base.HandleInput();
            
            if (Movement.input.magnitude>0f)
            {
                if (Movement.shiftInput)
                {
                    OnStartRunning();
                    return;
                }
                OnStartMoving();
                return;
            }

            if (Movement.spaceInput)
            {
                OnTryToJump();
                return;
            }

            machine.movement.Controller.SimpleMove(Vector3.zero);
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