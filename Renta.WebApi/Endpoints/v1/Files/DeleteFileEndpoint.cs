using System;
using FastEndpoints;
using Renta.Application.Features.Files.Command.Delete;
using Renta.WebApi.Helpers;

namespace Renta.WebApi.Endpoints.v1.Files;

public class DeleteFileEndpoint : Endpoint<DeleteFileCommand, DeleteFileResponse>
{
    public override void Configure()
    {
        Delete("/file/{PublicId}");
        Roles("Admin", "Dealer");
        Tags(RouteGroup.Files);
        Description(s => s
            .WithSummary("Delete a file from Cloudinary")
            .WithTags(RouteGroup.Files)
            .WithDescription("Permanently deletes a file using its public ID")
        );
    }

    public override async Task HandleAsync(DeleteFileCommand req, CancellationToken ct)
    {
        var response = await req.ExecuteAsync(ct);
        await Send.OkAsync(response, ct);
    }
}