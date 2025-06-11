using Godot;

namespace SleepyPrincess.BaseScenes;

public partial class Character : CharacterBody2D
{
    [Export]
    public int Acceleration = 512;

    [Export]
    public float MaxSpeed = 64;

    [Export]
    public float Friction = 0.25f;

    protected Sprite2D SpriteNode { get; private set; }

    public AnimationPlayer Animator { get; private set; }

    protected Vector2 MotionVector = Vector2.Zero;
    protected CharacterStateMachine StateMachine { get; private set; }

    private CollisionShape2D CollisionShape3D;

    public override void _Ready()
    {
        base._Ready();
        SpriteNode = (Sprite2D) GetNode("Sprite2D");
        Animator = (AnimationPlayer) GetNode("Animator");
        StateMachine = (CharacterStateMachine) GetNode("StateMachine");
        CollisionShape3D = (CollisionShape2D) GetNode("CollisionShape3D");
    }

    public override void _PhysicsProcess(double delta)
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
        if (!IsConnected(signal, new Callable(currentState, signalHandler)))
        {
            Connect(signal, new Callable(currentState, signalHandler));
        }
        //EmitSignal(signal, args);
    }
}