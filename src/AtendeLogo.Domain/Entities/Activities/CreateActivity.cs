namespace AtendeLogo.Domain.Entities.Activities;

public class CreateActivity : ActivityBase
{
    public string CreatedData { get; set; }

    public CreateActivity(string createdData)
    {
        ActivityType = ActivityType.Create;
        CreatedData = createdData;
    }
}
