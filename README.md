# Todo App (Angular + C# / .NET)

A simple full-stack To-Do SPA:

- **Backend:** ASP.NET Core 8 Web API, in-memory storage (no database, no persistence between runs).
- **Frontend:** Angular 17 (standalone components) styled with Bootstrap 5.

Features: add a task, list all tasks, mark complete/incomplete, delete a task.

```
TodoApp/
├── backend/
│   └── TodoApi/            # ASP.NET Core Web API
└── frontend/
    └── todo-app/            # Angular SPA
```

## Prerequisites

- [.NET 8 SDK](https://dotnet.microsoft.com/download)
- [Node.js 18+](https://nodejs.org/) and npm
- Angular CLI (`npm install -g @angular/cli`) — optional, `npx` works too

## 1. Run the backend

```bash
cd backend/TodoApi
dotnet restore
dotnet run
```

The API starts at **http://localhost:5000** (Swagger UI opens automatically at `/swagger` in Development).

Endpoints:

| Method | Route                    | Description              |
|--------|---------------------------|--------------------------|
| GET    | `/api/todos`              | List all tasks           |
| POST   | `/api/todos`               | Create a task (`{ "title": "..." }`) |
| PATCH  | `/api/todos/{id}/toggle`  | Toggle complete/incomplete |
| PUT    | `/api/todos/{id}`         | Update title/status      |
| DELETE | `/api/todos/{id}`         | Delete a task             |

CORS is pre-configured to allow requests from `http://localhost:4200` (the Angular dev server).

## 2. Run the frontend

In a separate terminal:

```bash
cd frontend/todo-app
npm install
npm start
```

This runs `ng serve` and opens the app at **http://localhost:4200**. The frontend expects the API to be running at `http://localhost:5000` (see `src/app/services/todo.service.ts` if you need to change the base URL).

## 3. Run tests

Run the backend unit tests from the repository root:

```bash
dotnet test backend/TodoApi.Tests/TodoApi.Tests.csproj
```

Run the Angular unit tests in headless Chrome:

```bash
cd frontend/todo-app
npm test
```

## Notes on design choices

- **In-memory store** (`InMemoryTodoService`) keeps things simple per the assessment's "no persistence required" note — data resets whenever the API restarts. Swapping in EF Core / a database later just means implementing `ITodoService` against a real store.
- **Standalone Angular components** (no NgModules) keep the frontend minimal — a single `TodoListComponent` handles add/list/toggle/delete, backed by a thin `TodoService` for HTTP calls.
- **Bootstrap 5** (via npm, included in `angular.json`) provides the styling — cards, list groups, form controls, buttons, and Bootstrap Icons for the check/trash icons.
