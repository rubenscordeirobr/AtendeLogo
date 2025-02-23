using FluentValidation;

namespace AtendeLogo.ArchitectureTests.TestSupport ;

public class UnitTestTypeNamesCollection
{
    private HashSet<string> _unitTestTypes { get; }

    public UnitTestTypeNamesCollection()
    {
        var applicationTestsAssemply = typeof(Application.UnitTests.Mocks.ContantesTest).Assembly;
        var useCaseTestsAssemply = typeof(UseCases.UnitTests.TestSupport.BrazilianFakeUtils).Assembly;
       
        var allTypes = applicationTestsAssemply.GetTypes()
            .Concat(useCaseTestsAssemply.GetTypes());

        _unitTestTypes = allTypes
            .Where(type => type.Name.EndsWith("Tests"))
            .Select(type => type.Name)
            .ToHashSet();
    }

    public int Count
        => _unitTestTypes.Count;

    public bool Contains(string item)
        => _unitTestTypes.Contains(item);

}

