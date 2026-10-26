using NextReader.AI.Dtos;
using NextReadR.Data.Repositories;
using NextReadR.Data.SemanticSearch;

namespace NextReader.AI.Tools;

public class BookTools
{
    private const int MaxBooksToReturn = 10;
    
    private readonly IBookRepository bookRepository;
    private readonly ISemanticBookSearch semanticBookSearch;

    public BookTools(IBookRepository bookRepository, ISemanticBookSearch semanticBookSearch)
    {
        this.bookRepository = bookRepository;
        this.semanticBookSearch = semanticBookSearch;
    }

    //ToDo: Add Validation
    public async Task<IEnumerable<BookDto>> SearchBooksByGenre(string genre)
    {
        var books = (await bookRepository.GetByGenre(genre)).Take(MaxBooksToReturn);
        return books.Select(b => new BookDto(b.Title, b.Author, b.Genres, b.Description, b.Isbn, b.PublishedYear, b.Themes));
    }

    //ToDo: Generic Search books that does both genre and keyword search, let the AI decide
    public async Task<IEnumerable<BookDto>> SearchBooksByKeywords(string keywords)
    {
        var books = (await bookRepository.GetByKeywords(keywords)).Take(MaxBooksToReturn);
        return books.Select(b => new BookDto(b.Title, b.Author, b.Genres, b.Description, b.Isbn, b.PublishedYear, b.Themes));
    }

    public async Task<IEnumerable<BookDto>> SearchBooksSemantically(string query)
    {
        var books = await semanticBookSearch.SearchAsync(query);
        return books.Select(b => new BookDto(b.Title, b.Author, b.Genres, b.Description, b.Isbn, b.PublishedYear, b.Themes));
    }
}