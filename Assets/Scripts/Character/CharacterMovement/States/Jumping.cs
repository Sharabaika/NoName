using Character.CharacterMovement.States;
using UnityEngine;
using UnityEngine.Experimental.XR;

namespace Player.States
{
    // TODO do i rly need this???
    public class Jumping: State
    {
        private Vector3 _velocity = Vector3.zero;
        
        public Jumping(StateMachine machine) : base(machine)
        {
            Type = Types.Jumping;
        }

        public Jumping(StateMachine machine, Vector3 velocity) : base(machine)
        {
            Type = Types.Jumping;
            _velocity = velocity;
        }
        
        public override void HandleInput()
        {
            base.HandleInput();
        }

        public override void Enter()
        {
            base.Enter();
            machine.ChangeState(new Falling(machine,Vector3.up*Movement.Stats.JumpSpeed + _velocity));
        }

        public override void Leave()
        {
        }
    }
}