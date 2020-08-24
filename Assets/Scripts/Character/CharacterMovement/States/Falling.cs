using Player;
using Player.States;
using UnityEngine;

namespace Character.CharacterMovement.States
{
    public class Falling : State
    {
        private Vector3 _velocity;
        
        public Falling(StateMachine machine) : base(machine)
        {
            Type = Types.Falling;
            _velocity = Vector3.zero;
            HideWeapon = true;
            CanAim = false;
        }
        
        public Falling(StateMachine machine, Vector3 velocity) : base(machine)
        {
            Type = Types.Falling;
            _velocity = velocity;
            HideWeapon = true;
            CanAim = false;
        }
        
        public override void HandleInput()
        {
            base.HandleInput();
            
            Movement.Controller.Move((Movement.Transform.rotation *machine.movement.input * Movement.Stats.FallingControllability + _velocity) *
                                     Time.deltaTime);
            _velocity += Physics.gravity * Time.deltaTime;
        }

        public override void OnLanding()
        {
            base.OnLanding();
            machine.ChangeState(new Standing(machine));
        }
    }
}