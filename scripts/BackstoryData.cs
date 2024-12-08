using System.Collections.Generic;
using System.Linq;
using Godot;

namespace hakim.scripts;

public record BackstoryTemplate(string Story, Traits[] RequiredTraits, bool IsInfected);

public class TimePeriodBackstory(List<BackstoryTemplate> backstories, List<string> infectionHints)
{
    public string GetBackstory(Traits[] personTraits, bool isInfected)
    {
        var matchingStories = backstories
            .Where(b => b.RequiredTraits.Any(personTraits.Contains) && b.IsInfected == isInfected)
            .ToList();

        if (matchingStories.Count == 0)
        {
            // Try finding any story with matching infection status
            matchingStories = backstories.Where(b => b.IsInfected == isInfected).ToList();

            if (matchingStories.Count == 0)
            {
                // Absolute fallback - use mysterious person story
                const string defaultStory = "A mysterious person that refuses to share their story.";
                if (isInfected)
                {
                    var randomHint = infectionHints[GD.RandRange(0, infectionHints.Count - 1)];
                    return $"{defaultStory} {randomHint}";
                }
                return defaultStory;
            }
        }

        var randomIndex = GD.RandRange(0, matchingStories.Count - 1);
        return matchingStories[randomIndex].Story;
    }
}

