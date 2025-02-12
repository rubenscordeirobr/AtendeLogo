namespace AtendeLogo.Shared.Interfaces.Commom;

public interface ISortable
{
    public double? SortOrder { get; }
}

public interface IDescendingSortable : ISortable
{

}

public interface IAscendingSortable : ISortable
{
}
