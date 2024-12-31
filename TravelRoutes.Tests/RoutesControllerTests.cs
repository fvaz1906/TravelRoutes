using Microsoft.AspNetCore.Mvc;
using Moq;
using TravelRoutes.API.Controllers;
using TravelRoutes.API.Interfaces;
using TravelRoutes.Models;

public class RoutesControllerTests
{
    private readonly Mock<IFileService> _mockFileService;
    private readonly RoutesController _controller;

    public RoutesControllerTests()
    {
        _mockFileService = new Mock<IFileService>();
        _controller = new RoutesController(_mockFileService.Object);
    }

    [Fact]
    public void RegisterRoute_ShouldReturnSuccess()
    {
        // Arrange
        var newRoute = new Routes
        {
            RouteOrigin = "A",
            RouteDestination = "B",
            Value = 100
        };

        // Act
        var result = _controller.RegisterRoute(newRoute);

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result);
        Assert.Equal("Route registered successfully!", okResult.Value);

        // Verificar se o método AppendToFile foi chamado com o caminho e conteúdo corretos
        _mockFileService.Verify(m => m.AppendToFile(It.IsAny<string>(), It.IsAny<string>()), Times.Once);
    }

    [Fact]
    public void GetBestRoute_ShouldReturnBestRoute()
    {
        // Arrange
        var routes = new List<Routes>
        {
            new Routes { RouteOrigin = "A", RouteDestination = "B", Value = 10 },
            new Routes { RouteOrigin = "B", RouteDestination = "C", Value = 15 }
        };

        _mockFileService.Setup(m => m.ReadFromFile(It.IsAny<string>())).Returns(routes);

        // Act
        var result = _controller.GetBestRoute("A", "C");

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result); // Verificar se é OkObjectResult
        var value = Assert.IsType<RouteResponse>(okResult.Value); // Verificar se o valor é do tipo RouteResponse

        // Verificar as propriedades do DTO
        Assert.Equal("A - B - C", value.Path);
        Assert.Equal(25, value.Cost);

        // Verificar se o método ReadFromFile foi chamado
        _mockFileService.Verify(m => m.ReadFromFile(It.IsAny<string>()), Times.Once);
    }
}