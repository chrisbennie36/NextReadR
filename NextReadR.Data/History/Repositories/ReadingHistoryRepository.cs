using Microsoft.EntityFrameworkCore;
using NextReadR.Data.Entities;
using NextReadR.Data.History.Mapping;

namespace NextReadR.Data.Repositories;

public class ReadingHistoryRepository : IReadingHistoryRepository
{
    private readonly ReadingHistoryMapper mapper = new();
    
    private readonly string goodreadsExportLocation = "C:\\Users\\christopherbennie\\Desktop\\NextReadRBooks\\GoodreadsExport.csv";

    public async Task<IEnumerable<ReadingHistory>> GetReadingHistory()
    {
        await using var db = new BookContext();
       
        return await db.ReadingHistory
            .OrderBy(b => b.Id)
            .Where(r => r.ShelfName == ShelfName.Read)
            .ToListAsync();
    }

    public Task<IEnumerable<ReadingHistory>> GetReadingHistoryForYear(int yearRead)
    {
        throw new NotImplementedException();
    }
    
    public async Task CreateMany(IEnumerable<ReadingHistory> readingHistories)
    {
        await using var db = new BookContext();

        foreach (var readingHistory in readingHistories)
        {
            db.Add(readingHistory);
        }

        await db.SaveChangesAsync();
    }

    public async Task PopulateReadingHistory()
    {
        var readingHistoriesToSeed = new ReadingHistoryMapper().MapReadingHistoryFromGoodreads(goodreadsExportLocation);

        await CreateMany(readingHistoriesToSeed);    
    }
}