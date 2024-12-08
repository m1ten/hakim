using System;
using Godot;

namespace hakim.scripts;

public partial class GameScene : Node
{
    private readonly GameState _gameState = new();
    private Person _currentPerson;
    private Label _dayLabel;
    private Label _scoreLabel;
    private Label _mandateLabel;
    private VBoxContainer _personInfo;
    private Button _approveButton;
    private Button _denyButton;

    public override void _Ready()
    {
        _dayLabel = GetNode<Label>("%DayLabel");
        _scoreLabel = GetNode<Label>("%ScoreLabel");
        _mandateLabel = GetNode<Label>("%MandateLabel");
        _personInfo = GetNode<VBoxContainer>("%PersonInfo");
        _approveButton = GetNode<Button>("%ApproveButton");
        _denyButton = GetNode<Button>("%DenyButton");

        _approveButton.Pressed += () => ProcessDecision(true);
        _denyButton.Pressed += () => ProcessDecision(false);

        UpdateUi();
        SpawnNewPerson();
    }

    private void ProcessDecision(bool approved)
    {
        if (_currentPerson == null)
        {
            GD.PrintErr("No current person to process decision for");
            return;
        }

        _gameState.ProcessDecision(_currentPerson, approved);

        if (_gameState.CurrentDay < 7)
        {
            _gameState.AdvanceDay();
            UpdateUi();
            SpawnNewPerson();
        }
        else
        {
            ShowGameOver();
        }
    }

    private void UpdateUi()
    {
        _dayLabel.Text = $"Day {_gameState.CurrentDay}: {_gameState.CurrentTimePeriod}";
        _scoreLabel.Text = $"Score: {_gameState.Score}";

        var mandate = MandateData.DailyMandates[_gameState.CurrentDay];
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

        if (nameLabel != null) nameLabel.Text = _currentPerson.Name;
        if (traitsLabel != null) traitsLabel.Text = $"Traits: {string.Join(", ", _currentPerson.GetTraits())}";
        if (symptomsLabel != null) symptomsLabel.Text = $"Symptoms: {string.Join(", ", _currentPerson.GetSymptoms())}";
    }

    private void ShowGameOver()
    {
        // TODO: Implement game over screen
    }
}