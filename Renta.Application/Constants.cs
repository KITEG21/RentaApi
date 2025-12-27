using System;

namespace Renta.Application;

public static class Constants
{
    #region Pagination
    public const string PageSize = "PageSize";
    public const string PageNumber = "PageNumber";
    public const string PageNumberMustBeAtLeastOneError = "Page number must be greater than or equal to 1.";
    public const string PageSizeMustBeAtLeastOneError = "Page size must be greater than or equal to 1.";
    #endregion
}
