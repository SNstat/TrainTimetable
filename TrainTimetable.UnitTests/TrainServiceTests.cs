using TrainTimetable.Business.Services;
using TrainTimetable.Data.Entities;

namespace TrainTimetable.UnitTests;

public class TrainServiceTests
{
    [Fact]
    public async Task TrainService_RegisterTrains_Valid()
    {
        // Arrange
        var repository = new FakeBaseRepository<Train>();
        var trainService = new TrainService(repository);
        var train = new Train
        {
            Name = "Thomas",
            SeatCount = 20,
            TrainManufacturerID = 1
        };

        // Act
        await trainService.RegisterAsync(train);

        // Assert
        var savedTrains = await repository.GetAllAsync();
        var savedTrain = Assert.Single(savedTrains);

        Assert.Equal(train.Name, savedTrain.Name);
        Assert.Equal(train.SeatCount, savedTrain.SeatCount);
        Assert.Equal(train.TrainManufacturerID, savedTrain.TrainManufacturerID);
    }

    [Theory]
    [InlineData("", 20, 1)]
    [InlineData(null, 20, 1)]
    [InlineData("Thomas", 0, 1)]
    [InlineData("Thomas", 1001, 1)]
    [InlineData("Thomas", 20, 0)]
    [InlineData("Thomas", 20, -1)]
    public async Task TrainService_RegisterTrains_ThrowsAsyncApplicationException(string name, int seatCount, int trainManufacturerID)
    {
        // Arrange
        var repository = new FakeBaseRepository<Train>();
        var trainService = new TrainService(repository);
        var train = new Train {
            Name = name,
            SeatCount = seatCount,
            TrainManufacturerID = trainManufacturerID
        };

        // Act
        var method = async () => await trainService.RegisterAsync(train);

        // Assert
        await Assert.ThrowsAsync<ApplicationException>(method);
    }
}
