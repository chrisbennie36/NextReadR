using Microsoft.EntityFrameworkCore;
using NextReadR.Data.Entities;

namespace NextReadR.Data.Repositories;

using System.Text.Json;

public class BookRepository : IBookRepository
{
    private readonly string seedDataLocation = "C:\\Users\\christopherbennie\\Desktop\\NextReadRBooks\\Books.json";

    private readonly IBookEmbeddingRepository bookEmbeddingRepository;
    
    public BookRepository(IBookEmbeddingRepository bookEmbeddingRepository)
    {
        this.bookEmbeddingRepository = bookEmbeddingRepository;
    }
    
    public async Task<IEnumerable<Book>> GetAll()
    {
       await using var db = new BookContext();
       
       return await db.Books
           .OrderBy(b => b.Id)
           .ToListAsync();
    }

    public async Task<Book> GetById(long id)
    {
        await using var db = new BookContext();
       
        return await db.Books
            .FirstOrDefaultAsync(b => b.Id == id);
    }

    public async Task<IEnumerable<Book>> GetByIds(IEnumerable<long> ids)
    {
        await using var db = new BookContext();
       
        return await db.Books
            .Where(b => ids.Contains(b.Id))
            .ToListAsync();
    }

    public async Task<IEnumerable<Book>> GetByGenre(string genre)
    {
        await using var db = new BookContext();
       
        return await db.Books
            .Where(b => b.Genres.Contains(genre))
            .OrderBy(b => b.Id)
            .ToListAsync();
    }
    
    public async Task<IEnumerable<Book>> GetByKeywords(string keywords)
    {
        await using var db = new BookContext();
       
        return await db.Books
            .Where(b => b.Description.Contains(keywords) || b.Themes.Contains(keywords))
            .OrderBy(b => b.Id)
            .ToListAsync();
    }

    public Task Create(Book book)
    {
        throw new NotImplementedException();
    }

    public async Task Create(ReadingHistory readingHistory)
    {
        await using var db = new BookContext();

        var existingReadingHistory = await db.ReadingHistory
            .FirstOrDefaultAsync(r => r.Id == readingHistory.Id);

        if (existingReadingHistory is null)
        {
            throw new InvalidOperationException($"ReadingHistory with Id {readingHistory.Id} was not found.");
        }

        var book = new Book
        {
            Title = readingHistory.Title,
            Description = string.Empty,
            Author = readingHistory.Author,
            Isbn = readingHistory.ISBN
        };

        await db.AddAsync(book);
        await db.SaveChangesAsync();

        //ToDo: Correct to have two SaveChangesAsync here?
        existingReadingHistory.BookId = book.Id;
        await db.SaveChangesAsync();
    }

    public async Task CreateMany(IEnumerable<Book> books)
    {
        await using var db = new BookContext();

        foreach (var book in books)
        {
            db.Add(book);
        }

        await db.SaveChangesAsync();
    }

    public Task Update(Book book)
    {
        throw new NotImplementedException();
    }

    public Task Delete(Book book)
    {
        throw new NotImplementedException();
    }

    public Task DeleteMany(IEnumerable<Book> books)
    {
        throw new NotImplementedException();
    }

    public async Task Seed()
    {
        var booksToSeed = JsonSerializer.Deserialize<IEnumerable<Book>>(File.ReadAllText(seedDataLocation));

        await CreateMany(booksToSeed);

        //Todo: Disabled for now as we want to create the embedding once we have the genres and descriptions populated
        //var createdBooks = await GetAll();

        /*foreach (var createdBook in createdBooks)
        {
            await bookEmbeddingRepository.AddBookEmbedding(createdBook, "text-embedding-3-large");
        }*/
    }

    public async Task UpdateMetadatasForBook(string title, string genres, string description)
    {
        await using var db = new BookContext();
       
        var book = await db.Books
            .Where(b => b.Title.ToLower().Contains(title.ToLower()))
            .OrderBy(b => b.Id)
            .FirstOrDefaultAsync();

        if (book == null)
        {
            throw new NullReferenceException($"Book with title {title} not found in database");
        }

        book.Genres = genres.Split(',').Select(g => g.Trim()).ToList();
        book.Description = description;

        await db.SaveChangesAsync();
        
        await bookEmbeddingRepository.AddBookEmbedding(book, "text-embedding-3-large");
    }
}