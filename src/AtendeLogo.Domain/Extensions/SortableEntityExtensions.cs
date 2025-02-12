namespace AtendeLogo.Domain.Extensions;

public static class SortableEntityExtensions
{
    public static void SetOrder(
        this ISortable entity,
        double sortOrder)
    {
        var properties = entity.GetType().GetPropertiesFromInterface<ISortable>();
        properties[nameof(ISortable.SortOrder)]
            .SetValue(entity, sortOrder);
    }
}
