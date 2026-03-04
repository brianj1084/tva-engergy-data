using System.Text.Json.Serialization;

namespace tva_engery_data.Models;

public class EiaApiResponse<T>
{
    [JsonPropertyName("response")]
    public EiaResponseBody<T>? Response { get; set; }
}

public class EiaResponseBody<T>
{
    [JsonPropertyName("data")]
    public List<T>? Data { get; set; }

    [JsonPropertyName("total")]
    public string? Total { get; set; }
}

public class HourlyGenerationEntry
{
    [JsonPropertyName("period")]
    public string Period { get; set; } = "";

    [JsonPropertyName("fueltype")]
    public string FuelType { get; set; } = "";

    [JsonPropertyName("type-name")]
    public string TypeName { get; set; } = "";

    [JsonPropertyName("value")]
    public string? Value { get; set; }

    [JsonPropertyName("value-units")]
    public string? ValueUnits { get; set; }

    public double ValueMw => double.TryParse(Value, out var v) ? v : 0;
}

public class GeneratorUnit
{
    [JsonPropertyName("period")]
    public string Period { get; set; } = "";

    [JsonPropertyName("plantid")]
    public string PlantId { get; set; } = "";

    [JsonPropertyName("plantName")]
    public string PlantName { get; set; } = "";

    [JsonPropertyName("generatorid")]
    public string GeneratorId { get; set; } = "";

    [JsonPropertyName("technology")]
    public string Technology { get; set; } = "";

    [JsonPropertyName("energy-source-desc")]
    public string EnergySourceDesc { get; set; } = "";

    [JsonPropertyName("stateid")]
    public string StateId { get; set; } = "";

    [JsonPropertyName("stateName")]
    public string StateName { get; set; } = "";

    [JsonPropertyName("nameplate-capacity-mw")]
    public string? NameplateCapacityMw { get; set; }

    [JsonPropertyName("net-summer-capacity-mw")]
    public string? NetSummerCapacityMw { get; set; }

    [JsonPropertyName("status")]
    public string Status { get; set; } = "";

    [JsonPropertyName("statusDescription")]
    public string StatusDescription { get; set; } = "";

    public double NameplateMw => double.TryParse(NameplateCapacityMw, out var v) ? v : 0;
    public double NetSummerMw => double.TryParse(NetSummerCapacityMw, out var v) ? v : 0;
}
