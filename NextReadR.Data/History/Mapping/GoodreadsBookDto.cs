using CsvHelper.Configuration.Attributes;

namespace NextReadR.Data.History.Mapping;

public class GoodreadsBookDto
{
    public string Author { get; set; }
    public string Title { get; set; }
    public string ISBN { get; set; }
    [Name("My Rating")]
    public decimal Rating { get; set; }
    [Name("Date Read")]
    public  string DateRead { get; set; }
    [Name("Exclusive Shelf")]
    public string ShelfName { get; set; }
    [Name("My Review")]
    public string DetailedReview { get; set; }
}