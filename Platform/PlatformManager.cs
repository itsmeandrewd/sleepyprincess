using System.Xml.XPath;
using Godot;

namespace SleepyPrincess.Platform
{
    public class PlatformManager : Node2D
    {
        private PackedScene _platformScene;
        private float _lastPlatformEdgeX;
        private float _lastPlatformY;
        private int _numPlatforms;
        
        private const float MinXSpacing = 7;
        private const float MaxXSpacing = 11;
        private const float MinYSpacing = 3;
        private const float MaxYSpacing = 7;
        private const int MaxPlatforms = 10;
        
        public override void _Ready()
        {
            _platformScene = (PackedScene) ResourceLoader.Load("res://Platform/Platform.tscn");
        }

        public void SpawnPlatform(Vector2 position, int length = 0, bool spawnCoffees = true)
        {
            var platformInstance = (Platform) _platformScene.Instance();
            platformInstance.BlockLength = length;
            platformInstance.CanSpawnCoffee = spawnCoffees;
            if (length < 1)
            {
                platformInstance.RandomizeLength = true;
            }

            AddChild(platformInstance);
            platformInstance.Connect(nameof(Platform.PlatformRemoved), this, nameof(_on_PlatformRemoved));
            platformInstance.GlobalPosition = position;
            _lastPlatformEdgeX = position.x + (Block.Width * length);
            _lastPlatformY = position.y;
            _numPlatforms += 1;
        }

        public void SpawnNext()
        {
            GD.Randomize();
            var xSpacing = (int) GD.RandRange(MinXSpacing, MaxXSpacing);
            var yDirection = GD.Randi() % 2 == 0 ? 1 : -1;
            var ySpacing = (int) GD.RandRange(MinYSpacing, MaxYSpacing);

            var nextXPos = _lastPlatformEdgeX + (xSpacing * Block.Width);
            var nextYPos = _lastPlatformY + (ySpacing * Block.Width * yDirection);
            if (nextYPos < 10 || nextYPos > 250)
            {
                nextYPos = _lastPlatformY + (ySpacing * Block.Width * yDirection * -1);
            }

            var nextPosition = new Vector2(nextXPos, nextYPos);
            SpawnPlatform(nextPosition);
        }

        public override void _Process(float delta)
        {
            base._Process(delta);
            if (_numPlatforms < MaxPlatforms)
            {
                SpawnNext();
            }
        }

        public void _on_PlatformRemoved()
        {
            _numPlatforms -= 1;
        }
    }
}
