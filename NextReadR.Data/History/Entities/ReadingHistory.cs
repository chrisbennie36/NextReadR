namespace NextReadR.Data.Entities;

public class ReadingHistory
{
    public long Id { get; set; }
    public long? BookId { get; set; }
    public Book? Book { get; set; }
    public string Title { get; set; }
    public string Author { get; set; }
    public int Rating { get; set; }
    public string ISBN { get; set; }
    public string DetailedReview { get; set; }
    public ShelfName ShelfName { get; set; }
    public DateTime? DateRead { get; set; }
}

public enum ShelfName
{
    Read,
    CurrentlyReading,
    WantToRead
}