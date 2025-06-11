using Godot;
using SleepyPrincess.BaseScenes;
using SleepyPrincess.Princess.States;

namespace SleepyPrincess.Princess
{
    public partial class Princess : PlatformCharacter
    {
        [Signal]
        public delegate void DieEventHandler(Vector2 position);

        [Signal]
        public delegate void DrankCoffeeEventHandler();
        
        public AudioStreamPlayer SoundEffectPlayer;
        private Camera2D _camera;

        public bool CanDoubleJump = true;

        // Called when the node enters the scene tree for the first time.
        public override void _Ready()
        {
            base._Ready();
            SoundEffectPlayer = (AudioStreamPlayer) GetNode("AudioStreamPlayer");
            StateMachine.SetState<WalkingState>();
            Facing = Direction.Right;
            _camera = (Camera2D) GetNode("Camera2D");
        }

        public void MoveRight(float delta)
        {
            HorizontalMove((float) Direction.Right, delta);
        }

        public void _on_VisibilityNotifier2D_screen_exited()
        {
            if (Position.Y > 100)
            {
                var lastCameraPosition = _camera.GlobalPosition;
                EmitSignal(nameof(Die), lastCameraPosition);
                QueueFree();
            }
        }

        public void _on_Area2D_area_entered(Area2D area)
        {
            var body = area.GetParent();
            if (body is Coffee)
            {
                body.QueueFree();
                MaxSpeed += 10;
                EmitSignal(nameof(DrankCoffee));
            }
        }

    }
}
