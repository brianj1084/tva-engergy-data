using Microsoft.AspNetCore.Mvc;
using tva_engery_data.Models;
using tva_engery_data.Services;

namespace tva_engery_data.Controllers;

public class GenerationController : Controller
{
    private readonly EiaApiService _eiaService;

    public GenerationController(EiaApiService eiaService)
    {
        _eiaService = eiaService;
    }

    public async Task<IActionResult> Index()
    {
        var hourlyTask = _eiaService.GetTvaHourlyGenerationAsync();
        var unitsTask = _eiaService.GetTvaGeneratorUnitsAsync();

        await Task.WhenAll(hourlyTask, unitsTask);

        var (hourlyEntries, hourlyPeriod) = hourlyTask.Result;
        var (units, unitsPeriod) = unitsTask.Result;

        var plantGroups = units
            .GroupBy(u => new { u.PlantId, u.PlantName, u.StateName })
            .OrderBy(g => g.Key.PlantName)
            .Select(g => new PlantGroup
            {
                PlantId = g.Key.PlantId,
                PlantName = g.Key.PlantName,
                StateName = g.Key.StateName,
                Generators = g.OrderBy(u => u.GeneratorId).ToList()
            })
            .ToList();

        var vm = new TvaGenerationViewModel
        {
            HourlyDataPeriod = hourlyPeriod,
            HourlyGeneration = hourlyEntries.OrderByDescending(e => e.ValueMw).ToList(),
            PlantGroups = plantGroups,
            UnitsDataPeriod = unitsPeriod,
            ErrorMessage = hourlyEntries.Count == 0 && units.Count == 0
                ? "Unable to retrieve data from EIA API. Please try again later."
                : null
        };

        return View(vm);
    }
}
