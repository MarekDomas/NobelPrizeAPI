using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Json;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace API;

public class PrizeWinner
{
    public string Name { get; set; }
    public string Category { get; set; }
    public string Description { get; set; }
    public int prizeAmount { get; set; }
    public int prizeAmountAdjusted { get; set; }

    public static List<Root> GetApiData(string category, int year)
    {
        category = category == "Physiology or Medicine" ? "med" : category.ToLower()[..3];
        var uri = $"https://api.nobelprize.org/2.0/nobelPrize/{category}/{year}";

        HttpClient client = new();
        var body = client.GetFromJsonAsync<List<Root>>(uri).Result;

        return body;
    }

    public PrizeWinner(Laureate laureate, string category, Root root)
    {
        Name = laureate.fullName is null ? laureate.orgName.en : laureate.fullName.en;
        Category = category;
        Description = laureate.motivation.en;
        prizeAmount = root.prizeAmount;
        prizeAmountAdjusted = root.prizeAmountAdjusted;

    }
}
// Root myDeserializedClass = JsonConvert.DeserializeObject<List<Root>>(myJsonResponse);
public class Category
{
    public string en { get; set; }
    public string no { get; set; }
    public string se { get; set; }
}

public class CategoryFullName
{
    public string en { get; set; }
    public string no { get; set; }
    public string se { get; set; }
}

public class FullName
{
    public string en { get; set; }
}

public class KnownName
{
    public string en { get; set; }
    public string no { get; set; }
}

public class Laureate
{
    public string id { get; set; }
    public KnownName knownName { get; set; }
    public FullName fullName { get; set; }
    public string portion { get; set; }
    public string sortOrder { get; set; }
    public Motivation motivation { get; set; }
    public Links links { get; set; }
    public OrgName orgName { get; set; }
}

public class Links
{
    public string rel { get; set; }
    public string href { get; set; }
    public string action { get; set; }
    public string types { get; set; }
}

public class Meta
{
    public string terms { get; set; }
    public string license { get; set; }
    public string disclaimer { get; set; }
}

public class Motivation
{
    public string en { get; set; }
    public string no { get; set; }
}

public class OrgName
{
    public string en { get; set; }
    public string no { get; set; }
}

public class Root
{
    public string awardYear { get; set; }
    public Category category { get; set; }
    public CategoryFullName categoryFullName { get; set; }
    public string dateAwarded { get; set; }
    public int prizeAmount { get; set; }
    public int prizeAmountAdjusted { get; set; }
    public Links links { get; set; }
    public List<Laureate> laureates { get; set; }
    public Meta meta { get; set; }
}

