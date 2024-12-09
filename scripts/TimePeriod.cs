using System;
using System.Collections.Generic;

namespace hakim.scripts;

public enum TimePeriods
{
    Ancient,
    Medieval,
    Victorian,
    Raj,
    WorldWar,
    Seventies,
    Modern
}

public abstract class TimePeriodClass(TimePeriods timePeriod)
{
    internal TimePeriods TimePeriodE { get; } = timePeriod;
    private string Description =>
        TimePeriodE switch
        {
            TimePeriods.Ancient => "A time of ancient civilizations",
            TimePeriods.Medieval => "Europe in the Middle Ages",
            TimePeriods.Victorian => "Empire and industrial revolution",
            TimePeriods.Raj => "British rule in India",
            TimePeriods.WorldWar => "A great war",
            TimePeriods.Seventies => "The 1970s",
            TimePeriods.Modern => "The present day",
            _ => throw new ArgumentOutOfRangeException()
        };

    internal int StartDate =>
        TimePeriodE switch
        {
            TimePeriods.Ancient => -3000,
            TimePeriods.Medieval => 500,
            TimePeriods.Victorian => 1837,
            TimePeriods.Raj => 1858,
            TimePeriods.WorldWar => 1914,
            TimePeriods.Seventies => 1970,
            TimePeriods.Modern => 2000,
            _ => throw new ArgumentOutOfRangeException()
        };

    internal int EndDate =>
        TimePeriodE switch
        {
            TimePeriods.Ancient => 0,
            TimePeriods.Medieval => 1400,
            TimePeriods.Victorian => 1901,
            TimePeriods.Raj => 1947,
            TimePeriods.WorldWar => 1918,
            TimePeriods.Seventies => 1980,
            TimePeriods.Modern => 2024,
            _ => throw new ArgumentOutOfRangeException()
        };
}

public enum Traits
{
    // Ancient (focus on survival and traditional ways)
    Ritualistic,
    Wanderlust,
    Superstitious,
    FiercelyIndependent,
    Stoic,
    Traditional,
    Nomadic,
    Tribal,
    Mystical,

    // Medieval (emphasis on social hierarchy and religious beliefs)
    Pious,
    Fearful,
    Suspicious,
    Opportunistic,
    Noble,
    Peasant,
    Religious,
    Unhygienic,
    Scholarly,

    // Raj (colonial and cultural characteristics)
    Ambitious,
    Creative,
    Merchant,
    SocialClimber,
    Intellectual,
    Artistic,
    Scientific,
    Inventive,
    Diplomatic,

    // Victorian (industrial age characteristics)
    Reserved,
    Prideful,
    Industrial,
    Proper,
    WorkingClass,
    UpperClass,
    Colonial,
    Progressive,

    // World War (wartime characteristics)
    Brave,
    Patriotic,
    Paranoid,
    Traumatized,
    Military,
    Civilian,
    Resistant,
    Resourceful,
    Determined,

    // Seventies (counterculture and social change)
    FreeSpirited,
    Hedonistic,
    OpenMinded,
    Rebellious,
    Activist,
    Conservative,
    Experimental,
    Urban,

    // Modern (technology and contemporary issues)
    TechSavvy,
    Introverted,
    Multitasker,
    RemoteWorker,
    DigitalNative,
    Environmentalist,
    GlobalCitizen,
    Anxious,
    HealthConscious
}

public enum Symptoms
{
    // Ancient (basic health issues)
    Fever,
    Fatigue,
    StomachAilments,
    Dehydration,
    Malnutrition,
    SkinLesions,
    JointPain,
    Weakness,
    Paralysis,

    // Medieval (plague-era symptoms)
    BlackSpots,
    Swelling,
    HighFever,
    Delirium,
    Coughing,
    OpenSores,
    Vomiting,
    Chills,
    LymphNodes,

    // Raj (colonial health issues)
    ChronicPain,
    DigestiveIssues,
    FeverAndChills,
    Rashes,
    WeakenedImmunity,
    RespiratoryProblems,
    NauseaVomiting,
    Headaches,
    Dizziness,

    // Victorian (industrial disease symptoms)
    PersistentCough,
    Exhaustion,
    NervePain,
    LeadPoisoning,
    IndustrialLungDisease,
    ChronicBronchitis,
    MalnutritionVictorian,
    PoorHygiene,
    Infection,

    // World War (war-related health issues)
    ShellShock,
    RespiratoryIssues,
    DigestiveIssuesWw,
    Trauma,
    ChemicalBurns,
    Pneumonia,
    Influenza,
    MalnutritionWw,
    MentalDistress,

    // Seventies (modern disease emergence)
    ChronicFatigue,
    ImmuneDeficiency,
    WeightLoss,
    NightSweats,
    PersistentFever,
    LymphaticIssues,
    Lethargy,
    OpportunisticInfections,
    NeurologicalSymptoms,

