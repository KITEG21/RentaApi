namespace Renta.Application.Features.Events.Command.Put;

public record class UpdateEventResponse
{
    public Guid Id { get; init; }
    public string Title { get; init; } = string.Empty;
    public DateTime EventDate { get; init; }
    public string Location { get; init; } = string.Empty;
    public int TotalCapacity { get; init; }
}
