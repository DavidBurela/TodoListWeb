using Microsoft.VisualStudio.TestTools.UnitTesting;
using NSubstitute;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using TodoListWeb.Controllers;
using TodoListWeb.Data.Repositories;
using TodoListWeb.Data.Uow;
using TodoListWeb.Models;


namespace TodoListWeb.Tests.Controllers
{
    [TestClass]
    public class TodoControllerUnitTest
    {
        // A helper method to set the role for the current HTTP context
        private static void SetHttpContextToHaveRole(TodoController controller, string role)
        {
            var fakeHttpContext = Substitute.For<HttpContextBase>();
            fakeHttpContext.User.IsInRole(role).Returns(true);

            var fakedControllerContext = new ControllerContext(fakeHttpContext, new RouteData(), controller);
            controller.ControllerContext = fakedControllerContext;
        }

        // most of the tests have the same set of data. This just cleans the test code up.
        public IQueryable<TodoItem> StandardTestData()
        {
            return new List<TodoItem>
            {
                new TodoItem{DueDate = new DateTime(2014, 1, 25)}, 
                new TodoItem{DueDate = new DateTime(2014, 1, 5), CompletedDate = new DateTime(2014, 1, 20)}, 
                new TodoItem{DueDate = new DateTime(2014, 1, 10)}
            }.AsQueryable();
        }

        [TestMethod]
        public void IndexShouldReturnAllItemsForManagers()
        {
            // Requirement #5: Managers can view a list of the tasks with those due first at the top of the list
            // Arrange
            var fakeData = StandardTestData();
            var repository = Substitute.For<ITodoItemRepository>(); // create repository mock
            repository.GetAll().Returns(fakeData);

            var controller = new TodoController(new TodoListUow(repository));
            SetHttpContextToHaveRole(controller, "manager");

            // Act
            var result = controller.Index() as ViewResult;

            // Assert
            var model = result.ViewData.Model as IEnumerable<TodoItem>;
            Assert.AreEqual(fakeData.Count(), model.Count());
        }

        [TestMethod]
        public void IndexShouldReturnSortedItemsForManagers()
        {
            // Requirement #5: Managers can view a list of the tasks with those due first at the top of the list
            // Arrange
            var fakeData = StandardTestData();
            var repository = Substitute.For<ITodoItemRepository>(); // create repository mock
            repository.GetAll().Returns(fakeData);

            var controller = new TodoController(new TodoListUow(repository));
            SetHttpContextToHaveRole(controller, "manager");

            // Act
            var result = controller.Index() as ViewResult;

            // Assert
            var model = (result.ViewData.Model as IEnumerable<TodoItem>).ToArray();

            var correctOrder = fakeData.OrderBy(p => p.DueDate).ToArray();
            for (int i = 0; i < correctOrder.Count(); i++)
            {
                Assert.AreEqual(correctOrder[i].DueDate, model[i].DueDate);
            }
        }

        [TestMethod]
        public void IndexShouldReturnOutstandingItemsForAnonymous()
        {
            // Requirement #6: Anonymous can view a list of the outstanding tasks with those due first at the top of the list
            // Arrange
            var fakeData = StandardTestData();
            var repository = Substitute.For<ITodoItemRepository>(); // create repository mock
            repository.GetAll().Returns(fakeData);

            var controller = new TodoController(new TodoListUow(repository));
            SetHttpContextToHaveRole(controller, "anonymous");

            // Act
            var result = controller.Index() as ViewResult;

            // Assert
            var model = result.ViewData.Model as IEnumerable<TodoItem>;
            Assert.AreEqual(fakeData.Count(p => p.CompletedDate == null), model.Count());
        }

