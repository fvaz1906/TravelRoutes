using TravelRoutes.Models;

namespace TravelRoutes.Services
{
    public class GraphService
    {
        // Dicionário para armazenar a lista de adjacências do grafo
        private readonly Dictionary<string, List<(string destination, int cost)>> _adjacencies = new();

        // Adiciona rotas ao grafo
        public void AddRoutes(IEnumerable<Routes> routes)
        {
            foreach (var route in routes)
            {
                // Adiciona o nó de origem ao grafo, caso não exista
                if (!_adjacencies.ContainsKey(route.RouteOrigin))
                {
                    _adjacencies[route.RouteOrigin] = new List<(string destination, int cost)>();
                }

                // Adiciona o nó de destino ao grafo, mesmo que ele não tenha conexões de saída
                if (!_adjacencies.ContainsKey(route.RouteDestination))
                {
                    _adjacencies[route.RouteDestination] = new List<(string destination, int cost)>();
                }

                // Adiciona a conexão entre origem e destino
                _adjacencies[route.RouteOrigin].Add((route.RouteDestination, route.Value));
            }
        }

        // Encontra o menor caminho e custo entre os nós de origem e destino
        public (List<string> path, int cost) FindShortestPath(string origin, string destination)
        {
            var costs = new Dictionary<string, int>(); // Armazena o custo para alcançar cada nó
            var previousNodes = new Dictionary<string, string>(); // Armazena o nó anterior no menor caminho
            var visited = new HashSet<string>(); // Rastreia os nós já visitados

            // Inicializa os custos de todos os nós como infinito
            foreach (var node in _adjacencies.Keys)
            {
                costs[node] = int.MaxValue;
            }
            costs[origin] = 0;

            // Fila de prioridade para processar os nós com o menor custo primeiro
            var priorityQueue = new SortedSet<(int cost, string node)>(Comparer<(int cost, string node)>.Create((a, b) =>
                a.cost == b.cost ? string.CompareOrdinal(a.node, b.node) : a.cost.CompareTo(b.cost)));

            priorityQueue.Add((0, origin));

            while (priorityQueue.Count > 0)
            {
                // Processa o nó com o menor custo
                var (currentCost, currentNode) = priorityQueue.First();
                priorityQueue.Remove(priorityQueue.First());

                // Se o destino for alcançado, reconstrói e retorna o caminho
                if (currentNode == destination)
                {
                    var path = ReconstructPath(previousNodes, destination);
                    return (path, currentCost);
                }

                // Ignora nós já visitados
                if (visited.Contains(currentNode))
                    continue;

                visited.Add(currentNode);

                // Ignora nós sem conexões de saída
                if (!_adjacencies.ContainsKey(currentNode))
                    continue;

                // Atualiza custos e fila de prioridade para os nós vizinhos
                foreach (var (neighbor, cost) in _adjacencies[currentNode])
                {
                    var newCost = currentCost + cost;
                    if (newCost < costs[neighbor])
                    {
                        costs[neighbor] = newCost;
                        previousNodes[neighbor] = currentNode;
                        priorityQueue.Add((newCost, neighbor));
                    }
                }
            }

            // Retorna um caminho vazio e custo máximo caso nenhuma rota seja encontrada
            return (new List<string>(), int.MaxValue);
        }

        // Reconstrói o menor caminho usando o dicionário de nós anteriores
        private static List<string> ReconstructPath(Dictionary<string, string> previousNodes, string destination)
        {
            var path = new List<string>();
            var current = destination;

            // Faz o backtracking do destino até a origem
            while (previousNodes.ContainsKey(current))
            {
                path.Insert(0, current);
                current = previousNodes[current];
            }

            // Adiciona o nó de origem ao caminho
            path.Insert(0, current);
            return path;
        }
    }
}
