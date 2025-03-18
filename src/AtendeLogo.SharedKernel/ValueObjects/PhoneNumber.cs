using AtendeLogo.Common.Infos;

namespace AtendeLogo.Shared.ValueObjects;

public record PhoneNumber : ValueObjectBase
{
    private PhoneNumberInfo _phoneNumberInfo;
    public Country CountryCode 
        => _phoneNumberInfo.CountryCode;
    public InternationalDialingCode InternationalDialingCode
        => _phoneNumberInfo.InternationalDialingCode;
    public string NationalNumber 
        => _phoneNumberInfo.NationalNumber;
    public string AreaCode 
        => _phoneNumberInfo.AreaCode;

    public string Number
         => _phoneNumberInfo.FullNumber;
    
    public PhoneNumber()
    {
        _phoneNumberInfo = PhoneNumberInfo.Unknown(string.Empty);
    }

    public PhoneNumber(string number)
    {
        _phoneNumberInfo = PhoneNumberInfoParser.Parse(number);
    }

    private PhoneNumber(PhoneNumberInfo phoneNumberInfo)
    {
        _phoneNumberInfo = phoneNumberInfo;
    }
    
    public static Result<PhoneNumber> Create(string number)
    {
        var result = PhoneNumberInfo.Create(number);
        if (result.IsFailure)
        {
            return result.AsFailure<PhoneNumber>();
        }
        return Result.Success(new PhoneNumber(result.Value!));
    }
}
