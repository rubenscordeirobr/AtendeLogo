using AtendeLogo.Shared.Interfaces.Shared;

namespace AtendeLogo.UseCases.Shared;
 
public sealed record AddressDto : IAddress
{
    public required string Street { get; init; }
    public required string Number { get; init; }
    public string? Complement { get; init; }
    public required string Neighborhood { get; init; }
    public required string City { get; init; }
    public required string State { get; init; }
    public required string ZipCode { get; init; }
    public required Country Country { get; init; }
}