    // Modern (contemporary health issues)
    ScreenFatigue,
    InsomniaModern,
    AnxietySymptoms,
    RespiratoryDistress,
    LossOfTaste,
    LossOfSmell,
    ChronicStress,
    PostViralFatigue,
    SocialIsolation
}

public static class TimePeriodData
{
    public static readonly Dictionary<TimePeriods, List<Traits>> TimePeriodTraits = new()
    {
        { TimePeriods.Ancient, [
                Traits.Ritualistic, Traits.Wanderlust, Traits.Superstitious, Traits.FiercelyIndependent,
                Traits.Stoic, Traits.Traditional, Traits.Nomadic, Traits.Tribal, Traits.Mystical
            ]
        },
        { TimePeriods.Medieval, [
                Traits.Pious, Traits.Fearful, Traits.Suspicious, Traits.Opportunistic,
                Traits.Noble, Traits.Peasant, Traits.Religious, Traits.Unhygienic, Traits.Scholarly
            ]
        },
        { TimePeriods.Raj, [
                Traits.Ambitious, Traits.Creative, Traits.Merchant, Traits.SocialClimber,
                Traits.Intellectual, Traits.Artistic, Traits.Scientific, Traits.Inventive, Traits.Diplomatic
            ]
        },
        { TimePeriods.Victorian, [
                Traits.Reserved, Traits.Prideful, Traits.Industrial, Traits.Proper,
                Traits.Scientific, Traits.WorkingClass, Traits.UpperClass, Traits.Colonial, Traits.Progressive
            ]
        },
        { TimePeriods.WorldWar, [
                Traits.Brave, Traits.Patriotic, Traits.Paranoid, Traits.Traumatized,
                Traits.Military, Traits.Civilian, Traits.Resistant, Traits.Resourceful, Traits.Determined
            ]
        },
        { TimePeriods.Seventies, [
                Traits.FreeSpirited, Traits.Hedonistic, Traits.OpenMinded, Traits.Rebellious,
                Traits.Activist, Traits.Conservative, Traits.Progressive, Traits.Experimental, Traits.Urban
            ]
        },
        { TimePeriods.Modern, [
                Traits.TechSavvy, Traits.Introverted, Traits.Multitasker, Traits.RemoteWorker,
                Traits.DigitalNative, Traits.Environmentalist, Traits.GlobalCitizen, Traits.Anxious,
                Traits.HealthConscious
            ]
        }
    };

    public static readonly Dictionary<TimePeriods, List<Symptoms>> TimePeriodSymptoms = new()
    {
        { TimePeriods.Ancient, [
                Symptoms.Fever, Symptoms.Fatigue, Symptoms.StomachAilments, Symptoms.Dehydration,
                Symptoms.Malnutrition, Symptoms.SkinLesions, Symptoms.JointPain, Symptoms.Weakness, Symptoms.Paralysis
            ]
        },
        { TimePeriods.Medieval, [
                Symptoms.BlackSpots, Symptoms.Swelling, Symptoms.HighFever, Symptoms.Delirium,
                Symptoms.Coughing, Symptoms.OpenSores, Symptoms.Vomiting, Symptoms.Chills, Symptoms.LymphNodes
            ]
        },
        { TimePeriods.Victorian, [
                Symptoms.PersistentCough, Symptoms.Exhaustion, Symptoms.NervePain, Symptoms.LeadPoisoning,
                Symptoms.IndustrialLungDisease, Symptoms.ChronicBronchitis, Symptoms.MalnutritionVictorian,
                Symptoms.PoorHygiene, Symptoms.Infection
            ]
        },
        { TimePeriods.Raj, [
                Symptoms.ChronicPain, Symptoms.DigestiveIssues, Symptoms.FeverAndChills, Symptoms.Rashes,
                Symptoms.WeakenedImmunity, Symptoms.RespiratoryProblems, Symptoms.NauseaVomiting, Symptoms.Headaches,
                Symptoms.Dizziness
            ]
        },
        { TimePeriods.WorldWar, [
                Symptoms.ShellShock, Symptoms.RespiratoryIssues, Symptoms.DigestiveIssuesWw, Symptoms.Trauma,
                Symptoms.ChemicalBurns, Symptoms.Pneumonia, Symptoms.Influenza, Symptoms.MalnutritionWw,
                Symptoms.MentalDistress
            ]
        },
        { TimePeriods.Seventies, [
                Symptoms.ChronicFatigue, Symptoms.ImmuneDeficiency, Symptoms.WeightLoss, Symptoms.NightSweats,
                Symptoms.PersistentFever, Symptoms.LymphaticIssues, Symptoms.Lethargy, Symptoms.OpportunisticInfections,
                Symptoms.NeurologicalSymptoms
            ]
        },
        { TimePeriods.Modern, [
                Symptoms.ScreenFatigue, Symptoms.InsomniaModern, Symptoms.AnxietySymptoms, Symptoms.RespiratoryDistress,
                Symptoms.LossOfTaste, Symptoms.LossOfSmell, Symptoms.ChronicStress, Symptoms.PostViralFatigue,
                Symptoms.SocialIsolation
            ]
        }
    };
}
