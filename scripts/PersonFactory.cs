using System;
using Godot;

namespace hakim.scripts;

public abstract class PersonFactory
{
    private static readonly string[] FirstNames =
    [
        "Ahmed", "Sofia", "Marcus", "Isabella", "Zhang", "Priya", "James",
        "Elena", "Mohammed", "Aisha", "Viktor", "Maria", "Kai", "Yuki"
    ];

    private static readonly string[] LastNames =
    [
        "Al-Rashid", "Silva", "von Weber", "Chang", "Jonas", "Smith",
        "Ivanov", "García", "Kim", "Müller", "Sato", "Singh"
    ];

    public static Person CreateForTimePeriod(TimePeriods period)
    {
        var timePeriod = new ConcreteTimePeriod(period);
        var name = GenerateName(); // Generate name first
        return new ConcretePerson(timePeriod, name); // Pass name to constructor
    }

    private static string GenerateName()
    {
        var firstName = FirstNames[GD.RandRange(0, FirstNames.Length)];
        var lastName = LastNames[GD.RandRange(0, LastNames.Length)];
        return $"{firstName} {lastName}";
    }
}

// Concrete implementations needed for the abstract classes
public class ConcreteTimePeriod(TimePeriods period) : TimePeriodClass(period);

public class ConcretePerson : Person
{
    public ConcretePerson(TimePeriodClass timePeriod, string name) : base(timePeriod)
    {
        Name = name; // Set name here
    }
}