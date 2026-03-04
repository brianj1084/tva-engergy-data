using System.Net.Http.Json;
using tva_engery_data.Models;

namespace tva_engery_data.Services;

public class EiaApiService
{
    private readonly HttpClient _httpClient;
    private readonly string _apiKey;
    private readonly ILogger<EiaApiService> _logger;

    public EiaApiService(HttpClient httpClient, IConfiguration configuration, ILogger<EiaApiService> logger)
    {
        _httpClient = httpClient;
        _apiKey = configuration["EIA_API_KEY"]
            ?? throw new InvalidOperationException("EIA API key not configured. Set EIA_API_KEY in appsettings.");
        _logger = logger;
    }

    /// <summary>
    /// Returns the most recent hour of TVA net generation by fuel type (EIA-930).
    /// Values are MWh for the hour, which equals average MW output.
    /// </summary>
    public async Task<(List<HourlyGenerationEntry> Entries, string? Period)> GetTvaHourlyGenerationAsync()
    {
        var url = "https://api.eia.gov/v2/electricity/rto/fuel-type-data/data/" +
                  $"?api_key={_apiKey}" +
                  "&facets[respondent][]=TVA" +
                  "&data[]=value" +
                  "&sort[0][column]=period&sort[0][direction]=desc" +
                  "&length=100";
        try
        {
            var response = await _httpClient.GetFromJsonAsync<EiaApiResponse<HourlyGenerationEntry>>(url);
            var data = response?.Response?.Data ?? [];
            var latestPeriod = data.FirstOrDefault()?.Period;
            var entries = latestPeriod != null
                ? data.Where(d => d.Period == latestPeriod).ToList()
                : data;
            return (entries, latestPeriod);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error fetching TVA hourly generation from EIA");
            return ([], null);
        }
    }

    /// <summary>
    /// Returns the most recent monthly inventory of TVA generator units (EIA-860M).
    /// Includes nameplate and net summer capacity per unit.
    /// </summary>
    public async Task<(List<GeneratorUnit> Units, string? Period)> GetTvaGeneratorUnitsAsync()
    {
        var url = "https://api.eia.gov/v2/electricity/operating-generator-capacity/data/" +
                  $"?api_key={_apiKey}" +
                  "&facets[balancing_authority_code][]=TVA" +
                  "&data[]=nameplate-capacity-mw&data[]=net-summer-capacity-mw" +
                  "&sort[0][column]=period&sort[0][direction]=desc" +
                  "&sort[1][column]=plantName&sort[1][direction]=asc" +
                  "&length=5000";
        try
        {
            var response = await _httpClient.GetFromJsonAsync<EiaApiResponse<GeneratorUnit>>(url);
            var data = response?.Response?.Data ?? [];
            var latestPeriod = data.FirstOrDefault()?.Period;
            var units = latestPeriod != null
                ? data.Where(d => d.Period == latestPeriod).ToList()
                : data;
            return (units, latestPeriod);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error fetching TVA generator units from EIA");
            return ([], null);
        }
    }
}
