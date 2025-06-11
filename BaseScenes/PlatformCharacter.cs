using System;
using Godot;

namespace SleepyPrincess.BaseScenes;

public partial class PlatformCharacter : Character
{
    [Export]
    public float Gravity = 200f;

    [Export]
    public float JumpPower = 200f;

    private float _lastYPos;

    [Export]
    public Direction Facing = Direction.Left;

    public enum Direction
    {
        Left = -1,
        Right = 1
    } 

    public bool IsFalling { get; private set; }

    public override void _PhysicsProcess(double delta)
    {
        ApplyGravity((float)delta);
        IsFalling = !IsOnFloor() && Position.Y > _lastYPos;
        _lastYPos = Position.Y;
        base._PhysicsProcess(delta);
    }

    protected override void ApplyMovement()
    {
        Velocity = MotionVector;
        MoveAndSlide();
    }

    private void ApplyGravity(float delta)
    {
        MotionVector.Y += Gravity * delta;
        MotionVector.Y = Mathf.Min(MotionVector.Y, JumpPower);
    }

    protected void HorizontalMove(float direction, float delta)
    {
        MotionVector.X += direction * Acceleration * delta;
        MotionVector.X = Mathf.Clamp(MotionVector.X, -MaxSpeed, MaxSpeed);

        Direction newFacing = GetDirectionFromFloat(direction);
        if (newFacing != Facing)
        {
            Flip();
        }

        Facing = newFacing;
    }

    private static Direction GetDirectionFromFloat(float direction)
    {
        return direction > 0 ? Direction.Right : Direction.Left;
    }

    public virtual void Jump(float delta)
    {
        var yForce = JumpPower * delta * 100;
        MotionVector.Y -= yForce;
        MotionVector.Y = Mathf.Max(MotionVector.Y, -yForce);
    }

    private void StopMoving()
    {
        MotionVector.X = Mathf.Lerp(MotionVector.X, 0, Friction);
    }

    protected virtual void Flip()
    {
        SpriteNode.FlipH = !SpriteNode.FlipH;
        Facing = Facing switch
        {
            Direction.Left => Direction.Right,
            Direction.Right => Direction.Left,
            _ => throw new ArgumentOutOfRangeException()
        };
    }
}