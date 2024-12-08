using Godot;

namespace hakim.scripts;

public partial class StartScreen : Control
{
    public override void _Ready()
    {
        var startButton = GetNode<Button>("%StartButton");
        startButton.Pressed += OnStartButtonPressed;
    }

    private void OnStartButtonPressed()
    {
        GetTree().ChangeSceneToFile("res://scenes/main.tscn");
    }
}