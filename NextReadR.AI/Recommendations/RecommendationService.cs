using NextReadR.Data.Entities;
using NextReadR.Data.Repositories;
using NextReadR.Data.SemanticSearch;

namespace NextReader.AI.Recommendations;

public class RecommendationService : IRecommendationService
{
    private readonly ISemanticBookSearch semanticBookSearch;
    private readonly IReadingHistoryRepository readingHistoryRepository;
    
    public RecommendationService(ISemanticBookSearch semanticBookSearch, IReadingHistoryRepository readingHistoryRepository)
    {
        this.semanticBookSearch = semanticBookSearch;
        this.readingHistoryRepository = readingHistoryRepository;
    }

    public async Task<IEnumerable<Book>> GetBookRecommendations(string query, CancellationToken cancellationToken = default)
    {
        var candidates = await semanticBookSearch.SearchAsync(
            query,
            cancellationToken);

        var history = await readingHistoryRepository.GetReadingHistory();

        return candidates
            .Where(candidate =>
                history.All(h => h.BookId != candidate.Id))
            .ToList();
    }
}