
using Microsoft.Extensions.AI;

namespace NextReader.AI;

/// <summary>
/// Low level service to communicate with the LLM
/// </summary>
public interface IChatService
{
    Task<ChatResult> Complete(
        ChatMessage[] messages,
        ChatOptions? options = null,
        CancellationToken cancellationToken = default);
}