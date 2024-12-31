using TravelRoutes.Models;

namespace TravelRoutes.Services
{
    public static class ReadRoutes
    {
        // Lê as rotas a partir de um arquivo CSV
        public static List<Routes> ReadRoutesFromFile(string csvFile)
        {
            var routes = new List<Routes>();

            // Lê todas as linhas do arquivo CSV
            foreach (var line in File.ReadAllLines(csvFile))
            {
                var parts = line.Split(',');

                // Ignora linhas com formato inválido
                if (parts.Length != 3) continue;

                var origin = parts[0];
                var destination = parts[1];
                var cost = int.Parse(parts[2]);

                // Adiciona a rota à lista de rotas
                routes.Add(new Routes { 
                    RouteOrigin = origin,
                    RouteDestination = destination, 
                    Value = cost });
            }

            return routes;
        }
    }
}
