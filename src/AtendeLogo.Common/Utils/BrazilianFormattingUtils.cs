using System.Globalization;

namespace AtendeLogo.Common.Utils;

public static class BrazilianFormattingUtils
{
    private static readonly CultureInfo _brazilianCultureInfo = new("pt-BR");

    public static string FormatCpf(string cpf)
    {
        var numbers = cpf.GetOnlyNumbers();
        if (numbers.Length != 11)
        {
            return cpf;
        }
        return $"{numbers[..3]}.{numbers[3..6]}.{numbers[6..9]}-{numbers[9..]}";
    }

    public static string FormatCnpj(string cnpj)
    {
        var numbers = cnpj.GetOnlyNumbers();
        if (numbers.Length != 14)
        {
            return cnpj;
        }
        return $"{numbers[..2]}.{numbers[2..5]}.{numbers[5..8]}/{numbers[8..12]}-{numbers[12..]}";
    }

    public static string FormatCep(string cep)
    {
        var numbers = cep.GetOnlyNumbers();
        if (numbers.Length != 8)
        {
            return cep;
        }
        return $"{numbers[..5]}-{numbers[5..]}";
    }

    public static string FormatPhone(string phone)
    {
        var numbers = phone.GetOnlyNumbers();
        if (numbers.Length == 10)
        {
            return $"({numbers[..2]}) {numbers[2..6]}-{numbers[6..]}";
        }
        if (numbers.Length == 11)
        {
            return $"({numbers[..2]}) {numbers[2..7]}-{numbers[7..]}";
        }
        return phone;
    }

    public static string FormatMoney(decimal value)
    {
        return value.ToString("C", _brazilianCultureInfo);
    }
}

