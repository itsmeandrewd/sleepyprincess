using Godot;
using static System.Linq.Enumerable;

namespace SleepyPrincess.Platform
{
    public class Platform : Node2D
    {
        [Signal]
        public delegate void PlatformRemoved();
        
        private PackedScene _blockScene;
        private PackedScene _coffeeScene;

        [Export] public int BlockLength = 3;

        [Export] public bool RandomizeLength;

        [Export] public bool CanSpawnCoffee;
        
        private const int MinBlockLength = 3;
        private const int MaxBlockLength = 8;
        private const int PercentChanceCoffee = 5;
        private int _blocksRemaining;

        // Called when the node enters the scene tree for the first time.
        public override void _Ready()
        {
            _blockScene = (PackedScene) ResourceLoader.Load("res://Block/Block.tscn");
            _coffeeScene = (PackedScene) ResourceLoader.Load("res://Coffee/Coffee.tscn");
            GD.Randomize();
            
            if (RandomizeLength)
            {
                BlockLength = (int) GD.RandRange(MinBlockLength, MaxBlockLength);
            }
            _blocksRemaining = BlockLength;

            foreach (var index in GD.Range(0, BlockLength))
            {
                AddBlock(index);
            }
        }

        private void AddBlock(int index)
        {
            var blockInstance = (Block) _blockScene.Instance();
                            
            AddChild(blockInstance);
            blockInstance.Connect(nameof(Block.BlockRemoved), this, nameof(_on_BlockRemoved));
            blockInstance.Position = new Vector2(blockInstance.Position.x + (index * Block.Width), blockInstance.Position.y);
            
            if (index == 0)
            {
                blockInstance.SetBlockType(Block.BlockTypes.Left);
            } else if (index == BlockLength - 1)
            {
                blockInstance.SetBlockType(Block.BlockTypes.Right);
            }
            else
            {
                blockInstance.SetBlockType(Block.BlockTypes.Middle);
            }

            GD.Randomize();
            if (CanSpawnCoffee && GD.RandRange(0, 100) < PercentChanceCoffee)
            {
                var coffeeInstance = (Coffee) _coffeeScene.Instance();
                AddChild(coffeeInstance);
                coffeeInstance.Position = new Vector2(blockInstance.Position.x, blockInstance.Position.y - 10);
            }
        }

        public void _on_BlockRemoved()
        {
            _blocksRemaining -= 1;
            if (_blocksRemaining == 0)
            {
                EmitSignal(nameof(PlatformRemoved));
                QueueFree();
            }
        }
        
    }
}
