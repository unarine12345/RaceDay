# RaceDay API 

## 1. Project Overview

RaceDay is a RESTful API developed using ASP.NET Core Web API and C#. The system is designed to help organisers manage race events and allow participants to enrol in events and view their race results.

The system separates users into two main roles: **Organiser** and **Participant**. Each role has different responsibilities and access permissions.

## 2. Project Objectives

The objectives of the RaceDay system are to:

- Allow users to register and log in.
- Provide secure password handling and authentication.
- Support role-based access for Organisers and Participants.
- Allow organisers to create and manage race events.
- Support event categories and participant enrolments.
- Allow organisers to record race results and finishing positions.
- Allow participants to view their enrolments and results.
- Provide API documentation through Swagger.
- Use automated testing and continuous integration to improve software quality.

## 3. Technologies Used

- **C#** – Programming language.
- **ASP.NET Core Web API** – REST API framework.
- **.NET 8** – Target framework.
- **Microsoft SQL Server** – Database management system.
- **Entity Framework Core** – Object-relational mapping and database access.
- **Swagger / OpenAPI** – API documentation and endpoint testing.
- **Visual Studio 2022** – Development environment.
- **Git and GitHub** – Version control and source code hosting.
- **GitHub Actions** – Automated build and test workflow.
- **xUnit** – Unit testing framework, if configured in the test project.

## 4. Main System Features

### User Authentication

- User registration.
- User login and logout.
- Password hashing.
- Authentication and authorisation.

### Organiser Functions

- Create and manage race events.
- Manage event categories.
- View participant enrolments for their events.
- Record finishing times and finishing positions.

### Participant Functions

- View available race events.
- Enrol in events and select a category.
- View personal enrolments.
- View personal race results.

### API Documentation

Swagger provides an interactive interface for viewing and testing available API endpoints.

## 5. Database Design

The system uses the following eight main entities:

1. **Role** – Defines user roles.
2. **User** – Stores user account information.
3. **Organiser** – Represents race organisers.
4. **Participant** – Represents race participants.
5. **Event** – Stores race event details.
6. **Category** – Stores event categories.
7. **EventEnrolment** – Connects participants to events.
8. **Result** – Stores finishing times and positions.

Entity Framework Core manages the relationships between these entities and the SQL Server database.

## 6. Project Structure

The main project contains the following components:

- `Controllers/` – Handles incoming API requests.
- `Data/` – Contains the database context.
- `DTOs/` – Defines data-transfer objects.
- `Models/` – Contains the entity classes.
- `Program.cs` – Configures and starts the API.
- `appsettings.json` – Stores application configuration, including the database connection string.
- `Properties/launchSettings.json` – Defines local development launch profiles.

The solution also contains a test project named `RaceDay.Tests`.

## 7. Installation and Setup

### Prerequisites

Install the following software:

- Visual Studio 2022.
- The .NET 8 SDK.
- Microsoft SQL Server or SQL Server LocalDB.
- Git, if cloning the repository from GitHub.

### Step 1: Clone the repository

Replace the placeholder URL with the URL of your GitHub repository.

```bash
git clone YOUR_GITHUB_REPOSITORY_URL
```

Open the downloaded solution file, `RaceDay.sln`, in Visual Studio 2022.

### Step 2: Configure the database

Open `appsettings.json` and check the `DefaultConnection` connection string.

Example for SQL Server LocalDB:

```json
{
  "Connection
