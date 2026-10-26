using Microsoft.Extensions.AI;

namespace NextReader.AI.Tools;

public static class BookToolsRegistry
{
    public static AIFunction CreateSearchBooksByGenreTool(
        BookTools bookTools)
    {
        return AIFunctionFactory.Create(
            bookTools.SearchBooksByGenre,
            name: "SearchBooksByGenre",
            description: """
                         Searches the user's book catalogue by genre.

                         Use this tool when you need to find books
                         that are available in the user's catalogue for a specified genre.
                         """);
    }
    
    public static AIFunction CreateSearchBooksByKeywordsTool(
        BookTools bookTools)
    {
        return AIFunctionFactory.Create(
            bookTools.SearchBooksByKeywords,
            name: "SearchBooksByKeywords",
            description: """
                         Searches the user's book catalogue using keywords.
                         
                         Use this tool when the user describes themes, topics,
                         subjects, characters, settings, or concepts they want
                         in a book.
                         
                         Examples:
                         - "books about dragons"
                         - "books set in Victorian London"
                         - "books about artificial intelligence"
                         
                         Do not use this tool when the user is simply asking
                         for a specific genre.
                         """);
    }
    
    public static AIFunction CreateSearchBooksSemanticallyTool(
        BookTools bookTools)
    {
        return AIFunctionFactory.Create(
            bookTools.SearchBooksSemantically,
            name: "SemanticSearchBooks",
            description: """
                         Searches the book catalogue using semantic similarity.

                         Use this tool when the user describes the type of book,
                         story, theme, mood, subject, setting, or experience they
                         are looking for, especially when an exact keyword search
                         may not find the most relevant books.

                         Examples:
                         - "I want a dark and atmospheric fantasy"
                         - "Something about a character struggling with their identity"
                         - "A story set in a magical kingdom with political intrigue"
                         - "Something similar in tone to a dark fairy tale"

                         Do not use this tool when the user is requesting a
                         straightforward structured filter such as a specific
                         genre, author, or publication year.
                         """);
    }
}