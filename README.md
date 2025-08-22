# Royal Library

This project is a personal book library database, developed as a 2-hour coding challenge for Torc. It allows you to store information about books you own, love, or want to read, and search for them by Author, ISBN, or Title.

![Screenshot](./screenshot.png)

## Features

- **Backend API:** Developed with .NET 9.0 and Entity Framework Core, providing search functionality for books.
- **Frontend Web Application:** Built with React, TypeScript, and Material-UI, offering a user-friendly interface to search and display book information.
- **Database:** PostgreSQL, managed with Docker Compose for easy setup.

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

### 2. Start the Database

Navigate to the project root directory and start the PostgreSQL and pgAdmin containers using Docker Compose:

```bash
docker compose up -d
```

pgAdmin will be accessible at `http://localhost:5050` (email: `admin@admin.com`, password: `admin`).

### 3. Apply Database Migrations

Navigate to the project root directory and apply the Entity Framework Core migrations to create the database schema:

```bash
dotnet ef database update --project BookLibrary.Api -s BookLibrary.Api
```

### 4. Seed Initial Data (Optional)

To populate the database with initial book data, copy the `INSERT` statements provided in the challenge description and execute them using pgAdmin's Query Tool for the `booklibrary` database.

### 5. Run the Backend API

Navigate to the project root directory and run the .NET API:

```bash
dotnet run --project BookLibrary.Api
```

The API will be listening on `http://localhost:5043`.

### 6. Run the Frontend Application

Navigate to the `frontend` directory and start the React development server:

```bash
cd frontend
npm install
npm start
```

The frontend application will be accessible at `http://localhost:3000`.

## Future Improvements / Notes

- Implement pagination for search results.
- Add functionality to add, edit, and delete books.
- Improve UI/UX with more advanced Material-UI components and responsive design.
- Implement user authentication and authorization.
- Write unit and integration tests for both backend and frontend.

