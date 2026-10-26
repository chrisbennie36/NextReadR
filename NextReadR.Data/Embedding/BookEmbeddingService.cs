using Microsoft.Data.Sqlite;
using Microsoft.Extensions.AI;
using NextReadR.Data.Entities;

namespace NextReadR.Data.Embedding;

public class BookEmbeddingService : IBookEmbeddingService
{
    private readonly IEmbeddingGenerator<string, Embedding<float>> _generator;

    public BookEmbeddingService(
        IEmbeddingGenerator<string, Embedding<float>> generator)
    {
        _generator = generator;
    }

    public async Task<float[]> GenerateEmbedding(Book book, CancellationToken cancellationToken = default)
    {
        await using var connection =
            new SqliteConnection("Data Source=books.db");

        await connection.OpenAsync();

        connection.LoadExtension("vec0");
        
        await using var deletecommand = connection.CreateCommand();

        deletecommand.CommandText = """
                                    DROP TABLE IF EXISTS BookEmbeddings;

                                    CREATE VIRTUAL TABLE BookEmbeddings USING vec0(
                                        BookId INTEGER PRIMARY KEY,
                                        Embedding FLOAT[1536]
                                    );
                                    """;

        await deletecommand.ExecuteNonQueryAsync();

        await using var command = connection.CreateCommand();

        command.CommandText = """
                              CREATE VIRTUAL TABLE IF NOT EXISTS BookEmbeddings USING vec0(
                                  BookId INTEGER PRIMARY KEY,
                                  Embedding FLOAT[1536]
                              );
                              """;

        await command.ExecuteNonQueryAsync();
        var text = $$"""
                     Title: {{book.Title}}
                     Genres: {{string.Join(", ", book.Genres)}} 
                     Description: {{book.Description}}
                     """;

        var embedding = await _generator.GenerateAsync(
            text,
            cancellationToken: cancellationToken);

        return embedding.Vector.ToArray();
    }
    
    public async Task<float[]> GenerateEmbedding(string query, CancellationToken cancellationToken = default)
    {
        await using var connection =
            new SqliteConnection("Data Source=books.db");

        await connection.OpenAsync();

        connection.LoadExtension("vec0");
        
        await using var deletecommand = connection.CreateCommand();

        deletecommand.CommandText = """
                                    DROP TABLE IF EXISTS BookEmbeddings;

                                    CREATE VIRTUAL TABLE BookEmbeddings USING vec0(
                                        BookId INTEGER PRIMARY KEY,
                                        Embedding FLOAT[1536]
                                    );
                                    """;

        await deletecommand.ExecuteNonQueryAsync();

        await using var command = connection.CreateCommand();

        command.CommandText = """
                              CREATE VIRTUAL TABLE IF NOT EXISTS BookEmbeddings USING vec0(
                                  BookId INTEGER PRIMARY KEY,
                                  Embedding FLOAT[1536]
                              );
                              """;

        await command.ExecuteNonQueryAsync();
        var embedding = await _generator.GenerateAsync(
            query,
            cancellationToken: cancellationToken);

        return embedding.Vector.ToArray();
    }
}