using System;
using Godot;
using RandomDataGenerator.FieldOptions;
using RandomDataGenerator.Randomizers;

namespace hakim.scripts;

public abstract class PersonFactory
{

    public static Person CreateForTimePeriod(TimePeriods period)
    {
        var timePeriod = new ConcreteTimePeriod(period);
        var name = GenerateName(); // Generate name first
        return new ConcretePerson(timePeriod, name); // Pass name to constructor
    }

    private static string GenerateName()
    {
        var randomizerFirstName = RandomizerFactory.GetRandomizer(new FieldOptionsFirstName());
        var randomizerLastName = RandomizerFactory.GetRandomizer(new FieldOptionsLastName());
        var firstName = randomizerFirstName.Generate();
        var lastName = randomizerLastName.Generate();
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