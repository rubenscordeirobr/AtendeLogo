using System.Text.RegularExpressions;
using AtendeLogo.Common.Enums;
using AtendeLogo.Common.Extensions;

namespace AtendeLogo.Common.Utils;

public static partial class CaseUtils
{

    [GeneratedRegex("^[A-Z]+[a-z0-9]+(?:[A-Z][a-z0-9]*)*$")]
    private static partial Regex PascalCaseRegex();

    [GeneratedRegex("^[a-z][a-z0-9]*(?:[A-Z][a-z0-9]*)*$")]
    private static partial Regex CamelCaseRegex();

    [GeneratedRegex("^[a-z0-9]+(?:_[a-z0-9]+)*$")]
    private static partial Regex SnakeCaseRegex();

    [GeneratedRegex("^[a-z0-9]+(?:-[a-z0-9]+)*$")]
    private static partial Regex KebabCaseRegex();

    [GeneratedRegex("^[A-Z0-9_]+$")]
    private static partial Regex UpperCaseRegex();

    [GeneratedRegex("^[A-Z0-9-]+$")]
    public static partial Regex KebabUpperCaseRegex();


    [GeneratedRegex("([a-z])([A-Z])")]
    private static partial Regex LowerToUpperRegex();

    // Regex to handle consecutive uppercase transitions
    [GeneratedRegex("([A-Z])([A-Z][a-z])")]
    private static partial Regex UpperToUpperRegex();

    [GeneratedRegex("^[a-z0-9]+$")]
    private static partial Regex LowerCaseRegex();

    public static CaseType GetCaseType(string? input)
    {
        Guard.NotNullOrWhiteSpace(input);

        if (string.IsNullOrWhiteSpace(input))
            return CaseType.Unknown;

        if (LowerCaseRegex().IsMatch(input))
            return CaseType.LowerCase;

        if (PascalCaseRegex().IsMatch(input))
            return CaseType.PascalCase;

        if (CamelCaseRegex().IsMatch(input))
            return CaseType.CamelCase;

        if (input.Contains('_') &&
            SnakeCaseRegex().IsMatch(input))
            return CaseType.SnakeCase;

        if (input.Contains('-') &&
            KebabCaseRegex().IsMatch(input))
            return CaseType.KebabCase;

        if (UpperCaseRegex().IsMatch(input))
            return CaseType.UpperCase;

        if (KebabUpperCaseRegex().IsMatch(input))
            return CaseType.KebabUpperCase;

     

        return CaseType.Unknown;
    }

    public static string ToSnakeCase(string? input)
    {
        Guard.NotNullOrWhiteSpace(input);

        var caseType = GetCaseType(input);
        switch (caseType)
        {
            case CaseType.UpperCase:

                return input.ToLower();

            case CaseType.KebabUpperCase:

                return input.Replace('-', '_').ToLower();

            case CaseType.PascalCase:
            case CaseType.CamelCase:

                return FromPascalCamelCaseToLowerCase(input, '_');

            case CaseType.SnakeCase:

                return input;
            case CaseType.KebabCase:

                return input.Replace('-', '_');

            case CaseType.LowerCase:

                return input;

            default:

                throw new NotSupportedException($"Format input '{input}' not supported");
        }
    }

    public static string ToKebabCase(string? input)
    {
        Guard.NotNullOrWhiteSpace(input);

        var caseType = GetCaseType(input);
        switch (caseType)
        {
            case CaseType.UpperCase:
                return input.Replace('_', '-').ToLower();
            case CaseType.KebabUpperCase:
                return input.ToLower();
            case CaseType.PascalCase:
            case CaseType.CamelCase:
                return FromPascalCamelCaseToLowerCase(input, '-');
            case CaseType.KebabCase:
                return input;
            case CaseType.SnakeCase:
                return input.Replace('_', '-');

            default:

                throw new NotSupportedException($"Format input '{input}' not supported");
        }
    }

    public static string ToPascalCase(string? input)
    {
        Guard.NotNullOrWhiteSpace(input);

        var caseType = GetCaseType(input);
        switch (caseType)
        {
            case CaseType.UpperCase:

                return ToPascalCase(input.ToLower());

            case CaseType.KebabUpperCase:

                return ToPascalCase(input.ToLower());

            case CaseType.PascalCase:

                return input;
            case CaseType.CamelCase:

                return input.Capitalize();

            case CaseType.SnakeCase:
            case CaseType.KebabCase:

                var splits = input.Split('_', '-');
                return ToPascalCamelCaseInternal(splits);

            default:
                throw new NotSupportedException($"Format input '{input}' not supported");
        }
    }

    public static string ToCamelCase(string? input)
    {
        Guard.NotNullOrWhiteSpace(input);
        var caseType = GetCaseType(input);
        switch (caseType)
        {
            case CaseType.UpperCase:
                return ToCamelCase(input.ToLower());
            case CaseType.KebabUpperCase:
                return ToCamelCase(input.ToLower());
            case CaseType.PascalCase:
                return input.Descapitalize();
            case CaseType.CamelCase:
                return input;
            case CaseType.SnakeCase:
            case CaseType.KebabCase:
                var splits = input.Split('_', '-');
                return ToCamelCaseInternal(splits);
            default:
                throw new NotSupportedException($"Format input '{input}' not supported");
        }
    }

    public static string ToUpperCase(string? input)
    {
        Guard.NotNullOrWhiteSpace(input);
        var caseType = GetCaseType(input);
        switch (caseType)
        {
            case CaseType.UpperCase:
                return input;
            case CaseType.KebabUpperCase:
                return input.Replace('-', '_');
            case CaseType.PascalCase:
            case CaseType.CamelCase:
                return input.ToUpper();
            case CaseType.SnakeCase:
                return input.Replace('_', '-').ToUpper();
            case CaseType.KebabCase:
                return input.Replace('-', '_').ToUpper();
            default:
                throw new NotSupportedException($"Format input '{input}' not supported");
        }
    }

    private static string ToPascalCamelCaseInternal(string[] splits)
    {
        return String.Concat(splits.Select(x => x.Capitalize()));
    }
    private static string ToCamelCaseInternal(string[] splits)
    {
        return String.Concat(splits.Select((x, i) => i == 0 ? x : x.Capitalize()));
    }

    private static string FromPascalCamelCaseToLowerCase(
        string input, 
        char separator)
    {
        Guard.NotNullOrWhiteSpace(input);

        var result = input?.ToString();
        if (String.IsNullOrEmpty(result))
            return String.Empty;

        result = LowerToUpperRegex()
            .Replace(result, $"$1{separator}$2"); // Add separator between lowercase-uppercase
        
        result = UpperToUpperRegex()
            .Replace(result, $"$1{separator}$2"); // Handle consecutive uppercase-uppercase transitions

        return result.ToLower();
    }

   
}
