namespace AtendeLogo.Domain.Entities.Activities;

public class UpdateActivity : ActivityBase
{
    public string OldData { get; set; }
    public string NewData { get; set; }
     
    public UpdateActivity(string oldData, string newData)  
    {
        OldData = oldData;
        NewData = newData;
        ActivityType = ActivityType.Update;
    }
}
