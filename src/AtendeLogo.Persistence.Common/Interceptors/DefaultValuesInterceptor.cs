using System.Data.Common;
using Microsoft.EntityFrameworkCore.Diagnostics;

namespace AtendeLogo.Persistence.Common.Interceptors;

public class DefaultValuesInterceptor : DbCommandInterceptor
{
    public override InterceptionResult<DbDataReader> ReaderExecuting(
       DbCommand command,
       CommandEventData eventData,
       InterceptionResult<DbDataReader> result)
    {
        ProcessDefaulValueParameters(command);
        return base.ReaderExecuting(command, eventData, result);
    }

    public override InterceptionResult<int> NonQueryExecuting(
        DbCommand command,
        CommandEventData eventData,
        InterceptionResult<int> result)
    {
        ProcessDefaulValueParameters(command);
        return base.NonQueryExecuting(command, eventData, result);
    }

    public override InterceptionResult<object> ScalarExecuting(
        DbCommand command,
        CommandEventData eventData,
        InterceptionResult<object> result)
    {
        ProcessDefaulValueParameters(command);
        return base.ScalarExecuting(command, eventData, result);
    }

    private void ProcessDefaulValueParameters(DbCommand command)
    {
        foreach (DbParameter parameter in command.Parameters)
        {
            if (parameter.Value is DateTime dt && dt == default)
            {
                parameter.Value = DBNull.Value;
            }

            if(parameter.Value is Guid guid && guid == default)
            {
                parameter.Value = DBNull.Value;
            }
        }
    }
}

