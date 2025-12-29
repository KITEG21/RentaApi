using System;
using FastEndpoints;
using Renta.Application.Features.Files.Command.Upload;
using Renta.WebApi.Helpers;

namespace Renta.WebApi.Endpoints.v1.Files;

public class UploadFileEndpoint : Endpoint<UploadFileCommand, UploadFileResponse>
{
    public override void Configure()
    {
        Post("/file/upload");
        Roles("Admin");
        Tags(RouteGroup.Files);
        AllowFileUploads();
        Description(s => s
            .WithSummary("Upload a file from Cloudinary")
            .WithTags(RouteGroup.Files)
            .WithDescription("Uploads an image file (JPEG, PNG, WEBP) with automatic optimization. Max size: 10MB")
        );
    }

    public override async Task HandleAsync(UploadFileCommand req, CancellationToken ct)
    {
        var response = await req.ExecuteAsync(ct);
        await Send.OkAsync(response, ct);
    }
}