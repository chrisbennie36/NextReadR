using NextReadR.Data.Entities;

namespace NextReadR.Data.Repositories;

public interface IBookRepository
{
    public Task<IEnumerable<Book>> GetAll();
    public Task<Book> GetById(long id);
    public Task<IEnumerable<Book>> GetByIds(IEnumerable<long> ids);
    public Task<IEnumerable<Book>> GetByGenre(string genre);
    public Task<IEnumerable<Book>> GetByKeywords(string keywords);
    public Task Create(Book book);
    public Task Create(ReadingHistory readingHistory);
    Task CreateMany(IEnumerable<Book> books);
    public Task Update(Book book);
    public Task Delete(Book book);
    public Task DeleteMany(IEnumerable<Book> books);
    public Task Seed();
    public Task UpdateMetadatasForBook(string title, string genres, string description);
}