using Microsoft.EntityFrameworkCore;

namespace TodoAPI
{
    public class TodoDB : DbContext
    {
        public TodoDB(DbContextOptions<TodoDB> options)
            : base(options) { }

        public DbSet<TodoItem> Todos { get; set; }
    }
}
