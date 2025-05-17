# Monolithic Web Apps

This repository contains a collection of monolithic web application projects, primarily built with ASP.NET Core and C#. Each folder represents a standalone project, ranging from blogs to product APIs.

## Table of Contents

- [blog-app](https://github.com/ofvarly/monolithic-web-apps/tree/main/blog-app)
- [course-app](https://github.com/ofvarly/monolithic-web-apps/tree/main/course-app)
- [efcore-app](https://github.com/ofvarly/monolithic-web-apps/tree/main/efcore-app)
- [forms-app](https://github.com/ofvarly/monolithic-web-apps/tree/main/forms-app)
- [identity-app](https://github.com/ofvarly/monolithic-web-apps/tree/main/identity-app)
- [meeting-app](https://github.com/ofvarly/monolithic-web-apps/tree/main/meeting-app)
- [movie-review-api](https://github.com/ofvarly/monolithic-web-apps/tree/main/movie-review-api)
- [products-api](https://github.com/ofvarly/monolithic-web-apps/tree/main/products-api)
- [store-app](https://github.com/ofvarly/monolithic-web-apps/tree/main/store-app)

---

## Project Summaries

### blog-app
A classic blog platform using ASP.NET Core MVC. Contains controllers, models, migrations, views, and a SQLite database (`blog.db`).

### course-app
An educational management app. Features standard ASP.NET directory structure and configuration files. Designed for learning and teaching scenarios.

### efcore-app
A sample app demonstrating Entity Framework Core usage. Includes migrations, models, and a sample database (`mydb.db`).

### forms-app
A web application focusing on handling forms. Built with ASP.NET Core, includes controllers, models, and views.

### identity-app
Demonstrates ASP.NET Core Identity for authentication and authorization. Contains models, migrations, tag helpers, and an `auth.db` database.

### meeting-app
A scheduling and meeting management application. Implements typical ASP.NET Core MVC structure.

### movie-review-api
A RESTful API for movie reviews. Includes a Dockerfile and docker-compose for containerization. Core logic is under the `src` directory.

### products-api
An API for managing products. Includes DTOs, migrations, models, and a sample database (`products.db`).

### store-app
A basic store or e-commerce app. The included README (`store-app/README.md`) provides .NET 8 installation links for Windows and MacOS.

---

## Getting Started

Each project is self-contained. To run a project:

1. Navigate to the desired app folder (e.g., `cd blog-app`).
2. Restore dependencies:  
   ```bash
   dotnet restore
   ```
3. Build and run:  
   ```bash
   dotnet run
   ```

Some projects require a local database file, which is included in the repository. Check each project for further configuration.

## Prerequisites

- [.NET 8 SDK](https://dotnet.microsoft.com/en-us/download/dotnet/8.0) (see `store-app/README.md` for platform-specific links)
- Docker (for `movie-review-api`)

## License

See individual projects for licensing details, if available.

---

> **Note:** This is a monorepo for demonstration and educational purposes. Each project can be used as a reference implementation or a starting point for .NET monolithic web apps.
