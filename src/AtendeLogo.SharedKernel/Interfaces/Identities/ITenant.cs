﻿namespace AtendeLogo.Shared.Interfaces.Identities;
public interface ITenant
{
    string Name { get; }
    string FiscalCode { get; }
    string Email { get; }
    Country Country { get; }
    Language Language { get; }
    Currency Currency { get; }
    BusinessType BusinessType { get; }
    TenantType TenantType { get; }
    PhoneNumber PhoneNumber { get; }
    TimeZoneOffset TimeZoneOffset { get; }
}
