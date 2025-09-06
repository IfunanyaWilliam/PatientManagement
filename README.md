# PatientManagement API Project

## Description

PatientManagement is an API for managing patients and their associated medical records. It provides endpoints for handling patient account information, personal details, and medical consultation records. The project is built with .NET 8.0 and is designed for extensibility and ease of integration.

## Table of Contents

- [Installation](#installation)
- [Usage](#usage)
- [Features](#features)
- [Configuration](#configuration)
- [License](#license)

## Installation

### Prerequisites

- [.NET SDK](https://dotnet.microsoft.com/download) (version 8.0)
- [Docker](https://www.docker.com/) (optional, for containerization)
- Programming language: C#

### Steps

1. Clone the repository:
    ```bash
    git clone https://github.com/IfunanyaWilliam/PatientManagement.git
    ```
2. Navigate to the project directory:
    ```bash
    cd PatientManagement
    ```
3. Restore dependencies:
    ```bash
    dotnet restore
    ```
4. Build the project:
    ```bash
    dotnet build
        ```
5. Run the API:
    ```bash
    dotnet run --project PatientManagement.Api
    ```

## Usage

- The API exposes endpoints for creating and managing patient accounts, retrieving patient information, and recording medical consultations.
- Use tools like [Postman](https://www.postman.com/) or [curl](https://curl.se/) to interact with the API.
- Example endpoint:
    ```
    POST /api/v1/Account/CreateUser
    ```
    Payload:
    ```json
    {
      "email": "user@example.com",
      "password": "yourpassword",
      "userRole": "Patient"
    }
    ```

## Features

- Patient account creation and management
- Secure authentication and authorization
- Medical record storage and retrieval
- API versioning
- Extensible command and query architecture (CQRS)
- Docker support for containerized deployment

## Configuration

- Configuration files are located in the `PatientManagement.Api` project.
- Environment variables can be used to override default settings.
- For Docker deployment, review the provided `Dockerfile` and `.dockerignore`.

## License

This project is licensed under the MIT License. 