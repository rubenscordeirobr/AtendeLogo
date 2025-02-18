using System.Linq.Expressions;

namespace AtendeLogo.Common.Extensions;

public static class ExpressionExtensions
{
    public static string GetMemberName<T>(this Expression<Func<T, object>> expression)
    {
        Expression body = expression.Body;

        if (body is UnaryExpression unaryExpression)
        {
            body = unaryExpression.Operand;
        }

        if (body is MemberExpression memberExpression)
        {
            return memberExpression.Member.Name;
        }

        throw new ArgumentException($"The Expression '{expression}' is not a member expression", nameof(expression));
    }
}

