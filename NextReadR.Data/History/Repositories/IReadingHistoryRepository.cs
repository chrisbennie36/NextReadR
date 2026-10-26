using NextReadR.Data.Entities;

namespace NextReadR.Data.Repositories;

public interface IReadingHistoryRepository
{
    Task<IEnumerable<ReadingHistory>> GetReadingHistory();
    Task<IEnumerable<ReadingHistory>> GetReadingHistoryForYear(int yearRead);
    Task PopulateReadingHistory();
}