        [TestMethod]
        public void IndexShouldReturnSortedItemsForAnonymous()
        {
            // Requirement #5: Managers can view a list of the tasks with those due first at the top of the list
            // Arrange
            var fakeData = StandardTestData();
            var repository = Substitute.For<ITodoItemRepository>(); // create repository mock
            repository.GetAll().Returns(fakeData);

            var controller = new TodoController(new TodoListUow(repository));
            SetHttpContextToHaveRole(controller, "anonymous");

            // Act
            var result = controller.Index() as ViewResult;

            // Assert
            var model = (result.ViewData.Model as IEnumerable<TodoItem>).ToArray();

            var correctOrder = fakeData.Where(p => p.CompletedDate == null).OrderBy(p => p.DueDate).ToArray();
            for (int i = 0; i < correctOrder.Count(); i++)
            {
                Assert.AreEqual(correctOrder[i].DueDate, model[i].DueDate);
            }
        }

        [TestMethod]
        public void CreateOnlyForManagers()
        {
            // Requirement #2: Managers can add a new task to the system
            var methodInfo = typeof(TodoController).GetMethod("Create", new Type[] { });
            AuthorizeAttribute attribute = methodInfo
                   .GetCustomAttributes(typeof(AuthorizeAttribute), true)
                   .Cast<AuthorizeAttribute>()
                   .FirstOrDefault();

            Assert.IsNotNull(attribute); // Check the attribute exists
            Assert.IsTrue(attribute.Roles.Contains("manager"));  // Check it contains your role
        }

        [TestMethod]
        public void CreatePostOnlyForManagers()
        {
            // Requirement #2: Managers can add a new task to the system
            var methodInfo = typeof(TodoController).GetMethod("Create", new Type[] { typeof(TodoItem) });
            AuthorizeAttribute attribute = methodInfo
                   .GetCustomAttributes(typeof(AuthorizeAttribute), true)
                   .Cast<AuthorizeAttribute>()
                   .FirstOrDefault();

            Assert.IsNotNull(attribute); // Check the attribute exists
            Assert.IsTrue(attribute.Roles.Contains("manager"));  // Check it contains your role
        }

        [TestMethod]
        public void EditOnlyForManagers()
        {
            // Requirement #2: Managers can add a new task to the system
            var methodInfo = typeof(TodoController).GetMethod("Edit", new Type[] { typeof(int?) });
            AuthorizeAttribute attribute = methodInfo
                   .GetCustomAttributes(typeof(AuthorizeAttribute), true)
                   .Cast<AuthorizeAttribute>()
                   .FirstOrDefault();

            Assert.IsNotNull(attribute); // Check the attribute exists
            Assert.IsTrue(attribute.Roles.Contains("manager"));  // Check it contains your role
        }

        [TestMethod]
        public void EditPostOnlyForManagers()
        {
            // Requirement #2: Managers can add a new task to the system
            var methodInfo = typeof(TodoController).GetMethod("Edit", new Type[] { typeof(TodoItem) });
            AuthorizeAttribute attribute = methodInfo
                   .GetCustomAttributes(typeof(AuthorizeAttribute), true)
                   .Cast<AuthorizeAttribute>()
                   .FirstOrDefault();

            Assert.IsNotNull(attribute); // Check the attribute exists
            Assert.IsTrue(attribute.Roles.Contains("manager"));  // Check it contains your role
        }

        [TestMethod]
        public void CompleteOnlyForManagers()
        {
            // Requirement #2: Managers can add a new task to the system
            var methodInfo = typeof(TodoController).GetMethod("Complete", new Type[] { typeof(int?) });
            AuthorizeAttribute attribute = methodInfo
                   .GetCustomAttributes(typeof(AuthorizeAttribute), true)
                   .Cast<AuthorizeAttribute>()
                   .FirstOrDefault();

            Assert.IsNotNull(attribute); // Check the attribute exists
            Assert.IsTrue(attribute.Roles.Contains("manager"));  // Check it contains your role
        }

        [TestMethod]
        public void CompleteUpdatesTodoItem()
        {
            // Requirement #4: Managers can complete a task
            // Arrange
            var repository = Substitute.For<ITodoItemRepository>(); // create repository mock
            repository.FindById(1).Returns(new TodoItem());

            var controller = new TodoController(new TodoListUow(repository));

            // Act
            var result = controller.Complete(1) as ViewResult;

            // Assert
            repository.Received().CompleteTask(1);
        }
    }
}
