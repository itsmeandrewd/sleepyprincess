using System;
using System.Collections.Generic;
using Godot;

namespace SleepyPrincess.BaseScenes
{
    public partial class CharacterStateMachine : Node
    {
        private Stack<Type> _stateHistory;
        public CharacterState CurrentState { get; private set; }

        public override void _Ready()
        {
            base._Ready();
            _stateHistory = new Stack<Type>();
        }

        public void PopFromHistory()
        {
            if (_stateHistory.Count > 0)
            {
                _stateHistory.Pop();
            }
        }

        public void SetState<T>() where T : CharacterState
        {
            SetState(typeof(T));
        }

        private void SetState(Type stateType)
        {
            if (_stateHistory.Count > 0 && stateType == _stateHistory.Peek())
            {
                RollbackState();
                return;
            }

            if (CurrentState != null)
            {
                _stateHistory.Push(CurrentState.GetType());
            }
            AddStateToScene(stateType);
        }

        private void RollbackState()
        {
            if (_stateHistory.Count == 0)
            {
                GD.Print("WARNING: tried to rollback state but not enough prior history!");
                return;
            }

            AddStateToScene(_stateHistory.Pop());
        }

        private void AddStateToScene(Type stateClass)
        {
            var newState = (CharacterState) Activator.CreateInstance(stateClass);
            newState.SetParent((Character) GetParent());
            AddChild(newState);
            CurrentState = newState;
        }

        public void _on_CharacterState_Travel(string newState)
        {
            SetState(Type.GetType(newState));
        }

        public void _on_CharacterState_TravelPrevious()
        {
            RollbackState();
        }

        private void PrintStackDebug()
        {
            GD.Print("State of _stackHistory:");
            Array.ForEach(_stateHistory.ToArray(), x => GD.Print(x.FullName));
        }
    }
}
