using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Transactions;
using TrainTimetable.Data.Entities;
using TrainTimetable.Data.Repositories;

namespace TrainTimetable.Business.Services;

public interface ITrainService
{
    Task RegisterAsync(Train train);

    Task UpdateInfoAsync(Train train);

    Task RemoveAsync(Train train);

    Task<Train?> FetchByIdAsync(int id);

    Task<IEnumerable<Train>> FetchAllAsync();

    Task<bool> IsTrainNumberUniqueAsync(int trainNumber, int exception);

    Task<bool> IsNameUniqueAsync(string name, string exception);
}

public class TrainService(
    IBaseRepository<Train> trainRepository) : ITrainService
{
    private static async Task ValidateTrain(Train train, bool isNew = false)
    {
        ArgumentNullException.ThrowIfNull(train);

        if (train.TrainNumber < 1 || train.TrainNumber > 1000000)
        {
            throw new ApplicationException("Invalid Train Number. Train Number must be at least 1.");
        }

        if (string.IsNullOrWhiteSpace(train.Name) || train.Name.Length > 250)
        {
            throw new ApplicationException("Invalid name. Name must not be null or empty and must be between 1 and 250 characters long.");
        }

        if (train.SeatCount < 1 || train.SeatCount > 1000)
        {
            throw new ApplicationException("Invalid number of seats. Valid range is 1 to 1000.");
        }

        if (train.TrainManufacturerID < 1)
        {
            throw new ApplicationException("Invalid manufacturer ID. Manufacturer ID must be at least 1.");
        }

        if (train.BikeSpaceCount < 0)
        {
            throw new ApplicationException("Invalid bike space count. Bike space count cannot be lower than 0.");
        }

        if (train.DisabledSeatCount < 0 || train.DisabledSeatCount > train.SeatCount)
        {
            throw new ApplicationException("Invalid seat count. Seat count cannot be lower than 0 nor higher than the seat count.");
        }
    }

    public async Task RegisterAsync(Train train)
    {
        await ValidateTrain(train, true);

        await trainRepository.InsertAsync(train);
    }

    public async Task UpdateInfoAsync(Train train)
    {
        await ValidateTrain(train);

        await trainRepository.UpdateAsync(train);
    }

    public async Task RemoveAsync(Train train)
    {
        await trainRepository.DeleteAsync(train);
    }

    public async Task<Train?> FetchByIdAsync(int id)
    {
        if (id <= 0)
            throw new ApplicationException("Invalid search ID. ID must be at least 1.");

        var query = await trainRepository.BuildQueryAsync(_ => _.ID == id,
            _ => _.Include(_ => _.TrainManufacturer));

        return query.FirstOrDefault();
    }

    public async Task<IEnumerable<Train>> FetchAllAsync()
    {
        var query = await trainRepository.BuildQueryAsync(_ => true,
            _ => _.Include(_ => _.TrainManufacturer));

        return query ?? [];
    }

    public async Task<bool> IsTrainNumberUniqueAsync(int trainNumber, int exception = 0)
    {
        var query = await trainRepository.BuildQueryAsync(
            _ => _.TrainNumber == trainNumber && _.TrainNumber != exception
        );

        return !query.Any();
    }

    public async Task<bool> IsNameUniqueAsync(string name, string exception = "")
    {
        var query = await trainRepository.BuildQueryAsync(
            _ => _.Name == name && _.Name != exception
        );

        return !query.Any();
    }
}
