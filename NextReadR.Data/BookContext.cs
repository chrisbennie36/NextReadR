using Microsoft.EntityFrameworkCore;
using NextReadR.Data.Entities;

namespace NextReadR.Data;

public class BookContext : DbContext
{
    public DbSet<Book> Books { get; set; }
    public DbSet<ReadingHistory> ReadingHistory { get; set; }

    public string DbPath { get; }

    public BookContext()
    {
        var dbFolder = Environment.SpecialFolder.LocalApplicationData;
        var rootPath = Environment.GetFolderPath(dbFolder);
        DbPath = Path.Join(rootPath, "books.db");
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<ReadingHistory>()
            .HasOne(b => b.Book)
            .WithOne(r => r.ReadingHistory)
            .HasForeignKey<ReadingHistory>(r => r.BookId)
            .IsRequired(false);
    }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder) => optionsBuilder.UseSqlite($"Data Source={DbPath}");
}