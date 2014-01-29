using System;
using System.Data.Entity;
using System.Linq;
using TodoListWeb.Models;

namespace TodoListWeb.Data.Repositories
{
    public interface ITodoItemRepository
    {
        IQueryable<TodoItem> GetAll();
        TodoItem FindById(int id);
        void Add(TodoItem entity);
        void Update(TodoItem entity);
        void CompleteTask(int id);
        void SaveChanges();
    }

    /// <summary>
    /// A repository class to abstract away the underlying data access. This allows me to mock the repository when testing.
    /// </summary>
    public class TodoItemRepository : ITodoItemRepository
    {
        private TodoListDbContext DbContext { get; set; }

        public TodoItemRepository()
        {
            DbContext = new TodoListDbContext();
        }

        public IQueryable<TodoItem> GetAll()
        {
            return DbContext.TodoItems;
        }

        public TodoItem FindById(int id)
        {
            return DbContext.TodoItems.Find(id);
        }

        public void Add(TodoItem entity)
        {
            var dbEntityEntry = DbContext.Entry(entity);
            if (dbEntityEntry.State != EntityState.Detached)
            {
                dbEntityEntry.State = EntityState.Added;
            }
            else
            {
                DbContext.TodoItems.Add(entity);
            }
        }
        public void Update(TodoItem entity)
        {
            var dbEntityEntry = DbContext.Entry(entity);
            if (dbEntityEntry.State == EntityState.Detached)
            {
                DbContext.TodoItems.Attach(entity);
            }
            dbEntityEntry.State = EntityState.Modified;
        }

        public void CompleteTask(int id)
        {
            var item = DbContext.TodoItems.Find(id);
            item.CompletedDate = DateTime.Now;
        }

        public void SaveChanges()
        {
            DbContext.SaveChanges();
        }
    }
}