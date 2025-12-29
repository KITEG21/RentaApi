using System;
using CloudinaryDotNet;
using CloudinaryDotNet.Actions;
using Microsoft.AspNetCore.Http;
using Renta.Application.Interfaces;

namespace Renta.Infrastructure.Services.Files;

public class CloudinaryFileService : IFileService
{
    private readonly Cloudinary _cloudinary;
    public CloudinaryFileService(Cloudinary cloudinary)
    {
        _cloudinary = cloudinary;
    }

    public async Task<ImageUploadResult> UploadAsync(IFormFile file, string folder)
    {
        if (file == null || file.Length == 0)
            throw new Exception("Invalid File");

        // Validar tipo
        var allowedTypes = new[] { "image/jpeg", "image/png", "image/webp" };
        if (!allowedTypes.Contains(file.ContentType))
            throw new Exception("Invalid Format. Only JPEG, PNG, and WEBP are allowed.");

        await using var stream = file.OpenReadStream();

        var uploadParams = new ImageUploadParams
        {
            File = new FileDescription(file.FileName, stream),
            Folder = folder,
            UseFilename = true,
            UniqueFilename = true,
            Overwrite = false,
            Transformation = new Transformation()
                .Width(1600)
                .Height(900)
                .Crop("limit")
                .Quality("auto")
                .FetchFormat("auto")
        };

        return await _cloudinary.UploadAsync(uploadParams);
    }
    
    public async Task DeleteAsync(string publicId)
    {
       
        var deleteParams = new DeletionParams(publicId);
        var result = await _cloudinary.DestroyAsync(deleteParams);

        if (result.Result != "ok")
            throw new Exception("Could not delete the image");
    }


}