public static class BackstoryData
{
    public static readonly Dictionary<TimePeriods, TimePeriodBackstory> TimePeriodBackstories = new()
    {
        { TimePeriods.Ancient, new TimePeriodBackstory(
            [
                new BackstoryTemplate(
                    "A mystic healer who travels between villages, keeping distance from others due to recent exposure to marsh fever.",
                    [Traits.Mystical, Traits.Ritualistic],
                    true
                ),
                new BackstoryTemplate(
                    "A nomadic trader who has crossed vast deserts, recently recovering from a mysterious ailment picked up in distant lands.",
                    [Traits.Wanderlust, Traits.Nomadic],
                    true
                ),
                new BackstoryTemplate(
                    "A healthy tribal elder who preserves ancient wisdom through oral traditions and ceremonies.",
                    [Traits.Traditional, Traits.Tribal],
                    false
                ),
                new BackstoryTemplate(
                    "A robust hunter who lives by ancient customs and follows the migration of animals.",
                    [Traits.FiercelyIndependent, Traits.Stoic],
                    false
                ),
                new BackstoryTemplate(
                    "A superstitious farmer who claims the spirits have cursed them with fever.",
                    [Traits.Superstitious, Traits.Traditional],
                    true
                ),
                new BackstoryTemplate(
                    "A wandering storyteller who avoids settlements due to spreading illness.",
                    [Traits.Wanderlust, Traits.Tribal],
                    true
                ),
                new BackstoryTemplate(
                    "A stoic medicine woman known for her immunity to common ailments.",
                    [Traits.Stoic, Traits.Mystical],
                    false
                ),
                new BackstoryTemplate(
                    "A desert nomad who follows strict purification rituals.",
                    [Traits.FiercelyIndependent, Traits.Ritualistic],
                    false
                )
            ],
            [
                "They seem unusually tired from their journey.",
                "Their clothes carry the distinct smell of medicinal herbs.",
                "They occasionally pause to catch their breath."
            ]
        )},

        { TimePeriods.Medieval, new TimePeriodBackstory(
            [
                new BackstoryTemplate(
                    "A noble who has been avoiding public gatherings due to the spreading plague in nearby towns.",
                    [Traits.Noble, Traits.Fearful],
                    true
                ),
                new BackstoryTemplate(
                    "A monk who tends to the sick in overcrowded quarters, showing signs of illness himself.",
                    [Traits.Religious, Traits.Unhygienic],
                    true
                ),
                new BackstoryTemplate(
                    "A scholarly abbott maintaining strict quarantine protocols in his monastery.",
                    [Traits.Religious, Traits.Scholarly],
                    false
                ),
                new BackstoryTemplate(
                    "A peasant family seeking refuge, having avoided affected areas during their journey.",
                    [Traits.Peasant, Traits.Suspicious],
                    false
                ),
                new BackstoryTemplate(
                    "A wandering scholar whose recent studies in plague-stricken cities left their mark.",
                    [Traits.Scholarly, Traits.Suspicious],
                    true
                ),
                new BackstoryTemplate(
                    "An opportunistic merchant who traded in infected ports.",
                    [Traits.Opportunistic, Traits.Pious],
                    true
                ),
                new BackstoryTemplate(
                    "A noble physician who maintains strict cleanliness standards.",
                    [Traits.Noble, Traits.Scholarly],
                    false
                ),
                new BackstoryTemplate(
                    "A monastery keeper who isolates from the spreading plague.",
                    [Traits.Religious, Traits.Fearful],
                    false
                )
            ],
            [
                "Dark marks are visible on their skin.",
                "They seem feverish and weak.",
                "Their lymph nodes appear swollen."
            ]
        )},

        { TimePeriods.Renaissance, new TimePeriodBackstory(
            [
                new BackstoryTemplate(
                    "An artist whose recent work has been interrupted by recurring fevers from the ports.",
                    [Traits.Artistic, Traits.Creative],
                    true
                ),
                new BackstoryTemplate(
                    "A merchant whose trading partners have recently succumbed to a mysterious ailment.",
                    [Traits.Merchant, Traits.Ambitious],
                    true
                ),
                new BackstoryTemplate(
                    "A diplomat carrying letters of good health from multiple city-states.",
                    [Traits.Diplomatic, Traits.SocialClimber],
                    false
                ),
                new BackstoryTemplate(
                    "An inventor whose latest work focuses on improving public sanitation.",
                    [Traits.Scientific, Traits.Inventive],
                    false
                ),
                new BackstoryTemplate(
                    "An intellectual whose experiments with disease samples proved dangerous.",
                    [Traits.Intellectual, Traits.Scientific],
                    true
                ),
                new BackstoryTemplate(
                    "A social climber who frequented infected noble courts.",
                    [Traits.SocialClimber, Traits.Ambitious],
                    true
                ),
                new BackstoryTemplate(
                    "A creative inventor developing new methods of disease prevention.",
                    [Traits.Creative, Traits.Inventive],
                    false
                ),
                new BackstoryTemplate(
                    "A diplomatic physician studying health practices across Europe.",
                    [Traits.Diplomatic, Traits.Scientific],
                    false
                )
            ],
            [
                "Their hands shake slightly when gesturing.",
                "They appear gaunt and malnourished.",
                "Occasional coughing interrupts their speech."
            ]
        )},

        { TimePeriods.Victorian, new TimePeriodBackstory(
            [
                new BackstoryTemplate(
                    "A factory owner whose workers have been falling ill to industrial fumes.",
                    [Traits.Industrial, Traits.UpperClass],
                    true
                ),
                new BackstoryTemplate(
                    "A worker seeking treatment for persistent coughing from the mills.",
                    [Traits.WorkingClass, Traits.Progressive],
                    true
                ),
                new BackstoryTemplate(
                    "A scientist promoting new sanitation methods in urban areas.",
                    [Traits.Scientific, Traits.Progressive],
                    false
                ),
                new BackstoryTemplate(
                    "A colonial administrator who strictly follows modern hygiene practices.",
                    [Traits.Colonial, Traits.Reserved],
                    false
                ),
                new BackstoryTemplate(
                    "A prideful industrialist hiding symptoms from their factory's poor conditions.",
                    [Traits.Prideful, Traits.Industrial],
                    true
                ),
                new BackstoryTemplate(
                    "A progressive reformer who caught illness while documenting worker conditions.",
                    [Traits.Progressive, Traits.Scientific],
                    true
                ),
                new BackstoryTemplate(
                    "A proper lady who strictly follows modern medical advice.",
                    [Traits.Proper, Traits.Reserved],
                    false
                ),
                new BackstoryTemplate(
                    "An upper-class physician promoting vaccination programs.",
                    [Traits.UpperClass, Traits.Progressive],
                    false
                ),
                new BackstoryTemplate(
                    "A textile mill supervisor ignoring the dangers of cotton dust.",
                    [Traits.Industrial, Traits.Prideful],
                    true
                ),
                new BackstoryTemplate(
                    "A colonial explorer returning with tropical maladies.",
                    [Traits.Colonial, Traits.Ambitious],
                    true
                ),
                new BackstoryTemplate(
                    "A society doctor championing the latest medical innovations.",
                    [Traits.Scientific, Traits.UpperClass],
                    false
                ),
                new BackstoryTemplate(
                    "A proper gentleman advocating for public health reforms.",
                    [Traits.Proper, Traits.Progressive],
                    false
                )
            ],
            [
                "Their breathing is labored and wheezy.",
                "Coal dust marks their clothing.",
                "Their complexion is unusually pale."
            ]
        )},

        { TimePeriods.WorldWar, new TimePeriodBackstory(
            [
                new BackstoryTemplate(
                    "A soldier evacuated from the trenches after a severe fever outbreak.",
                    [Traits.Military, Traits.Traumatized],
                    true
                ),
                new BackstoryTemplate(
                    "A nurse who has been treating infected wounds in field hospitals.",
                    [Traits.Civilian, Traits.Determined],
                    true
                ),
                new BackstoryTemplate(
                    "A resistance fighter who has avoided contaminated areas.",
                    [Traits.Resistant, Traits.Brave],
                    false
                ),
                new BackstoryTemplate(
                    "A resourceful spy who maintains strict personal hygiene.",
                    [Traits.Paranoid, Traits.Resourceful],
                    false
                ),
                new BackstoryTemplate(
                    "A patriotic soldier concealing symptoms to stay in service.",
                    [Traits.Patriotic, Traits.Military],
                    true
                ),
                new BackstoryTemplate(
                    "A resourceful medic exposed to numerous battlefield diseases.",
                    [Traits.Resourceful, Traits.Determined],
                    true
                ),
                new BackstoryTemplate(
                    "A brave resistance member who survived epidemic outbreaks.",
                    [Traits.Brave, Traits.Resistant],
                    false
                ),
                new BackstoryTemplate(
                    "A civilian doctor maintaining a clean field hospital.",
                    [Traits.Civilian, Traits.Paranoid],
                    false
                )
            ],
            [
                "They struggle to catch their breath.",
                "Their uniform is stained with medical supplies.",
                "A persistent cough plagues them."
            ]
        )},

        { TimePeriods.Seventies, new TimePeriodBackstory(
            [
                new BackstoryTemplate(
                    "A free spirit whose recent travels have left them unusually fatigued.",
                    [Traits.FreeSpirited, Traits.Experimental],
                    true
                ),
                new BackstoryTemplate(
                    "An activist whose community has been struck by a mysterious illness.",
                    [Traits.Activist, Traits.Progressive],
                    true
                ),
                new BackstoryTemplate(
                    "A conservative politician advocating for stricter health measures.",
                    [Traits.Conservative, Traits.Urban],
                    false
                ),
                new BackstoryTemplate(
                    "A rebellious youth who practices alternative medicine and healthy living.",
                    [Traits.OpenMinded, Traits.Rebellious],
                    false
                ),
                new BackstoryTemplate(
                    "A hedonistic club-goer showing early signs of a new epidemic.",
                    [Traits.Hedonistic, Traits.Urban],
                    true
                ),
                new BackstoryTemplate(
                    "An experimental artist whose commune has fallen ill.",
                    [Traits.Experimental, Traits.OpenMinded],
                    true
                ),
                new BackstoryTemplate(
                    "A conservative doctor promoting awareness of emerging diseases.",
                    [Traits.Conservative, Traits.Activist],
                    false
                ),
                new BackstoryTemplate(
                    "A rebellious health worker educating about prevention.",
                    [Traits.Rebellious, Traits.FreeSpirited],
                    false
                )
            ],
            [
                "They appear unusually thin and tired.",
                "Night sweats have left them dehydrated.",
                "Their immune system seems compromised."
            ]
        )},

        { TimePeriods.Modern, new TimePeriodBackstory(
            [
                new BackstoryTemplate(
                    "A remote worker developing sustainable technology solutions, who has been isolating due to recent exposure.",
                    [Traits.RemoteWorker, Traits.Environmentalist],
                    true
                ),
                new BackstoryTemplate(
                    "A healthy digital nomad running an online business while traveling the world.",
                    [Traits.DigitalNative, Traits.GlobalCitizen],
                    false
                ),
                new BackstoryTemplate(
                    "An anxious tech worker who tested positive but needs to travel.",
                    [Traits.Anxious, Traits.TechSavvy],
                    true
                ),
                new BackstoryTemplate(
                    "A multitasking influencer who ignored quarantine guidelines.",
                    [Traits.Multitasker, Traits.Introverted],
                    true
                ),
                new BackstoryTemplate(
                    "A health-conscious global citizen following all safety protocols.",
                    [Traits.HealthConscious, Traits.GlobalCitizen],
                    false
                ),
                new BackstoryTemplate(
                    "An environmentalist promoting both health and sustainability.",
                    [Traits.Environmentalist, Traits.TechSavvy],
                    false
                ),
                new BackstoryTemplate(
                    "A burned-out tech executive showing symptoms of chronic stress and fatigue.",
                    [Traits.TechSavvy, Traits.Multitasker],
                    true
                ),
                new BackstoryTemplate(
                    "An anxious social media manager who attended a superspreader event.",
                    [Traits.Anxious, Traits.DigitalNative],
                    true
                ),
                new BackstoryTemplate(
                    "A remote wellness coach practicing what they preach.",
                    [Traits.RemoteWorker, Traits.HealthConscious],
                    false
                ),
                new BackstoryTemplate(
                    "An introverted programmer maintaining strict health protocols.",
                    [Traits.Introverted, Traits.TechSavvy],
                    false
                )
            ],
            [
                "They keep their distance during conversation.",
                "They frequently use hand sanitizer.",
                "Their voice sounds slightly hoarse."
            ]
        )}
    };
}