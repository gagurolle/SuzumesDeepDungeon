using CsvHelper;
using CsvHelper.Configuration;
using System.Globalization;

namespace SuzumesDeepDungeon.Services.CSVLoad
{
    public class CSVLoad
    {
        public  List<TwitchStatisticGames> LoadGames(string filePath)
        {
            var config = new CsvConfiguration(CultureInfo.InvariantCulture)
            {
                Delimiter = ",",                         // Разделитель (по умолчанию ",")
                HasHeaderRecord = true,                   // Флаг наличия заголовков
                MissingFieldFound = null,                 // Игнорировать отсутствующие поля
                HeaderValidated = null,                   // Игнорировать неизвестные заголовки
            };

            using (var reader = new StreamReader(filePath))
            using (var csv = new CsvReader(reader, config))
            {
                return csv.GetRecords<TwitchStatisticGames>().ToList();
            }
        }

    }

    public class TwitchStatisticGames
    {
        public string gameName { get; set; } = string.Empty;
        public double StreamTime { get; set; }
        public double TimeShare { get; set; } 
        public double AvgViewers { get; set; } 
        public double MaxViewers { get; set; } 
        public double FollowersPhr { get; set; }
        public DateTime LastSeen { get; set; } 
    }
}
