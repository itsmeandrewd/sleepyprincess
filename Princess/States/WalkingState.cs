using Godot;
using SleepyPrincess.BaseScenes;

namespace SleepyPrincess.Princess.States
{
    public class WalkingState : PrincessState
    {
        // Called when the node enters the scene tree for the first time.
        public override void _Ready()
        {
            base._Ready();
            PrincessScene.Animator.Play("Walking");
            PrincessScene.CanDoubleJump = true;
        }

        public override void _PhysicsProcess(float delta)
        {
            base._PhysicsProcess(delta);
            PrincessScene.MoveRight(delta);
            if (Input.IsActionPressed("jump"))
            {
                PrincessScene.Jump(delta);
                Travel<JumpingState>();
            }
        }
    }
}
