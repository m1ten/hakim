using Godot;

namespace hakim.scripts;

public class GameState
{
    public int CurrentDay { get; private set; } = 1;
    public int Score { get; private set; }
    public TimePeriods CurrentTimePeriod => (TimePeriods)(CurrentDay - 1);

    public void ProcessDecision(Person person, bool approved)
    {
        if (person == null)
        {
            GD.PrintErr("Cannot process decision for null person");
            return;
        }

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
        if (CurrentDay < 7) CurrentDay++;
    }
}