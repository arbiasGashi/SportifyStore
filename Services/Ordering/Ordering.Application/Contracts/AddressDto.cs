namespace Ordering.Application.Contracts;

public sealed record AddressDto(
    string FirstName,
    string LastName,
    string Email,
    string AddressLine,
    string Country,
    string State,
    string ZipCode);
