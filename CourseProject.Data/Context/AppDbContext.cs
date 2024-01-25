using CourseProject.Domain.Entities.Books;
using CourseProject.Domain.Entities.Messages;
using CourseProject.Domain.Entities.Users;
using Microsoft.EntityFrameworkCore;

namespace CourseProject.Data.Contexts;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
    {
    }

    public DbSet<User> Users { get; set; }
    public DbSet<Book> Books { get; set; }
    public DbSet<Comment> Comments { get; set; }
    public DbSet<Like> Likes { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        //optionsBuilder.UseSqlite("Filename=StoreBook.db");
        base.OnConfiguring(optionsBuilder);
    }
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {

        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<User>()
            .HasIndex(u => u.Email)
            .IsUnique();

        modelBuilder.Entity<User>()
            .HasMany(b => b.Books)
            .WithOne(u => u.User)
            .OnDelete(DeleteBehavior.SetNull);

        modelBuilder.Entity<User>()
            .HasMany(b => b.Comments)
            .WithOne(c => c.User)
            .OnDelete(DeleteBehavior.SetNull);
        modelBuilder.Entity<User>()
            .HasMany(b => b.Likes)
            .WithOne(u => u.User)
            .OnDelete(DeleteBehavior.SetNull);

        modelBuilder.Entity<Book>()
            .HasMany(b => b.Comments)
            .WithOne(c => c.Book)
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<Book>()
            .HasMany(b => b.Likes)
            .WithOne(c => c.Book)
            .OnDelete(DeleteBehavior.Cascade);

    }
}
