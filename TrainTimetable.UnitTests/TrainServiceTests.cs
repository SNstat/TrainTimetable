using TrainTimetable.Business.Services;
using TrainTimetable.Data.Entities;
using TrainTimetable.UnitTests.FakeRepositories;

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
            TrainNumber = 1000,
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

        Train train = null!;

        // Act
        var method = async () => await trainService.RegisterAsync(train!);

        // Assert
        await Assert.ThrowsAsync<ArgumentNullException>(method);
    }

    [Theory]
    [InlineData("", 20, 1, 0, 0)]
    [InlineData("Thomas", 0, 1, 0, 0)]
    [InlineData("Thomas", 1001, 1, 0, 0)]
    [InlineData("Thomas", 20, 0, 0, 0)]
    [InlineData("Thomas", 20, -1, 0, 0)]
    [InlineData("Thomas", 20, 1, -1, 0)]
    [InlineData("Thomas", 20, 1, 21, 0)]
    [InlineData("Thomas", 20, 1, 0, -1)]
    internal async Task TrainService_RegisterAsync_ThrowsApplicationException(string name, int seatCount, int trainManufacturerID, int disabledSeatCount, int bikeSpaceCount)
    {
        // Arrange
        var repository = new FakeBaseRepository<Train>();
        var trainService = new TrainService(repository);

        var train = new Train {
            Name = name,
            SeatCount = seatCount,
            TrainManufacturerID = trainManufacturerID,
            DisabledSeatCount = disabledSeatCount,
            BikeSpaceCount = bikeSpaceCount
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
            TrainNumber = 1000,
            Name = "Thomas",
            SeatCount = 20,
            TrainManufacturerID = 1
        };

        // Act
        await trainService.RegisterAsync(train);

        train.TrainNumber = 1001;
        train.Name = "Henry";
        train.SeatCount = 30;
        train.TrainManufacturerID = 2;

        await trainService.UpdateInfoAsync(train);

        // Assert
        var savedTrains = await repository.GetAllAsync();
        var savedTrain = Assert.Single(savedTrains);

        Assert.Equal(1001, savedTrain.TrainNumber);
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
            TrainNumber= 1000,
            Name = "Thomas",
            SeatCount = 20,
            TrainManufacturerID = 1
        };

        // Act
        await trainService.RegisterAsync(train);

        train = null;

        var method = async () => await trainService.UpdateInfoAsync(train!);

        // Assert
        await Assert.ThrowsAsync<ArgumentNullException>(method);
    }

    [Theory]
    [InlineData(1001, "", 20, 1)]
    [InlineData(1001, "Thomas", 0, 1)]
    [InlineData(1001, "Thomas", 1001, 1)]
    [InlineData(1001, "Thomas", 20, 0)]
    [InlineData(1001, "Thomas", 20, -1)]
    [InlineData(-1, "Thomas", 20, 1)]
    [InlineData(9999999, "Thomas", 20, 1)]
    internal async Task TrainService_UpdateInfoAsync_ThrowsApplicationException(int trainNumber, string name, int seatCount, int trainManufacturerID)
    {
        // Arrange
        var repository = new FakeBaseRepository<Train>();
        var trainService = new TrainService(repository);

        var train = new Train
        {
            TrainNumber = 1000,
            Name = "Thomas",
            SeatCount = 20,
            TrainManufacturerID = 1
        };

        // Act
        await trainService.RegisterAsync(train);

        train.TrainNumber = trainNumber;
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
            TrainNumber = 1000,
            Name = "Thomas",
            SeatCount = 20,
            TrainManufacturerID = 1
        };

        // Act
        await trainService.RegisterAsync(train);

        Train? demandedTrain = await trainService.FetchByIdAsync(1);

        // Assert
        Assert.Equal(train.ID, demandedTrain?.ID);
        Assert.Equal(train.Name, demandedTrain?.Name);
        Assert.Equal(train.SeatCount, demandedTrain?.SeatCount);
        Assert.Equal(train.TrainManufacturerID, demandedTrain?.TrainManufacturerID);
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
            TrainNumber = 1000,
            Name = "Thomas",
            SeatCount = 20,
            TrainManufacturerID = 1
        };

        // Act
        await trainService.RegisterAsync(train);

        Train? demandedTrain = await trainService.FetchByIdAsync(2);

        // Assert
        Assert.Null(demandedTrain);
    }

    [Theory]
    [InlineData(-1)]
    [InlineData(0)]
    internal async Task TrainService_GetByIdAsync_ThrowsApplicationException(int id)
    {
        // Arrange
        var repository = new FakeBaseRepository<Train>();
        var trainService = new TrainService(repository);

        var train = new Train
        {
            TrainNumber = 1000,
            Name = "Thomas",
            SeatCount = 20,
            TrainManufacturerID = 1
        };

        // Act
        await trainService.RegisterAsync(train);

        var method = async () => await trainService.FetchByIdAsync(id);

        // Assert
        await Assert.ThrowsAsync<ApplicationException>(method);
    }

    [Fact]
    internal async Task TrainService_ListAllAsync_ReturnsValidObjectList()
    {
        // Arrange
        var repository = new FakeBaseRepository<Train>();
        var repositoryTrainManufacturer = new FakeBaseRepository<TrainManufacturer>();
        var trainService = new TrainService(repository);

        var lineSchedules = new List<LineSchedule>()
        {
            new() { ID = 1 },
            new() { ID = 2 },
            new() { ID = 3 }
        };

        var trainManufacturers = new List<TrainManufacturer>()
        {
            new() { ID = 1, Name = "Manufacturer1" },
            new() { ID = 2, Name = "Manufacturer2" }
        };

        var trains = new List<Train>()
        {
            new() { ID = 1, TrainNumber = 1001, TrainManufacturerID = 1, Name = "Marcus", SeatCount = 60, LineSchedules = lineSchedules },
            new() { ID = 2, TrainNumber = 1002, TrainManufacturerID = 2, Name = "Piercy", SeatCount = 50, LineSchedules = lineSchedules },
            new() { ID = 3, TrainNumber = 1003, TrainManufacturerID = 1, Name = "Henry", SeatCount = 76 },
            new() { ID = 4, TrainNumber = 1004, TrainManufacturerID = 2, Name = "Thomas", SeatCount = 100 }
        };

        // Act
        foreach (var manufacturer in trainManufacturers)
        {
            await repositoryTrainManufacturer.InsertAsync(manufacturer);
        }

        foreach (var train in trains)
        {
            await trainService.RegisterAsync(train);
        }

        var demandedTrains = await trainService.FetchAllAsync();

        // Assert
        Assert.Equal(trains, demandedTrains);
    }

    [Fact]
    internal async Task TrainService_ListAllAsync_ReturnsEmptyICollection()
    {
        // Arrange
        var repository = new FakeBaseRepository<Train>();
        var trainService = new TrainService(repository);

        // Act
        var demandedTrains = await trainService.FetchAllAsync();

        // Assert
        Assert.Empty(demandedTrains);
    }

    [Fact]
    internal async Task TrainService_IsTrainNumberUniqueAsync_ReturnsTrue()
    {
        // Arrange
        var repository = new FakeBaseRepository<Train>();
        var trainService = new TrainService(repository);

        int uniqueTrainNumber = 1000;

        var lineSchedules = new List<LineSchedule>()
        {
            new() { ID = 1 },
            new() { ID = 2 },
            new() { ID = 3 }
        };

        var trains = new List<Train>()
        {
            new() { ID = 1, TrainNumber = 1001, TrainManufacturerID = 1, Name = "Marcus", SeatCount = 60, LineSchedules = lineSchedules },
            new() { ID = 2, TrainNumber = 1002, TrainManufacturerID = 2, Name = "Piercy", SeatCount = 50, LineSchedules = lineSchedules },
            new() { ID = 3, TrainNumber = 1003, TrainManufacturerID = 1, Name = "Henry", SeatCount = 76 },
            new() { ID = 4, TrainNumber = 1004, TrainManufacturerID = 2, Name = "Thomas", SeatCount = 100 }
        };

        // Act

        foreach (var trainsItem in trains)
        {
            await repository.InsertAsync(trainsItem);
        }

        bool isTrainNumberUnique = await trainService.IsTrainNumberUniqueAsync(uniqueTrainNumber);

        // Assert
        Assert.True(isTrainNumberUnique);
    }

    [Theory]
    [InlineData(1001)]
    [InlineData(1002)]
    [InlineData(1003)]
    [InlineData(1004)]
    internal async Task TrainService_IsTrainNumberUniqueAsync_ReturnsFalse(int trainNumber)
    {
        // Arrange
        var repository = new FakeBaseRepository<Train>();
        var trainService = new TrainService(repository);

        var lineSchedules = new List<LineSchedule>()
        {
            new() { ID = 1 },
            new() { ID = 2 },
            new() { ID = 3 }
        };

        var trains = new List<Train>()
        {
            new() { ID = 1, TrainNumber = 1001, TrainManufacturerID = 1, Name = "Marcus", SeatCount = 60, LineSchedules = lineSchedules },
            new() { ID = 2, TrainNumber = 1002, TrainManufacturerID = 2, Name = "Piercy", SeatCount = 50, LineSchedules = lineSchedules },
            new() { ID = 3, TrainNumber = 1003, TrainManufacturerID = 1, Name = "Henry", SeatCount = 76 },
            new() { ID = 4, TrainNumber = 1004, TrainManufacturerID = 2, Name = "Thomas", SeatCount = 100 }
        };

        // Act

        foreach (var trainsItem in trains)
        {
            await repository.InsertAsync(trainsItem);
        }

        bool isTrainNumberUnique = await trainService.IsTrainNumberUniqueAsync(trainNumber);

        // Assert
        Assert.False(isTrainNumberUnique);
    }

    [Fact]
    internal async Task TrainService_IsTrainNameUniqueAsync_ReturnsTrue()
    {
        // Arrange
        var repository = new FakeBaseRepository<Train>();
        var trainService = new TrainService(repository);

        string uniqueTrainName = "TrainName";

        var lineSchedules = new List<LineSchedule>()
        {
            new() { ID = 1 },
            new() { ID = 2 },
            new() { ID = 3 }
        };

        var trains = new List<Train>()
        {
            new() { ID = 1, TrainNumber = 1001, TrainManufacturerID = 1, Name = "Marcus", SeatCount = 60, LineSchedules = lineSchedules },
            new() { ID = 2, TrainNumber = 1002, TrainManufacturerID = 2, Name = "Piercy", SeatCount = 50, LineSchedules = lineSchedules },
            new() { ID = 3, TrainNumber = 1003, TrainManufacturerID = 1, Name = "Henry", SeatCount = 76 },
            new() { ID = 4, TrainNumber = 1004, TrainManufacturerID = 2, Name = "Thomas", SeatCount = 100 }
        };

        // Act

        foreach (var trainsItem in trains)
        {
            await repository.InsertAsync(trainsItem);
        }

        bool isTrainNumberUnique = await trainService.IsNameUniqueAsync(uniqueTrainName);

        // Assert
        Assert.True(isTrainNumberUnique);
    }

    [Theory]
    [InlineData("Marcus")]
    [InlineData("Piercy")]
    [InlineData("Henry")]
    [InlineData("Thomas")]
    internal async Task TrainService_IsTrainNameUniqueAsync_ReturnsFalse(string trainName)
    {
        // Arrange
        var repository = new FakeBaseRepository<Train>();
        var trainService = new TrainService(repository);

        var lineSchedules = new List<LineSchedule>()
        {
            new() { ID = 1 },
            new() { ID = 2 },
            new() { ID = 3 }
        };

        var trains = new List<Train>()
        {
            new() { ID = 1, TrainNumber = 1001, TrainManufacturerID = 1, Name = "Marcus", SeatCount = 60, LineSchedules = lineSchedules },
            new() { ID = 2, TrainNumber = 1002, TrainManufacturerID = 2, Name = "Piercy", SeatCount = 50, LineSchedules = lineSchedules },
            new() { ID = 3, TrainNumber = 1003, TrainManufacturerID = 1, Name = "Henry", SeatCount = 76 },
            new() { ID = 4, TrainNumber = 1004, TrainManufacturerID = 2, Name = "Thomas", SeatCount = 100 }
        };

        // Act

        foreach (var trainsItem in trains)
        {
            await repository.InsertAsync(trainsItem);
        }

        bool isTrainNumberUnique = await trainService.IsNameUniqueAsync(trainName);

        // Assert
        Assert.False(isTrainNumberUnique);
    }
}
