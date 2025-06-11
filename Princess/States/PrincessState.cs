using SleepyPrincess.BaseScenes;

namespace SleepyPrincess.Princess.States;

public partial class PrincessState : CharacterState
{
    protected Princess PrincessScene;

    public override void _Ready()
    {
        base._Ready();
        PrincessScene = (Princess) Parent;
    }
}