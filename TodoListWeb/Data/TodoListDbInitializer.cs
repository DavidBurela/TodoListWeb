using System.Data.Entity;

namespace TodoListWeb.Data
{
    public class TodoListDbInitializer : DropCreateDatabaseIfModelChanges<TodoListDbContext>
    {
    }
}