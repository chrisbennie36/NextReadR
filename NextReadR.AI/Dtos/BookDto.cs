namespace NextReader.AI.Dtos;

public record BookDto(string Title, string Author, IEnumerable<string> Genres, string Description, string Isbn, int PublishedYear, IEnumerable<string> Keywords);