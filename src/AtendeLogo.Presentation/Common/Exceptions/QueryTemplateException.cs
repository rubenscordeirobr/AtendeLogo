namespace AtendeLogo.Presentation.Common.Exceptions;

public class QueryTemplateException : HttpTemplateException
{
    public QueryTemplateException(string message) : base(message)
    {
    }
}
