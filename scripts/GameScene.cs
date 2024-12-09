using System;
using System.Linq;
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
    private Label _feedbackLabel;
    private Tween _feedbackTween;

    private Control _diseaseInfoScreen;
    private Label _diseaseDescriptionLabel;
    private Label _diseaseOriginLabel;
    private Label _diseaseFactsLabel;
    private Button _continueButton;

    // private AudioStreamPlayer _buttonSound;
    private AudioStreamPlayer _correctSound;
    private AudioStreamPlayer _wrongSound;
    private AudioStreamPlayer _dayEndSound;
    private AudioStreamPlayer _gameOverSound;

    // Add new field for tracking active sound
    private AudioStreamPlayer _currentSound;

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

        _diseaseInfoScreen = GetNode<Control>("%DiseaseInfoScreen");
        _diseaseDescriptionLabel = GetNode<Label>("%DiseaseDescription");
        _diseaseOriginLabel = GetNode<Label>("%DiseaseOrigin");
        _diseaseFactsLabel = GetNode<Label>("%DiseaseFacts");
        _continueButton = GetNode<Button>("%ContinueButton");

        _continueButton.Pressed += OnContinuePressed;

        // Show initial disease info before starting day 1
        ShowDiseaseInfo();
        _mainUI.Hide(); // Hide main UI until player continues

        _restartButton.Pressed += RestartGame;
        _gameOverScreen.Hide();

        // Initialize audio players
        // _buttonSound = new AudioStreamPlayer();
        _correctSound = new AudioStreamPlayer();
        _wrongSound = new AudioStreamPlayer();
        _dayEndSound = new AudioStreamPlayer();
        _gameOverSound = new AudioStreamPlayer();

        // AddChild(_buttonSound);
        AddChild(_correctSound);
        AddChild(_wrongSound);
        AddChild(_dayEndSound);
        AddChild(_gameOverSound);

        // Load audio streams
        // _buttonSound.Stream = GD.Load<AudioStream>("res://sounds/click.wav");
        _correctSound.Stream = GD.Load<AudioStream>("res://sounds/correct.wav");
        _wrongSound.Stream = GD.Load<AudioStream>("res://sounds/incorrect.wav");
        _dayEndSound.Stream = GD.Load<AudioStream>("res://sounds/day_end.wav");
        _gameOverSound.Stream = GD.Load<AudioStream>("res://sounds/game_over.wav");

        UpdateUi();
        SpawnNewPerson();
    }

    private void ShowDiseaseInfo()
    {
        var disease = DiseaseData.Diseases.FirstOrDefault(d =>
            d.Value.Conditions.ContainsKey(_gameState.CurrentTimePeriod)).Key;

        if (disease != Disease.None)
        {
            var info = DiseaseData.Diseases[disease];
            _diseaseDescriptionLabel.Text = info.Description;
            _diseaseOriginLabel.Text = $"Origin: {info.Origin}";
            _diseaseFactsLabel.Text = $"Quick Facts:\n• {string.Join("\n• ", info.QuickFacts)}\n\nSources:\n• {string.Join("\n• ", info.Sources)}";

            _diseaseInfoScreen.Show();
            _mainUI.Hide();
        }
        else
        {
            StartDay();
        }
    }

    private void OnContinuePressed()
    {
        // _buttonSound.Play();
        StartDay();
    }

    private void StartDay()
    {
        _diseaseInfoScreen.Hide();
        _mainUI.Show();
        ShowDayTutorial();
    }

    private void PlaySound(AudioStreamPlayer sound)
    {
        _currentSound?.Stop();

        _currentSound = sound;
        _currentSound.Play();
    }

    private void ProcessDecision(bool approved)
    {
        if (_currentPerson == null) return;

        var result = _gameState.ProcessDecision(_currentPerson, approved);

        // Use new PlaySound method
        if (result.IsCorrect)
        {
            PlaySound(_correctSound);
        }
        else
        {
            PlaySound(_wrongSound);
            // Show feedback for wrong decisions
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
            _feedbackTween.Finished += () =>
            {
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
            ShowDiseaseInfo(); // Show disease info before starting new day
        }
        else
        {
            ShowGameOver();
        }

        if (_gameState.CurrentDay <= 7)
        {
            ShowDayTutorial();
        }

        if (_gameState.RemainingPeople == 0 && _gameState.CurrentDay <= 7)
        {
            PlaySound(_dayEndSound);
        }
        else if (_gameState.CurrentDay > 7)
        {
            PlaySound(_gameOverSound);
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
        PlaySound(_gameOverSound);
        _finalScoreLabel.Text = $"Final Score: {_gameState.Score}";
        _gameOverScreen.Show();
        _mainUI.Hide(); // Hide the main UI
        _approveButton.Disabled = true;
        _denyButton.Disabled = true;
    }

    private void RestartGame()
    {
        // // _buttonSound.Play();
        // _gameState.Reset();
        // _gameOverScreen.Hide();

        // // Show disease info at start of new game
        // ShowDiseaseInfo();

        // UpdateUi();
        // SpawnNewPerson();

        // just exit the game
        GetTree().Quit();
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