using Character.CharacterMovement.States;
using Player.States;
using UnityEditor;
using UnityEngine;

namespace Player
{
    public class StateMachine
    {
        public State State => _currentState;
        private State _currentState;
        public readonly PlayerMovement movement;

        public void ChangeState(State state)
        {
            _currentState.Leave();
            _currentState = state;
            _currentState.Enter();
            state.HandleInput();
        }

        public StateMachine(PlayerMovement movement)
        {
            this.movement = movement;
            _currentState = new Falling(this);
            _currentState.Enter();
        }
    }
}