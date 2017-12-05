using System.Threading.Tasks;

namespace SmartHotel.Clients.Core.Services.Chart
{
    public interface IChartService
    {
        Task<Microcharts.Chart> GetTemperatureChartAsync();
        Task<Microcharts.Chart> GetGreenChartAsync();
    }
}