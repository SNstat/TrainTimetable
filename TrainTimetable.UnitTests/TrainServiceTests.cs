using TrainTimetable.Business.Services;
using TrainTimetable.Data.Entities;

namespace TrainTimetable.UnitTests;

public class TrainServiceTests
{
    [Fact]
    public async Task TrainService_RegisterMinimal_Valid()
    {
        // Arrange
        var repository = new FakeRepository<Train>();
        var service = new TrainService(repository);

        // Act
        await service.RegisterAsync(new Train() { Name = "Pero", SeatCount = 100 });

        // Assert
        var trains = await service.GetAllActiveAsync();
        Assert.NotNull(trains);
        Assert.Single(trains);
    }

    [Theory]
    [InlineData("Ćuću", 20, 1)]
    [InlineData("Ćerer", 30, 2)]
    [InlineData("rereu", 1000, 0)]
    [InlineData("Ćurere", 232, 1)]
    [InlineData("Ću3213312", 33, 1)]
    public async Task TrainService_RegisterTrains_Valid(string name, int seatCount, int trainManufacturerID)
    {
        // Arrange
        var repository = new FakeRepository<Train>();
        var service = new TrainService(repository);

        // Act
        await service.RegisterAsync(new Train() { Name = name, SeatCount = seatCount, TrainManufacturerID = trainManufacturerID });

        // Assert
        var trains = await service.GetAllActiveAsync();
        Assert.NotNull(trains);
        Assert.Single(trains);

    }

    [Theory]
    [InlineData("", 10, 1)]
    [InlineData("Ću123213231", -10, 1)]
    [InlineData("Ću123213231", 10022, 0)]
    public async Task TrainService_RegisterTrains_Invalid(string name, int seatCount, int trainManufacturerID)
    {
        // Arrange
        var repository = new FakeRepository<Train>();
        var service = new TrainService(repository);

        // Act
        var method = async () => 
            await service.RegisterAsync(new Train() { Name = name, SeatCount = seatCount, TrainManufacturerID = trainManufacturerID });

        // Assert
        await Assert.ThrowsAsync<ApplicationException>(method);
    }

}
