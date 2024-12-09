using Godot;

namespace hakim.scripts;

public partial class StartScreen : Control
{
    private Label _introLabel;

    public override void _Ready()
    {
        var startButton = GetNode<Button>("%StartButton");
        startButton.Pressed += OnStartButtonPressed;

        _introLabel = GetNode<Label>("%IntroLabel");
        DisplayIntroduction();
    }

    private void OnStartButtonPressed()
    {
        GetTree().ChangeSceneToFile("res://scenes/main.tscn");
    }

    private void DisplayIntroduction()
    {
        const string introText = """
                                made by miten
                                
                                The year is 2157. Time travel has become a reality, but with it came an unexpected crisis. Historical diseases, long thought extinct, have found new pathways through time into our present.
                                
                                Amara, a city-state at the crossroads of time, stands as humanity's last defense against these temporal plagues. As their most skilled physician, you have been chosen as the new Hakim—the city's guardian of health.
                                
                                The previous Hakim vanished under mysterious circumstances, and now the burden falls upon you. Will you protect Amara from the ancient diseases that threaten its existence?
                                """;

        _introLabel.Text = introText;
    }
}