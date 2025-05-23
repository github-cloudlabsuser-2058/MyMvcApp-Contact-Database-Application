using Xunit;
using Microsoft.AspNetCore.Mvc;
using MyMvcApp.Controllers;
using MyMvcApp.Models;
using System.Linq;

namespace MyMvcApp.Tests
{
    public class UserControllerTests
    {
        public UserControllerTests()
        {
            // Limpiar la lista antes de cada test
            UserController.userlist.Clear();
        }

        [Fact]
        public void Index_ReturnsViewWithUserList()
        {
            // Arrange
            UserController.userlist.Add(new User { Id = 1, Name = "Test", Email = "test@mail.com" });
            var controller = new UserController();

            // Act
            var result = controller.Index() as ViewResult;

            // Assert
            Assert.NotNull(result);
            var model = Assert.IsAssignableFrom<System.Collections.Generic.List<User>>(result.Model);
            Assert.Single(model);
        }

        [Fact]
        public void Details_ReturnsView_WhenUserExists()
        {
            // Arrange
            UserController.userlist.Add(new User { Id = 2, Name = "User2", Email = "user2@mail.com" });
            var controller = new UserController();

            // Act
            var result = controller.Details(2) as ViewResult;

            // Assert
            Assert.NotNull(result);
            var model = Assert.IsType<User>(result.Model);
            Assert.Equal(2, model.Id);
        }

        [Fact]
        public void Details_ReturnsNotFound_WhenUserDoesNotExist()
        {
            var controller = new UserController();

            var result = controller.Details(999);

            Assert.IsType<NotFoundResult>(result);
        }

        [Fact]
        public void Create_Post_AddsUserAndRedirects_WhenModelIsValid()
        {
            var controller = new UserController();
            var user = new User { Name = "Nuevo", Email = "nuevo@mail.com" };

            var result = controller.Create(user) as RedirectToActionResult;

            Assert.NotNull(result);
            Assert.Equal("Index", result.ActionName);
            Assert.Single(UserController.userlist);
            Assert.Equal("Nuevo", UserController.userlist.First().Name);
        }

        [Fact]
        public void Create_Post_ReturnsView_WhenModelIsInvalid()
        {
            var controller = new UserController();
            controller.ModelState.AddModelError("Name", "Required");
            var user = new User();

            var result = controller.Create(user) as ViewResult;

            Assert.NotNull(result);
            Assert.IsType<User>(result.Model);
        }

        [Fact]
        public void Edit_Post_UpdatesUserAndRedirects_WhenModelIsValid()
        {
            UserController.userlist.Add(new User { Id = 3, Name = "Old", Email = "old@mail.com" });
            var controller = new UserController();
            var updatedUser = new User { Id = 3, Name = "Updated", Email = "updated@mail.com" };

            var result = controller.Edit(3, updatedUser) as RedirectToActionResult;

            Assert.NotNull(result);
            Assert.Equal("Index", result.ActionName);
            Assert.Equal("Updated", UserController.userlist.First(u => u.Id == 3).Name);
        }

        [Fact]
        public void Edit_Post_ReturnsNotFound_WhenUserDoesNotExist()
        {
            var controller = new UserController();
            var user = new User { Id = 99, Name = "NoExiste", Email = "no@mail.com" };

            var result = controller.Edit(99, user);

            Assert.IsType<NotFoundResult>(result);
        }

        [Fact]
        public void Edit_Post_ReturnsView_WhenModelIsInvalid()
        {
            UserController.userlist.Add(new User { Id = 4, Name = "Test", Email = "test@mail.com" });
            var controller = new UserController();
            controller.ModelState.AddModelError("Name", "Required");
            var user = new User { Id = 4, Name = "", Email = "" };

            var result = controller.Edit(4, user) as ViewResult;

            Assert.NotNull(result);
            Assert.IsType<User>(result.Model);
        }

        [Fact]
        public void Delete_Post_RemovesUserAndRedirects_WhenUserExists()
        {
            UserController.userlist.Add(new User { Id = 5, Name = "DeleteMe", Email = "delete@mail.com" });
            var controller = new UserController();

            var result = controller.Delete(5, null) as RedirectToActionResult;

            Assert.NotNull(result);
            Assert.Equal("Index", result.ActionName);
            Assert.Empty(UserController.userlist);
        }

        [Fact]
        public void Delete_Post_ReturnsNotFound_WhenUserDoesNotExist()
        {
            var controller = new UserController();

            var result = controller.Delete(999, null);

            Assert.IsType<NotFoundResult>(result);
        }
    }
}