using TrainTimetable.Business.Services;
using TrainTimetable.Data.Entities;
using TrainTimetable.UnitTests.FakeRepositories;

namespace TrainTimetable.UnitTests;

public class TrainManufacturerServiceTests
{
    [Fact]
    internal async Task TrainManufacturerService_FetchTrainManufacturerItemsAsync_ReturnsObjectListContainingAllEnitites()
    {
        // Arrange
        var repository = new FakeBaseRepository<TrainManufacturer>();
        var trainManufacturerService = new TrainManufacturerService(repository);
        var manufacturers = new List<TrainManufacturer>()
        {
            new() { ID = 1,  Name = "1 Manufacturer" },
            new() { ID = 2,  Name = "11 Manufacturer" },
            new() { ID = 3,  Name = "2 Manufacturer" },
            new() { ID = 4,  Name = "22 Manufacturer" }
        };

        // Act
        foreach (var manufacturer in manufacturers)
        {
            await repository.InsertAsync(manufacturer);
        }

        var actualManufacturers = await trainManufacturerService.FetchTrainManufacturerItemsAsync(string.Empty);

        // Assert
        Assert.Equal(manufacturers, actualManufacturers);
    }

    [Fact]
    internal async Task StationService_FetchStationItemsAsync_ReturnsValidObjectList()
    {
        // Arrange
        var repository = new FakeBaseRepository<TrainManufacturer>();
        var trainManufacturerService = new TrainManufacturerService(repository);
        var manufacturers = new List<TrainManufacturer>()
        {
            new() { ID = 1,  Name = "1 Manufacturer" },
            new() { ID = 2,  Name = "11 Manufacturer" },
            new() { ID = 3,  Name = "2 Manufacturer" },
            new() { ID = 4,  Name = "22 Manufacturer" }
        };

        var expectedManufacturers = new List<TrainManufacturer>()
        {
            manufacturers[0],
            manufacturers[1]
        };

        // Act
        foreach (var manufacturer in manufacturers)
        {
            await repository.InsertAsync(manufacturer);
        }

        var actualManufacturers = await trainManufacturerService.FetchTrainManufacturerItemsAsync("1");

        // Assert
        Assert.Equal(expectedManufacturers, actualManufacturers);
    }

    [Fact]
    internal async Task StationService_FetchStationItemsAsync_ReturnsEmptyObjectList()
    {
        // Arrange
        var repository = new FakeBaseRepository<TrainManufacturer>();
        var trainManufacturerService = new TrainManufacturerService(repository);
        var manufacturers = new List<TrainManufacturer>()
        {
            new() { ID = 1,  Name = "1 Manufacturer" },
            new() { ID = 2,  Name = "11 Manufacturer" },
            new() { ID = 3,  Name = "2 Manufacturer" },
            new() { ID = 4,  Name = "22 Manufacturer" }
        };

        // Act
        foreach (var manufacturer in manufacturers)
        {
            await repository.InsertAsync(manufacturer);
        }

        var actualManufacturers = await trainManufacturerService.FetchTrainManufacturerItemsAsync("3");

        // Assert
        Assert.Empty(actualManufacturers);
    }
}
