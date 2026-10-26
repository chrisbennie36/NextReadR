namespace NextReader.AI;

public sealed record ChatResult(
    string Response, 
    long PromptTokens, 
    long CompletionTokens, 
    TimeSpan Duration);