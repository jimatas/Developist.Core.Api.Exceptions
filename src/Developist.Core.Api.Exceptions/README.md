# Developist.Core.Api.Exceptions

## Overview
The `Developist.Core.Api.Exceptions` library provides a set of core exception types for use in .NET applications, designed to handle common error scenarios in ASP.NET Core MVC/API projects.

## Exceptions

The library provides the following exception types, each corresponding to a common HTTP error status code:

#### [`ApiException`](ApiException.cs)
Base class for all API exceptions.

#### [`BadRequestException`](BadRequestException.cs)
(HTTP 400): Indicates a bad request error.

#### [`ConflictException`](ConflictException.cs)
(HTTP 409): Indicates a conflict error.

#### [`ForbiddenException`](ForbiddenException.cs)
(HTTP 403): Indicates a forbidden error.

#### [`NotFoundException`](NotFoundException.cs)
(HTTP 404): Indicates a not found error.

#### [`UnauthorizedException`](UnauthorizedException.cs)
(HTTP 401): Indicates an unauthorized error.

#### [`UnprocessableEntityException`](UnprocessableEntityException.cs)
(HTTP 422): Indicates an unprocessable entity error.
