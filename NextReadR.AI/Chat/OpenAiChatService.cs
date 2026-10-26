using Microsoft.Extensions.AI;

namespace NextReader.AI.Chat;

public class OpenAiChatService : IChatService
{
    private readonly IChatClient chatClient;

    public OpenAiChatService(IChatClient chatClient)
    {
        this.chatClient = chatClient;
    }

    public async Task<ChatResult> Complete(
        ChatMessage[] messages,
        ChatOptions? options = null,
        CancellationToken cancellationToken = default)
    {
        var start = DateTime.UtcNow;

        using var timeoutTokenSource = CancellationTokenSource.CreateLinkedTokenSource(cancellationToken);
        
        timeoutTokenSource.CancelAfter(TimeSpan.FromSeconds(5));

        var response = await chatClient.GetResponseAsync(
            messages,
            options,
            timeoutTokenSource.Token);

        var duration = DateTime.UtcNow - start;
        
        logDiagnostics(response.Messages);

        return new ChatResult(
            Response: response.Text,
            PromptTokens: response.Usage?.InputTokenCount ?? 0,
            CompletionTokens: response.Usage?.OutputTokenCount ?? 0,
            Duration: duration);
    }

    private void logDiagnostics(IList<ChatMessage> messages)
    {
        foreach (var message in messages)
        {
            foreach (var content in message.Contents)
            {
                switch (content)
                {
                    case FunctionCallContent functionCall:
                        Console.WriteLine(
                            $"Called tool: {functionCall.Name}");
                        break;

                    case TextContent text:
                        Console.WriteLine(
                            $"Text: {text.Text}");
                        break;
                }
            }
        }
    }
}