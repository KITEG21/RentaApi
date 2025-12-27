using System;

namespace Renta.Domain.Settings;

public record ThrottleSettings
{
    public int HitLimit { get; init; }
    public int DurationSeconds { get; init; }
}
