# Posterr.API

This is a .NET 6 API for managing posts with MySQL as the database. The API supports operations like retrieving and inserting posts, with features based on the documentation provided.
This version is using a communication with database based on stored procedures because of the provided scope and to show a bit more knowledge in the SQL, but it can be done with Entity Framework as well.
 
## Running the application
 0. You need the latest version from **[Docker](https://www.docker.com/products/docker-desktop/)** and **Docker compose**
 1. Clone repository `git clone https://github.com/anddMF/PosterrApi`
 2. Inside the directory where the docker-compose.yml is located, use the following: `docker-compose up --build`
 3. Once it's finished, navigate to `http://localhost:5000/swagger/index.html`

## Critique

##  Repository Structure

```sh
└── PosterrApi/
    ├── Assets
    │   └── DB
    │       └── Initial dump for the database.
    ├── Posterr.API
    │   ├── Controllers
    │   │   ├── Controllers from the project.
    │   ├── Dockerfile
    │   ├── Entities
    │   │   ├── Models used by the application at the running and business layer.
    │   ├── Infrastructure
    │   │   ├── DAL
    │   │   │   └── Data access folder.
    │   │   │   ├── DAO
    │   │   │   │   ├── Models used by the infra layer.
    │   │   └── Factories
    │   │       ├── Factories used by the project for external communications.
    │   ├── Interfaces
    │   │   ├── Interfaces from the project.
    │   ├── Middlewares
    │   │   └── Middlewares from the project
    │   ├── Services
    │   │   ├── Services with the business logic from the scope.
    ├── Posterr.API.Test
    │   ├── Project for unit testing.
```

[![linkedin](https://img.shields.io/badge/linkedin-0A66C2?style=for-the-badge&logo=linkedin&logoColor=white)](https://www.linkedin.com/in/andrew-moraes-f/)