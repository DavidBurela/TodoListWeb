using System.Data.Entity;
using TodoListWeb.Models;

namespace TodoListWeb.Data
{
    /// <summary>
    /// Entity framework code first Db Context.
    /// This will automatically generate the database & tables for me.
    /// </summary>
    public class TodoListDbContext : DbContext
    {
        // Automatically use the connection string in the web.config
        public TodoListDbContext()
            : base("DefaultConnection")
        {
        }

        // Tables
        public DbSet<TodoItem> TodoItems { get; set; }
    }
}