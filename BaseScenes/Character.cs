using Godot;

namespace SleepyPrincess.BaseScenes
{
    public class Character : KinematicBody2D
    {
        [Export]
        public int Acceleration = 512;

        [Export]
        public float MaxSpeed = 64;

        [Export]
        public float Friction = 0.25f;

        public Sprite SpriteNode { get; private set; }

        public AnimationPlayer Animator { get; private set; }

        protected Vector2 MotionVector = Vector2.Zero;
        public CharacterStateMachine StateMachine { get; private set; }
        
        public CollisionShape2D CollisionShape;

        public override void _Ready()
        {
            base._Ready();
            SpriteNode = (Sprite) GetNode("Sprite");
            Animator = (AnimationPlayer) GetNode("Animator");
            StateMachine = (CharacterStateMachine) GetNode("StateMachine");
            CollisionShape = (CollisionShape2D) GetNode("CollisionShape");
        }

        public override void _PhysicsProcess(float delta)
        {
            base._PhysicsProcess(delta);
            ApplyMovement(); 
        }

        protected virtual void ApplyMovement()
        {
            
        }
        
        protected void EmitToState(string signal, params object[] args)
        {
            var currentState = StateMachine.CurrentState;
            var signalHandler = $"_on_{signal}";
            if (!IsConnected(signal, currentState, signalHandler))
            {
                Connect(signal, currentState, signalHandler);
            }
            EmitSignal(signal, args);
        }
    }
}
