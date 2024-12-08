
using System;
using System.Collections.Generic;
using System.Linq;
using Godot;

namespace hakim.scripts;

public record BackstoryTemplate(string Story, Traits[] RequiredTraits);

public class TimePeriodBackstory
{
    private readonly List<BackstoryTemplate> _backstories;

    public TimePeriodBackstory(List<BackstoryTemplate> backstories)
    {
        _backstories = backstories;
    }

    public string GetBackstory(Traits[] personTraits)
    {
        var matchingStories = _backstories
            .Where(b => b.RequiredTraits.All(personTraits.Contains))
            .ToList();

        if (!matchingStories.Any())
            return _backstories[0].Story; // Default story if no match

        var randomIndex = GD.RandRange(0, matchingStories.Count - 1);
        return matchingStories[randomIndex].Story;
    }
}

public static class BackstoryData
{
    public static readonly Dictionary<TimePeriods, TimePeriodBackstory> TimePeriodBackstories = new()
    {
        { TimePeriods.Ancient, new TimePeriodBackstory([
            new BackstoryTemplate(
                "A mystic healer who travels between villages, performing sacred rituals to ward off evil spirits.",
                [Traits.Mystical, Traits.Ritualistic]
            ),
            new BackstoryTemplate(
                "A nomadic trader who has crossed vast deserts, bringing exotic goods from distant lands.",
                [Traits.Wanderlust, Traits.Nomadic]
            ),
            new BackstoryTemplate(
                "A tribal elder who preserves ancient wisdom through oral traditions and ceremonies.",
                [Traits.Traditional, Traits.Tribal]
            ),
            new BackstoryTemplate(
                "A solitary hunter who lives by ancient customs and follows the migration of animals.",
                [Traits.FiercelyIndependent, Traits.Stoic]
            )
        ])},

        { TimePeriods.Medieval, new TimePeriodBackstory([
            new BackstoryTemplate(
                "A noble from a prestigious family, seeking refuge from political intrigue at court.",
                [Traits.Noble, Traits.Opportunistic]
            ),
            new BackstoryTemplate(
                "A monk who has dedicated their life to preserving knowledge in monastery scriptorium.",
                [Traits.Religious, Traits.Scholarly]
            ),
            new BackstoryTemplate(
                "A peasant fleeing from their lord's harsh rule, hoping for a better life.",
                [Traits.Peasant, Traits.Fearful]
            ),
            new BackstoryTemplate(
                "A wandering scholar who studies the stars and ancient texts.",
                [Traits.Scholarly, Traits.Suspicious]
            )
        ])},

        { TimePeriods.Renaissance, new TimePeriodBackstory([
            new BackstoryTemplate(
                "An artist seeking patronage, whose controversial works challenge traditional views.",
                [Traits.Artistic, Traits.Creative]
            ),
            new BackstoryTemplate(
                "A wealthy merchant who funds scientific expeditions while building trade networks.",
                [Traits.Merchant, Traits.Ambitious]
            ),
            new BackstoryTemplate(
                "An inventor whose revolutionary ideas have drawn unwanted attention from authorities.",
                [Traits.Scientific, Traits.Inventive]
            ),
            new BackstoryTemplate(
                "A diplomat navigating complex alliances between powerful city-states.",
                [Traits.Diplomatic, Traits.SocialClimber]
            )
        ])},

        { TimePeriods.Victorian, new TimePeriodBackstory([
            new BackstoryTemplate(
                "A factory owner pioneering new industrial processes while battling worker unrest.",
                [Traits.Industrial, Traits.UpperClass]
            ),
            new BackstoryTemplate(
                "A working-class reformer fighting for better conditions in the mills.",
                [Traits.WorkingClass, Traits.Progressive]
            ),
            new BackstoryTemplate(
                "A scientist whose unconventional theories challenge societal norms.",
                [Traits.Scientific, Traits.Progressive]
            ),
            new BackstoryTemplate(
                "A colonial administrator seeking to escape a scandal in the Empire.",
                [Traits.Colonial, Traits.Reserved]
            )
        ])},

        { TimePeriods.WorldWar, new TimePeriodBackstory([
            new BackstoryTemplate(
                "A decorated soldier haunted by the horrors of trench warfare.",
                [Traits.Military, Traits.Traumatized]
            ),
            new BackstoryTemplate(
                "A resistance fighter working undercover to protect their community.",
                [Traits.Resistant, Traits.Brave]
            ),
            new BackstoryTemplate(
                "A civilian nurse treating wounded soldiers on the front lines.",
                [Traits.Civilian, Traits.Determined]
            ),
            new BackstoryTemplate(
                "A spy gathering intelligence while dealing with constant paranoia.",
                [Traits.Paranoid, Traits.Resourceful]
            )
        ])},

        { TimePeriods.Seventies, new TimePeriodBackstory([
            new BackstoryTemplate(
                "A peace activist organizing protests against ongoing conflicts.",
                [Traits.Activist, Traits.Progressive]
            ),
            new BackstoryTemplate(
                "A free-spirited artist exploring new forms of expression.",
                [Traits.FreeSpirited, Traits.Experimental]
            ),
            new BackstoryTemplate(
                "A conservative politician resisting rapid social changes.",
                [Traits.Conservative, Traits.Urban]
            ),
            new BackstoryTemplate(
                "A counterculture leader promoting alternative lifestyles.",
                [Traits.OpenMinded, Traits.Rebellious]
            )
        ])},

        { TimePeriods.Modern, new TimePeriodBackstory([
            new BackstoryTemplate(
                "A remote worker developing sustainable technology solutions.",
                [Traits.RemoteWorker, Traits.Environmentalist]
            ),
            new BackstoryTemplate(
                "A digital nomad running an online business while traveling the world.",
                [Traits.DigitalNative, Traits.GlobalCitizen]
            ),
            new BackstoryTemplate(
                "An anxious social media influencer advocating for mental health awareness.",
                [Traits.Anxious, Traits.HealthConscious]
            ),
            new BackstoryTemplate(
                "A tech entrepreneur balancing work-life integration in the digital age.",
                [Traits.TechSavvy, Traits.Multitasker]
            )
        ])}
    };
}