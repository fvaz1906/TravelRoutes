using Microsoft.AspNetCore.Mvc;
using TravelRoutes.API.Interfaces;
using TravelRoutes.Models;
using TravelRoutes.Services;

namespace TravelRoutes.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RoutesController : ControllerBase
    {
        private readonly IFileService _fileService;
        private readonly string _csvFile = Path.Combine("..", "TravelRoutes.Shared", "rotas.csv");

        public RoutesController(IFileService fileService)
        {
            _fileService = fileService;
        }

        [HttpPost]
        [Route("register")]
        public IActionResult RegisterRoute([FromBody] Routes routes)
        {
            try
            {
                var line = $"{routes.RouteOrigin},{routes.RouteDestination},{routes.Value}";
                _fileService.AppendToFile(_csvFile, line + "\n");
                return Ok("Route registered successfully!");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
                return StatusCode(500, "Error while registering the route.");
            }
        }

        [HttpGet]
        [Route("best")]
        public IActionResult GetBestRoute([FromQuery] string origin, [FromQuery] string destination)
        {
            try
            {
                var routes = _fileService.ReadFromFile(_csvFile);

                var graph = new GraphService();
                graph.AddRoutes(routes);

                var (path, cost) = graph.FindShortestPath(origin, destination);

                if (path.Count == 0)
                    return NotFound("No route found.");

                // Retornar o DTO em vez de um tipo anônimo
                var response = new RouteResponse
                {
                    Path = string.Join(" - ", path),
                    Cost = cost
                };

                return Ok(response);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
                return StatusCode(500, "Error while finding the best route.");
            }
        }
    }

}
