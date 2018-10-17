using Microcharts;
using SkiaSharp;
using SmartHotel.Clients.Core.Controls;
using System.Threading.Tasks;

namespace SmartHotel.Clients.Core.Services.Chart
{
    public class FakeChartService : IChartService
    {
        public async Task<Microcharts.Chart> GetTemperatureChartAsync()
        {
            await Task.Delay(500);

            var data = new[]
            {
                new Entry(50)
                {
                        ValueLabel = "50",
                        Color = SKColor.Parse("#104950"),
                },
                new Entry(72)
                {
                        ValueLabel = "76",
                        Color = SKColor.Parse("#348E94"),
                },
                new Entry(81)
                {
                        ValueLabel = "82",
                        Color = SKColor.Parse("#D97F55"),
                },
                new Entry(90)
                {
                        ValueLabel = "90",
                        Color = SKColor.Parse("#EFBCB0"),
                }
            };

            return new TemperatureChart() { Entries = data };
        }

        public async Task<Microcharts.Chart> GetGreenChartAsync()
        {
            await Task.Delay(500);

            var data = new[]
            {
                new Entry(120)
                {
                        Label = "06:00",
                        ValueLabel = "120",
                        Color = SKColor.Parse("#BC4C1B"),
                },
                new Entry(140)
                {
                        Label = "10:00",
                        ValueLabel = "140",
                        Color = SKColor.Parse("#BC4C1B"),
                },
                new Entry(45)
                {
                        Label = "14:00",
                        ValueLabel = "45",
                        Color = SKColor.Parse("#0BC3B6"),
                },
                new Entry(100)
                {
                        Label = "18:00",
                        ValueLabel = "100",
                        Color = SKColor.Parse("#0BC3B6"),
                },
                new Entry(130)
                {
                        Label = "22:00",
                        ValueLabel = "130",
                        Color = SKColor.Parse("#BC4C1B"),
                },
                new Entry(75)
                {
                        Label = "02:00",
                        ValueLabel = "75",
                        Color = SKColor.Parse("#0BC3B6"),
                }
            };

            return new GreenChart() { Entries = data };
        }
    }
}