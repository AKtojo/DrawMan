using System.Collections.Generic;
using UnityEngine;

namespace DrawMan.AI
{
    public abstract class State<T> : ScriptableObject
    {
        [SerializeField] [HideInInspector] protected int id = 0;

        public int ID => id;

        public abstract void Initialize(T owner);
        public abstract void Enter(FiniteStateMachine<T> fsm, T container);
        public abstract void Exit(FiniteStateMachine<T> fsm, T container);
        public abstract void Execute(FiniteStateMachine<T> fsm, T container);
    }

    public class FiniteStateMachine<T>
    {
        private T container;

        public Dictionary<int, State<T>> States;
        public State<T> CurrentState { get { return States[_currentIndex]; } }
        private int _currentIndex;

        public FiniteStateMachine(T container, State<T>[] instates, int firstState)
        {
            this.container = container;
            States = new Dictionary<int, State<T>>();

            foreach (var state in instates)
            {
                States.Add(state.ID, state);
                States[state.ID].Initialize(this.container);
            }

            _currentIndex = firstState;
            CurrentState.Enter(this, this.container);
        }

        public void ChangeState(int newStateIndex)
        {
            //CurrentState.Exit();
            //CurrentIndex = newState;
            //CurrentState = States[newState];
            //CurrentState.Enter();
            States[_currentIndex].Exit(this, this.container);
            _currentIndex = newStateIndex;
            States[_currentIndex].Enter(this, this.container);
        }
    }
}
