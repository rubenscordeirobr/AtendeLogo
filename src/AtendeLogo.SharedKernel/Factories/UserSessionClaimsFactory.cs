using System.Security.Claims;
using AtendeLogo.Common.Utils;
using AtendeLogo.Shared.Constants;
using AtendeLogo.Shared.Interfaces.Identities;
using AtendeLogo.Shared.Models.Security;

namespace AtendeLogo.Shared.Factories;

public static class UserSessionClaimsFactory
{
    public static UserSessionClaims Create(
        IUserSession userSession,
        IUser user)
    {
        Guard.NotNull(userSession);
        Guard.NotNull(user);

        return Create(
            user.Name,
            user.Email,
            user.PhoneNumber.Number,
            userSession.Id,
            userSession.UserRole,
            userSession.UserType,
            null);
    }

    public static Result<UserSessionClaims> Create(
        IEnumerable<Claim> claims, 
        DateTime expiration)
    {
        Guard.NotNull(claims);

        var claimsList = claims as IList<Claim> ?? [.. claims];
        var name = claimsList.FirstOrDefault(static c => c.Type == ClaimTypes.Name)?.Value;
        var email = claimsList.FirstOrDefault(c => c.Type == ClaimTypes.Email)?.Value;
        var phoneNumber = claimsList.FirstOrDefault(c => c.Type == ClaimTypes.MobilePhone)?.Value;
        var sessionId = claimsList.FirstOrDefault(c => c.Type == UserSessionClaimTypes.SessionId)?.Value;
        var userRole = claimsList.FirstOrDefault(c => c.Type == ClaimTypes.Role)?.Value;
        var userType = claimsList.FirstOrDefault(c => c.Type == UserSessionClaimTypes.UserType)?.Value;
         
        return Create(
           name: name,
           email: email,
           phoneNumber: phoneNumber,
           sessionIdString: sessionId,
           userRoleString: userRole,
           userTypeString: userType,
           expiration: expiration);
    }

    private static Result<UserSessionClaims> Create(
        string? name,
        string? email,
        string? phoneNumber,
        string? sessionIdString,
        string? userRoleString,
        string? userTypeString,
        DateTime? expiration)
    {
        if (string.IsNullOrWhiteSpace(name))
        {
            return Result.Failure<UserSessionClaims>(
                new ParserError(
                    null,
                    "UserSessionClaims.Create",
                    "Name is null or empty"));
        }

        if (string.IsNullOrWhiteSpace(email))
        {
            return Result.Failure<UserSessionClaims>(
                new ParserError(
                    null,
                    "UserSessionClaims.Create",
                    "Email is null or empty"));
        }

        if (string.IsNullOrWhiteSpace(phoneNumber))
        {
            return Result.Failure<UserSessionClaims>(
                new ParserError(
                    null,
                    "UserSessionClaims.Create",
                    "PhoneNumber is null or empty"));
        }

        if (!Guid.TryParse(sessionIdString, out var session_Id))
        {
            return Result.Failure<UserSessionClaims>(
                new ParserError(
                    null,
                    "UserSessionClaims.Create",
                    $"Failed to parse Guid from string: {sessionIdString}"));
        }

        if (!EnumUtils.TryParse<UserType>(userTypeString, out var userType))
        {
            return Result.Failure<UserSessionClaims>(
                new ParserError(
                    null,
                    "UserSessionClaims.Create",
                    $"Failed to parse UserType from string: {userTypeString}"));
        }

        if (!EnumUtils.TryParse<UserRole>(userRoleString, out var userRole))
        {
            return Result.Failure<UserSessionClaims>(
                new ParserError(
                    null,
                    "UserSessionClaims.Create",
                    $"Failed to parse UserRole from string: {userRoleString}"));
        }

        if (expiration is null)
        {
            return Result.Failure<UserSessionClaims>(
                new ParserError(
                    null,
                    "UserSessionClaims.Create",
                    $"Expiration is null"));
        }

        if (expiration < DateTime.UtcNow)
        {
            return Result.Failure<UserSessionClaims>(
                new ParserError(
                    null,
                    "UserSessionClaims.Create",
                    $"Expiration is in the past"));
        }

        return Result.Success(Create(name, email, phoneNumber, session_Id, userRole, userType, expiration));

    }

    public static UserSessionClaims Create(
        string name,
        string email,
        string phoneNumber,
        Guid session_Id,
        UserRole userRole,
        UserType userType,
        DateTime? expiration)
    {
        return new UserSessionClaims(
          name,
          email,
          phoneNumber,
          session_Id,
          userRole,
          userType,
          expiration);
    }
}
