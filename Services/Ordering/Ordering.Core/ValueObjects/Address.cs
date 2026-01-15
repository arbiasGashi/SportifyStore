using Ordering.Core.Exceptions;

namespace Ordering.Core.ValueObjects;

public sealed record Address(
    string FirstName,
    string LastName,
    string Email,
    string AddressLine,
    string Country,
    string State,
    string ZipCode)
{
    public static Address Create(
        string firstName,
        string lastName,
        string email,
        string addressLine,
        string country,
        string state,
        string zipCode)
    {
        if (string.IsNullOrWhiteSpace(firstName))
        {
            throw new DomainException("FirstName is required.");
        }

        if (string.IsNullOrWhiteSpace(lastName))
        {
            throw new DomainException("LastName is required.");
        }

        if (string.IsNullOrWhiteSpace(email))
        {
            throw new DomainException("Email is required.");
        }

        if (string.IsNullOrWhiteSpace(addressLine))
        {
            throw new DomainException("AddressLine is required.");
        }

        if (string.IsNullOrWhiteSpace(country))
        {
            throw new DomainException("Country is required.");
        }

        if (string.IsNullOrWhiteSpace(zipCode))
        {
            throw new DomainException("ZipCode is required.");
        }

        return new Address(firstName, lastName, email, addressLine, country, state, zipCode);
    }
}
