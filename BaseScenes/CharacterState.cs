using Godot;

namespace SleepyPrincess.BaseScenes
{
    public class CharacterState : Node
    {
        [Signal]
        public delegate void CharacterStateTravel(string newState);

        [Signal]
        public delegate void CharacterStateTravelPrevious();

        protected Character Parent;

        public override void _Ready()
        {
            base._Ready();
            Connect(
                nameof(CharacterStateTravel),
                GetParent(),
                nameof(CharacterStateMachine._on_CharacterState_Travel)
            );
            Connect(
                nameof(CharacterStateTravelPrevious),
                GetParent(),
                nameof(CharacterStateMachine._on_CharacterState_TravelPrevious)
            );
        }

        protected void Travel<T>() where T : CharacterState
        {
            TravelState(typeof(T).FullName);
        }

        protected void TravelPrevious()
        {
            EmitSignal(nameof(CharacterStateTravelPrevious));
            QueueFree();
        }

        private void TravelState(string newState)
        {
            EmitSignal(nameof(CharacterStateTravel), newState);
            QueueFree();
        }

        public void SetParent(Character parentCharacter)
        {
            Parent = parentCharacter;
        }
    }
}
