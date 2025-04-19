namespace AtendeLogo.Common.Enums;

public enum InternationalDialingCode
{
    [UndefinedValue]
    Unknown = 0,

    // North America
    [SharedInternationalDialingCode]
    UnitedStatesOrCanada = 1,
    Mexico = 52,
     
    // South America
    Argentina = 54,
    Bolivia = 591,
    Brazil = 55,
    Chile = 56,
    Colombia = 57,
    Ecuador = 593,
    Guyana = 592,
    Paraguay = 595,
    Peru = 51,
    Suriname = 597,
    Uruguay = 598,
    Venezuela = 58,

    // Europe
    Italy = 39,
    France = 33,
    Germany = 49,
    Spain = 34,
    Portugal = 351,
    UnitedKingdom = 44

}
