namespace AtendeLogo.Domain.Entities.Activities;

public class DeleteActivity : ActivityBase
{
    public string DeletedData { get; set; }
    public DeleteActivity(string deletedData)
    {
        ActivityType = ActivityType.Delete;
        DeletedData = deletedData;
    }
}
