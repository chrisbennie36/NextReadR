namespace NextReadR.Data.Entities;

public class Book
{
    public long Id { get; set; }
    public string Title { get; set; }
    public string Author { get; set; }
    public string Description { get; set; }
    public IList<string> Genres { get; set; } = [];
    public int PublishedYear { get; set; }
    public int PageCount { get; set; }
    public double Rating { get; set; }
    public int RatingCount { get; set; }
    public string Language { get; set; } = "";
    public string Isbn { get; set; } = "";
    public List<string> Themes { get; set; } = [];
    public ReadingHistory? ReadingHistory { get; set; }
    //public float[]? Embedding { get; set; }
}