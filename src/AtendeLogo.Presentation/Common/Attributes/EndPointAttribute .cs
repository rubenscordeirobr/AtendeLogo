namespace AtendeLogo.Presentation.Common.Attributes;

[AttributeUsage(AttributeTargets.Class)]
public class EndPointAttribute : Attribute
{
    public string RoutePrefix { get; }
    public EndPointAttribute(string routePrefix)
    {
        RoutePrefix = routePrefix;
    }
}
