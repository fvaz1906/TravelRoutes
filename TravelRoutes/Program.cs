using TravelRoutes.Services;

if (args.Length < 1)
{
    // Verifica se o arquivo CSV foi passado como argumento
    Console.WriteLine("Usage: executable <file.csv>");
    return;
}

string csvFile = args[0];

if (!File.Exists(csvFile))
{
    // Verifica se o arquivo CSV existe
    Console.WriteLine($"File {csvFile} not found.");
    return;
}

try
{
    // Lê as rotas do arquivo CSV
    var routes = ReadRoutes.ReadRoutesFromFile(csvFile);

    // Cria o grafo e adiciona as rotas
    var graph = new GraphService();
    graph.AddRoutes(routes);

    // Solicita ao usuário a origem da rota
    Console.WriteLine("Enter origin:");
    string originInput = Console.ReadLine();

    // Solicita ao usuário o destino da rota
    Console.WriteLine("Enter destination:");
    string destinationInput = Console.ReadLine();

    // Calcula o menor custo e caminho entre os pontos
    var (path, cost) = graph.FindShortestPath(originInput, destinationInput);

    if (path.Count == 0)
    {
        // Exibe mensagem se nenhuma rota foi encontrada
        Console.WriteLine("No route found.");
    }
    else
    {
        // Exibe a melhor rota e o custo associado
        Console.WriteLine($"Best route: {string.Join(" - ", path)} at a cost of ${cost}");
    }
}
catch (Exception ex)
{
    // Trata erros inesperados
    Console.WriteLine($"Error: {ex.Message}");
}
