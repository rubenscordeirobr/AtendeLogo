using AtendeLogo.Common;
using AtendeLogo.Domain.Entities.Activities;
using AtendeLogo.Shared.Enums;

namespace AtendeLogo.Persistence.Activity.Documents;

public static class ActivityDocumentMapper 
{
    public static ActivityDocument MapToDocument(ActivityBase activity)
    {
        Guard.NotNull(activity);

        var document = new ActivityDocument
        {
            Tenant_Id = activity.Tenant_Id,
            UserSession_Id = activity.UserSession_Id,
            ActivityType = activity.ActivityType,
            Description = activity.Description,
        };

        switch (activity)
        {
            case CreatedActivity createActivity:
                document.CreatedData = createActivity.CreatedData;
                break;
            case UpdatedActivity updateActivity:
                document.OldData = updateActivity.OldData;
                document.NewData = updateActivity.NewData;
                break;
            case DeletedActivity deleteActivity:
                document.DeletedData = deleteActivity.DeletedData;
                break;
            case LoginSuccessfulActivity authActivity:
                document.IPAddress = authActivity.IPAddress;
                document.AuthenticationType = authActivity.AuthenticationType;
                break;
            case LoginFailedActivity failedAuthActivity:
                document.IPAddress = failedAuthActivity.IPAddress;
                document.UserAgent = failedAuthActivity.UserAgent;
                document.PasswordFailed = failedAuthActivity.PasswordFailed;
                break;
        }

        if (activity is EntityActivity entityActivity)
        {
            document.QualifiedTypeName = entityActivity.QualifiedTypeName;
            document.Entity_Id = entityActivity.Entity_Id;
        }

        return document;
    }

    public static ActivityBase MapToDomain(ActivityDocument document)
    {
        Guard.NotNull(document);

        switch (document.ActivityType)
        {
            case ActivityType.Created:

                return new CreatedActivity
                {
                    Id = document.Id,
                    Tenant_Id = document.Tenant_Id,
                    UserSession_Id = document.UserSession_Id,
                    ActivityDate = document.ActivityAt,
                    Description = document.Description,
                    CreatedData = document.CreatedData ?? string.Empty,
                    QualifiedTypeName = document.QualifiedTypeName ?? string.Empty,
                    Entity_Id = document.Entity_Id.GetValueOrDefault()
                };

            case ActivityType.Updated:

                return new UpdatedActivity
                {
                    Id = document.Id,
                    Tenant_Id = document.Tenant_Id,
                    UserSession_Id = document.UserSession_Id,
                    Description = document.Description,
                    ActivityDate = document.ActivityAt,
                    OldData = document.OldData ?? string.Empty,
                    NewData = document.NewData ?? string.Empty,
                    QualifiedTypeName = document.QualifiedTypeName ?? string.Empty,
                    Entity_Id = document.Entity_Id.GetValueOrDefault()
                };

            case ActivityType.Deleted:

                return new DeletedActivity
                {
                    Id = document.Id,
                    Tenant_Id = document.Tenant_Id,
                    UserSession_Id = document.UserSession_Id,
                    Description = document.Description,
                    ActivityDate = document.ActivityAt,
                    DeletedData = document.DeletedData ?? string.Empty,
                    QualifiedTypeName = document.QualifiedTypeName ?? string.Empty,
                    Entity_Id = document.Entity_Id.GetValueOrDefault()
                };

            case ActivityType.LoginSuccessful:

                return new LoginSuccessfulActivity
                {
                    Id = document.Id,
                    Tenant_Id = document.Tenant_Id,
                    UserSession_Id = document.UserSession_Id,
                    Description = document.Description,
                    ActivityDate = document.ActivityAt,
                    AuthenticationType = document.AuthenticationType ?? AuthenticationType.Unknown,
                    IPAddress = document.IPAddress ?? "Unknown"
                };

            case ActivityType.LoginFailed:

                return new LoginFailedActivity
                {
                    Id = document.Id,
                    Tenant_Id = document.Tenant_Id,
                    UserSession_Id = document.UserSession_Id,
                    Description = document.Description,
                    ActivityDate = document.ActivityAt,
                    IPAddress = document.IPAddress ?? "Unknown",
                    UserAgent = document.UserAgent ?? "Unknown",
                    PasswordFailed = document.PasswordFailed ?? "Unknown",
                };
            default:

                throw new ArgumentException($"Unsupported ActivityType: {document.ActivityType}");
        }
    }
}
