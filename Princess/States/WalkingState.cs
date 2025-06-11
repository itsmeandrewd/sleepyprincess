using Godot;
using SleepyPrincess.BaseScenes;

namespace SleepyPrincess.Princess.States;

public partial class WalkingState : PrincessState
{
    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        base._Ready();
        PrincessScene.Animator.Play("Walking");
    }

    public override void _PhysicsProcess(double delta)
    {
        base._PhysicsProcess(delta);
        PrincessScene.MoveRight((float)delta);
        if (PrincessScene.CanDoubleJump() && Input.IsActionPressed("jump"))
        {
            PrincessScene.JumpCount += 1;
            PrincessScene.Jump((float)delta);
            Travel<JumpingState>();
        }
    }
}