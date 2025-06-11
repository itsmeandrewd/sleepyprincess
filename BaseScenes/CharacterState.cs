using Godot;

namespace SleepyPrincess.BaseScenes
{
    public partial class CharacterState : Node
    {
        [Signal]
        public delegate void CharacterStateTravelEventHandler(string newState);

        [Signal]
        public delegate void CharacterStateTravelPreviousEventHandler();

        protected Character Parent;

        public override void _Ready()
        {
            base._Ready();
            Connect(nameof(CharacterStateTravel), new Callable(GetParent(), "_on_CharacterState_Travel"));
            Connect(nameof(CharacterStateTravelPrevious), new Callable(GetParent(), "_on_CharacterState_TravelPrevious"));
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
