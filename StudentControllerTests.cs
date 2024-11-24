
using Moq;
using Xunit;
using Microsoft.AspNetCore.Mvc;
using Student_Crud_Operation_Using_Session_State_In_Asp.net_Core_MVC.Controllers;
using Student_Crud_Operation_Using_Session_State_In_Asp.net_Core_MVC.Models;
using Microsoft.AspNetCore.Http;
using System.Collections.Generic;
using System.Linq;
namespace StudentCrudUnitTest
{
    public class StudentControllerTests
    {
        private readonly Mock<ISessionService> _sessionServiceMock;
        private readonly StudentController _controller;

        public StudentControllerTests()
        {
            _sessionServiceMock = new Mock<ISessionService>();

            _controller = new StudentController(_sessionServiceMock.Object);
        }




        // Test Create GET action
        [Fact]
        public void Create_ReturnsViewResult_WithNewStudent()
        {
            // Act
            var result = _controller.Create();

            // Assert
            var viewResult = Assert.IsType<ViewResult>(result);
            Assert.IsAssignableFrom<Student>(viewResult.Model);
        }
        // Test Create POST action - Valid Model

        [Fact]
        public void Create_Post_ValidModel_ReturnsRedirectToActionResult()
        {
            // Arrange
            var student = new Student { Name = "John", Email = "john@example.com" };

            var students = new List<Student>();

            // Mock the session service to return an empty list of students
            _sessionServiceMock.Setup(s => s.GetStudentList()).Returns(students);

            // Mock the session service to update the student list
            _sessionServiceMock.Setup(s => s.SetStudentList(It.IsAny<List<Student>>()))
                .Callback<List<Student>>((studentList) => students = studentList);

            // Act
            var result = _controller.Create(student);

            // Assert
            var redirectResult = Assert.IsType<RedirectToActionResult>(result);
            Assert.Equal("Index", redirectResult.ActionName);

            // Verify that the student was added to the list
            Assert.Single(students);
            Assert.Equal("John", students[0].Name);
        }
        [Fact]
        public void Create_PostInvalidModel_ReturnsViewWithModel()
        {
            // Arrange
            var student = new Student { Id = 0, Name = "", Email = "invalidEmail" }; // Invalid model
            _controller.ModelState.AddModelError("Name", "Name is required");

            // Act
            var result = _controller.Create(student);

            // Assert
            var viewResult = Assert.IsType<ViewResult>(result);
            Assert.Equal(student, viewResult.Model);
        }
        [Fact]
        public void Edit_Get_ReturnsNotFoundIfStudentDoesNotExist()
        {
            // Arrange
            _sessionServiceMock.Setup(s => s.GetStudentList()).Returns(new List<Student>());

            // Act
            var result = _controller.Edit(1);

            // Assert
            Assert.IsType<NotFoundResult>(result);
        }
        [Fact]
        public void Edit_PostValidModel_RedirectsToIndex()
        {
            // Arrange
            var students = new List<Student>
            {
                new Student { Id = 1, Name = "John Doe", Email = "john.doe@example.com" }
            };
            _sessionServiceMock.Setup(s => s.GetStudentList()).Returns(students);
            _sessionServiceMock.Setup(s => s.SetStudentList(It.IsAny<List<Student>>()));

            var updatedStudent = new Student { Id = 1, Name = "John Doe Updated", Email = "john.updated@example.com" };

            // Act
            var result = _controller.Edit(updatedStudent);

            // Assert
            var redirectResult = Assert.IsType<RedirectToActionResult>(result);
            Assert.Equal("Index", redirectResult.ActionName);
        }
        [Fact]
        public void Delete_Post_DeletesStudentAndRedirectsToIndex()
        {
            // Arrange
            var students = new List<Student>
            {
                new Student { Id = 1, Name = "John Doe", Email = "john.doe@example.com" }
            };
            _sessionServiceMock.Setup(s => s.GetStudentList()).Returns(students);
            _sessionServiceMock.Setup(s => s.SetStudentList(It.IsAny<List<Student>>()));

            // Act
            var result = _controller.Delete(1);

            // Assert
            var redirectResult = Assert.IsType<RedirectToActionResult>(result);
            Assert.Equal("Index", redirectResult.ActionName);
        }

        [Fact]
        public void IsEmailInUse_ReturnsJsonResult()
        {
            // Arrange
            var students = new List<Student>
            {
                new Student { Id = 1, Name = "John Doe", Email = "john.doe@example.com" }
            };
            _sessionServiceMock.Setup(s => s.GetStudentList()).Returns(students);

            // Act
            var result = _controller.IsEmailInUse("john.doe@example.com");

            // Assert
            var jsonResult = Assert.IsType<JsonResult>(result);
            Assert.Equal("Email is already taken", jsonResult.Value);
        }
    }
}