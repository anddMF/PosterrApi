# Posterr.API

**[Application requirements](https://onstrider.notion.site/Strider-Web-Back-end-Assessment-3-0-9dc16f041f5e4ac3913146bd7a8467c7)** <br>
This is a .NET 6 API for managing posts with MySQL as the database. The API supports operations like retrieving and inserting posts, with features based on the documentation provided.
This version is using a communication with database based on stored procedures because of the provided scope and to show a bit more knowledge in the SQL, but it can be done with Entity Framework as well.
 
## Running the application
 0. You need the latest version from **[Docker](https://www.docker.com/products/docker-desktop/)** and **Docker compose**
 1. Clone repository 
    ```sh
    git clone https://github.com/anddMF/PosterrApi
    ```
 2. Inside the directory where the docker-compose.yml is located, use the following: 
    ```sh
    docker-compose up --build
    ```
 3. Once it's finished, navigate to **[localhost:5000/swagger/index.html](http://localhost:5000/swagger/index.html)**

## Critique
There is always room to improve the codebase, it can be in performance or even readability. For me, an improvement for the near future could be the following:
- A better contract for the responses based on what the frontend needs, especially about error messages that cannot be shown to the end user with sensitive data. 
- Add more tests to different scenarios.
- The addition of a SQL Sink for Serilog could be useful for the easy tracking of bugs.

**Scaling**
As the app grows, not only in users but also in scope, some problems that I think could happen are:

- Database: queries will get slower with higher volumes of requests and datasets, I would add indexes for the most used columns in JOINS for better search and filtering results. Maybe a cache storage like Redis could a solution for some types of data.
- Authentication: as it wasn't a requirement, the controllers do not have user validation but as the app grows, that is a critical liability that should be addressed, a JWT-based authentication and role-based authorization would do the trick.
- Docker Swarm or Kubernetes: to deal with the growing amount of requests/traffic a good alternative would be to orchestrate multiple container instances of the API to scale based on demand and can serve as load balancing (which also can be done and with AWS ELB, for example).

##  Repository Structure

```bash
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
