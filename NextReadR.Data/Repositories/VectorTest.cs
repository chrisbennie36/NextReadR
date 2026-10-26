using System.Text.Json;
using Microsoft.Data.Sqlite;
using NextReadR.Data.Embedding;

namespace NextReadR.Data.Repositories;

public class VectorTest
{
    private const int EmbeddingDimensions = 1536;

    private readonly IBookRepository bookRepository;
    private readonly IBookEmbeddingService bookEmbeddingService;
    
    public VectorTest(IBookRepository bookRepository, IBookEmbeddingService bookEmbeddingService)
    {
        this.bookRepository = bookRepository;
        this.bookEmbeddingService = bookEmbeddingService;
    }
    
    public async Task Test()
    {
        var book = (await bookRepository.GetAll()).First();
        
        var connectionString = "Data Source=books.db";

        await using var connection = new SqliteConnection(connectionString);

        await connection.OpenAsync();

// sqlite-vec is a native SQLite extension.
        connection.LoadExtension("vec0");

        await using var command = connection.CreateCommand();

        command.CommandText = $"""
                              CREATE VIRTUAL TABLE IF NOT EXISTS BookEmbeddings USING vec0(
                                  BookId INTEGER PRIMARY KEY,
                                  Embedding FLOAT[{EmbeddingDimensions}]
                              );
                              """;

        await command.ExecuteNonQueryAsync();
        
        //var embedding = await bookEmbeddingService.GenerateEmbedding(book);
        
        /*await using var addVectorCommand = connection.CreateCommand();

        addVectorCommand.CommandText = """
                              INSERT INTO BookEmbeddings (BookId, Embedding)
                              VALUES ($bookId, $embedding)
                              """;

        addVectorCommand.Parameters.AddWithValue("$bookId", book.Id);
        addVectorCommand.Parameters.AddWithValue(
            "$embedding",
            JsonSerializer.Serialize(embedding));

        await addVectorCommand.ExecuteNonQueryAsync();*/
        
        var queryVector = await bookEmbeddingService.GenerateEmbedding(
            "I want a fantasy book involving dragons");
        
        await using var vectorCommand = connection.CreateCommand();

        vectorCommand.CommandText = """
                              SELECT
                                  BookId,
                                  distance
                              FROM BookEmbeddings
                              WHERE Embedding MATCH $query
                                AND k = 5
                              ORDER BY distance;
                              """;

        vectorCommand.Parameters.AddWithValue(
            "$query",
            JsonSerializer.Serialize(queryVector));

        await using var reader =
            await vectorCommand.ExecuteReaderAsync();

        while (await reader.ReadAsync())
        {
            var bookId = reader.GetInt32(0);
            var distance = reader.GetDouble(1);

            Console.WriteLine(
                $"Book: {bookId}, Distance: {distance}");
        }
    }
}