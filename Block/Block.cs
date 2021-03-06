using Godot;
using System;

public class Block : StaticBody2D
{
    [Signal]
    public delegate void BlockRemoved();
    
    
    private Sprite _sprite;
    private bool _wasOnScreen;

    public const int Width = 8;

    public enum BlockTypes
    {
        Left = 0,
        Middle,
        Right
    };

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        base._Ready();
        _sprite = (Sprite) GetNode("Sprite");
    }

    public void SetBlockType(BlockTypes blockType)
    {
        _sprite.Frame = (int) blockType;
    }

    public void _on_VisibilityNotifier2D_screen_entered()
    {
        _wasOnScreen = true;
    }

    public void _on_VisibilityNotifier2D_screen_exited()
    {
        if (_wasOnScreen)
        {
            EmitSignal(nameof(BlockRemoved));
            QueueFree();
        }
    }

}
