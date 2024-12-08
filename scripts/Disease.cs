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

public class DiseaseInfo
{
    public string Description { get; set; }
    public string Origin { get; set; }
    public List<string> QuickFacts { get; set; }

    public DiseaseInfo(string description, string origin, List<string> quickFacts)
    {
        Description = description;
        Origin = origin;
        QuickFacts = quickFacts;
    }
}

public static class DiseaseDescriptions
{
    public static readonly Dictionary<Disease, DiseaseInfo> DiseaseDetails = new()
    {
        { Disease.Malaria, new DiseaseInfo(
            "Malaria is a life-threatening disease caused by parasites transmitted to humans through the bites of infected female Anopheles mosquitoes.",
            "Originated in tropical and subtropical regions of the world, particularly Africa, Asia, and South America.",
            [
                "Malaria is caused by Plasmodium parasites.",
                "Symptoms include fever, chills, and flu-like illness.",
                "It kills over 200,000 people annually, mainly in sub-Saharan Africa.",
                "Preventive measures include using insecticide-treated nets and antimalarial drugs."
            ]
        )},
        { Disease.BubonicPlague, new DiseaseInfo(
            "The Bubonic Plague is a deadly bacterial infection spread by fleas that lived on rats. It causes fever, fatigue, and swollen lymph nodes, and was responsible for widespread pandemics.",
            "Believed to have originated in Central Asia, it spread to Europe via trade routes in the 6th century.",
            [
                "Caused by the bacterium Yersinia pestis.",
                "Historically known as the 'Black Death'.",
                "Symptoms include sudden fever, chills, and swollen lymph nodes, called buboes.",
                "It killed approximately one-third of Europe’s population in the 14th century."
            ]
        )},
        { Disease.Cholera, new DiseaseInfo(
            "Cholera is an infectious disease caused by ingesting contaminated food or water. It leads to severe diarrhea and dehydration, which can be fatal without treatment.",
            "Originated in the Indian subcontinent, cholera has caused several pandemics since the 19th century.",
            [
                "Caused by the bacterium Vibrio cholerae.",
                "Symptoms include rapid-onset watery diarrhea and vomiting.",
                "It is primarily spread through contaminated water and poor sanitation.",
                "Cholera outbreaks have been successfully controlled with improved sanitation and water treatment."
            ]
        )},
        { Disease.Tuberculosis, new DiseaseInfo(
            "Tuberculosis (TB) is a bacterial infection that primarily affects the lungs but can spread to other parts of the body. It is spread through the air when an infected person coughs or sneezes.",
            "TB has been known since ancient times, and it is still prevalent in many parts of the world.",
            [
                "Caused by the bacterium Mycobacterium tuberculosis.",
                "Symptoms include persistent cough, chest pain, and blood in the sputum.",
                "TB is often associated with poverty, malnutrition, and weakened immune systems.",
                "It is treatable with antibiotics, but drug-resistant TB is a growing concern."
            ]
        )},
        { Disease.SpanishFlu, new DiseaseInfo(
            "The Spanish Flu was an influenza pandemic that spread worldwide in 1918-1919, infecting about one-third of the global population and causing millions of deaths.",
            "The origin of the Spanish Flu is still debated, though it likely originated in the United States, spreading during World War I.",
            [
                "Caused by the H1N1 influenza A virus.",
                "It had a high mortality rate, particularly among healthy young adults.",
                "Over 50 million people are estimated to have died globally.",
                "The pandemic led to widespread health measures, including quarantines and mask-wearing."
            ]
        )},
        { Disease.HIV, new DiseaseInfo(
            "HIV (Human Immunodeficiency Virus) is a virus that attacks the immune system, potentially leading to AIDS (Acquired Immunodeficiency Syndrome), which severely weakens the body’s ability to fight infections.",
            "HIV likely originated from chimpanzees in Central Africa, crossing into humans through hunting and consumption of bush-meat.",
            [
                "HIV is spread through blood, semen, vaginal fluids, and breast milk.",
                "While there is no cure, antiretroviral therapy (ART) can control the virus and enable individuals to live normal lifespans.",
                "AIDS, the final stage of HIV, severely compromises the immune system, making individuals susceptible to opportunistic infections."
            ]
        )},
        { Disease.Covid19, new DiseaseInfo(
            "COVID-19 is a highly contagious disease caused by the SARS-CoV-2 virus. It primarily affects the respiratory system but can also have severe long-term effects on other organs.",
            "COVID-19 emerged in late 2019 in Wuhan, China, and spread rapidly around the globe, leading to a pandemic in early 2020.",
            [
                "Caused by the SARS-CoV-2 virus.",
                "Symptoms include fever, cough, shortness of breath, and loss of taste or smell.",
                "COVID-19 spreads through respiratory droplets and can cause severe illness in older adults and those with preexisting conditions.",
                "Vaccines and public health measures such as masks and social distancing have helped reduce transmission."
            ]
        )}
    };
}
