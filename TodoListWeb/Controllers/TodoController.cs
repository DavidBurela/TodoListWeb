using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using TodoListWeb.Models;
using TodoListWeb.Data;

namespace TodoListWeb.Controllers
{
    public class TodoController : Controller
    {
        private TodoListDbContext db = new TodoListDbContext();

        // GET: /Todo/
        public ActionResult Index()
        {
            return View(db.TodoItems.ToList());
        }

        // GET: /Todo/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            TodoItem todoitem = db.TodoItems.Find(id);
            if (todoitem == null)
            {
                return HttpNotFound();
            }
            return View(todoitem);
        }

        // GET: /Todo/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: /Todo/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include="Id,Title,Details,DueDate,CompletedDate")] TodoItem todoitem)
        {
            if (ModelState.IsValid)
            {
                db.TodoItems.Add(todoitem);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(todoitem);
        }

        // GET: /Todo/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            TodoItem todoitem = db.TodoItems.Find(id);
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
        public ActionResult Edit([Bind(Include="Id,Title,Details,DueDate,CompletedDate")] TodoItem todoitem)
        {
            if (ModelState.IsValid)
            {
                db.Entry(todoitem).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(todoitem);
        }

        // GET: /Todo/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            TodoItem todoitem = db.TodoItems.Find(id);
            if (todoitem == null)
            {
                return HttpNotFound();
            }
            return View(todoitem);
        }

        // POST: /Todo/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            TodoItem todoitem = db.TodoItems.Find(id);
            db.TodoItems.Remove(todoitem);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
