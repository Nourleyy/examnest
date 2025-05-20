# 📚✍️ ExamNest API 🚀

[![GitHub stars](https://img.shields.io/github/stars/gitnasr/ExamNest?style=social)](https://github.com/gitnasr/ExamNest/stargazers)
[![GitHub forks](https://img.shields.io/github/forks/gitnasr/ExamNest?style=social)](https://github.com/gitnasr/ExamNest/network/members)
[![GitHub issues](https://img.shields.io/github/issues/gitnasr/ExamNest)](https://github.com/gitnasr/ExamNest/issues)
[![License](https://img.shields.io/github/license/gitnasr/ExamNest)](https://github.com/gitnasr/ExamNest/blob/main/LICENSE)
![.NET](https://img.shields.io/badge/.NET-9.0-blueviolet)
<!-- Add more badges like build status if you have CI/CD setup -->

## ✨ Project Description

ExamNest is a robust backend API built with C# .NET Core 9.0 for managing a comprehensive online examination system. It provides endpoints for user authentication, managing educational resources like branches, tracks, and courses, handling question banks and exams, and processing student submissions and results. The API is designed with role-based access control (RBAC) to ensure secure interactions for administrators, instructors, and students.

## 🎯 What This Project Does

Here's a breakdown of its core functionalities:

*   **🔐 Authentication & Authorization:** Secure user registration, login, token management (JWT), Refresh Tokens, and role-based access control.
*   **🏫 Academic Structure Management:** CRUD operations for Branches, Tracks, and Courses.
*   **❓ Question Bank:** Create, manage, and retrieve questions and their choices.
*   **📝 Exam Creation & Management:** Generate exams based on courses and questions, set schedules, and manage exam details.
*   **📄 Submission Handling:** Allow students to submit their answers to exams and process these submissions, including scoring.
*   **📊 Reporting:** Retrieve exam results for students and view submission details.
*   **👥 User Role Management:** Admins can upgrade users to specific roles (Instructor, Student) and assign them to academic structures.

## 📂 Project Structure Overview

The project is organized into logical directories to maintain separation of concerns:

*   `Controllers/`: Handles incoming HTTP requests and returns responses.
*   `DTO/`: Data Transfer Objects for transferring data between layers.
*   `Models/`: Entity Framework Core models representing the database schema, including Identity models and Stored Procedure results.
*   `Repositories/`: Implements data access logic, abstracting the database.
*   `Services/`: Contains business logic and integrates repositories and other components.
*   `Interfaces/`: Defines contracts for services and repositories, enabling Dependency Injection.
*   `Migrations/`: Entity Framework Core database migration files.
*   `Exceptions/`: Custom exception classes.
*   `Extensions/`: Extension methods for configuring services (Identity, Repositories, Services, Swagger, Validation).
*   `Filters/`: Custom action filters (API Response wrapping, JWT cookie handling).
*   `Middlewares/`: Custom middleware for handling exceptions and token injection.
*   `Enums/`: Enumerations for roles and permissions.
*   `Factories/`: Factory classes.

## ⚙️ Functionality Breakdown

Each controller exposes specific API endpoints:

*   **`AuthenticationController`**:
    *   `POST /api/Authentication/login`: Authenticates a user and issues JWT tokens.
    *   `POST /api/Authentication/register`: Registers a new user with a 'Pending' role.
    *   `GET /api/Authentication/me`: Retrieves the current authenticated user's details and role.
    *   `POST /api/Authentication/refresh`: Refreshes expired access tokens using a refresh token.
*   **`BranchesController`**:
    *   `GET /api/Branches`: Retrieves a paginated list of all branches (Instructor, Admin).
    *   `GET /api/Branches/{id}`: Retrieves a specific branch by ID (All Authenticated Roles).
    *   `POST /api/Branches`: Creates a new branch (Instructor, Admin).
    *   `PUT /api/Branches/{id}`: Updates an existing branch (Instructor, Admin).
    *   `DELETE /api/Branches/{id}`: Deletes a branch (Admin).
*   **`ChoicesController`**:
    *   `GET /api/Choices/{id}`: Retrieves a specific choice by ID (Instructor, Admin).
    *   `POST /api/Choices`: Creates a new choice for a question (Instructor, Admin).
    *   `PUT /api/Choices/{id}`: Updates an existing choice (Instructor, Admin).
    *   `DELETE /api/Choices/{id}`: Deletes a choice (Instructor, Admin).
*   **`CoursesController`**:
    *   `GET /api/Courses`: Retrieves a paginated list of all courses (All Authenticated Roles).
    *   `GET /api/Courses/{id}`: Retrieves a specific course by ID (All Authenticated Roles).
    *   `POST /api/Courses`: Creates a new course (Instructor, Admin).
    *   `PUT /api/Courses/{id}`: Updates an existing course (Instructor, Admin).
    *   `DELETE /api/Courses/{id}`: Deletes a course (Admin).
*   **`ExamsController`**:
    *   `GET /api/Exams`: Retrieves a paginated list of all exams (Instructor, Admin).
    *   `GET /api/Exams/{id}`: Retrieves details for a specific exam (All Authenticated Roles).
    *   `GET /api/Exams/{id}/display`: Retrieves exam questions and choices formatted for display (All Authenticated Roles).
    *   `GET /api/Exams/student-results`: Retrieves a specific student's result for a given exam (Instructor, Admin).
    *   `POST /api/Exams`: Creates a new exam (Instructor, Admin).
    *   `PUT /api/Exams`: Updates an existing exam's details (Instructor, Admin).
    *   `DELETE /api/Exams/{id}`: Deletes an exam (Admin).
*   **`InstructorsController`**:
    *   `GET /api/Instructors`: Retrieves a paginated list of instructors (Instructor, Admin).
    *   `GET /api/Instructors/{id}`: Retrieves a specific instructor by ID (All Authenticated Roles).
    *   `PUT /api/Instructors/{id}`: Updates an instructor's details (e.g., track, branch) (Instructor, Admin).
    *   `DELETE /api/Instructors/{id}`: Deletes an instructor (Admin).
*   **`ManagementController`**:
    *   `POST /api/Management/upgrade`: Upgrades a user's role to Instructor or Student and assigns them to a track/branch (Admin).
*   **`QuestionBankController`**:
    *   `GET /api/QuestionBank`: Retrieves a paginated list of all questions (Instructor, Admin).
    *   `GET /api/QuestionBank/{id}`: Retrieves a specific question by ID (Instructor, Admin).
    *   `GET /api/QuestionBank/{id}/choices`: Retrieves all choices for a specific question (Instructor, Admin).
    *   `POST /api/QuestionBank`: Creates a new question (Instructor, Admin).
    *   `PUT /api/QuestionBank/{id}`: Updates an existing question (Instructor, Admin).
    *   `DELETE /api/QuestionBank/{id}`: Deletes a question (Admin).
*   **`StudentsController`**:
    *   `GET /api/Students`: Retrieves a paginated list of students (Admin, Instructor).
    *   `GET /api/Students/{id}`: Retrieves a specific student by ID (Admin, Instructor).
    *   `PUT /api/Students/{id}`: Updates a student's details (e.g., track, branch) (Admin, Instructor).
    *   `DELETE /api/Students/{id}`: Deletes a student (Admin).
*   **`SubmissionsController`**:
    *   `GET /api/Submissions`: Retrieves a paginated list of all submissions (Instructor, Admin).
    *   `GET /api/Submissions/{id}`: Retrieves a specific submission by ID (Student - their own, Instructor, Admin).
    *   `GET /api/Submissions/{id}/details`: Retrieves detailed answers for a specific submission (Instructor, Admin).
    *   `POST /api/Submissions`: Creates a new exam submission for the current student (Student).
    *   `DELETE /api/Submissions/{id}`: Deletes a submission (Admin).
*   **`TracksController`**:
    *   `GET /api/Tracks`: Retrieves a paginated list of all tracks (All Authenticated Roles).
    *   `GET /api/Tracks/{id}`: Retrieves a specific track by ID (All Authenticated Roles).
    *   `POST /api/Tracks`: Creates a new track (Instructor, Admin).
    *   `PUT /api/Tracks/{id}`: Updates an existing track (Instructor, Admin).
    *   `DELETE /api/Tracks/{id}`: Deletes a track (Admin).

## 🧠 Key Architectural Concepts & Patterns

ExamNest is built using several established architectural concepts and design patterns to ensure maintainability, testability, and scalability.


### Design Patterns

*   **🏢 Repository Pattern:** This pattern abstracts the data layer, separating the application logic from data access concerns. Interfaces like `IBranchRepository` define the data operations (CRUD, specific queries), and concrete classes like `BranchRepository` provide the actual implementation using EF Core and Stored Procedures. This makes the code more testable and easier to switch data sources if needed.
*   **🏭 Factory Pattern:** The `UserUpgradeFactory` is used to create instances of `Instructor` or `Student` based on the user's desired role upgrade. This centralizes the object creation logic, making the `ManagementController` cleaner and decoupling it from the specifics of how `Instructor` and `Student` objects are instantiated.
*   **💉 Dependency Injection (DI):** ASP.NET Core's built-in DI container is used extensively. Services and repositories are registered in `Program.cs` (`AddRepositories`, `AddServices`) and injected into constructors of classes that depend on them (Controllers, Services, Repositories themselves). This promotes loose coupling and testability.

### Algorithms

*   **⚖️ Exam Scoring Algorithm (`CorrectExam` Stored Procedure):** This stored procedure calculates a student's score for a given exam submission. It compares the `StudentAnswer` for each question against the `ModelAnswer` in the `QuestionBank` and sums up the `Points` for correctly answered questions. It returns detailed results including points earned, total possible points, and counts of answered and correct questions.
*   **❓ Question Selection Algorithm (`CreateExamAndGetId` Stored Procedure):** This stored procedure selects a specified number of questions (`NumberOfQuestions`) from the `QuestionBank` for a given `CourseID` to create a new exam. While the exact selection logic (e.g., random, specific criteria) isn't detailed in the C# code, the presence of the stored procedure indicates an algorithm is used to populate the `ExamQuestions` table. It suggests a mechanism for building an exam from a pool of questions.

### Clean Code Concepts

* separation of Concerns: Different responsibilities are handled by distinct classes and layers. This makes the codebase easier to understand, modify, and test.
* 🔁 DRY (Don't Repeat Yourself): Common functionality is extracted into reusable components. The `IGeneric` interface and `GenericRepository` provide common CRUD operations. Extension methods bundle service configurations.
* 🚫 Error Handling: Custom exceptions provide specific error types. The `GlobalExceptionHandlerMiddleware` centralizes exception handling, providing consistent and informative error responses to clients instead of letting raw exceptions propagate.
* ✅ Input Validation: FluentValidation is integrated to ensure that incoming DTOs meet specified constraints (`AuthenticationDTOValidation`, `ExamCreatePayloadValidator`, etc.) early in the request pipeline, preventing invalid data from reaching the business logic.
* Unified Response Structure: To make it easier and expected from the frontend we provided a standered API Response Structure 

## 🛠️ Technologies Used

*   **ASP.NET Core Web API**
*   **Entity Framework Core**
*   **SQL Server**
*   **Identity**
*   **JWT (JSON Web Tokens) Authentication**
*   **FluentValidation**
*   **Swagger/OpenAPI**
*   **Bogus**

---

Made with ❤️ by [Nourleyy](https://github.com/nourleyy) & [NASR](https://github.com/gitnasr)
