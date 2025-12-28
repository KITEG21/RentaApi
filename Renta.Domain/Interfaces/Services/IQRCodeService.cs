namespace Renta.Domain.Interfaces.Services;

public interface IQRCodeService
{
    string GenerateQRCodeString(Guid ticketId, Guid eventId, Guid clientId);
    byte[] GenerateQRCodeImage(string qrCodeData);
    bool ValidateQRCode(string qrCode, Guid ticketId);
}