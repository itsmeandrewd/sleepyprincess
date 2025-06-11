using Godot;

namespace SleepyPrincess.Princess.States
{
    public partial class FallingState : PrincessState
    {
        public override void _Ready()
        {
            base._Ready();
            PrincessScene.Animator.Play("Falling");
        }

        public override void _Process(double delta)
        {
            base._Process(delta);
            
            
            if (PrincessScene.IsOnFloor())
            {
                Travel<WalkingState>();
            }
        }

        public override void _PhysicsProcess(double delta)
        {
            base._PhysicsProcess(delta);
            PrincessScene.MoveRight((float)delta);
            if (PrincessScene.CanDoubleJump && Input.IsActionJustPressed("jump"))
            {
                PrincessScene.Jump((float)delta);
                PrincessScene.CanDoubleJump = false;
            }
        }
    }
}