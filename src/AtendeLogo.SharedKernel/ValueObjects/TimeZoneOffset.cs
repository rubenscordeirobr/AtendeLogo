﻿using System.Text.Json;
using System.Text.Json.Serialization;

namespace AtendeLogo.Shared.ValueObjects;

public sealed record TimeZoneOffset : ValueObjectBase
{
    public string Offset { get; private set; }
    public string Location { get; private set; }
    public TimeSpan OffsetTimeSpan
        => TimeSpan.Parse(Offset);

    [JsonConstructor]
    private TimeZoneOffset(string offset, string location)
    {
        Offset = offset;
        Location = location;
    }

    public sealed override string ToString()
        => $"Offset: {Offset} Location:{Location}";

    public string ToJson()
        => JsonSerializer.Serialize(this);

    public static Result<TimeZoneOffset> Create(string offset, string location)
    {
        if (string.IsNullOrWhiteSpace(offset))
        {
            return Result.ValidationFailure<TimeZoneOffset>(
                "TimeZone.OffsetEmpty",
                "Offset cannot be empty.");
        }

        if (string.IsNullOrWhiteSpace(location))
        {
            return Result.ValidationFailure<TimeZoneOffset>(
                "TimeZine.LocationEmpty",
                "Location cannot be empty.");
        }

        if (!TimeSpan.TryParse(offset, out var offsetTimeSpan))
        {
            return Result.ValidationFailure<TimeZoneOffset>(
                "TimeZone.InvalidOffset",
                $"Invalid offset. {offset}");
        }
        return Result.Success(new TimeZoneOffset(offset, location));
    }
     
    public static TimeZoneOffset Default
        => new("00:00", "UTC");
     
    public static TimeZoneOffset Parse(string json)
    {
        return JsonSerializer.Deserialize<TimeZoneOffset>(json)
            ?? Default;
    }
}
