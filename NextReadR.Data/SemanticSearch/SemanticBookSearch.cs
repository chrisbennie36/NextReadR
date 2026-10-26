using Microsoft.Extensions.AI;
using NextReadR.Data.Embedding;
using NextReadR.Data.Entities;
using NextReadR.Data.Repositories;

namespace NextReadR.Data.SemanticSearch;

public class SemanticBookSearch : ISemanticBookSearch
{
    const int MaxBooks = 5;
    
    private readonly IBookEmbeddingRepository bookEmbeddingRepository;
    private readonly IBookRepository bookRepository;

    public SemanticBookSearch(IBookEmbeddingRepository bookEmbeddingRepository, IBookRepository bookRepository)
    {
        this.bookEmbeddingRepository = bookEmbeddingRepository;
        this.bookRepository = bookRepository;
    }

    public async Task<IReadOnlyList<Book>> SearchAsync(
        string query,
        CancellationToken cancellationToken = default)
    {
        var vectorMatch = await bookEmbeddingRepository.Search(query);

        //return await bookRepository.GetById(vectorMatch.BookId);

        /*return await bookVectorRepository.SearchAsync(
            embedding.Vector.ToArray(),
            MaxBooks,
            cancellationToken);*/

        return [];
    }
}