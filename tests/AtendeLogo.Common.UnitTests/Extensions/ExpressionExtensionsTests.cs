using System.Linq.Expressions;

namespace AtendeLogo.Common.UnitTests.Extensions;

public class ExpressionExtensionsTests
{
    [Fact]
    public void GetMemberName_ShouldReturnMemberName_WhenExpressionIsValid()
    {
        // Arrange
        Expression<Func<TestClass, object>> expression = x => x.TestProperty;

        // Act
        var memberName = expression.GetMemberName();

        // Assert
        memberName.Should().Be(nameof(TestClass.TestProperty));
    }

    [Fact]
    public void GetMemberName_ShouldThrowArgumentException_WhenExpressionIsNotMemberExpression()
    {
        // Arrange
#pragma warning disable CS8603 // Possible null reference return.
        Expression<Func<TestClass, object>> expression = x => x.ToString();
#pragma warning restore CS8603 // Possible null reference return.

        // Act
        Action act = () => expression.GetMemberName();

        // Assert
        act.Should().Throw<ArgumentException>()
            .WithMessage("The Expression 'x => x.ToString()' is not a member expression*");
    }

    private class TestClass
    {
        public string? TestProperty { get; set; }
    }
}

