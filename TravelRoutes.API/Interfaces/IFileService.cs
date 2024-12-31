using TravelRoutes.Models;

namespace TravelRoutes.API.Interfaces
{
    public interface IFileService
    {
        void AppendToFile(string filePath, string content);
        List<Routes> ReadFromFile(string filePath);
    }
}
