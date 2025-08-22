# Royal Library

This project is a personal book library database, developed as a 2-hour coding challenge for Torc. It allows you to store information about books you own, love, or want to read, and search for them by Author, ISBN, Title, or Ownership status. You can also sort the results by various criteria.

![Screenshot](./screenshot.png)

## Features

- **Backend API:** Developed with .NET 9.0 and Entity Framework Core, providing search, filtering, pagination, and sorting functionality for books.
- **Frontend Web Application:** Built with React, TypeScript, and Material-UI, offering a user-friendly interface to search and display book information.
- **Database:** PostgreSQL, managed with Docker Compose for easy setup.
- **Layered Architecture:** Backend is refactored into Controller, Service, and Repository layers for better separation of concerns and maintainability.
- **Unit Tests:** Comprehensive unit tests for the Service layer ensure business logic correctness.

## Setup and Run

Follow these steps to get the project up and running on your local machine.

### Prerequisites

- [.NET 9.0 SDK](https://dotnet.microsoft.com/download/dotnet/9.0)
- [Node.js and npm](https://nodejs.org/)
- [Docker](https://www.docker.com/get-started)

### 1. Clone the repository

```bash
git clone <repository_url>
cd torc_project
```

### 2. Run the Application

To get the entire application (database, backend API, and frontend) up and running with a single command, navigate to the project root directory and execute:

```bash
docker compose up -d && dotnet ef database update --project BookLibrary.Api -s BookLibrary.Api && dotnet run --project BookLibrary.Api & cd frontend && npm install && npm start
```

**Note:**
*   This command will:
    *   Start the PostgreSQL and pgAdmin containers in the background.
    *   Apply database migrations and seed initial data.
    *   Start the backend API in the background.
    *   Install frontend dependencies and start the React development server.
*   You might need to run `npm install` in the `frontend` directory separately if it fails during the combined command.

### Accessing the Application

*   **Frontend:** Accessible at `http://localhost:3000`
*   **Backend API:** Listening on `http://localhost:5043`
*   **Swagger UI:** Access the API documentation and test endpoints at `http://localhost:5043/swagger/index.html`
*   **pgAdmin:** Accessible at `http://localhost:5050` (email: `admin@admin.com`, password: `admin`).

### Filtering and Ordering

The application allows you to filter books by:
-   **Author**
-   **ISBN**
-   **Title**
-   **Ownership Status:** (Own, Love, Want to Read) - supports multiple selections.

You can also order the search results by:
-   **Title**
-   **Author**
-   **ISBN**
-   **Category**

### Running Tests

Unit tests for the backend Service layer are available. To run them, navigate to the project root directory and execute:

```bash
dotnet test BookLibrary.Tests/BookLibrary.Tests.csproj
```

### Database Migrations and Seeding

If you need to apply migrations or re-seed the database manually:

1.  **Apply Migrations:**
    ```bash
    dotnet ef database update --project BookLibrary.Api -s BookLibrary.Api
    ```
2.  **Seed Initial Data:** The initial data is seeded as part of the `AddBookSeedData` migration. If you need to re-seed or add more data, you can modify the `Sql/SeedData.sql` file in the `BookLibrary.Api` project and then apply the migration again (after potentially reverting the database to a state before the seed data migration).

## Future Improvements / Notes

- Implement functionality to add, edit, and delete books.
- Improve UI/UX with more advanced Material-UI components and responsive design.
- Implement user authentication and authorization.
- Further improve test coverage (e.g., integration tests, frontend tests).