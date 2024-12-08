using System;
using System.Collections.Generic;
using System.Linq;
using Godot;

namespace hakim.scripts;


public abstract class Person()
{
    private TimePeriodClass TimePeriod { get; set; }

    public required Guid Id { get; set; } = Guid.NewGuid();
    public required string Name { get; init; }

    private uint YearOfBirth { get; set; }

    private List<Traits> Traits { get; } = [];
    private List<Symptoms> Symptoms { get; } = [];

    internal Disease InfectedBy { get; private set; } = Disease.None;

    protected Person(TimePeriodClass timePeriod) : this()
    {
        TimePeriod = timePeriod;

        YearOfBirth = (uint)GD.RandRange(timePeriod.StartDate, timePeriod.EndDate);

        var allTraits = TimePeriodData.TimePeriodTraits[timePeriod.TimePeriodE];
        var allSymptoms = TimePeriodData.TimePeriodSymptoms[timePeriod.TimePeriodE];

        // Randomly add traits
        for (var i = 0; i < GD.RandRange(1, 5); i++)
        {
            var randomTrait = allTraits[GD.RandRange(0, allTraits.Count)];
            if (!Traits.Contains(randomTrait))
                Traits.Add(randomTrait);
        }

        for (var i = 0; i < GD.RandRange(0, 3); i++)
        {
            var randomSymptom = allSymptoms[GD.RandRange(0, allSymptoms.Count)];
            if (!Symptoms.Contains(randomSymptom))
                Symptoms.Add(randomSymptom);
        }

        Infect();
    }

    public bool HasTraits(Traits[] requiredTraits)
    {
        return requiredTraits.All(Traits.Contains);
    }

    public IEnumerable<string> GetTraits() => Traits.Select(t => t.ToString());
    public IEnumerable<string> GetSymptoms() => Symptoms.Select(s => s.ToString());

    private void Infect()
    {
        if (GD.Randf() >= 0.5f)
        {
            InfectedBy = Disease.None;
            return;
        }

        foreach (var (disease, info) in DiseaseData.Diseases)
        {
            if (disease == Disease.None) continue;

            if (!info.Conditions.TryGetValue(TimePeriod.TimePeriodE, out var condition))
                continue;

            if (condition.RequiredTraits.All(Traits.Contains) &&
                condition.RequiredSymptoms.All(Symptoms.Contains))
            {
                InfectedBy = disease;
                return;
            }
        }

        InfectedBy = Disease.None;
    }
}

