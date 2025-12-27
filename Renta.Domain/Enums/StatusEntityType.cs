namespace Renta.Domain.Enums;

public enum StatusEntityType
{
    /// <summary>
    ///Deleted by platform
    /// </summary>
    Delete,
    Active,
    Inactive=2,
    /// <summary>
    ///Deleted by logged user
    /// </summary>
    DeleteUser,
    /// <summary>
    /// Temporally disabled
    /// </summary>
    Offline,
    InEdition,
}
