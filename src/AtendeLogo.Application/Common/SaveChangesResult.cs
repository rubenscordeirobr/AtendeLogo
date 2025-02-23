using System.Diagnostics.CodeAnalysis;

namespace AtendeLogo.Application.Common;

public class SaveChangesResult
{
    [MemberNotNullWhen(false, nameof(Error))]
    [MemberNotNullWhen(false, nameof(Exception))]
    public bool IsSuccess { get; }

    public int AffectedRows { get; }

    public IDomainEventContext DomainEventContext { get; }

    public Error? Error { get; }

    public Exception? Exception
        => GetException();

    public SaveChangesResult(IDomainEventContext eventDomainContext, Error error)
    {
        Guard.NotNull(eventDomainContext);
        Guard.NotNull(error);

        DomainEventContext = eventDomainContext;
        IsSuccess = false;
        Error = error;
        AffectedRows = 0;
    }

    public SaveChangesResult(
        IDomainEventContext eventDomainContext,
        int affectedRows)
    {
        Guard.NotNull(eventDomainContext);

        DomainEventContext = eventDomainContext;
        AffectedRows = affectedRows;
        IsSuccess = true;
        Error = null;
    }

    private Exception? GetException()
    {
        if (IsSuccess)
            return null;

        if (Error is null)
            throw new InvalidOperationException("Error is null when IsSuccess is false");

        return Error switch
        {
            DatabaseError databaseError => databaseError.Exception,
            OperationCanceledError operationError => operationError.Exception,
            DomainEventError domainEventError => new DomainEventException(domainEventError.FormattedMessage ?? "Unknown error"),
            _ => new Exception(Error!.FormattedMessage ?? "Unknown error")
        };
    }

    public static SaveChangesResult Success(
       IDomainEventContext eventDomainContext,
       int rowAffects)
    {
        return new SaveChangesResult(eventDomainContext, rowAffects);
    }

    public static SaveChangesResult SaveChangesError(
        Exception exception,
        IDomainEventContext domainEventContext)
    {
        return DatabaseError(exception, domainEventContext, "EntityFramewor.SaveChangesError");

    }
    public static SaveChangesResult TransactionRollbackError(
        Exception exception, 
        IDomainEventContext domainEventContext)
    {
        return DatabaseError(exception, domainEventContext, "EntityFramewor.TransactionRollbackError");
    }

    public static SaveChangesResult DomainEventError(
        IDomainEventContext domainEventContext,
        DomainEventError error)
    {
        return new SaveChangesResult(domainEventContext, error);
    }

    public static SaveChangesResult OperationCanceledError(
        CancellationToken cancellationToken,
        IDomainEventContext eventDomainContext)
    {
        var exception = new OperationCanceledException(
            $"IsCancellationRequested {cancellationToken.IsCancellationRequested}",
            cancellationToken);

        var operationError = new OperationCanceledError(
            Exception: exception,
            Code: "EntityFramewor.IsCancellationRequested",
            Message: exception.Message
        );

        return new SaveChangesResult(eventDomainContext, operationError);
    }

    public static SaveChangesResult OperationCanceledError(
        OperationCanceledException exception,
        IDomainEventContext domainEventContext)
    {
        var operationError = new OperationCanceledError(
          Exception: exception,
          Code: "EntityFramewor.OperationCanceledError",
          Message: exception.Message
       );

        return new SaveChangesResult(domainEventContext, operationError);
    }

    private static SaveChangesResult DatabaseError(
        Exception exception,
        IDomainEventContext domainEventContext,
        string errorCode)
    {
        var databaseError = new DatabaseError(
            Exception: exception,
            Code: errorCode,
            Message: exception.Message
        );

        return new SaveChangesResult(domainEventContext, databaseError);
    }
}
