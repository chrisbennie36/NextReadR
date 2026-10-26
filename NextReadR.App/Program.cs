using Microsoft.Extensions.AI;
using NextReader.AI.Factories;
using NextReader.AI.Prompts;
using NextReader.AI.Recommendations;
using NextReader.AI.Tools;
using NextReadR.Data;
using NextReadR.Data.Embedding;
using NextReadR.Data.Entities;
using NextReadR.Data.Repositories;
using NextReadR.Data.SemanticSearch;

/*Console.WriteLine("Which Book genre are you looking for today?");
var genre = Console.ReadLine();

if (string.IsNullOrEmpty(genre))
{
    Console.WriteLine("Not sure what you mean pal");
    return;
}

var booksRepository = new BookRepository();

var booksOfGenre = (await booksRepository.GetByGenre(genre)).ToList();

if (!booksOfGenre.Any())
{
    Console.WriteLine($"No books found for genre: {genre}");
    return;
}*/

var promptBuilder = new PromptBuilder();
/*var prompt = promptBuilder.BuildRecommendationPrompt(
    $"I am looking for a book in the {genre} genre. Can you recommend one from my collection?");*/

var prompt = promptBuilder.BuildRecommendationPrompt(
    "I want a dark fantasy novel involving dragons. Can you recommend one from my collection?");

var chatServiceFactory = new OpenAiChatServiceFactory();

IEmbeddingGenerator<string, Embedding<float>> embeddingGenerator =
    chatServiceFactory.OpenAiClient.GetEmbeddingClient("text-embedding-3-small").AsIEmbeddingGenerator();

var bookEmbeddingService = new BookEmbeddingService(embeddingGenerator);
var bookEmbeddingRepository = new BookEmbeddingRepository("Data Source=books.db", bookEmbeddingService);
var bookRepository = new BookRepository(bookEmbeddingRepository);

var readingHistoryRepository = new ReadingHistoryRepository();

await readingHistoryRepository.PopulateReadingHistory();

var mostRecentHistory = (await readingHistoryRepository.GetReadingHistory()).Take(20);

foreach (var history in mostRecentHistory)
{
    await bookRepository.Create(history);
}

await UpdateBookDetails();

var semantic = new SemanticBookSearch(bookEmbeddingRepository, bookRepository);
var recommendationService = new RecommendationService(semantic, readingHistoryRepository);

var result = await recommendationService.GetBookRecommendations("I want a political book");

Console.WriteLine(result.Count());

async Task UpdateBookDetails()
{
    /*await bookRepository.UpdateMetadatasForBook("Bonfire of the vanities",
        "Literary Fiction, Satire, Social Drama, Legal Thriller",
        "a satirical social drama exploring class, race, and politics in 1980s New York City through the fall of a wealthy bond trader.");*/
    await bookRepository.UpdateMetadatasForBook("Chokepoints: American Power in the Age of Economic Warfare", "Political Science, International Relations, Economics, Geopolitics", "An history of America's means of applying economic pressure to achieve its foreign policy objectives, and the consequences of those actions for the global economy and international relations");
    await bookRepository.UpdateMetadatasForBook("Sophie's World", "Philisophical Fiction, Young Adult Fiction, Metafiction, Coming-of-age", "a classic philosophical novel that explores the history of Western thought through the framing story of a teenage girl receiving mysterious letters.");
    await bookRepository.UpdateMetadatasForBook("Brotherless Night", "Historical Fiction, Literary Fiction, War Fiction, Coming-of-age", "a historical literary novel detailing the personal, political, and moral struggles of a young Tamil woman during the Sri Lankan Civil War");
    await bookRepository.UpdateMetadatasForBook("Wellness", "Literary Fiction, Contemporary Fiction, Satire, Domestic Fiction", "a work of contemporary literary fiction and satire that examines modern marriage, wellness culture, algorithms, and urban life");
    await bookRepository.UpdateMetadatasForBook("Japan in the passing lane",
        "Non-Fiction, Labour Studies, Investigative Journalism, Memoir",
        "a seminal firsthand account of assembly-line work at Toyota, making it a classic of investigative labor reporting and sociological nonfiction");
    await bookRepository.UpdateMetadatasForBook("Empire of Dragons", "Historical Fiction, Fantasy, Adventure",
        "A group of Roman legionaries escape imprisonment, meet an exiled Chinese prince and travel to China to help him win back his kingdom");
    await bookRepository.UpdateMetadatasForBook(
        "We Wish to Inform You That Tomorrow We Will Be Killed With Our Families: Stories from Rwanda",
        "Non-Fiction, Narrative Journalism, History, War Reporting",
        "a landmark work of investigative journalism and narrative nonfiction detailing the 1994 Rwandan genocide and its political aftermath.");
    await bookRepository.UpdateMetadatasForBook("The Sorrow of War",
        "War Fiction, Historical Fiction, Literary Fiction, Psychological Fiction",
        "a seminal autobiographical novel depicting the trauma and devastating psychological aftermath experienced by a North Vietnamese soldier during the Vietnam War.");
    await bookRepository.UpdateMetadatasForBook("When Mandela Goes", "Non-Fiction, Political Science, African Studies, Current Affairs", " a work of political analysis examining the projected future and challenges of post-apartheid South Africa after Nelson Mandela's departure from office");
}

var bookTools = new BookTools(bookRepository, new SemanticBookSearch(bookEmbeddingRepository, bookRepository));

var searchBooksByGenreTool = BookToolsRegistry.CreateSearchBooksByGenreTool(bookTools);
var searchBooksByKeywordsTool = BookToolsRegistry.CreateSearchBooksByKeywordsTool(bookTools);
var booksSemanticSearchTool = BookToolsRegistry.CreateSearchBooksSemanticallyTool(bookTools);

//var chatService = chatServiceFactory.CreateChatService();

/*await chatService.Complete(prompt, new ChatOptions
    {
        ModelId = "gpt-5.6-luna",
        Tools = [searchBooksByGenreTool, searchBooksByKeywordsTool, booksSemanticSearchTool]
    }, CancellationToken.None)
    .ContinueWith(task =>
    {
        if (task.IsCompletedSuccessfully)
        {
            var result = task.Result;
            Console.WriteLine($"Recommended Book: {result.Response}");
        }
        else
        {
            Console.WriteLine($"Error: {task.Exception?.Message}");
        }
    });*/