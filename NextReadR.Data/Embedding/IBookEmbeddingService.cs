using NextReadR.Data.Entities;

namespace NextReadR.Data.Embedding;

public interface IBookEmbeddingService
{
    Task<float[]> GenerateEmbedding(Book book, CancellationToken cancellationToken = default);
    Task<float[]> GenerateEmbedding(string query, CancellationToken cancellationToken = default);

}