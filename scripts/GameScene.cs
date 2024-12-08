using System;
using Godot;

namespace hakim.scripts;

public partial class GameScene : Node
{
    // Change the property to a field and initialize it
    private readonly GameState _gameState = new();
    private Person _currentPerson;
    private Label _dayLabel;
    private Label _scoreLabel;
    private Label _mandateLabel;
    private VBoxContainer _personInfo;
    private Button _approveButton;
    private Button _denyButton;
    private Control _gameOverScreen;
    private Label _finalScoreLabel;
    private Button _restartButton;
    private Control _mainUI; // Add reference to main UI container
    private Label _tutorialLabel;
    private Timer _tipTimer;
    private int _currentTipIndex;
    private Label _tipLabel;
    private Label _feedbackLabel;  // Add new field
    private Tween _feedbackTween; // Add this field with other fields

    public override void _Ready()
    {
        // Ensure GameState is created before anything else
        if (_gameState == null)
        {
            GD.PrintErr("Failed to initialize GameState");
            return;
        }

        _dayLabel = GetNode<Label>("%DayLabel");
        _scoreLabel = GetNode<Label>("%ScoreLabel");
        _mandateLabel = GetNode<Label>("%MandateLabel");
        _personInfo = GetNode<VBoxContainer>("%PersonInfo");
        _approveButton = GetNode<Button>("%ApproveButton");
        _denyButton = GetNode<Button>("%DenyButton");

        _approveButton.Pressed += () => ProcessDecision(true);
        _denyButton.Pressed += () => ProcessDecision(false);

        _gameOverScreen = GetNode<Control>("%GameOverScreen");
        _finalScoreLabel = GetNode<Label>("%FinalScoreLabel");
        _restartButton = GetNode<Button>("%RestartButton");

        _mainUI = GetNode<Control>("UI");

        _tutorialLabel = GetNode<Label>("%TutorialLabel");
        _tipLabel = GetNode<Label>("%TipLabel");
        _tipTimer = GetNode<Timer>("%TipTimer");

        _tipTimer.Timeout += ShowNextTip;
        _tipTimer.Start();

        _feedbackLabel = GetNode<Label>("%FeedbackLabel");
        _feedbackLabel.Hide();

        ShowDayTutorial();

        _restartButton.Pressed += RestartGame;
        _gameOverScreen.Hide();

        UpdateUi();
        SpawnNewPerson();
    }

    private void ProcessDecision(bool approved)
    {
        if (_currentPerson == null) return;

        var result = _gameState.ProcessDecision(_currentPerson, approved);

        // Show feedback for wrong decisions
        if (!result.IsCorrect)
        {
            var reason = approved switch
            {
                true when _currentPerson.InfectedBy != Disease.None =>
                    $"Wrong! You let in someone infected with {_currentPerson.InfectedBy}",
                false when _currentPerson.InfectedBy == Disease.None => "Wrong! You denied entry to a healthy person",
                true when !_gameState.IsPersonAllowedByMandate(_currentPerson) =>
                    "Wrong! This person did not meet the Wazir's mandate requirements",
                _ => "Wrong! You made an incorrect decision"
            };

            // Kill existing tween if any
            _feedbackTween?.Kill();
            
            // Reset feedback label
            _feedbackLabel.Show();
            _feedbackLabel.Text = reason;
            _feedbackLabel.Modulate = new Color(1, 0.3f, 0.3f, 1);
            
            // Create new tween
            _feedbackTween = CreateTween();
            _feedbackTween
                .TweenProperty(_feedbackLabel, "modulate:a", 0.0f, 3.0f)
                .SetTrans(Tween.TransitionType.Linear);
            _feedbackTween.Finished += () => {
                _feedbackLabel.Hide();
                _feedbackTween = null;
            };
        }

        if (_gameState.RemainingPeople > 0)
        {
            SpawnNewPerson();
            UpdateUi();
        }
        else if (_gameState.CurrentDay < 7)
        {
            _gameState.AdvanceDay();
            SpawnNewPerson();
            UpdateUi();
        }
        else
        {
            ShowGameOver();
        }

        // Changed condition to show tutorial for all days including day 7
        if (_gameState.CurrentDay <= 7)
        {
            ShowDayTutorial();
        }
    }

    private void UpdateUi()
    {
        if (_gameState == null)
        {
            return;
        }

        if (_dayLabel != null)
            _dayLabel.Text = $"Day {_gameState.CurrentDay}: {_gameState.CurrentTimePeriod} ({_gameState.RemainingPeople} remaining)";

        if (_scoreLabel != null)
            _scoreLabel.Text = $"Score: {_gameState.Score}";

        if (_mandateLabel != null && MandateData.DailyMandates.TryGetValue(_gameState.CurrentDay, out var mandate))
            _mandateLabel.Text = mandate.Description;
    }

    private void SpawnNewPerson()
    {
        try
        {
            _currentPerson = PersonFactory.CreateForTimePeriod(_gameState.CurrentTimePeriod);
            if (_currentPerson == null)
            {
                GD.PrintErr("Failed to create person");
                return;
            }
            UpdatePersonInfo();
        }
        catch (Exception e)
        {
            GD.PrintErr($"Error spawning person: {e.Message}");
        }
    }

    private void UpdatePersonInfo()
    {
        if (_currentPerson == null) return;

        var nameLabel = _personInfo?.GetNode<Label>("%NameLabel");
        var traitsLabel = _personInfo?.GetNode<Label>("%TraitsLabel");
        var symptomsLabel = _personInfo?.GetNode<Label>("%SymptomsLabel");
        var backstoryLabel = _personInfo?.GetNode<Label>("%BackstoryLabel"); // Add this line

        if (nameLabel != null) nameLabel.Text = _currentPerson.Name;
        if (traitsLabel != null) traitsLabel.Text = $"Traits: {string.Join(", ", _currentPerson.GetTraits())}";
        if (symptomsLabel != null) symptomsLabel.Text = $"Symptoms: {string.Join(", ", _currentPerson.GetSymptoms())}";
        if (backstoryLabel != null) backstoryLabel.Text = $"Backstory: {_currentPerson.Backstory}"; // Add this line
    }

    private void ShowGameOver()
    {
        _finalScoreLabel.Text = $"Final Score: {_gameState.Score}";
        _gameOverScreen.Show();
        _mainUI.Hide(); // Hide the main UI
        _approveButton.Disabled = true;
        _denyButton.Disabled = true;
    }

    private void RestartGame()
    {
        _gameState.Reset();
        _gameOverScreen.Hide();
        _mainUI.Show();
        _approveButton.Disabled = false;
        _denyButton.Disabled = false;
        
        // Reset game state and UI
        UpdateUi();
        ShowDayTutorial(); // Add this line to reset tutorial message
        SpawnNewPerson();
    }

    private void ShowDayTutorial()
    {
        if (_tutorialLabel != null)
        {
            var message = TutorialData.DayMessages[_gameState.CurrentDay - 1];
            _tutorialLabel.Text = message;
            _tutorialLabel.Modulate = new Color(1, 0.9f, 0.7f, 1); // Set color but don't fade
        }
    }

    private void ShowNextTip()
    {
        if (_tipLabel != null)
        {
            _currentTipIndex = (_currentTipIndex + 1) % TutorialData.GameplayTips.Length;
            _tipLabel.Text = TutorialData.GameplayTips[_currentTipIndex];

            // Fade only the tip
            _tipLabel.Modulate = Colors.White;
            CreateTween()
                .TweenProperty(_tipLabel, "modulate:a", 0.5f, 1.0f)
                .SetTrans(Tween.TransitionType.Linear);
        }
    }
}