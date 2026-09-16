using System.Collections;
using TrainTimetable.Business.Models;
using TrainTimetable.Business.Services;
using TrainTimetable.Data.Entities;
using TrainTimetable.UnitTests.FakeRepositories;

namespace TrainTimetable.UnitTests;

public class TimetableServiceTests
{
    private static DateOnly CorrectDate => DateOnly.FromDateTime(DateTime.Now.AddDays(1));
    private static DateOnly IncorrectDate => DateOnly.FromDateTime(DateTime.Now.AddDays(-1));

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
    internal async Task TimetableServiceTests_FetchLineItemsAsync_ReturnsEmptyLineItemList()
    {
        // Arrange
        var repository = new FakeBaseRepository<LineSchedule>();
        var pricing = new PaymentService();
        var TimetableServiceTests = new TimetableService(repository, pricing);

        int departureStationId = 1;
        int arrivalStationId = 2;
        var date = CorrectDate;

        // Act
        var method = async () => await TimetableServiceTests.FetchTimetableItemsAsync(departureStationId, arrivalStationId, date, 1);
        IEnumerable<TimetableItem> list = await method();
        
        // Assert
        Assert.Empty(list);
    }

    [Theory]
    [ClassData(typeof(IncorrectTestData))]
    internal async Task TimetableServiceTests_FetchLineItemsAsync_ReturnsApplicationException(int _departureStationID, int _arrivalStationId, DateOnly _date)
    {
        // Arrange
        var repository = new FakeBaseRepository<LineSchedule>();
        var pricing = new PaymentService();
        var TimetableServiceTests = new TimetableService(repository, pricing);

        int departureStationId = _departureStationID;
        int arrivalStationId = _arrivalStationId;
        var date = _date;

        // Act
        var method = async () => await TimetableServiceTests.FetchTimetableItemsAsync(departureStationId, arrivalStationId, date, 1);

        // Assert
        await Assert.ThrowsAsync<ApplicationException>(method);
    }
}
