using System;
using Renta.Domain.Interfaces.Services;

namespace Renta.Infrastructure.Services;

public class DateTimeService : IDateTimeService
{
    public DateTime NowUtc => DateTime.UtcNow;
}