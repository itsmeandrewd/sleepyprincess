using Godot;

namespace SleepyPrincess.BaseScenes;

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
        Connect(SignalName.CharacterStateTravel, new Callable(GetParent(), "_on_CharacterState_Travel"));
        Connect(SignalName.CharacterStateTravelPrevious, new Callable(GetParent(), "_on_CharacterState_TravelPrevious"));
    }

    protected void Travel<T>() where T : CharacterState
    {
        TravelState(typeof(T).FullName);
    }

    private void TravelPrevious()
    {
        EmitSignal(SignalName.CharacterStateTravelPrevious);
        QueueFree();
    }

    private void TravelState(string newState)
    {
        EmitSignal(SignalName.CharacterStateTravel, newState);
        QueueFree();
    }

    public void SetParent(Character parentCharacter)
    {
        Parent = parentCharacter;
    }
}