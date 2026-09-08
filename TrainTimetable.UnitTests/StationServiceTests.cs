using TrainTimetable.Business.Services;
using TrainTimetable.Data.Entities;
using TrainTimetable.UnitTests.Repositories;

namespace TrainTimetable.UnitTests;

public class StationServiceTests
{
    [Fact]
    internal async Task StationService_FetchStationItemsAsync_ReturnsObjectListContainingAllEnitites()
    {
        // Arrange
        var repository = new FakeBaseRepository<Station>();
        var stationService = new StationService(repository);
        var stations = new List<Station>()
        {
            new() { ID = 1,  Name = "1 Station", CountryID = 1},
            new() { ID = 2,  Name = "11 Station", CountryID = 1},
            new() { ID = 3,  Name = "2 Station", CountryID = 1},
            new() { ID = 4,  Name = "22 Station", CountryID = 1}
        };

        IEnumerable<KeyValuePair<int, string>> expectedStations = new List<KeyValuePair<int, string>>()
        {
            new(1, "1 Station"),
            new(2, "11 Station"),
            new(3, "2 Station"),
            new(4, "22 Station")
        };

        // Act
        foreach (var station in stations) {
            await repository.InsertAsync(station);
        }

        var actualStations = await stationService.FetchStationItemsAsync(string.Empty);

        // Assert
        Assert.Equal(expectedStations, actualStations);
    }

    [Fact]
    internal async Task StationService_FetchStationItemsAsync_ReturnsValidObjectList()
    {
        // Arrange
        var repository = new FakeBaseRepository<Station>();
        var stationService = new StationService(repository);
        var stations = new List<Station>()
        {
            new() { ID = 1,  Name = "1 Station", CountryID = 1},
            new() { ID = 2,  Name = "11 Station", CountryID = 1},
            new() { ID = 3,  Name = "2 Station", CountryID = 1},
            new() { ID = 4,  Name = "22 Station", CountryID = 1}
        };

        var expectedStations = new List<KeyValuePair<int, string>>()
        {
            new(1, "1 Station"),
            new(2, "11 Station")
        };

        // Act
        foreach (var station in stations)
        {
            await repository.InsertAsync(station);
        }

        var actualStations = await stationService.FetchStationItemsAsync("1");

        // Assert
        Assert.Equal(expectedStations, actualStations);
    }

    [Fact]
    internal async Task StationService_FetchStationItemsAsync_ReturnsEmptyObjectList()
    {
        // Arrange
        var repository = new FakeBaseRepository<Station>();
        var stationService = new StationService(repository);
        var stations = new List<Station>()
        {
            new() { ID = 1,  Name = "1 Station", CountryID = 1},
            new() { ID = 2,  Name = "11 Station", CountryID = 1},
            new() { ID = 3,  Name = "2 Station", CountryID = 1},
            new() { ID = 4,  Name = "22 Station", CountryID = 1}
        };

        // Act
        foreach (var station in stations)
        {
            await repository.InsertAsync(station);
        }

        var actualStations = await stationService.FetchStationItemsAsync("3");

        // Assert
        Assert.Empty(actualStations);
    }

    [Fact]
    internal async Task StationService_FetchStationItemsAsync_ThrowsArgumentNullException()
    {
        // Arrange
        var repository = new FakeBaseRepository<Station>();
        var stationService = new StationService(repository);
        string? stationName = null;

        // Act
        var method = async () => await stationService.FetchStationItemsAsync(stationName);

        // Assert
        await Assert.ThrowsAsync<ArgumentNullException>(method);
    }
}
