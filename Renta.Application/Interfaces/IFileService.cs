using System;
using CloudinaryDotNet.Actions;
using Microsoft.AspNetCore.Http;

namespace Renta.Application.Interfaces;

public interface IFileService
{
    Task<ImageUploadResult> UploadAsync(
            IFormFile file,
            string folder
        );

    Task DeleteAsync(string publicId);
}
