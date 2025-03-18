namespace AtendeLogo.Persistence.Common.Helpers;

internal static class FilterExpressionHelper
{
    internal static string AppendSoftDeleteFilter(
        string? currentFilter,
        string deletedColumnName)
    {
        var deletedFilter = $"{deletedColumnName} = false";
        if (string.IsNullOrEmpty(currentFilter))
        {
            return deletedFilter;
        }

        if(currentFilter.Contains(deletedColumnName))
        {
            return currentFilter;
        }
        return $"{currentFilter} AND {deletedFilter}";
    }
}

