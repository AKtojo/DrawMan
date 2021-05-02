using DrawMan.Components;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DrawMan.Core
{
    enum EnemyBStates { Idle = 0, Battle = 1 }

    public abstract class State : ScriptableObject
    {
        public State Next;
        public int ID = -1;

        public abstract void Initialize(GameObject obj);
        public abstract void Enter();
        public abstract void Exit();
        public abstract void Execute();
    }

    public class FiniteStateMachine
    {
        private GameObject owner;

        public Dictionary<int, State> States;
        public State CurrentState { get { return States[_currentIndex]; } }
        private int _currentIndex;

        public FiniteStateMachine(GameObject obj, State[] instates, int firstState)
        {
            owner = obj;
            States = new Dictionary<int, State>();

            foreach (var state in instates)
            {
                States.Add(state.ID, state);
                States[state.ID].Initialize(obj);
            }

            _currentIndex = firstState;
            CurrentState.Enter();
        }

        public void ChangeState(int newStateIndex)
        {
            //CurrentState.Exit();
            //CurrentIndex = newState;
            //CurrentState = States[newState];
            //CurrentState.Enter();
            States[_currentIndex].Exit();
            _currentIndex = newStateIndex;
            States[_currentIndex].Enter();
        }
    }
}
