# Developist.Core.Api.Exceptions.Filters

## Overview
The `Developist.Core.Api.Exceptions.Filters` library enhances ASP.NET Core applications with error handling and standardized problem details responses.

### Usage
The library automatically handles unhandled exceptions in ASP.NET Core MVC/API controllers, generating standardized `ProblemDetails` responses based on mapped HTTP status codes.
For instances of [`ApiException`](../Developist.Core.Api.Exceptions/ApiException.cs) or its derivatives, manual mapping of exceptions to HTTP status codes is not required.

### Installation
Install the `Developist.Core.Exceptions.Api.Filters` package using the NuGet Package Manager:

`dotnet add package Developist.Core.Exceptions.Api.Filters`

Configure global exception handling:

```csharp
// Register GlobalExceptionFilterAttribute as a singleton service.
builder.Services.AddSingleton<GlobalExceptionFilterAttribute>();

// Optionally, customize GlobalExceptionFilterAttribute behavior by configuring specific options.
builder.Services.Configure<GlobalExceptionFilterOptions>(options =>
{
    // Map exceptions to HTTP status codes.
    options.MapExceptionToStatusCode<ArgumentException>(HttpStatusCode.BadRequest);
    options.MapExceptionToStatusCode<OutOfMemoryException>(HttpStatusCode.InsufficientStorage);
});

// Add GlobalExceptionFilterAttribute as a filter for all controllers and action methods.
builder.Services.AddControllers(options =>
{
    options.Filters.AddService<GlobalExceptionFilterAttribute>();
});
```

Alternatively, annotate any action method with the [`GlobalExceptionFilterAttribute`](GlobalExceptionFilterAttribute.cs), like so:

```csharp
[HttpGet]
[GlobalExceptionFilter]
public IEnumerable<Customer> GetCustomerData() { .... }
```

Note that this method will use type activation instead of dependency injection to create a `GlobalExceptionFilterAttribute` instance.
