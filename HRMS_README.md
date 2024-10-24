# HR Management System (HRMS)

## Overview

The **HR Management System (HRMS)** is a comprehensive platform built to manage employees, companies, shifts, leaves, expenses, and holidays within an organization. This system is designed to streamline HR processes, improve efficiency, and provide a user-friendly interface for admins, managers, and employees. The system offers role-based dashboards, notifications, and approval mechanisms to simplify daily HR tasks.

This project leverages modern technologies, best coding practices, and design patterns to ensure scalability, maintainability, and performance.

## Table of Contents

- [Technologies Used](#technologies-used)
- [Features](#features)
- [Project Structure](#project-structure)
- [Design Patterns](#design-patterns)
- [Database Design](#database-design)
- [Setup](#setup)
- [Usage](#usage)
- [License](#license)

## Technologies Used

### Backend (C# / .NET)

- **ASP.NET Core 8.0**: The backend API framework for building scalable and high-performance web applications.
- **Entity Framework Core 8.0**: ORM for database management and migrations, utilizing **SQL Server**.
- **JWT Authentication**: Secure user authentication using JSON Web Tokens.
- **FluentValidation**: For defining validation rules and enforcing input data integrity.
- **AutoMapper**: For seamless object mapping between layers.
- **MailKit**: For handling email-related operations such as notifications and approvals.

### Frontend (React)

- **React**: A popular JavaScript library for building user interfaces.
- **Vite**: Fast build tool for modern web development.
- **TailwindCSS**: A utility-first CSS framework for styling.
- **Context API / Hooks**: For state management and handling global data.

### Other Tools

- **Swagger (Swashbuckle)**: For generating API documentation and exploring endpoints.
- **Vercel**: For continuous deployment and hosting the frontend.

## Features

- **Admin, Manager, Employee Dashboards**: Role-based dashboards with access to different functionalities.
- **Company Management**: CRUD operations on companies.
- **Employee Management**: Managing employee details, shifts, and leaves.
- **Leave and Expense Requests**: Employees can submit requests, and managers can approve them.
- **Notification System**: Role-based notifications for upcoming events, holidays, and company announcements.
- **Role-Based Access Control**: Secure endpoints with JWT-based authentication for admins, managers, and employees.

## Project Structure

### Backend (ASP.NET Core / C#)

```
HrManagementSystem
│
├── API
│   ├── Controllers
│   ├── Middleware
│   └── Models
├── BusinessLogic
│   ├── Services
│   ├── DTOs
│   └── Validators
├── DataLayer
│   ├── Entities
│   ├── Migrations
│   ├── Repositories
│   └── UnitOfWork
└── ServiceLayer
    ├── EmailService
    ├── FileService
    └── LoggingService
```

### Frontend (React)

```
src
│
├── components
├── hooks
├── layouts
├── pages
├── routes
└── services
```

## Design Patterns

The project follows **SOLID principles** and implements key design patterns to ensure clean and maintainable architecture:

- **Repository Pattern**: To handle data access logic and separation from business logic.
- **Unit of Work Pattern**: To manage transactions across repositories and ensure atomicity.
- **Service Layer**: Centralized business logic for reusability and clean code.
- **DTOs (Data Transfer Objects)**: For secure and efficient data transfer between layers.
- **AutoMapper**: To handle object transformations efficiently.

## Database Design

The system is backed by a well-structured **SQL Server** database with entities for managing employees, companies, shifts, leaves, expenses, resumes, and more. Key relations and entities include:

- **Employees**: Manages all employee data, linked to resumes, certifications, and experiences.
- **Companies**: Represents companies within the system.
- **Shifts, Leaves, Expenses**: Tracks employee schedules, time-off requests, and submitted expenses.
- **Notifications**: Manages all notifications for upcoming events, birthdays, and announcements.

![Database Diagram](./path_to_your_database_diagram.png)

## Setup

### Backend

1. Clone the repository:

   ```bash
   git clone https://github.com/botanoz/hr-management-system.git
   cd hr-management-system
   ```

2. Navigate to the backend project:

   ```bash
   cd HrManagementSystem.API
   ```

3. Install required packages:

   ```bash
   dotnet restore
   ```

4. Run the project:

   ```bash
   dotnet run
   ```

5. The backend will be running on `https://localhost:5001`.

### Frontend

1. Navigate to the frontend folder:

   ```bash
   cd hr-management-frontend
   ```

2. Install dependencies:

   ```bash
   npm install
   ```

3. Start the development server:

   ```bash
   npm start
   ```

4. The frontend will be running on `http://localhost:3000`.

## Usage

- Admins can log in to manage companies, employees, and overall system settings.
- Managers can view their team’s shifts, leave requests, and approve expenses.
- Employees can submit their leave and expense requests, and view their upcoming shifts and holidays.

## License

This project is licensed under the **MIT License**. Feel free to contribute or fork the project!

---

If you have any questions or suggestions, feel free to open an issue or submit a pull request. Contributions are welcome!
