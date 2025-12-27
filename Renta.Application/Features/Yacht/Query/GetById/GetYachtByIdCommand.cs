using System.Windows.Input;
using FastEndpoints;

namespace Renta.Application.Features.Yacht.Query.GetById;

public record GetYachtByIdCommand : ICommand<GetYachtByIdResponse>
{
    public Guid Id { get; set; }
}
