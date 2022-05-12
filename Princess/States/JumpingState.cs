using Godot;

namespace SleepyPrincess.Princess.States
{
    public class JumpingState : PrincessState
    {
        // Declare member variables here. Examples:
        // private int a = 2;
        // private string b = "text";

        // Called when the node enters the scene tree for the first time.
        public override void _Ready()
        {
            base._Ready();
            PrincessScene.Animator.Play("Jumping");
            PrincessScene.SoundEffectPlayer.Play();
        }

        public override void _Process(float delta)
        {
            base._Process(delta);

            if (PrincessScene.IsFalling)
            {
                Travel<FallingState>();
            } else if (PrincessScene.IsOnFloor())
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
