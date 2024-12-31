using TravelRoutes.API.Interfaces;
using TravelRoutes.Models;

namespace TravelRoutes.API.Services
{
    public class FileService : IFileService
    {
        public void AppendToFile(string filePath, string content)
        {
            System.IO.File.AppendAllText(filePath, content);
        }

        public List<Routes> ReadFromFile(string filePath)
        {
            var lines = System.IO.File.ReadAllLines(filePath);
            return lines.Select(line => {
                var parts = line.Split(',');
                return new Routes
                {
                    RouteOrigin = parts[0],
                    RouteDestination = parts[1],
                    Value = int.Parse(parts[2])
                };
            }).ToList();
        }
    }
}
