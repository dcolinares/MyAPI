using Microsoft.AspNetCore.Mvc;
using Moq;
using MyAPI.Controllers;
using MyAPI.Model;
using MyAPI.Repositories;
using NUnit.Framework; // Add this using directive

namespace MyAPI.APITest
{
    [TestFixture]
    public class UserControllerTests
    {
        private UserController _userController;
        private Mock<Service> _service;

        [SetUp]
        public void Setup()
        {
            //_service = new Mock<Service>(constructorArg1, constructorArg2);
            _userController = new UserController(_service.Object);
        }

        [Test]
        public void GetUserTest_ReturnsOKResult()
        {
            // Arrange
            var userName = "testUser";
            var password = "testPass";
            var mockUser = new UserModel { FirstName = userName, Password = password }; // Mock user object

            _service.Setup(s => s.User.GetUser(userName, password)).Returns(mockUser);

            // Act
            var result = _userController.GetUser(userName, password);

            // Assert
            Assert.That(result, Is.InstanceOf<OkObjectResult>());
            var okResult = result as OkObjectResult;
            Assert.That(okResult, Is.Not.Null);
            Assert.That(okResult!.Value, Is.EqualTo(mockUser));
        }

        [Test]
        public void GetUser_InvalidUser_ReturnsNotFound()
        {
            // Arrange
            var userName = "invalidUser";
            var password = "wrongPass";

            _service.Setup(s => s.User.GetUser(userName, password)).Returns((UserModel)null);

            // Act
            var result = _userController.GetUser(userName, password);

            // Assert
            Assert.That(result, Is.InstanceOf<NotFoundResult>());
        }
    }
}