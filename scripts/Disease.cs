using System.Collections.Generic;

namespace hakim.scripts;

public enum Disease
{
    None,
    Malaria,
    BubonicPlague,
    Cholera,
    Tuberculosis,
    SpanishFlu,
    HIV,
    Covid19
}

public record DiseaseCondition(Traits[] RequiredTraits, Symptoms[] RequiredSymptoms);

public static class DiseaseData
{
    public static readonly Dictionary<TimePeriods, Dictionary<Disease, DiseaseCondition>> TimePeriodDiseases = new()
    {
        {
            TimePeriods.Ancient, new Dictionary<Disease, DiseaseCondition>
            {
                { Disease.Malaria, new DiseaseCondition(
                    [Traits.Wanderlust, Traits.Nomadic],
                    [Symptoms.Fever, Symptoms.Weakness, Symptoms.Dehydration]
                )}
            }
        },
        {
            TimePeriods.Medieval, new Dictionary<Disease, DiseaseCondition>
            {
                { Disease.BubonicPlague, new DiseaseCondition(
                    [Traits.Fearful, Traits.Unhygienic],
                    [Symptoms.BlackSpots, Symptoms.Swelling, Symptoms.LymphNodes]
                )}
            }
        },
        {
            TimePeriods.Renaissance, new Dictionary<Disease, DiseaseCondition>
            {
                { Disease.Cholera, new DiseaseCondition(
                    [Traits.Merchant, Traits.SocialClimber],
                    [Symptoms.DigestiveIssues, Symptoms.NauseaVomiting, Symptoms.WeakenedImmunity]
                )}
            }
        },
        {
            TimePeriods.Victorian, new Dictionary<Disease, DiseaseCondition>
            {
                { Disease.Tuberculosis, new DiseaseCondition(
                    [Traits.Working_Class, Traits.Industrial],
                    [Symptoms.PersistentCough, Symptoms.ChronicBronchitis, Symptoms.IndustrialLungDisease]
                )}
            }
        },
        {
            TimePeriods.WorldWar, new Dictionary<Disease, DiseaseCondition>
            {
                { Disease.SpanishFlu, new DiseaseCondition(
                    [Traits.Military, Traits.Traumatized],
                    [Symptoms.Influenza, Symptoms.Pneumonia, Symptoms.RespiratoryIssues]
                )}
            }
        },
        {
            TimePeriods.Seventies, new Dictionary<Disease, DiseaseCondition>
            {
                { Disease.HIV, new DiseaseCondition(
                    [Traits.FreeSpirited, Traits.Experimental],
                    [Symptoms.ImmuneDeficiency, Symptoms.WeightLoss, Symptoms.OpportunisticInfections]
                )}
            }
        },
        {
            TimePeriods.Modern, new Dictionary<Disease, DiseaseCondition>
            {
                { Disease.Covid19, new DiseaseCondition(
                    [Traits.Remote_Worker, Traits.Digital_Native],
                    [Symptoms.LossOfTaste, Symptoms.LossOfSmell, Symptoms.RespiratoryDistress]
                )}
            }
        }
    };
}