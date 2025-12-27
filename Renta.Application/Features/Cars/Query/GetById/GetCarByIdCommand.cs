using System.Windows.Input;
using FastEndpoints;

namespace Renta.Application.Features.Cars.Query.GetById;

public record GetCarByIdCommand : ICommand<GetCarByIdResponse>
{
    public Guid Id { get; set; }
}