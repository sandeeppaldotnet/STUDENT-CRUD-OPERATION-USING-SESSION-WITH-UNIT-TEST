# Student Crud Operation Using Session State In Asp.net Core MVC
=

This project demonstrates how to implement CRUD (Create, Read, Update, Delete) operations for a student model using session state in an ASP.NET Core MVC application. It also demonstrates how to use session storage to persist data between requests without a database.

## Features

- **Create a new student**: Add new student details with validation.
- **Read student list**: View a list of all added students.
- **Update student details**: Edit existing student information.
- **Delete student**: Remove a student from the list.
- **Session Storage**: All data is stored in the session to simulate persistence without a database.

## Technologies Used

- ASP.NET Core MVC (version 8)
- C#
- Entity Framework Core (optional if needed for future database integrations)
- Sessions in ASP.NET Core
- JSON serialization and deserialization for session storage
- Model Validation (Client-side and Server-side)
- Unit Testing with xUnit and Moq

## Prerequisites

To run this project locally, ensure that you have the following installed:

- [.NET SDK 8.0 or higher](https://dotnet.microsoft.com/download/dotnet)
- A code editor such as [Visual Studio](https://visualstudio.microsoft.com/), [Visual Studio Code](https://code.visualstudio.com/), or any editor of your choice.
- **Optional**: If you want to run the unit tests, install [xUnit](https://xunit.net/) and [Moq](https://github.com/moq/moq4) for mocking dependencies.

## Getting Started

Follow these steps to get the project up and running locally:

1. **Clone the repository**:

    ```bash
    git clone https://github.com/yourusername/student-crud-operation-using-session-state-in-aspnet-core-mvc.git
    cd student-crud-operation-using-session-state-in-aspnet-core-mvc
    ```

2. **Restore the dependencies**:

    If you’re using the .NET CLI:

    ```bash
    dotnet restore
    ```

    If you’re using Visual Studio, dependencies should be restored automatically when you open the project.

3. **Run the project**:

    To run the project, use the following command:

    ```bash
    dotnet run
    ```

    Alternatively, press **F5** in Visual Studio to start the application.

4. **Navigate to the application**:

    Once the application is running, open your browser and go to:

    ```
    http://localhost:5000
    ```

    This will take you to the home page where you can perform CRUD operations on the student data.

## Directory Structure

Here is a basic structure of the project:


## Unit Test Project

The **Student CRUD Operation** project includes an **xUnit Test Project** that verifies the functionality of the session-based CRUD operations. The tests ensure that the controller behaves as expected and that the session service correctly stores and retrieves the student data.

### Test Project Overview

The test project is located in the `Student_Crud_Operation_Using_Session_State_In_Asp.net_Core_MVC.Tests` directory.

Key parts of the test project:

1. **Mocking dependencies**: The `SessionService` is mocked using the `Moq` library to isolate unit tests from actual session state.
2. **Controller Testing**: We test the `StudentController` actions (like `Create`, `Edit`, `Delete`) using **xUnit** and **Moq**.
3. **SessionService**: The service is responsible for handling the session data, and the unit tests validate that data is properly stored and retrieved from the session.

### Setting Up the Unit Test Project

1. **Install test dependencies**:

   If you're using the .NET CLI, run the following command in the root project directory to add the test project:

    ```bash
    dotnet new xunit -n Student_Crud_Operation_Using_Session_State_In_Aspnet_Core_MVC.Tests
    cd Student_Crud_Operation_Using_Session_State_In_Aspnet_Core_MVC.Tests
    dotnet add reference ../Student_Crud_Operation_Using_Session_State_In_Aspnet_Core_MVC
    dotnet add package Moq
    ```

2. **Unit Test Example**:

   Here is an example test for the `StudentController`:

    ```csharp
    using Moq;
    using Student_Crud_Operation_Using_Session_State_In_Asp.net_Core_MVC.Controllers;
    using Student_Crud_Operation_Using_Session_State_In_Asp.net_Core_MVC.Models;
    using Xunit;
    using Microsoft.AspNetCore.Mvc;
    using System.Collections.Generic;
    using System.Linq;

    public class StudentControllerTests
    {
        private readonly Mock<ISessionService> _mockSessionService;
        private readonly StudentController _controller;

        public StudentControllerTests()
        {
            _mockSessionService = new Mock<ISessionService>();
            _controller = new StudentController(_mockSessionService.Object);
        }

        [Fact]
        public void Create_Post_ValidModel_ReturnsRedirectToActionResult()
        {
            // Arrange
            var student = new Student { Name = "John Doe", Email = "john.doe@example.com" };
            var students = new List<Student>();

            _mockSessionService.Setup(s => s.GetStudentList()).Returns(students);

            // Act
            var result = _controller.Create(student);

            // Assert
            var redirectToActionResult = Assert.IsType<RedirectToActionResult>(result);
            Assert.Equal("Index", redirectToActionResult.ActionName);
        }

        [Fact]
        public void Create_Post_InvalidModel_ReturnsView()
        {
            // Arrange
            var student = new Student { Name = "", Email = "invalidEmail" }; // Invalid data

            _controller.ModelState.AddModelError("Name", "Name is required");

            // Act
            var result = _controller.Create(student);

            // Assert
            var viewResult = Assert.IsType<ViewResult>(result);
            Assert.Equal(student, viewResult.Model);
        }

        [Fact]
        public void IsEmailInUse_ValidEmail_ReturnsJsonFalse()
        {
            // Arrange
            var students = new List<Student>
            {
                new Student { Id = 1, Name = "John Doe", Email = "john.doe@example.com" }
            };

            _mockSessionService.Setup(s => s.GetStudentList()).Returns(students);

            // Act
            var result = _controller.IsEmailInUse("jane.doe@example.com");

            // Assert
            var jsonResult = Assert.IsType<JsonResult>(result);
            Assert.Equal(true, jsonResult.Value);
        }
    }
    ```

3. **Run the Unit Tests**:

   To run the unit tests, use the following command in the terminal:

    ```bash
    dotnet test
    ```

   Or run the tests from Visual Studio's Test Explorer.

### Unit Test Scenarios

- **Create**: Test that the controller correctly creates a new student and stores it in the session.
- **Update**: Test that the controller correctly updates an existing student.
- **Delete**: Test that the controller correctly removes a student.
- **Email Validation**: Test that the `IsEmailInUse` method correctly checks for duplicate emails.

## Contribution

If you would like to contribute to this project, feel free to fork the repository, create a branch, and submit a pull request. Please make sure your changes do not break existing functionality and include appropriate tests if necessary.

## License

This project is licensed under the MIT License - see the [LICENSE](LICENSE) file for details.

---

If you encounter any issues or have questions, feel free to open an issue in the repository, and I will be happy to help.
