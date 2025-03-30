using AtendeLogo.Domain.Primitives;

namespace AtendeLogo.Persistence.Identity.Extensions;

internal static class EntityBaseExtensions
{
    internal static void SetCreateSession(
        this EntityBase entity,
        Guid createdSession_Id)
    {
        entity.SetPropertyValue(x => x.CreatedSession_Id, createdSession_Id);
        entity.SetPropertyValue(x => x.LastUpdatedSession_Id, createdSession_Id);
    }
}
