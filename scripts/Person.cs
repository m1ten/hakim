using System;
using System.Collections.Generic;
using System.Linq;
using Godot;

namespace hakim.scripts;

public abstract class Person
{
    private TimePeriodClass TimePeriod { get; set; }
    public Guid Id { get; } = Guid.NewGuid(); // Remove required and init, set directly
    public string Name { get; protected set; } // Make settable by derived classes

    private uint YearOfBirth { get; set; }

    private List<Traits> Traits { get; } = [];
    private List<Symptoms> Symptoms { get; } = [];

    internal Disease InfectedBy { get; private set; } = Disease.None;

    protected Person(TimePeriodClass timePeriod)
    {
        ArgumentNullException.ThrowIfNull(timePeriod);

        TimePeriod = timePeriod;

        // Remove ID and Name initialization from here since they're handled elsewhere

        YearOfBirth = (uint)GD.RandRange(timePeriod.StartDate, timePeriod.EndDate);

        var allTraits = TimePeriodData.TimePeriodTraits[timePeriod.TimePeriodE];
        var allSymptoms = TimePeriodData.TimePeriodSymptoms[timePeriod.TimePeriodE];

        // Randomly add 1-3 traits
        var traitCount = GD.RandRange(1, Math.Min(3, allTraits.Count));
        while (Traits.Count < traitCount)
        {
            var randomIndex = (int)GD.RandRange(0, allTraits.Count - 1);
            var randomTrait = allTraits[randomIndex];
            if (!Traits.Contains(randomTrait))
                Traits.Add(randomTrait);
        }

        // Randomly add 0-2 symptoms
        var symptomCount = GD.RandRange(0, Math.Min(2, allSymptoms.Count));
        while (Symptoms.Count < symptomCount)
        {
            var randomIndex = (int)GD.RandRange(0, allSymptoms.Count - 1);
            var randomSymptom = allSymptoms[randomIndex];
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
        // Force infection if we need to meet the minimum rate
        var forceInfection = GameState.Instance.ShouldForceInfection;

        if (!forceInfection && GD.Randf() >= 0.5f)
        {
            InfectedBy = Disease.None;
            return;
        }

        // Try to infect with a disease from the current time period
        foreach (var (disease, info) in DiseaseData.Diseases)
        {
            if (disease == Disease.None) continue;

            if (!info.Conditions.TryGetValue(TimePeriod.TimePeriodE, out var condition))
                continue;

            // If forcing infection, only check symptoms
            if (forceInfection && condition.RequiredSymptoms.All(Symptoms.Contains) ||
                condition.RequiredTraits.All(Traits.Contains) && condition.RequiredSymptoms.All(Symptoms.Contains))
            {
                InfectedBy = disease;
                return;
            }
        }

        // If we need to force infection, add the trait and symptom required for the first disease in the current time period
        if (forceInfection)
        {
            var firstDisease = DiseaseData.Diseases.FirstOrDefault(d => d.Value.Conditions.ContainsKey(TimePeriod.TimePeriodE));
            
            if (firstDisease.Value != null)
            {
                var firstCondition = firstDisease.Value.Conditions[TimePeriod.TimePeriodE];
                Traits.AddRange(firstCondition.RequiredTraits);
                Symptoms.AddRange(firstCondition.RequiredSymptoms);
                InfectedBy = firstDisease.Key;
            }
        }

        InfectedBy = Disease.None;
    }
}

