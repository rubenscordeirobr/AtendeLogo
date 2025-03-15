using System.Linq.Expressions;

namespace AtendeLogo.Common;

public static class ErrorCodeFactory
{
    
    public static string CreateInvalidCodeFor(Type type, string propertyName)
    {
        return $"{type.Name}.{propertyName}Invalid";
    }

    public static string CreateInvalidCodeFor<T>(Expression<Func<T, object>> propertyExpression)
    {
        var propertyName = propertyExpression.GetMemberName();
        return CreateInvalidCodeFor(typeof(T), propertyName);
    }

    public static string CommandValidatorFoundCodeFor(string commandTypeName)
    {
        return $"{commandTypeName}.ValidatorNotFound";
    }
}

