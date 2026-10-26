using Microsoft.Extensions.AI;

namespace NextReader.AI.Prompts;

public class PromptBuilder
{
    public ChatMessage[] BuildRecommendationPrompt(
        string userPrompt)
    {
        return
        [
            new ChatMessage(
                ChatRole.System,
                """
                You are a book recommendation assistant.
                
                Use the available catalogue tools to find books
                that match the user's request.
                
                Only recommend books returned by the catalogue tools.
                
                Explain why your recommendations match the user's request.
                """),

            new ChatMessage(
                ChatRole.User,
                userPrompt)
        ];
    }
}