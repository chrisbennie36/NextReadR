using NextReadR.Data.Entities;

namespace NextReadR.Data.SemanticSearch;

public interface ISemanticBookSearch
{
    Task<IReadOnlyList<Book>> SearchAsync(
        string query,
        CancellationToken cancellationToken = default);
}