using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using TodoListWeb.Data.Uow;
using TodoListWeb.Models;
using TodoListWeb.Data;

namespace TodoListWeb.Controllers
{
    public class TodoController : Controller
    {
        public TodoListUow TodoListUow { get; set; }

        public TodoController(TodoListUow todoListUow)
        {
            TodoListUow = todoListUow;
        }

        // GET: /Todo/
        [AllowAnonymous]
        public ActionResult Index()
        {
            IQueryable<TodoItem> data;

            if (HttpContext.User.IsInRole("manager"))
                // Requirement #5: Managers can view a list of the tasks with those due first at the top of the list
                data = TodoListUow.GetAllTodoItems();
            else
                // Requirement #6: Anonymous can view a list of the outstanding tasks with those due first at the top of the list
                data = TodoListUow.GetOutstandingTodoItems();
            return View(data);
        }

        // GET: /Todo/Create
        [Authorize(Roles = "manager")]
        public ActionResult Create()
        {
            return View();
        }

        // POST: /Todo/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "manager")]
        public ActionResult Create([Bind(Include = "Id,Title,Details,DueDate")] TodoItem todoitem)
        {
            if (ModelState.IsValid)
            {
                TodoListUow.AddTodoItem(todoitem);
                return RedirectToAction("Index");
            }

            return View(todoitem);
        }

        // GET: /Todo/Edit/5
        [Authorize(Roles = "manager")]
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var todoitem = TodoListUow.FindTodoItemById((int)id);
            if (todoitem == null)
            {
                return HttpNotFound();
            }
            return View(todoitem);
        }

        // POST: /Todo/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "manager")]
        public ActionResult Edit([Bind(Include = "Id,Title,Details,DueDate,CompletedDate")] TodoItem todoitem)
        {
            if (ModelState.IsValid)
            {
                TodoListUow.UpdateTodoItem(todoitem);
                return RedirectToAction("Index");
            }
            return View(todoitem);
        }

        // GET: /TodoItem/Complete/5
        [Authorize(Roles = "manager")]
        public ActionResult Complete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var todoitem = TodoListUow.FindTodoItemById((int)id);
            if (todoitem == null)
            {
                return HttpNotFound();
            }
            TodoListUow.CompleteTodoItem((int)id);
            return RedirectToAction("Index");
        }
    }
}
