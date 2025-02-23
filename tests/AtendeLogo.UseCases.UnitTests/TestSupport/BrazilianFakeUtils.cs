namespace AtendeLogo.UseCases.UnitTests.TestSupport;

public static class BrazilianFakeUtils
{
    private static readonly Random _random = new Random();
   
    public static string GenerateCpf()
    {
        int[] cpf = new int[11];

        // Generate the first 9 digits randomly
        for (int i = 0; i < 9; i++)
        {
            cpf[i] = _random.Next(0, 10);
        }

        // Calculate the first verification digit
        int sum = 0;
        for (int i = 0; i < 9; i++)
        {
            sum += cpf[i] * (10 - i);
        }
        int remainder = (sum * 10) % 11;
        cpf[9] = (remainder == 10 || remainder == 11) ? 0 : remainder;

        // Calculate the second verification digit
        sum = 0;
        for (int i = 0; i < 10; i++)
        {
            sum += cpf[i] * (11 - i);
        }
        remainder = (sum * 10) % 11;
        cpf[10] = (remainder == 10 || remainder == 11) ? 0 : remainder;

        // Format CPF as "000.000.000-00"
        return string.Format("{0}{1}{2}.{3}{4}{5}.{6}{7}{8}-{9}{10}",
            cpf[0], cpf[1], cpf[2],
            cpf[3], cpf[4], cpf[5],
            cpf[6], cpf[7], cpf[8],
            cpf[9], cpf[10]);
    }
}

