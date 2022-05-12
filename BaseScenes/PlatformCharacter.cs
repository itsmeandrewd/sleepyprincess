using System;
using Godot;

namespace SleepyPrincess.BaseScenes
{
    public class PlatformCharacter : Character
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

        public override void _PhysicsProcess(float delta)
        {
            ApplyGravity(delta);
            IsFalling = !IsOnFloor() && Position.y > _lastYPos;
            _lastYPos = Position.y;
            base._PhysicsProcess(delta);
        }

        protected override void ApplyMovement()
        {
            MotionVector = MoveAndSlide(MotionVector, Vector2.Up);
        }

        protected void ApplyGravity(float delta)
        {
            MotionVector.y += Gravity * delta;
            MotionVector.y = Mathf.Min(MotionVector.y, JumpPower);
        }

        public void HorizontalMove(float direction, float delta)
        {
            MotionVector.x += direction * Acceleration * delta;
            MotionVector.x = Mathf.Clamp(MotionVector.x, -MaxSpeed, MaxSpeed);

            Direction newFacing = GetDirectionFromFloat(direction);
            if (newFacing != Facing)
            {
                Flip();
            }

            Facing = newFacing;
        }

        protected static Direction GetDirectionFromFloat(float direction)
        {
            return direction > 0 ? Direction.Right : Direction.Left;
        }

        public virtual void Jump(float delta)
        {
            var yForce = JumpPower * delta * 100;
            MotionVector.y -= yForce;
            MotionVector.y = Mathf.Max(MotionVector.y, -yForce);
        }

        public void StopMoving()
        {
            MotionVector.x = Mathf.Lerp(MotionVector.x, 0, Friction);
        }

        public virtual void Flip()
        {
            SpriteNode.FlipH = !SpriteNode.FlipH;
            switch (Facing)
            {
                case Direction.Left:
                    Facing = Direction.Right;
                    break;
                case Direction.Right:
                    Facing = Direction.Left;
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }
    }
}
