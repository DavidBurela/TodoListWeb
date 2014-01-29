using System.Linq;
using TodoListWeb.Data.Repositories;
using TodoListWeb.Models;

namespace TodoListWeb.Data.Uow
{
    public class TodoListUow
    {
        public ITodoItemRepository TodoItemRepository { get; set; }

        public TodoListUow(ITodoItemRepository todoItemRepository)
        {
            TodoItemRepository = todoItemRepository;
        }

        public TodoItem FindTodoItemById(int id)
        {
            return TodoItemRepository.FindById(id);
        }

        public void AddTodoItem(TodoItem todoItem)
        {
            // Requirement #2: add a new task to the system

            TodoItemRepository.Add(todoItem);
            TodoItemRepository.SaveChanges();
        }

        public void UpdateTodoItem(TodoItem todoItem)
        {
            // Requirement #3: edit an existing task
            TodoItemRepository.Update(todoItem);
            TodoItemRepository.SaveChanges();
        }

        public void CompleteTodoItem(int id)
        {
            // Requirement #4: Managers can complete a task
            TodoItemRepository.CompleteTask(id);
            TodoItemRepository.SaveChanges();
        }

        public IQueryable<TodoItem> GetAllTodoItems()
        {
            // Requirement #5: Managers can view a list of the tasks with those due first at the top of the list
            var items = TodoItemRepository.GetAll()
                .OrderBy(p => p.DueDate);

            return items;
        }

        public IQueryable<TodoItem> GetOutstandingTodoItems()
        {
            // Requirement #6: Anonymous can view a list of the outstanding tasks with those due first at the top of the list
            var items = GetAllTodoItems()
                .Where(p => p.CompletedDate == null);

            return items;
        }

    }
}