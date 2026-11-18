# Employee API Load Tester

A C# console application that authenticates against an employee API and sends concurrent load-test requests to the employee endpoint.

## Features

- Logs in to the API with JWT authentication
- Sends five concurrent `GET` requests to `/api/Employees`
- Counts successful and failed requests
- Displays the first employee returned by successful responses
- Measures and reports total execution time

## Technology

- C#
- .NET 10
- `HttpClient`
- `System.Text.Json`
- Asynchronous/parallel HTTP requests

## Project Structure

```text
EmployeeApiLoadTester/
├── Api hitting taste using employee.slnx
└── Api hitting taste using employee/
    ├── Api hitting taste using employee.csproj
    ├── Employee.cs
    └── Program.cs
```

## Prerequisites

- [.NET 10 SDK](https://dotnet.microsoft.com/download)
- An API running locally on `https://localhost:7146`
- A login account registered in the API
- Trust configured for the local development HTTPS certificate

## API Requirements

The application currently expects two endpoints:

- `POST https://localhost:7146/api/Auth/login`
- `GET https://localhost:7146/api/Employees`

The login request must contain:

```json
{
  "username": "sanskar@example.com",
  "password": "sanskar@123"
}
```

The login response must include a JWT token:

```json
{
  "token": "your-jwt-token"
}
```

The employees endpoint must be accessible with the returned bearer token and return a JSON array compatible with:

```json
[
  {
    "id": 1,
    "name": "Employee Name",
    "email": "employee@example.com"
  }
]
```

## Run the Application

Restore and run the project:

```bash
dotnet run --project "Api hitting taste using employee/Api hitting taste using employee.csproj"
```

Or open `Api hitting taste using employee.slnx` in a compatible IDE and run the project.

## Example Output

```text
Request 1 Success | Employee: John Doe
Request 2 Success | Employee: John Doe
Request 3 Success | Employee: John Doe
Request 4 Success | Employee: John Doe
Request 5 Failed | Status: 401

============ RESULT ============
Success : 4
Failed  : 1
Total Time : 215 ms
```

## Configuration

Update the values in `Program.cs` to match your API:

- API login and employee URLs
- Login username and password
- Number of concurrent requests

The concurrent request count is currently controlled by this loop:

```csharp
for (int i = 1; i <= 5; i++)
```

## Notes

- This is a small concurrency and JWT authentication example, not a production load-testing framework.
- Request results are stored in shared counters using `Interlocked.Increment` to support concurrent tasks safely.
- If the API uses an HTTP address or a different HTTPS port, update the URLs in `Program.cs`.
- Never commit real passwords or production credentials to source control.
