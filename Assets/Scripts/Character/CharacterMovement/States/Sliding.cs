using Player;
using Player.States;
using UnityEngine;
using Weapons;

namespace Character.CharacterMovement.States
{
    public class Sliding : State
    {
        public Sliding(StateMachine machine) : base(machine)
        {
            Type = Types.Sliding;
            Restrictions = WeaponPositioningRestrictions.CantAim;
            CanReload = true;
        }

        public override void HandleInput()
        {
            base.HandleInput();

            var slopeForce = Movement.SlopeForce();
            Movement.Controller.SimpleMove(slopeForce +
                                           Movement.transform.rotation * Input.movementInput *
                                           Movement.Stats.FallingControllability);
        }

        public override void OnLanding()
        {
            base.OnLanding();
            machine.ChangeState(new Standing(machine));
        }
    }
}