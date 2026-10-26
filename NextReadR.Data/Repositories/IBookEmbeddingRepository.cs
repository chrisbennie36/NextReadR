using NextReadR.Data.Entities;

namespace NextReadR.Data.Repositories;

public interface IBookEmbeddingRepository
{
    Task CreateBookEmbeddingTable();
    Task AddBookEmbedding(Book book, string embeddingModel);
    Task<IEnumerable<BookVectorMatch>> Search(string query);
}