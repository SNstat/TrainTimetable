using Microsoft.VisualStudio.TestPlatform.ObjectModel;
using TrainTimetable.Business.Services;
using TrainTimetable.Data.Entities;

namespace TrainTimetable.UnitTests;

public class TrainServiceTests
{
    [Fact]
    internal async Task TrainService_RegisterAsync_ReturnsValidObject()
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

    [Fact]
    internal async Task TrainService_RegisterAsync_ThrowsArgumentNullException()
    {
        // Arange
        var repository = new FakeBaseRepository<Train>();
        var trainService = new TrainService(repository);
        Train train = null;

        // Act
        var method = async () => await trainService.RegisterAsync(train);

        // Assert
        await Assert.ThrowsAsync<ArgumentNullException>(method);
    }

    [Theory]
    [InlineData("", 20, 1)]
    [InlineData(null, 20, 1)]
    [InlineData("Thomas", 0, 1)]
    [InlineData("Thomas", 1001, 1)]
    [InlineData("Thomas", 20, 0)]
    [InlineData("Thomas", 20, -1)]
    internal async Task TrainService_RegisterAsync_ThrowsApplicationException(string name, int seatCount, int trainManufacturerID)
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

    [Fact]
    internal async Task TrainService_UpdateInfoAsync_ReturnsValidObject()
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

        train.Name = "Henry";
        train.SeatCount = 30;
        train.TrainManufacturerID = 2;

        await trainService.UpdateInfoAsync(train);

        // Assert
        var savedTrains = await repository.GetAllAsync();
        var savedTrain = Assert.Single(savedTrains);

