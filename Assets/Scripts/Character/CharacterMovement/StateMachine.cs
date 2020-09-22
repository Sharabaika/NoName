using System;
using Character.CharacterMovement;
using Character.CharacterMovement.States;
using Player.States;
using UnityEditor;
using UnityEngine;
using Weapons;

namespace Player
{
    public class StateMachine
    {
        private State _currentState;
        public State State => _currentState;

        public PlayerMovement Movement { get; private set; }
        
        public Action<State> OnStateChange; 
        public void ChangeState(State state)
        {
            _currentState.Leave();
            _currentState = state;
            OnStateChange?.Invoke(state);
            _currentState.Enter();
            state.HandleInput();
        }

        public StateMachine(PlayerMovement movement)
        {
            Movement = movement;
            _currentState = new Falling(this, Vector3.zero);
            _currentState.Enter();
        }
    }
}