using Player;
using Player.States;
using UnityEngine;

namespace Character.CharacterMovement.States
{
    public class Sliding : State
    {
        public Sliding(StateMachine machine) : base(machine)
        {
            Type = Types.Sliding;
        }

        public override void HandleInput()
        {
            base.HandleInput();

            var slopeForce = Movement.SlopeForce();
            Movement.Controller.SimpleMove(slopeForce +
                                           Movement.transform.rotation * Movement.input *
                                           Movement.Stats.FallingControllability);
        }

        public override void OnLanding()
        {
            base.OnLanding();
            machine.ChangeState(new Standing(machine));
        }
    }
}