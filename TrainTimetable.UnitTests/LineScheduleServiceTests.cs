using System.Collections;
using TrainTimetable.Business.Models;
using TrainTimetable.Business.Services;
using TrainTimetable.Data.Entities;

namespace TrainTimetable.UnitTests;

public class LineScheduleServiceTests
{
    private static DateOnly CorrectDate => DateOnly.FromDateTime(DateTime.UtcNow.AddDays(1));
    private static DateOnly IncorrectDate => DateOnly.FromDateTime(DateTime.UtcNow.AddDays(-1));

    public class IncorrectTestData : IEnumerable<object[]>
    {
        public IEnumerator<object[]> GetEnumerator()
        {
            yield return new object[] { 0, 1, CorrectDate };
            yield return new object[] { -1, 1, CorrectDate };
            yield return new object[] { 1, 0, CorrectDate };
            yield return new object[] { 1, -1, CorrectDate };
            yield return new object[] { 1, 1, IncorrectDate };
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }

    [Fact]
    internal async Task LineScheduleService_FetchLineItemsAsync_ReturnsEmptyLineItemList()
    {
        // Arrange
        var repository = new FakeBaseRepository<LineSchedule>();
        var lineScheduleService = new LineScheduleService(repository);

        int departureStationId = 1;
        int arrivalStationId = 2;
        var date = CorrectDate;

        // Act
        var method = async () => await lineScheduleService.FetchLineItemsAsync(departureStationId, arrivalStationId, date);
        IEnumerable<LineItem> list = await method();
        
        // Assert
        Assert.Empty(list);
    }

    [Theory]
    [ClassData(typeof(IncorrectTestData))]
    internal async Task LineScheduleService_FetchLineItems_ReturnsApplicationException(int _departureStationID, int _arrivalStationId, DateOnly _date)
    {
        // Arrange
        var repository = new FakeBaseRepository<LineSchedule>();
        var lineScheduleService = new LineScheduleService(repository);

        int departureStationId = _departureStationID;
        int arrivalStationId = _arrivalStationId;
        var date = _date;

        // Act
        var method = async () => await lineScheduleService.FetchLineItemsAsync(departureStationId, arrivalStationId, date);

        // Assert
        await Assert.ThrowsAsync<ApplicationException>(method);
    }
}
