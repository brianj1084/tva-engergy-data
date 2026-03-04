namespace tva_engery_data.Models;

public class TvaGenerationViewModel
{
    public string? HourlyDataPeriod { get; set; }
    public List<HourlyGenerationEntry> HourlyGeneration { get; set; } = [];
    public List<PlantGroup> PlantGroups { get; set; } = [];
    public string? UnitsDataPeriod { get; set; }
    public string? ErrorMessage { get; set; }

    public double TotalGenerationMw => HourlyGeneration.Sum(e => e.ValueMw > 0 ? e.ValueMw : 0);
    public int TotalUnitCount => PlantGroups.Sum(p => p.Generators.Count);
    public double TotalCapacityMw => PlantGroups.Sum(p => p.TotalCapacityMw);
}

public class PlantGroup
{
    public string PlantId { get; set; } = "";
    public string PlantName { get; set; } = "";
    public string StateName { get; set; } = "";
    public List<GeneratorUnit> Generators { get; set; } = [];
    public double TotalCapacityMw => Generators.Sum(g => g.NameplateMw);
}
