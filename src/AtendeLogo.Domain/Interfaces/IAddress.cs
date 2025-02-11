﻿namespace AtendeLogo.Domain.Interfaces;
public interface IAddress
{
    string Street { get; }
    string Number { get; }
    string Complement { get; }
    string Neighborhood { get; }
    string City { get; }
    string State { get; }
    string Country { get; }
    string ZipCode { get; }
}
