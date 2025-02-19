using NetArchTest.Rules;

namespace AtendeLogo.ArchitectureTests;

public static class TestResultExtensions
{
    public static string GetFailingTypeNames(this TestResult result)
    {
        if (result.FailingTypeNames is null)
            return string.Empty;

        return string.Join(",", result.FailingTypeNames);
    }
}
