using System.Security;
using Character.CharacterMovement;
using Character.CharacterMovement.States;
using UnityEngine;
using UnityEngine.XR;

namespace Player.States
{
    public class Walking:State
    {
        
        public Walking(StateMachine machine) : base(machine)
        {
            Type = Types.Walking;
        }
        
        public override void HandleInput()
        {
            
            if (Input.shiftInput)
            {
                OnStartRunning();
                return;
            }

            if (Input.spaceInput)
            {
                OnTryToJump();
                return;
            }

            if (Input.movementInput == Vector3.zero)
            {
                OnStopMoving();
                return;
            }

            var slopeForce = Movement.SlopeForce();
            Movement.Controller.SimpleMove(Movement.Transform.rotation * Input.movementInput * Movement.Stats.WalkSpeed + slopeForce);
        }

        public override void OnLooseGround()
        {
            base.OnLooseGround();
            machine.ChangeState(new Falling(machine,
                Movement.Transform.rotation * Input.movementInput * Movement.Stats.WalkSpeed));
        }

        public override void OnStopMoving()
        {
            base.OnStopMoving();
            machine.ChangeState(new Standing(machine));
        }

        public override void OnStartRunning()
        {
            base.OnStartRunning();
            machine.ChangeState(new Running(machine));
        }

        public override void OnTryToJump()
        {
            base.OnTryToJump();
            machine.ChangeState(new Jumping(machine,
                Movement.Transform.rotation * Input.movementInput * Movement.Stats.WalkSpeed));
        }
    }
}