using System;
using Godot;

namespace hakim.scripts;

public class GameState
{
    public static GameState Instance { get; private set; }

    public GameState() => Instance ??= this;

    private const int PeoplePerDay = 5;
    private const float MinInfectionRate = 0.33f;

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
    }

    public void ProcessDecision(Person person, bool approved)
    {
        if (person == null)
        {
            GD.PrintErr("Cannot process decision for null person");
            return;
        }

        RemainingPeople--;
        if (person.InfectedBy != Disease.None) InfectedCount++;

        var isInfected = person.InfectedBy != Disease.None;
        var mustApprove = MandateData.DailyMandates.TryGetValue(CurrentDay, out var mandate) &&
                         mandate != null &&
                         person.HasTraits(mandate.RequiredTraits);

        switch (approved)
        {
            case true when !isInfected:
                Score += 1;
                break;
            case true when isInfected:
            case false when mustApprove:
                Score -= 2;
                break;
        }
    }

    private bool CheckMandate(Person person)
    {
        return MandateData.DailyMandates.TryGetValue(CurrentDay, out var mandate) && person.HasTraits(mandate.RequiredTraits);
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
        RemainingPeople > 0 &&
        InfectedCount < Math.Ceiling(PeoplePerDay * MinInfectionRate);
}