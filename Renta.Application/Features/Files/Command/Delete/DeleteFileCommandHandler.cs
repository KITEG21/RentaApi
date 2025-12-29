using System;
using Microsoft.EntityFrameworkCore;
using Renta.Application.Interfaces;
using Renta.Domain.Entities.Multimedia;
using Renta.Domain.Interfaces.Repositories;

namespace Renta.Application.Features.Files.Command.Delete;

public class DeleteFileCommandHandler : CoreCommandHandler<DeleteFileCommand, DeleteFileResponse>
{
    private readonly IFileService _fileService;

    public DeleteFileCommandHandler(
        IActiveUserSession activeUserSession,
        IUnitOfWork unitOfWork,
        IFileService fileService) : base(activeUserSession, unitOfWork)
    {
        _fileService = fileService;
    }

    public override async Task<DeleteFileResponse> ExecuteAsync(
        DeleteFileCommand command,
        CancellationToken ct = default)
    {
        Photo? photo = null;
        string publicIdToDelete = command.PublicId;

        // If PhotoId provided, get the photo from database
        if (command.PhotoId.HasValue)
        {
            var photoRepo = UnitOfWork!.WriteDbRepository<Photo>();
            photo = await photoRepo.GetAll()
                .FirstOrDefaultAsync(p => p.Id == command.PhotoId.Value, ct);

            if (photo == null)
            {
                ThrowError("Photo not found", 404);
            }

            // Extract publicId from ImageUrl if needed
            if (string.IsNullOrWhiteSpace(publicIdToDelete))
            {
                publicIdToDelete = ExtractPublicIdFromUrl(photo.ImageUrl);
            }
        }

        if (string.IsNullOrWhiteSpace(publicIdToDelete))
        {
            ThrowError("PublicId is required", 400);
        }

        try
        {
            // Delete from Cloudinary
            await _fileService.DeleteAsync(publicIdToDelete);

            // Delete Photo entity from database if found
            if (photo != null)
            {
                var photoRepo = UnitOfWork!.WriteDbRepository<Photo>();
                await photoRepo.DeleteAsync(photo, false);
                await UnitOfWork.CommitChangesAsync();
            }

            return new DeleteFileResponse
            {
                Success = true,
                Message = "File and photo record deleted successfully"
            };
        }
        catch (Exception ex)
        {
            return new DeleteFileResponse
            {
                Success = false,
                Message = ex.Message
            };
        }
    }

    private string ExtractPublicIdFromUrl(string imageUrl)
    {
        // Extract publicId from Cloudinary URL
        // Example: https://res.cloudinary.com/cloud/image/upload/v123456/folder/image.jpg
        // PublicId: folder/image
        
        try
        {
            var uri = new Uri(imageUrl);
            var segments = uri.AbsolutePath.Split('/');
            
            // Find the index after "upload" or version
            var uploadIndex = Array.IndexOf(segments, "upload");
            if (uploadIndex >= 0 && uploadIndex + 2 < segments.Length)
            {
                // Skip version if present (starts with 'v')
                var startIndex = segments[uploadIndex + 1].StartsWith("v") 
                    ? uploadIndex + 2 
                    : uploadIndex + 1;
                
                var pathSegments = segments.Skip(startIndex);
                var fullPath = string.Join("/", pathSegments);
                
                // Remove file extension
                var lastDot = fullPath.LastIndexOf('.');
                return lastDot > 0 ? fullPath.Substring(0, lastDot) : fullPath;
            }
        }
        catch
        {
            // If parsing fails, return the URL as-is
        }

        return imageUrl;
    }
}