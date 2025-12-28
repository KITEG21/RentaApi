// Create: Renta.Infrastructure/Services/QRCode/QRCodeService.cs
using System.Security.Cryptography;
using System.Text;
using QRCoder;
using Renta.Domain.Interfaces.Services;

namespace Renta.Infrastructure.Services.QRCode;

public class QRCodeService : IQRCodeService
{
    private const string SECRET_KEY = "YourSecretKeyHere123!"; // Move to config
    
    public string GenerateQRCodeString(Guid ticketId, Guid eventId, Guid clientId)
    {
        var timestamp = DateTime.UtcNow.Ticks;
        var dataString = $"{ticketId}|{eventId}|{clientId}|{timestamp}";
        
        // Create signature to prevent tampering
        var signature = GenerateSignature(dataString);
        
        return $"{dataString}|{signature}";
    }
    
    public byte[] GenerateQRCodeImage(string qrCodeData)
    {
        using var qrGenerator = new QRCodeGenerator();
        using var qrCodeDataResp = qrGenerator.CreateQrCode(qrCodeData, QRCodeGenerator.ECCLevel.Q);
        using var qrCode = new PngByteQRCode(qrCodeDataResp);
        
        return qrCode.GetGraphic(20);
    }
    
    public bool ValidateQRCode(string qrCode, Guid ticketId)
    {
        try
        {
            var parts = qrCode.Split('|');
            if (parts.Length != 5) return false;
            
            var dataString = string.Join("|", parts.Take(4));
            var signature = parts[4];
            
            var expectedSignature = GenerateSignature(dataString);
            
            return signature == expectedSignature && parts[0] == ticketId.ToString();
        }
        catch
        {
            return false;
        }
    }
    
    private string GenerateSignature(string data)
    {
        using var hmac = new HMACSHA256(Encoding.UTF8.GetBytes(SECRET_KEY));
        var hash = hmac.ComputeHash(Encoding.UTF8.GetBytes(data));
        return Convert.ToBase64String(hash)
            .Replace("+", "")
            .Replace("/", "")
            .Replace("=", "")
            .Substring(0, 32);
    }
}