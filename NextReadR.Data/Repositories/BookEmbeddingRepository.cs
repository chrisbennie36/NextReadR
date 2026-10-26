using System.Text.Json;
using Microsoft.Data.Sqlite;
using NextReadR.Data.Embedding;
using NextReadR.Data.Entities;

namespace NextReadR.Data.Repositories;

public class BookEmbeddingRepository : IBookEmbeddingRepository
{
    private readonly string connectionString;
    private readonly IBookEmbeddingService bookEmbeddingService;
    
    public BookEmbeddingRepository(string connectionString, IBookEmbeddingService bookEmbeddingService)
    {
        this.connectionString = connectionString;
        this.bookEmbeddingService = bookEmbeddingService;
    }
    
    public async Task CreateBookEmbeddingTable()
    {
        using var connection = new SqliteConnection(connectionString);
        connection.Open();
        loadVectorExtension(connection);
        
        await using var deletecommand = connection.CreateCommand();

        deletecommand.CommandText = """
                                    DROP TABLE IF EXISTS BookEmbeddings;

                                    CREATE VIRTUAL TABLE BookEmbeddings USING vec0(
                                        BookId INTEGER PRIMARY KEY,
                                        Embedding FLOAT[1536]
                                    );
                                    """;

        await deletecommand.ExecuteNonQueryAsync();

        using var command = connection.CreateCommand();
        command.CommandText = @"
            CREATE TABLE IF NOT EXISTS BookEmbeddings (
                BookId INTEGER PRIMARY KEY,
                Vector BLOB NOT NULL
            );
        ";
        await command.ExecuteNonQueryAsync();
    }
    
    public async Task AddBookEmbedding(Book book, string embeddingModel)
    {
        using var connection = new SqliteConnection(connectionString);
        connection.Open();
        loadVectorExtension(connection);
        
        //ToDo: CB: Testing purposes - why does it still say the table does not contain a vector column?
        await using var deletecommand = connection.CreateCommand();

        deletecommand.CommandText = """
                                    DROP TABLE IF EXISTS BookEmbeddings;

                                    CREATE VIRTUAL TABLE BookEmbeddings USING vec0(
                                        BookId INTEGER PRIMARY KEY,
                                        Embedding FLOAT[1536]
                                    );
                                    """;

        await deletecommand.ExecuteNonQueryAsync();
        
        var embedding = await bookEmbeddingService.GenerateEmbedding(book);

        using var command = connection.CreateCommand();
        command.CommandText = @"
            INSERT INTO BookEmbeddings (BookId, Vector)
            VALUES ($bookId, $vector);
        ";
        command.Parameters.AddWithValue("$bookId", book.Id);
        command.Parameters.AddWithValue("$vector", embedding);

        await command.ExecuteNonQueryAsync();
    }

    public async Task<IEnumerable<BookVectorMatch>> Search(string query)
    {
        var matches = new List<BookVectorMatch>();
        
        using var connection = new SqliteConnection(connectionString);
        connection.Open();
        loadVectorExtension(connection);
        
        var embedding = await bookEmbeddingService.GenerateEmbedding(query);

        using var command = connection.CreateCommand();
        command.CommandText = """
                                    SELECT
                                        BookId,
                                        distance
                                    FROM BookEmbeddings
                                    WHERE Embedding MATCH $query
                                      AND k = 5
                                    ORDER BY distance;
                                    """;

        command.Parameters.AddWithValue(
            "$query",
            JsonSerializer.Serialize(embedding));
        
        await using var reader =
            await command.ExecuteReaderAsync();
        
        int bookId;
        double distance;

        while (await reader.ReadAsync())
        {
            bookId = reader.GetInt32(0);
            distance = reader.GetDouble(1);
            
            matches.Add(new BookVectorMatch(bookId, distance));

            Console.WriteLine(
                $"Book: {bookId}, Distance: {distance}");
        }

        return matches;
    }

    private void loadVectorExtension(SqliteConnection connection)
    {
        connection.LoadExtension("vec0");
    }
}