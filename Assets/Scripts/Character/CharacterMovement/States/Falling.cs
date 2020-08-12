namespace Player.States
{
    public class Falling : State
    {
        public Falling(StateMachine machine) : base(machine)
        {
            IsGroundedState = false;
            IsMovingState = true;
            Type = Types.Falling;
        }
        
        public override void HandleInput()
        {
            base.HandleInput();
            Movement.AddRelativeForce(Movement.input*Movement.Stats.FallingControllability);
        }

        public override void Enter()
        {
        }

        public override void Leave()
        {
        }

        public override void OnLanding()
        {
            base.OnLanding();
            machine.ChangeState(new Standing(machine));
        }
    }
}