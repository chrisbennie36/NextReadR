using System.Globalization;
using CsvHelper;
using NextReadR.Data.Entities;

namespace NextReadR.Data.History.Mapping;

public class ReadingHistoryMapper
{
    public IEnumerable<ReadingHistory> MapReadingHistoryFromGoodreads(string goodreadsExportLocation)
    {
        using var reader = new StreamReader(goodreadsExportLocation);
        using var csv = new CsvReader(reader, CultureInfo.InvariantCulture);

        return map(csv.GetRecords<GoodreadsBookDto>().ToList());
    }

    private IEnumerable<ReadingHistory> map(IEnumerable<GoodreadsBookDto> records)
    {
        return records.Select(r => new ReadingHistory
        {
            BookId = null,
            Book = null,
            Author = r.Author,
            Title = r.Title,
            ISBN = r.ISBN,
            Rating = (int)r.Rating,
            DateRead = !string.IsNullOrWhiteSpace(r.DateRead) ? DateTime.Parse(r.DateRead) : null,
            ShelfName = mapShelfName(r.ShelfName),
            DetailedReview = r.DetailedReview
        });
    }
    
    private ShelfName mapShelfName(string shelfName)
    {
        return shelfName switch
        {
            "read" => ShelfName.Read,
            "currently-reading" => ShelfName.CurrentlyReading,
            "to-read" => ShelfName.WantToRead,
            _ => throw new InvalidOperationException("Unsupported shelf name: " + shelfName)
        };
    }
}