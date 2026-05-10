# Doctor Track API

Backend API for a **Doctor License Management module** built for a Medical SaaS platform.

## Tech Stack

* ASP.NET Core 10 Web API
* Dapper
* SQL Server
* Clean Architecture
* Swagger/OpenAPI
* AutoMapper (optional)

---

## Features

### Doctor Management

* Create doctor
* Get all doctors
* Get doctor by Id
* Update doctor
* Update doctor status
* Soft delete doctor

### Business Rules

* Prevent duplicate license number
* Auto mark **Expired** if license expiry date is less than today
* Required field validation
* Soft delete support

### Database Features

* SQL Server database
* Stored procedure for doctor listing
* Status logic inside SQL query
* Search & filter support in stored procedure

---

## API Endpoints

| Method | Endpoint                   | Description          |
| ------ | -------------------------- | -------------------- |
| GET    | `/api/doctors`             | Get all doctors      |
| GET    | `/api/doctors/{id}`        | Get doctor by Id     |
| POST   | `/api/doctors`             | Create doctor        |
| PUT    | `/api/doctors/{id}`        | Update doctor        |
| PATCH  | `/api/doctors/{id}/status` | Update doctor status |
| DELETE | `/api/doctors/{id}`        | Soft delete doctor   |

---

## Database Setup

### 1. Create Database

Run this in SQL Server:

```sql
CREATE DATABASE DoctorTrack;
```

---

### 2. Create Doctors Table

```sql
CREATE TABLE Doctor
(
    Id INT IDENTITY(1,1) PRIMARY KEY,

    FullName NVARCHAR(150) NOT NULL,

    Email NVARCHAR(150) NOT NULL,

    Specialization NVARCHAR(100) NOT NULL,

    LicenseNumber NVARCHAR(100) NOT NULL UNIQUE,

    LicenseExpiryDate DATE NOT NULL,

    Status NVARCHAR(50) NOT NULL DEFAULT 'Active',

    CreatedDate DATETIME NOT NULL DEFAULT GETDATE(),

    IsDeleted BIT NOT NULL DEFAULT 0
);
```

---

### 3. Create Stored Procedure

```sql
CREATE PROCEDURE sp_GetDoctors
(
    @Search NVARCHAR(100) = NULL,
    @Status NVARCHAR(50) = NULL
)
AS
BEGIN

SELECT
    Id,
    FullName,
    Email,
    Specialization,
    LicenseNumber,
    LicenseExpiryDate,

    CASE
        WHEN LicenseExpiryDate < CAST(GETDATE() AS DATE)
            THEN 'Expired'
        ELSE Status
    END AS Status,

    CreatedDate

FROM Doctor
WHERE IsDeleted = 0

AND (
    @Search IS NULL
    OR FullName LIKE '%' + @Search + '%'
    OR LicenseNumber LIKE '%' + @Search + '%'
)

AND (
    @Status IS NULL
    OR
    (
        CASE
            WHEN LicenseExpiryDate < CAST(GETDATE() AS DATE)
                THEN 'Expired'
            ELSE Status
        END
    ) = @Status
)

ORDER BY CreatedDate DESC

END
```

---

## Configuration

Update connection string in:

`appsettings.json`

```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Server=localhost;Database=DoctorTrack;Trusted_Connection=True;TrustServerCertificate=True"
  }
}
```

For SQL Authentication:

```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Server=localhost;Database=DoctorTrack;User Id=sa;Password=yourPassword;TrustServerCertificate=True"
  }
}
```

---

## Required NuGet Packages

Run:

```bash
dotnet add package Dapper

dotnet add package Microsoft.Data.SqlClient

dotnet add package Swashbuckle.AspNetCore
```

Optional:

```bash
dotnet add package AutoMapper

dotnet add package AutoMapper.Extensions.Microsoft.DependencyInjection
```

---

## Run Project

### Restore Packages

```bash
dotnet restore
```

### Build

```bash
dotnet build
```

### Run

```bash
dotnet run
```

---

## Swagger

After running:

```txt
https://localhost:xxxx/swagger
```

Replace `xxxx` with the generated port.

---

## Sample Request Payload

### Create Doctor

```json
{
  "fullName": "John Doe",
  "email": "john@example.com",
  "specialization": "Cardiology",
  "licenseNumber": "DOC-1001",
  "licenseExpiryDate": "2027-12-31"
}
```

### Update Status

```json
"Suspended"
```

---

## Design Decisions

### Why Clean Architecture?

Used to separate concerns and improve maintainability.

### Why Dapper?

Dapper was chosen for lightweight, high-performance database access and easier stored procedure execution.

### Why Soft Delete?

To avoid permanent data loss and preserve historical records.

### Why Stored Procedure?

Required by assignment and used for optimized search/filter listing.

---

## Future Improvements

* JWT Authentication
* Pagination
* FluentValidation
* Serilog Logging
* Docker Support
* Unit Testing
* Role-Based Authorization

---

## Author

Technical Assignment – Doctor License Management Module
