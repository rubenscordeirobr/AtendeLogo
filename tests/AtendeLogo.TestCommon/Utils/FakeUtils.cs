using AtendeLogo.Common.Utils;

namespace AtendeLogo.TestCommon.Utils;

public static class FakeUtils
{
    public static string GenerateFakeEmail()
    {
        return $"fake{RandomUtils.GenerateRandomNumber(10)}@example.com";
    }
}
