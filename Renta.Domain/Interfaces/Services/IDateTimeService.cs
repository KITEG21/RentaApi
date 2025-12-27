using System;

namespace Renta.Domain.Interfaces.Services;

public interface IDateTimeService
{
    DateTime NowUtc { get; }
}