using System;
using Godot;

namespace hakim.scripts;

public class GameState
{
    public static GameState Instance { get; private set; }

    public GameState() => Instance ??= this;

    private const int PeoplePerDay = 5;
    private const float MinInfectionRate = 0.33f;
    private const int CorrectDecisionScore = 2;
    private const int WrongDecisionScore = -3;
    private const int MandateViolationScore = -4;
    private const float BaseInfectionRate = 0.3f; // 30% base infection rate
    private const int PEOPLE_PER_DAY = 5;
    private int _infectedCount = 0;

    public int CurrentDay { get; private set; } = 1;
    public int Score { get; private set; }
    public int RemainingPeople { get; private set; } = PeoplePerDay;
    public TimePeriods CurrentTimePeriod => (TimePeriods)(CurrentDay - 1);
    private int InfectedCount { get; set; }

    // Add reset method
    public void Reset()
    {
        CurrentDay = 1;
        Score = 0;
        RemainingPeople = PeoplePerDay;
        InfectedCount = 0;
        _infectedCount = 0;
    }

    public class DecisionResult
    {
        public bool IsCorrect { get; set; }
        public int ScoreChange { get; set; }
    }

    public (bool IsCorrect, int ScoreChange) ProcessDecision(Person person, bool approved)
    {
        var isHealthy = person.InfectedBy == Disease.None;
        var meetsMandate = IsPersonAllowedByMandate(person);

        int scoreChange;
        bool isCorrect;

        if (meetsMandate)
        {
            scoreChange = approved ? ScoreConstants.ApproveMandate : ScoreConstants.DenyMandate;
            isCorrect = approved;
        }
        else if (isHealthy)
        {
            scoreChange = approved ? ScoreConstants.ApproveHealthy : ScoreConstants.DenyHealthy;
            isCorrect = approved;
        }
        else
        {
            scoreChange = approved ? ScoreConstants.ApproveUnhealthy : ScoreConstants.DenyUnhealthy;
            isCorrect = !approved;
        }

        Score += scoreChange;
        RemainingPeople--;

        return (isCorrect, scoreChange);
    }

    public bool IsPersonAllowedByMandate(Person person)
    {
        return !MandateData.DailyMandates.TryGetValue(CurrentDay, out var mandate) || person.HasTraits(mandate.RequiredTraits);
    }

    public void AdvanceDay()
    {
        if (CurrentDay < 7)
        {
            CurrentDay++;
            RemainingPeople = PeoplePerDay;
            InfectedCount = 0;
        }
    }

    public bool ShouldForceInfection =>
        // Force infection if we haven't had any infected people today
        RemainingPeople == 1 && _infectedCount == 0;
}