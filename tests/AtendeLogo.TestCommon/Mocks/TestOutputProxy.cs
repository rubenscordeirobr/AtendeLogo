namespace AtendeLogo.TestCommon.Mocks;

public class TestOutputProxy: ITestOutputHelper
{
    private ITestOutputHelper? _testOutput;

    public void AddTestOutput(ITestOutputHelper testOutput)
    {
        if (_testOutput is not null &&
            ReferenceEquals( _testOutput, testOutput))
            return;

        _testOutput = testOutput;
    }

    public void WriteLine(string message)
    {
        _testOutput?.WriteLine(message);
    }

    public void WriteLine(string format, params object[] args)
    {
        _testOutput?.WriteLine(format, args);
    }
}

