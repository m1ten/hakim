using System.Collections.Generic;

namespace hakim.scripts;

public record MandateRequirement(
    TimePeriods TimePeriod,
    Traits[] RequiredTraits,
    string Description
);

public static class MandateData
{
    public static readonly Dictionary<int, MandateRequirement> DailyMandates = new()
    {
        { 1, new MandateRequirement(TimePeriods.Ancient,
            [Traits.Mystical, Traits.Ritualistic],
            "Allow all religious figures entry to preserve ancient wisdom")},

        { 2, new MandateRequirement(TimePeriods.Medieval,
            [Traits.Noble, Traits.Religious],
            "Grant entry to all nobles and clergy to maintain diplomatic relations")},
        
        { 3, new MandateRequirement(TimePeriods.Victorian,
            [Traits.UpperClass, Traits.Scientific],
            "Admit industrial innovators and aristocrats for technological advancement")},

        { 4, new MandateRequirement(TimePeriods.Raj,
            [Traits.Artistic, Traits.Intellectual],
            "Welcome artists and scholars to enrich Amara's culture")},

        { 5, new MandateRequirement(TimePeriods.WorldWar,
            [Traits.Military, Traits.Brave],
            "Provide sanctuary to war heroes and military personnel")},

        { 6, new MandateRequirement(TimePeriods.Seventies,
            [Traits.Activist, Traits.Progressive],
            "Accept social reformers and activists to promote equality")},

        { 7, new MandateRequirement(TimePeriods.Modern,
            [Traits.DigitalNative, Traits.Environmentalist],
            "Welcome tech experts and environmental advocates for sustainable future")}
    };
}