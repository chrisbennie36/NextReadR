namespace NextReadR.Data;

public sealed record BookEmbedding(int BookId, string EmbeddingModel, float[] Vector);