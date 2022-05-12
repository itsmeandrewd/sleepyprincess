using Godot;
using System;

public class GUI : MarginContainer
{
    private TextureProgress _awakeBar;
    private Label _scoreText;
    private Label _hiScoreText;
        
    public bool Enabled = true;

    public int Score
    {
        get => int.Parse(_scoreText.Text);
        set
        {
            if (Enabled)
            {
                _scoreText.Text = value.ToString();
            }
        }
    }

    public int HiScore
    {
        get => int.Parse(_hiScoreText.Text);
        set
        {
            if (Enabled)
            {
                _hiScoreText.Text = value.ToString();
            }
        }
    }

    public double AwakeProgress
    {
        get => _awakeBar.Value;
        set
        {
            if (Enabled)
            {
                _awakeBar.Value = value;
            }
        } 
    }

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        _awakeBar = (TextureProgress) GetNode("Rows/AwakeRow/AwakeContainer/Bar");
        _scoreText = (Label) GetNode("Rows/ScoreRow/Text");
        _hiScoreText = (Label) GetNode("Rows/HiScoreRow/Text");
    }

    public void SetMaxAwakeProgress(double maxAwakeProgress)
    {
        _awakeBar.MaxValue = maxAwakeProgress;
    }
    

}
