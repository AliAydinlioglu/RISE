# Rise - TIAO2

## Team Members

- Iliass Assoued - iliass.assoued@student.hogent.be - [@Iliassassoued](https://github.com/Iliassassoued)
- Ali Aydinlioglu - ali.aydinlioglu@student.hogent.be - [@AliAydinlioglu](https://github.com/AliAydinlioglu)
- Wim Dedulle - wim.dedulle@student.hogent.be - [@WimDedulle](https://github.com/WimDedulle)
- Pieter Pletinckx - pieter.pletinckx@student.hogent.be - [@pspletinckx](https://github.com/pspletinckx)
- Pieter Swillens - pieter.swillens@student.hogent.be - [@Pieter-Swillens](https://github.com/Pieter-Swillens)
- Andy Wauters - andy.wauters@student.hogent.be - [@ndwauterss](https://github.com/ndwauterss)
- Marek Zakrzewski - marek.zakrzewski@student.hogent.be - [@MarekZakrzewskiHoGent](https://github.com/MarekZakrzewskiHoGent)

## Technologies & Packages Used

- [Blazor](https://dotnet.microsoft.com/en-us/apps/aspnet/web-apps/blazor) - Frontend.
- [ASP.NET 9](https://dotnet.microsoft.com/en-us/apps/aspnet) - Backend.
- [Entity Framework 9](https://learn.microsoft.com/en-us/ef/) - Database Access with Unit Of Work and Repository patterns.
- [EntityFrameworkCore Triggered](https://github.com/koenbeuk/EntityFrameworkCore.Triggered) - Database Triggers which are agnostic to the database provider.
- [User Secrets](https://docs.microsoft.com/en-us/aspnet/core/security/app-secrets) - Securely store secrets in DEV.
- [GuardClauses](https://github.com/ardalis/GuardClauses) - Validation Helper.
- [Ardalis.Result](https://github.com/ardalis/Result) - A result abstraction that can be mapped to HTTP response codes if needed.
- [FastEndpoints](https://fast-endpoints.com/) - is a developer friendly alternative to Minimal APIs & MVC.
- [Serilog](https://serilog.net/) - Framework for structured tracable logging to Console and Files.
- [FluentValidation](https://docs.fluentvalidation.net/en/latest/) - is a .NET library for building strongly-typed validation rules.
- [Blazored.FluentValidation](https://docs.fluentvalidation.net/en/latest/) - Blazor + Fluentvalidation.
- [bUnit](https://bunit.dev) - Blazor Component Testing.
- [xUnit](https://xunit.net) - (Unit) Testing.
- [nSubstitute](https://nsubstitute.github.io) - Mocking for testing.
- [Shouldly](https://docs.shouldly.org) - Helper for testing.
- [Destructurama.Attributed](https://github.com/destructurama/attributed) - Masking for sensitive datatypes.

## Software 
1. Install [Rider](https://www.jetbrains.com/rider/) or [Visual Studio](https://visualstudio.microsoft.com/)
2. Make sure you have [ASP.NET 9](https://dotnet.microsoft.com/en-us/download) installed (comes with Rider and Visual Studio) 

## Installation Instructions

1. Clone the repository

2. Open the `Rise.sln` file in [Rider](https://www.jetbrains.com/rider/), [Visual Studio](https://visualstudio.microsoft.com/) or  [Visual Studio Code](https://code.visualstudio.com/). (we prefer Rider, but you're free to choose.)

3. Run the project using the `Rise.Server` project as the startup project

4. The project should open in your default browser on port 5001.

5. The database (SQLite) will be created. However you will have to switch the database provider of your choosing 

   1. **SQL Server**

      Package: Microsoft.EntityFrameworkCore.SqlServer

      🔗 [NuGet Link](https://www.nuget.org/packages/Microsoft.EntityFrameworkCore.SqlServer/)

   2. **MariaDB**

      Package: Pomelo.EntityFrameworkCore.MySql

      🔗 [NuGet Link](https://www.nuget.org/packages/Pomelo.EntityFrameworkCore.MySql/)

   3. **PostgreSQL**

      Package: Npgsql.EntityFrameworkCore.PostgreSQL

      🔗 [NuGet Link](https://www.nuget.org/packages/Npgsql.EntityFrameworkCore.PostgreSQL/)

   4. Mongo etc... 

## Using Docker
To have a full environment, Docker is being used for 2 projects:
- Rise.Client
- Rise.Server

MariaDB is used instead of SQLite.

Go to https://www.docker.com/ for downloading Docker Desktop installation file.
Make sure docker desktop is up and running.
Go to solution path:

```
docker compose build --no-cache
```

```
docker compose up
```


## Creation of the database

Is done by the app itself using migrations. To add and remove migrations, install the dotnet ef tool globally by running the following command in your terminal (only do this once)

```
dotnet tool install --global dotnet-ef
```

## Migrations

Adapting the database schema can be done using migrations. To create a new migration, run the following command in the `src` folder

```
dotnet ef migrations add YourMigrationName --startup-project Rise.Server --project Rise.Persistence
```

And then update the database using the following command, or run the `Rise.Server`

```
dotnet ef database update --startup-project Rise.Server --project Rise.Persistence
```

## Usefull Commands

In the `src/Rise.Server` folder

`dotnet watch --non-interactive` 

The `dotnet watch` command is a file watcher. When it detects a change, it runs the `dotnet run` command or a specified `dotnet` command. If it runs `dotnet run`, and the change is supported for [hot reload](https://learn.microsoft.com/en-us/dotnet/core/tools/dotnet-watch#hot-reload), it hot reloads the specified application. If the change isn't supported, it restarts the application. This process enables fast iterative development from the command line.

`dotnet run`

The `dotnet run` command provides a convenient option to run your application from the source code with one command. It's useful for fast iterative development from the command line. The command depends on the [`dotnet build`](https://learn.microsoft.com/en-us/dotnet/core/tools/dotnet-build) command to build the code. Any requirements for the build apply to `dotnet run` as wel

`dotnet clean `- you won't need this often

The `dotnet clean` command cleans the output of the previous build. It's implemented as an [MSBuild target](https://learn.microsoft.com/en-us/visualstudio/msbuild/msbuild-targets), so the project is evaluated when the command is run. Only the outputs created during the build are cleaned. Both intermediate (*obj*) and final output (*bin*) folders are cleaned.

## Authentication

Authentication and authorization is handled by Microsoft Entra ID. 

- The **Blazor WASM** app handles **user login** via Microsoft Entra ID (OpenID Connect + PKCE).
- The **API** validates the **JWT access token** sent by Blazor.
- **No Client Secret** is required unless the API calls other services on its own (not applicable here).

### Users (development)

- regular@rise2526t2campusappoutlook.onmicrosoft.com (pw: Tako499281)

### Roles

There are 3 built-in roles, but adjust as needed

- Public (may be used for visuals)
- RegularStudent
- DistanceStudent

### Use cases

- GetInfo
- LoginCallback (after successful login in client)

### Requirements

- Microsoft Entra ID (Azure AD) tenant
- 2 App registrations (client + server)
- Access to [https://entra.microsoft.com](https://entra.microsoft.com)

### Setup Microsoft Entra ID

### 1. App registrations

#### A. FastEndpoints API (backend)

1. Go to **App registrations → New registration**
2. Name it e.g. `FastEndpointsAPI`
3. Type: "Accounts in this organizational directory only"
4. Click **Register**

#### Expose an API
1. Go to **Expose an API**
2. Set the **Application ID URI**, e.g.: api://<client-id>
3. Add a **scope**:
- **Scope name:** e.g. `user.read`
- **Who can consent:** Admins and users
- **Admin consent display name:** Access API
- **Admin consent description:** Allows access to FastEndpoints API
- Click **Add scope**
4. Go to **Manifest** and ensure `"accessTokenAcceptedVersion": 2` is set.

#### B. Blazor WASM (frontend)

1. Go to **Microsoft Entra ID → App registrations → New registration**
2. Name it e.g. `BlazorWasmClient`
3. Choose **"Accounts in this organizational directory only"**
4. Set the redirect URI:
    - `https://{base-url}/api/identity/accounts/login-callback`
      - replace {base-url} by the base url of your client
      - in development this will probably be localhost:5001, but port may be different
5. Click **Register**

#### Configure the app
- Note the **Application (Client) ID** and **Directory (Tenant) ID**
- Go to **Authentication**
    - Add a logout redirect URI:  
      `https://{base-url}/authentication/logout-callback`
      - replace {base-url} by the base url of your client
      - in development this will probably be localhost:5001, but port may be different
  - Enable *Allow public client flows (PKCE)*

#### Configure API access
1. Go to **API permissions**
2. Click **Add a permission → My APIs (or All API's) → [your API]**
3. Select the scope `user.read` (or your custom scope)
4. Click **Grant admin consent**

### 2. Project Configuration

#### A. FastEndpoints API
#### Add following items in `Program.cs`
````csharp
using Microsoft.Identity.Web;
using Microsoft.AspNetCore.Authentication.JwtBearer;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddMicrosoftIdentityWebApi(builder.Configuration.GetSection("AzureAd"));

builder.Services 
    ...
    .AddAuthorization()
    .AddCors(options =>
    {
        options.AddPolicy("FrontendPolicy", policy =>
        {
            var frontendUrl = builder.Configuration["FrontendUrl"];
            policy.WithOrigins(frontendUrl)
                  .AllowAnyMethod()
                  .AllowAnyHeader();
        });
    });

var app = builder.Build();

app
    ...
    .UseAuthentication()
    .UseAuthorization()
    ...
    .UseCors("FrontendPolicy");

app.Run();

````
#### Add following items in appsettings.json and replace props between brackets by values from settings in Microsoft Entra ID
````json
"FrontendUrl": "{frontend-url}",
"AzureAd": {
    "Instance": "https://login.microsoftonline.com/",
    "Domain": "{domain}.onmicrosoft.com",
    "ClientId": "{api-client-id}",
    "TenantId": "{tenant-id}",
    "Audience": "api://{api-client-id}"
}
````

#### B. Blazor WASM
Install package in Rise.Client:
````dotnet add package Microsoft.Authentication.WebAssembly.Msal````


Add to Rise.Client/Program.cs:
````csharp
    builder.Services.AddMsalAuthentication(options =>
    {
        builder.Configuration.Bind("AzureAd", options.ProviderOptions.Authentication);
        options.ProviderOptions.DefaultAccessTokenScopes.Add("api://8ae1a8dc-c2c6-44c9-bed8-7bbce3f84590/access_as_user");
        options.ProviderOptions.LoginMode = "redirect";
        options.ProviderOptions.Cache.CacheLocation = "localStorage"; 
    });
````    
Last option will make sure that your token is saved in localstorage, so that when you refresh a page you're still logged in. You can also change it with 'sessionStorage' or 'memory'.

Add to Rise.Client/appsettings.json
````json
  "AzureAd": {
    "Authority": "https://login.microsoftonline.com/052a5cbb-135d-45c5-b50e-689b56d29142",
    "ClientId": "dbcbd040-8ba4-4ed9-9004-fafff85674c7",
    "ValidateAuthority": true
  },
````

Add to Rise.Client/wwwroot/index.html
````html
    <script src="_content/Microsoft.Authentication.WebAssembly.Msal/AuthenticationService.js"></script>
    <script src="_framework/blazor.webassembly.js"></script>
````
## Solution Structure Overview

The template is designed as a boilerplate or template for .NET solutions, following best practices for structuring projects, separation of concerns, and maintainability. Here's a breakdown of the solution structure and its workings, explained:

When you open the solution, you’ll notice it’s organized into multiple projects, which is a common approach in large, enterprise-level applications. Each project within the solution has a specific responsibility. This approach is based on the **Clean Architecture**, **Domain-Driven Design (DDD)** and **Vertical Slicing**  principles. The goal is to keep different aspects of the application separated and independent, making it easier to scale, maintain, and test.

Here are the main projects in the solution:

1. **Domain**
2. **Services**
3. **Persistence**
4. **Server**
5. **Client**
6. **Shared**

Let’s look at each of these in more detail:

------

### 1. **Domain Project**

**Folder**: `Domain`

**Purpose**: The **Domain** project holds the core logic of the application. It defines the business rules, which are independent of the UI, database, or any external technology. The principle here is to keep the domain logic isolated, making sure it’s not affected by external frameworks or infrastructure.

**Typical Contents**:

- **Entities**: Classes that represent the core objects of the application, such as `Order`, `Customer`, or `Product`.

- **Value Objects**: Immutable objects that represent a concept (like `Money` or `Address`).

  > Currently not provided in the template, but you can read more here: [Domain Driven Design - Best Practises](https://hogent-web.github.io/csharp/chapters/03/slides/index.html#75)

**Why this separation?**: Keeping the domain logic separate ensures that the business rules remain consistent even if the application’s presentation or infrastructure changes. This allows for flexibility and ensures that changes to other parts of the system don't break the business logic.

------

### 2. **Services Project**

**Folder**: `Services`

**Purpose**: The **Services** project is responsible for the application-specific logic, such as orchestrating use cases, handling commands, and queries, and processing workflows. It acts as an intermediary between the **Domain** and the **Infrastructure** or **API** layers.

**Typical Contents**:

- **Use Cases**: These classes are responsible for specific actions in the system, like creating an order or processing a payment.

**Why this separation?**: This project enforces the **Separation of Concerns (SoC)**. It also makes testing easier, as this layer can be unit tested without worrying about external dependencies. 

> Note that we can swap out the API for something else, for example a console application and the business rules will still apply.
>
> We do not recommend abstracting your database as we see it as a migration to another database provider, not an abstraction. Read more about it [Should you Abstract the Database ](https://enterprisecraftsmanship.com/posts/should-you-abstract-database/). However you will have to switch to a real database provider not SQLite.

------

### 3. **Persistence Project**

**Folder**: `Persistence`

**Purpose**: The **Persistence** project deals with Database mappings and database migrations, that's it.

**Typical Contents**:

- **Configurations**: Entity configurations, for example how a product is mapped to a table in SQL, using Entity Framework Core.
- **Data Migrations**: Scripts or classes for evolving the database schema over time.
- **Triggers**: Stuff that needs to happen when something is saved or retrieved from the database. It's rather optional but these triggers are database agnostic (they will work for any provider e.g. MariaDb, Microsoft SQL Server,... )

**Why this separation?**: So it's easier to find the configurations and keep them out of the **Domain** logic, Domain classes should **not** know how they're stored.

------

### 4. **Server Project**

**Folder**: `Server`

**Purpose**: The **API** project is the entry point for the application, where the HTTP endpoints are defined. It handles requests from clients (via RESTful HTTP requests) and returns responses. It uses **FastEndspoints** to expose application functionality to the outside world.

**Typical Contents**:

- **Endpoints**: These handle HTTP requests and responses. They receive requests, pass them to the appropriate application service, and return the result.
- **Processors**: Custom components that handle cross-cutting concerns such as logging, or error handling.
- **Dependency Injection Configuration**: The **Server** project contains the setup for the dependency injection container, where the various services and other dependencies are registered.
- **Serving the Blazor Client** : If no endspoints are found, the **Server** returns the Blazor WebAssembly (WASM) **Client**, it's rather optional but it makes hosting a lot easier (No CORS issues etc.)

**Centralized Response Handling**:

You might notice something interesting about how endpoints send responses. In `Program.cs`, the FastEndpoints configuration includes `ep.DontAutoSendResponse()`. This setting disables the default behavior where an endpoint would immediately send back whatever it returns.

So how are responses sent? We use a custom **Post-Processor** called `GlobalResponseSender`. This processor runs after every endpoint and is responsible for creating the final HTTP response. It takes the object returned by your endpoint—typically an `Ardalis.Result`—and intelligently maps it to the correct HTTP status code.

For example:
- If your endpoint returns a successful `Result<ProductDto>`, the processor creates a `200 OK` response containing the product data.
- If it returns `Result.Invalid(errors)`, the processor creates a `400 Bad Request` response with the validation errors.
- If it returns `Result.NotFound()`, it becomes a `404 Not Found` response.

This pattern is powerful because it keeps your endpoint logic clean and focused on its core task, while ensuring all your API responses are consistent and handled in one central place.

**Why this separation?**: The **API** layer provides a clean separation between the user interface (UI) and the business logic. This project acts as the boundary between your back-end system and the outside world, and it enforces that external clients (e.g., mobile apps or front-end websites) communicate in a consistent and defined way.

------

### 5. **Client Project**

**Folder**: `Client` 

A Blazor Web Assembly Standalone client, just like React, Vue, Svelte, Angular,... but written in C#. 

---

### 6. **Shared Project**

The **Shared** project is the glue between the **Client** and the **Server**. It decouples the Domain from the the Client, therefore we can still adjust the database , Services and Domain layer without breaking any clients. If we don't remove properties from the Data Transfer Objects (**DTO**)

- **Service Interfaces**: The contract between the **Client** and **API**.
- **Data Transfer Objects**: Simple classes without any domain logic. They're used to transfer data from the **API** to the **Client**.

------

### 7. **Testing Projects**

**Folder**: `Client.Tests`, ``Services.Tests` and `Domain.Tests`

While not always included in the base template, most well-architected solutions should have dedicated testing projects, typically organized into **Unit Tests**, **Integration Tests**, and possibly **End-to-End Tests**.

- **Unit Tests**: Test individual components (usually found in the `Domain` or `Client` layers) in isolation from dependencies.

- **Integration Tests**: Ensure different parts of the system work together correctly (e.g., API and database).

  > We did not provide any integration tests, these are for you to figure out. But you can take a look [here](https://fast-endpoints.com/docs/integration-unit-testing#integration-testing) to get you in the right direction.

By separating the tests into their own projects, you ensure that they remain maintainable, modular, and focused on the specific functionality being tested.

---

### 8. **Cross-Cutting Concerns**

In some solutions, you may see additional projects or services to handle **cross-cutting concerns** like **logging**, **caching**, **authorization**, or **exception handling**. These concerns can be plugged into multiple layers of the solution but are typically handled in the **Persistence** / **Infrastructure** and **Server** projects.

------

### Key Concepts Explained

1. **Separation of Concerns (SoC)**: Each project in the solution has a single, well-defined responsibility. By separating concerns, changes in one part of the system (e.g., switching databases) do not ripple through the entire codebase.
2. **Dependency Injection (DI)**: This design pattern is used to inject dependencies into classes. The **API** project often configures DI, so classes get the services or repositories they need without creating them directly. This promotes loose coupling and makes the code easier to test.
3. **Domain-Driven Design (DDD)**: The structure of the **Domain** project follows DDD principles, where the business rules and logic are core to the application and should be isolated from infrastructure concerns. This keeps your business logic intact even as external technologies evolve.

------

### Conclusion

The `dotnet-template` solution is structured to encourage scalability, maintainability, and testability. Each project serves a distinct purpose:

- **Domain** defines core business logic.
- **Services** manages use cases and orchestrates the flow of information.
- **Persistence** handles the interaction with external systems and data storage.
- **API** exposes the functionality to the outside world via HTTP.
- **Client** a User Interface that could be swapped if need be.

## Course

There is .NET course from 1-2 years ago which is no longer maintained but still relevant..

https://hogent-web.github.io/csharp/

