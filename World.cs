using System;
using Godot;
using SleepyPrincess.Platform;

namespace SleepyPrincess
{
    public class World : Node2D
    {
        private Princess.Princess _princess;
        private PlatformManager _platformManager;
        private AudioStreamPlayer _deathKyah;
        private AudioStreamPlayer _bgMusic;
        private Camera2D _deathCamera;
        private GUI _gui;
        private Timer _scoreTimer;

        private float _princessBaseSpeed;
        private File _scoreFile;
        private float _lastPrincessX;        

        private const string ScoreFilePath = "user://score.sav";

        // Called when the node enters the scene tree for the first time.
        public override void _Ready()
        {
            _princess = (Princess.Princess) GetNode("Princess");
            _princessBaseSpeed = _princess.MaxSpeed;
            _platformManager = (PlatformManager) GetNode("PlatformManager");
            _deathKyah = (AudioStreamPlayer) GetNode("DeathKyah");
            _bgMusic = (AudioStreamPlayer) GetNode("BGMusic");
            _deathCamera = (Camera2D) GetNode("DeathCamera");
            _scoreTimer = (Timer) GetNode("ScoreTimer");
            
            _gui = (GUI) GetNode("CanvasLayer/GUI");
            _scoreFile = new File();
            _lastPrincessX = _princess.Position.x;
            ResetGame();
            LoadHighScore();

            var initialPlatformPosition = new Vector2(_princess.GlobalPosition.x, _princess.GlobalPosition.y + 32);
            _platformManager.SpawnPlatform(initialPlatformPosition, 30, false);
        }

        public void _on_Princess_Die (Vector2 position)
        {
            _scoreTimer.Stop();
            _gui.Enabled = false;
            _deathKyah.VolumeDb += 5;
            _bgMusic.VolumeDb -= 10;
            _deathKyah.Play();
            _deathCamera.GlobalPosition = position;
            _deathCamera.Current = true;
            SaveHighScore();
        }

        public void _on_SpeedTimer_timeout()
        {
            _princess.MaxSpeed -= 1.1f;
            _gui.AwakeProgress = _princess.MaxSpeed;
        }

        public void _on_Princess_DrankCoffee()
        {
            _gui.AwakeProgress =_princess.MaxSpeed;
        }

        public void _on_ScoreTimer_timeout()
        {
            if (!IsInstanceValid(_princess))
            {
                return;
            }
            
            GD.Print(_princess.Position.x - _lastPrincessX);
            _gui.Score += (int)(_princess.Position.x - _lastPrincessX) / 10;
            if (_gui.Score >= _gui.HiScore)
            {
                _gui.HiScore = _gui.Score;
            }

            _lastPrincessX = _princess.Position.x;
        }

        private void ResetGame()
        {
            _gui.SetMaxAwakeProgress(_princessBaseSpeed);
            _gui.Score = 0;
            _gui.AwakeProgress = 0;
        }

        private void LoadHighScore()
        {
            if (!_scoreFile.FileExists(ScoreFilePath))
            {
                _scoreFile.Open(ScoreFilePath, File.ModeFlags.Write);
                _scoreFile.Store32(0);
                _gui.HiScore = 0;
            }
            else
            {
                _scoreFile.Open(ScoreFilePath, File.ModeFlags.Read);
                _gui.HiScore = (int) _scoreFile.Get32();
            }
            _scoreFile.Close();
        }

        private void SaveHighScore()
        {
            _scoreFile.Open(ScoreFilePath, File.ModeFlags.Write);
            _scoreFile.Store32((uint) _gui.HiScore);
            _scoreFile.Close();
        }
    }
}