        Assert.Equal("Henry", savedTrain.Name);
        Assert.Equal(30, savedTrain.SeatCount);
        Assert.Equal(2, savedTrain.TrainManufacturerID);
    }

    [Fact]
    internal async Task TrainService_UpdateInfoAsync_ThrowsArgumentNullException()
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

        train = null;

        var method = async () => await trainService.UpdateInfoAsync(train);

        // Assert
        await Assert.ThrowsAsync<ArgumentNullException>(method);
    }

    [Theory]
    [InlineData("", 20, 1)]
    [InlineData(null, 20, 1)]
    [InlineData("Thomas", 0, 1)]
    [InlineData("Thomas", 1001, 1)]
    [InlineData("Thomas", 20, 0)]
    [InlineData("Thomas", 20, -1)]
    internal async Task TrainService_UpdateIndoAsync_ThrowsApplicationException(string name, int seatCount, int trainManufacturerID)
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

        train.Name = name;
        train.SeatCount = seatCount;
        train.TrainManufacturerID = trainManufacturerID;

        var method = async () => await trainService.RegisterAsync(train);

        // Assert
        await Assert.ThrowsAsync<ApplicationException>(method);
    }

    [Fact]
    internal async Task TrainService_GetByIdAsync_ReturnsValidObject()
    {
        // Arrange
        var repository = new FakeBaseRepository<Train>();
        var trainService = new TrainService(repository);
        var train = new Train
        {
            ID = 1,
            Name = "Thomas",
            SeatCount = 20,
            TrainManufacturerID = 1
        };

        // Act
        await trainService.RegisterAsync(train);

        Train? demandedTrain = await trainService.GetByIdAsync(1);

        // Assert
        Assert.Equal(train.ID, demandedTrain.ID);
        Assert.Equal(train.Name, demandedTrain.Name);
        Assert.Equal(train.SeatCount, demandedTrain.SeatCount);
        Assert.Equal(train.TrainManufacturerID, demandedTrain.TrainManufacturerID);
    }

    [Fact]
    internal async Task TrainService_GetByIdAsync_ReturnsNull()
    {
        // Arrange
        var repository = new FakeBaseRepository<Train>();
        var trainService = new TrainService(repository);
        var train = new Train
        {
            ID = 1,
            Name = "Thomas",
            SeatCount = 20,
            TrainManufacturerID = 1
        };

        // Act
        await trainService.RegisterAsync(train);

        Train? demandedTrain = await trainService.GetByIdAsync(2);

        // Assert
        Assert.Null(demandedTrain);
    }

    [Theory]
    [InlineData(-1)]
    [InlineData(0)]
    internal async Task TrainService_GetByIdAsync_ThrowsApplicationException(int id)
    {
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

        var method = async () => await trainService.GetByIdAsync(id);

        // Assert
        await Assert.ThrowsAsync<ApplicationException>(method);
    }

    [Fact]
    internal async Task TrainService_ListAllAsync_ReturnsValidObjectList()
    {
        // Arrange
        var repository = new FakeBaseRepository<Train>();
        var trainService = new TrainService(repository);
        var lines = new List<Line>()
        {
            new() { ID = 1 },
            new() { ID = 2 },
            new() { ID = 3 }
        };

        var trains = new List<Train>()
        {
            new() { ID = 1, TrainManufacturerID = 1, Name = "Marcus", SeatCount = 60, Lines = lines },
            new() { ID = 2, TrainManufacturerID = 2, Name = "Piercy", SeatCount = 50, Lines = lines },
            new() { ID = 3, TrainManufacturerID = 1, Name = "Henry", SeatCount = 76 },
            new() { ID = 4, TrainManufacturerID = 2, Name = "Thomas", SeatCount = 100 }
        };

        // Act
        foreach (var train in trains)
        {
            await trainService.RegisterAsync(train);
        }

        var demandedTrains = await trainService.ListAllAsync();

        // Arange

        Assert.Equal(trains, demandedTrains);
    }

    [Fact]
    internal async Task TrainService_ListAllAsync_ReturnsEmptyICollection()
    {
        // Arrange
        var repository = new FakeBaseRepository<Train>();
        var trainService = new TrainService(repository);

        // Act
        var demandedTrains = await trainService.ListAllAsync();

        // Arange

        Assert.Equal(demandedTrains, []);
    }

    [Fact]
    internal async Task TrainService_ListAllActiveAsync_ReturnsValidObjectList()
    {
        // Arrange
        var repository = new FakeBaseRepository<Train>();
        var trainService = new TrainService(repository);
        var lines = new List<Line>()
        {
            new() { ID = 1 },
            new() { ID = 2 },
            new() { ID = 3 }
        };

        var trains = new List<Train>()
        {
            new() { ID = 1, TrainManufacturerID = 1, Name = "Marcus", SeatCount = 60, Lines = lines },
            new() { ID = 2, TrainManufacturerID = 2, Name = "Piercy", SeatCount = 50, Lines = lines },
            new() { ID = 3, TrainManufacturerID = 1, Name = "Henry", SeatCount = 76 },
            new() { ID = 4, TrainManufacturerID = 2, Name = "Thomas", SeatCount = 100 }
        };

        var activeTrains = new List<Train>()
        {
            new() { ID = 1, TrainManufacturerID = 1, Name = "Marcus", SeatCount = 60, Lines = lines },
            new() { ID = 2, TrainManufacturerID = 2, Name = "Piercy", SeatCount = 50, Lines = lines }
        };

        // Act
        foreach (var train in trains)
        {
            await trainService.RegisterAsync(train);
        }

        var demandedTrains = await trainService.ListAllActiveAsync();

        // Arange

        Assert.Equivalent(activeTrains, demandedTrains);
    }

    [Fact]
    internal async Task TrainService_ListAllActiveAsync_ReturnsEmptyICollection()
    {
        // Arrange
        var repository = new FakeBaseRepository<Train>();
        var trainService = new TrainService(repository);

        // Act
        var demandedTrains = await trainService.ListAllActiveAsync();

        // Arange

        Assert.Equal(demandedTrains, []);
    }

    [Fact]
    internal async Task TrainService_ListAllInactiveAsync_ReturnsValidObjectList()
    {
        // Arrange
        var repository = new FakeBaseRepository<Train>();
        var trainService = new TrainService(repository);
        var lines = new List<Line>()
        {
            new() { ID = 1 },
            new() { ID = 2 },
            new() { ID = 3 }
        };

        var trains = new List<Train>()
        {
            new() { ID = 1, TrainManufacturerID = 1, Name = "Marcus", SeatCount = 60, Lines = lines },
            new() { ID = 2, TrainManufacturerID = 2, Name = "Piercy", SeatCount = 50, Lines = lines },
            new() { ID = 3, TrainManufacturerID = 1, Name = "Henry", SeatCount = 76 },
            new() { ID = 4, TrainManufacturerID = 2, Name = "Thomas", SeatCount = 100 }
        };

        var inactiveTrains = new List<Train>()
        {
            new() { ID = 3, TrainManufacturerID = 1, Name = "Henry", SeatCount = 76 },
            new() { ID = 4, TrainManufacturerID = 2, Name = "Thomas", SeatCount = 100 }
        };

        // Act
        foreach (var train in trains)
        {
            await trainService.RegisterAsync(train);
        }

        var demandedTrains = await trainService.ListAllInactiveAsync();

        // Arange

        Assert.Equivalent(inactiveTrains, demandedTrains);
    }

    [Fact]
    internal async Task TrainService_ListAllInactiveAsync_ReturnsEmptyICollection()
    {
        // Arrange
        var repository = new FakeBaseRepository<Train>();
        var trainService = new TrainService(repository);

        // Act
        var demandedTrains = await trainService.ListAllInactiveAsync();

        // Arange

        Assert.Equal(demandedTrains, []);
    }
}
