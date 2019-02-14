using Microsoft.EntityFrameworkCore;

namespace RedisSample.Models
{
    public class ApplicationDbContext:DbContext
    {
           public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
    {
    }
        public DbSet<Task> Task { get; set; }
    }

    public class Task{
        public int Id { get; set; } 
        public int JobId { get; set; }
        public int TaskTypeId { get; set; }
        public int StatusId { get; set; }
        public string Barcode { get; set; }
        public string ToLocation { get; set; }

    }
}