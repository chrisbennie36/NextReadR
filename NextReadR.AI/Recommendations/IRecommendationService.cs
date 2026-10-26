using NextReadR.Data.Entities;

namespace NextReader.AI.Recommendations;

public interface IRecommendationService
{
    Task<IEnumerable<Book>> GetBookRecommendations(
        string query,
        CancellationToken cancellationToken = default);
}