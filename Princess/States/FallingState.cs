using Godot;

namespace SleepyPrincess.Princess.States
{
    public class FallingState : PrincessState
    {
        public override void _Ready()
        {
            base._Ready();
            PrincessScene.Animator.Play("Falling");
        }

        public override void _Process(float delta)
        {
            base._Process(delta);
            
            
            if (PrincessScene.IsOnFloor())
            {
                Travel<WalkingState>();
            }
        }

        public override void _PhysicsProcess(float delta)
        {
            base._PhysicsProcess(delta);
            PrincessScene.MoveRight(delta);
            if (PrincessScene.CanDoubleJump && Input.IsActionJustPressed("jump"))
            {
                PrincessScene.Jump(delta);
                PrincessScene.CanDoubleJump = false;
            }
        }
    }
}