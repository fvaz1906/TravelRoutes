# TravelRoutes API

Este projeto é uma API para registro de rotas e cálculo da melhor rota entre dois pontos. Ele possui dois principais endpoints:

- **POST /api/routes/register**: Registra uma nova rota no arquivo CSV.
- **GET /api/routes/best**: Calcula a melhor rota entre dois pontos com base em um arquivo CSV de rotas preexistente.

A API usa um modelo de grafo para calcular a menor rota entre dois pontos com base em valores associados às rotas.

## Tecnologias Usadas

- **.NET 8** (para o desenvolvimento da API)
- **xUnit** (para testes)
- **Moq** (para mocking em testes unitários)
- **CSV File** (para persistência de rotas)

## Estrutura do Projeto

Este projeto contém as seguintes soluções:

- **TravelRoutes.API**: A API que fornece os endpoints para registrar e calcular as rotas.
- **TravelRoutes.Models**: Contém os modelos de dados utilizados pela API, como `Routes` e `RouteResponse`.
- **TravelRoutes.Services**: Implementações de serviços, como a leitura e escrita de arquivos CSV e o cálculo do grafo.
- **TravelRoutes.Tests**: Testes automatizados para garantir o funcionamento correto da API.

## Como Executar a API

### Pré-requisitos

- **.NET 8 SDK** instalado em sua máquina.
- **Visual Studio** ou **VSCode** (opcional, mas recomendado).

### Passos para Execução

1. **Clone o Repositório**:

   ```bash
   git clone https://github.com/seu-usuario/TravelRoutes.git
   cd TravelRoutes
   ```

2. **Restaurar as Dependências**:

   ```bash
	dotnet restore
   ```

3. **Executar a API**:

	```bash
	dotnet run --project TravelRoutes.API
	```

### Testes Automatizados

Os testes automatizados estão localizados na pasta TravelRoutes.Tests. Para executar os testes, utilize o comando dentro do projeto de testes:

```bash
dotnet test
```

### Endpoints da API

1. **POST /api/routes/register**:

Esse endpoint registra uma nova rota no arquivo CSV. Você deve enviar os dados no corpo da requisição no formato JSON.

```bash
{
  "RouteOrigin": "A",
  "RouteDestination": "B",
  "Value": 10
}
```

Resposta Esperada:

- 200 OK: Caso a rota seja registrada com sucesso.
- 500 Internal Server Error: Caso ocorra um erro ao registrar a rota.

Exemplo de Resposta:

```bash
{
  "message": "Route registered successfully!"
}
```

2. **GET /api/routes/best**:

Esse endpoint retorna a melhor rota entre dois pontos, calculando o menor custo baseado nas rotas registradas. Você deve fornecer os parâmetros origin e destination na URL.

Exemplo de Requisição:

```bash
GET https://localhost:5001/api/routes/best?origin=A&destination=C
```

Resposta Esperada:

- 200 OK: Caso uma rota seja encontrada.
- 404 Not Found: Caso não haja rota entre os pontos.
- 500 Internal Server Error: Caso ocorra um erro ao calcular a melhor rota.

Exemplo de Resposta:

```bash
{
  "Path": "A - B - C",
  "Cost": 25
}
```

### Como Configurar e Modificar o Arquivo CSV

O arquivo CSV que armazena as rotas pode ser encontrado em TravelRoutes.Shared/rotas.csv. Este arquivo pode ser modificado manualmente ou por meio da API, utilizando o endpoint register.

## Estrutura do CSV

O arquivo CSV possui a seguinte estrutura:

```bash
RouteOrigin,RouteDestination,Value
A,B,10
B,C,15
A,C,25
```

Cada linha representa uma rota com origem, destino e valor associado.