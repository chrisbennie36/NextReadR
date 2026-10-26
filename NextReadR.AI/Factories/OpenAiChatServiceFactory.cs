using Microsoft.Extensions.AI;
using NextReader.AI.Chat;
using OpenAI;

namespace NextReader.AI.Factories;

public class OpenAiChatServiceFactory : IChatServiceFactory
{
    public OpenAIClient OpenAiClient { get; }

    public OpenAiChatServiceFactory()
    {
        //ToDo: Naughty Naughty
        OpenAiClient =
            new OpenAIClient(
                "");
    }

    public IChatService CreateChatService()
    {
#pragma warning disable OPENAI001
        var responsesClient = OpenAiClient.GetResponsesClient();
#pragma warning restore OPENAI001

#pragma warning disable OPENAI001

        IChatClient chatClient = responsesClient.AsIChatClient();
        
        return new OpenAiChatService(new FunctionInvokingChatClient(chatClient));
    }
}