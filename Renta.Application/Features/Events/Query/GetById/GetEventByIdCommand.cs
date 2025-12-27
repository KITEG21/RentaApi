using System.Windows.Input;
using FastEndpoints;

namespace Renta.Application.Features.Events.Query.GetById;

public record GetEventByIdCommand : ICommand<GetEventByIdResponse>
{
    public Guid Id { get; set; }
}
